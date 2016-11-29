using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Termoservis.Common.Extensions;
using Termoservis.DAL;
using Termoservis.DAL.Repositories;
using Termoservis.Models;

namespace Termoservis.MigrationTool
{
    class Program
    {
        public static String TitleCaseString(String s)
        {
            if (s == null) return s;

            String[] words = s.Split(' ', '.');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join(" ", words);
        }

        static void Main(string[] args)
        {
            const string dataPath =
                "C:\\Users\\aleks\\OneDrive\\Documents\\Development\\Termoservis\\Customers - Migration data";
            const string placesDataSource = "naselja.csv";
            const string customersDataSource = "termoserviskorisnici.csv";

            bool seedPlaces = false;

            // Create country if doesnt exit
            var defaultCountry = "Hrvatska";
            using (var context = new ApplicationDbContext())
            {
                if (!context.Countries.Any(c => c.Name.ToLower() == defaultCountry))
                {
                    context.Countries.Add(new Country
                    {
                        Name = defaultCountry,
                        SearchKeywords = defaultCountry.AsSearchable()
                    });
                    context.SaveChanges();
                }
            }

            // Seed places
            Dictionary<string, string> placesDict = new Dictionary<string, string>();
            var placesreader = new CsvReader(new StreamReader(Path.Combine(dataPath, placesDataSource), Encoding.UTF8));
            while (placesreader.Read())
                placesDict[placesreader.CurrentRecord[0]] = placesreader[1].Trim().ToLower().Replace(". ", ".").Replace("sveti ", "sv.");
            if (seedPlaces)
            {
                using (var context = new ApplicationDbContext())
                {
                    Console.WriteLine("Seeding places...");

                    var commonCountry = context.Countries.FirstOrDefault();

                    int state = 0;
                    int totalState = placesDict.Count;
                    int width = 50;
                    int startTop = Console.CursorTop;
                    Console.CursorVisible = false;
                    foreach (var placeKvp in placesDict)
                    {
                        if (!context.Places.Any(p => p.Name.ToLower() == placeKvp.Value))
                        {
                            context.Places.Add(new Place
                            {
                                CountryId = commonCountry.Id,
                                Name = TitleCaseString(placeKvp.Value),
                                SearchKeywords = placeKvp.Value.AsSearchable()
                            });
                        }

                        state++;
                        Console.SetCursorPosition(0, startTop);
                        Console.Write("[");
                        for (int stateProgressIndex = 0; stateProgressIndex < width; stateProgressIndex++)
                        {
                            if (stateProgressIndex < width * (state / (double) totalState))
                                Console.Write("#");
                            else Console.Write(" ");
                        }
                        Console.Write("]");
                        Console.WriteLine("{0:0000}/{1:0000}", state, totalState);
                    }

                    Console.CursorVisible = true;
                    Console.WriteLine("Saving places... This might take a while.");
                    var changes = context.SaveChanges();
                    Console.WriteLine("Places done. {0} changes", changes);
                }
            }

            // Seed workers
            var workers = new List<string>()
            {
                "marko",
                "neven",
                "martin",
                "mario",
                "dino"
            };
            using (var context = new ApplicationDbContext())
            {
                foreach (var worker in workers)
                {
                    if (!context.Workers.Any(w => w.Name.ToLower() == worker))
                    {
                        context.Workers.Add(new Worker {Name = TitleCaseString(worker)});
                    }
                }
                var changes = context.SaveChanges();

                Console.WriteLine("Workers done. {0} changes", changes);
            }

            var reader = new CsvReader(new StreamReader(Path.Combine(dataPath, customersDataSource), Encoding.UTF8));
            int counter = 0;
            int progress = 0;
            int matchCounter = 0;
            var addressPlaces = new List<string>();
            var places = new List<string>();
            var statusTop = Console.CursorTop;
            Console.CursorVisible = false;
            while (reader.Read())
            {
                // Skip header
                if (counter++ == 0)continue;

                if (reader.CurrentRecord.Length != 20)
                {
                    //Console.WriteLine("ERROR: " + reader.Parser.RawRecord);
                }
                else
                {
                    var name = reader[1];
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        //Console.WriteLine("INVALID NAME: " + reader.Parser.RawRecord);
                        continue;
                    }

                    // Address
                    var address = reader[2].Trim();
                    var addressCorrected = address.ToLower().Replace(". ", ".").Replace("-", " ").Replace("??", "").Replace("sveti ", "sv.");

                    // Match place
                    var matchedPlace = string.Empty;
                    var addressPlace = addressCorrected.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()?.Trim();
                    var place = reader[3].Trim().ToLower().Replace(". ", ".").Replace("-", " ").Replace("??", "").Replace("sveti ", "sv.");
                    if (!string.IsNullOrWhiteSpace(place) && placesDict.ContainsKey(place))
                        place = placesDict[place];
                    var smallPlaceMatch = placesDict.Values.FirstOrDefault(p => addressPlace?.StartsWith(p) ?? false);
                    if (placesDict.ContainsValue(addressPlace))
                        matchedPlace = addressPlace;
                    else if (placesDict.ContainsValue(place))
                        matchedPlace = place;
                    else if (!string.IsNullOrEmpty(smallPlaceMatch))
                        matchedPlace = smallPlaceMatch;
                    var hasPlace = !string.IsNullOrWhiteSpace(matchedPlace);
                    if (!hasPlace)
                        matchCounter++;
                    
                    // Telephone 1
                    var tel1 = reader[7].Trim();
                    var hasTel1 = !string.IsNullOrWhiteSpace(tel1);

                    // Telephone 2
                    var tel2 = reader[8].Trim();
                    var hasTel2 = !string.IsNullOrWhiteSpace(tel2);

                    // Device name
                    var deviceName = reader[9].Trim();
                    var hasDevice = !string.IsNullOrWhiteSpace(deviceName);

                    // Device manufacturer
                    var hasDeviceManufacturer = false;
                    var deviceManuf = reader[10].Trim().Replace(",", "").ToLowerInvariant();
                    if (deviceManuf == "1" || deviceManuf == "v")
                        deviceManuf = "vaillant";
                    else if (deviceManuf == "2" || deviceManuf == "j")
                        deviceManuf = "junkers";
                    else if (deviceManuf == "3")
                        deviceManuf = "viessmann";
                    if (string.IsNullOrWhiteSpace(deviceManuf) || deviceManuf == "vaillant" || deviceManuf == "junkers" ||
                        deviceManuf == "viessmann" || deviceManuf == "bosch")
                        hasDeviceManufacturer = true;

                    // Commission date
                    var commissionDate = reader[12];
                    DateTime lastCommissionDate;
                    var hasLastCommissionDate = DateTime.TryParse(commissionDate, out lastCommissionDate);
                    
                    // Parse work items
                    var service = reader[14].Trim().Replace("&nbsp;", " ").Replace("\r", "").Replace("\n","").Replace("<div>","").Replace("</div>","\n").Trim();
                    var serviceLines = service.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim().TrimStart('-').Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
                    var lastStartIndex = 0;
                    DateTime? lastDate = null;
                    int lastPrice = 0;
                    var desc = string.Empty;
                    var worker = string.Empty;
                    var workItems = new List<Tuple<DateTime?, int, string, string>>();
                    for (int index = 0; index < serviceLines.Count; index++)
                    {
                        DateTime itemDate;
                        var isWorkItemStart = DateTime.TryParseExact(
                            (serviceLines[index].Split(' ').FirstOrDefault() ?? string.Empty).Trim('.'), 
                            "dd.MM.yy", 
                            new CultureInfo("hr"),
                            DateTimeStyles.AssumeLocal, out itemDate);
                        if (!isWorkItemStart)
                            isWorkItemStart=DateTime.TryParseExact(
                            (serviceLines[index].Split(' ').FirstOrDefault() ?? string.Empty).Trim('.'),
                            "dd.MM.yyyy",
                            new CultureInfo("hr"),
                            DateTimeStyles.AssumeLocal, out itemDate);

                        if (isWorkItemStart)
                        {
                            if (index != lastStartIndex)
                            {
                                workItems.Add(new Tuple<DateTime?, int, string, string>(lastDate, lastPrice, desc, worker));
                                lastPrice = 0;
                            }

                            // Try parse price
                            var split = serviceLines[index].Split(new[] {' ', '-'},
                                StringSplitOptions.RemoveEmptyEntries);
                            for (int priceIndex = 1; priceIndex < split.Length; priceIndex++)
                            {
                                var priceString = split.ElementAt(priceIndex).Replace(",", ".").Replace(" ", "").Trim();
                                double price;
                                var hasPrice = double.TryParse(priceString, out price);
                                if (hasPrice)
                                {
                                    lastPrice = (int) price;
                                    break;
                                }
                            }
                            for (int workerIndex = 1; workerIndex < split.Length; workerIndex++)
                            {
                                var currentCorrected = split[workerIndex].Trim().ToLower();
                                if (workers.Contains(currentCorrected))
                                {
                                    worker = workers.FirstOrDefault(w => w == currentCorrected);
                                    break;
                                }
                            }

                            lastStartIndex = index;
                            lastDate = itemDate;
                            desc = string.Empty;
                            //matchCounter++;
                        }

                        desc += serviceLines[index] + "\n";
                    }
                    if (!string.IsNullOrWhiteSpace(desc))
                        workItems.Add(new Tuple<DateTime?, int, string, string>(lastDate, lastPrice, desc, worker));

                    // Service date
                    var serviceDate = reader[15];
                    DateTime lastServiceDate;
                    var hasLastServiceDate = DateTime.TryParse(serviceDate, out lastServiceDate);

                    // Repair date
                    var repair = reader[18];
                    DateTime lastRepairDate;
                    var hasLastRepairDate = DateTime.TryParse(repair, out lastRepairDate);
                    
                    // Handle creation date
                    var creation = reader[16];
                    DateTime creationDate = DateTime.MaxValue;
                    if (!DateTime.TryParse(creation, out creationDate))
                    {
                        creationDate = DateTime.MaxValue;
                        if (hasLastCommissionDate && lastCommissionDate < creationDate)
                            creationDate = lastCommissionDate;
                        if (hasLastServiceDate && lastServiceDate < creationDate)
                            creationDate = lastServiceDate;
                        if (hasLastRepairDate && lastRepairDate < creationDate)
                            creationDate = lastRepairDate;
                        
                        if (creationDate == DateTime.MinValue)
                            Console.WriteLine("INVALID CREATION DATE" + creation);
                    }


                    Console.SetCursorPosition(0, statusTop);
                    Console.WriteLine(progress++);

                    // Instantiate new context
                    var context = new ApplicationDbContext();

                    // Create telephone numbers
                    List<TelephoneNumber> customerTelephoneNumbers = new List<TelephoneNumber>();
                    if (hasTel1 && tel1.Any(char.IsDigit))
                    {
                        var telephoneNumber1 = new TelephoneNumber
                        {
                            Number = tel1,
                            SearchKeywords =
                                tel1.Aggregate(string.Empty, (s, c) => s + (char.IsDigit(c) ? c.ToString() : "")).Trim()
                        };
                        customerTelephoneNumbers.Add(context.TelephoneNumbers.Add(telephoneNumber1));
                        context.SaveChanges();
                    }
                    if (hasTel2 && tel2.Any(char.IsDigit))
                    {
                        var telephoneNumber2 = new TelephoneNumber
                        {
                            Number = tel2,
                            SearchKeywords =
                                tel2.Aggregate(string.Empty, (s, c) => s + (char.IsDigit(c) ? c.ToString() : "")).Trim()
                        };
                        customerTelephoneNumbers.Add(context.TelephoneNumbers.Add(telephoneNumber2));
                        context.SaveChanges();
                    }

                    // Ensure address exists
                    var customerAddress = context.Addresses.FirstOrDefault(a => a.SearchKeywords == addressCorrected);
                    if (customerAddress == null)
                    {
                        var placeSearchable = place.AsSearchable();
                        if (string.IsNullOrWhiteSpace(addressCorrected))
                            addressCorrected = "Nepoznata adresa";
                        customerAddress = new Address
                        {
                            StreetAddress = TitleCaseString(addressCorrected),
                            SearchKeywords = addressCorrected.AsSearchable(),
                            PlaceId = hasPlace ? context.Places.FirstOrDefault(p => p.SearchKeywords == placeSearchable)?.Id : null
                        };
                        customerAddress = context.Addresses.Add(customerAddress);
                        context.SaveChanges();
                    }

                    // Create customer device
                    List<CustomerDevice> customerDevices = new List<CustomerDevice>();
                    if (hasDevice)
                    {
                        var customerDevice = new CustomerDevice
                        {
                            CommissionDate = hasLastCommissionDate ? new DateTime?(lastCommissionDate) : null,
                            Name = deviceName.ToUpper(),
                            Manufacturer = TitleCaseString(deviceManuf)
                        };
                        customerDevices.Add(context.CustomerDevices.Add(customerDevice));
                        context.SaveChanges();
                    }

                    // Create new customer
                    var customer = new Customer
                    {
                        CreationDate = creationDate,
                        Name = TitleCaseString(name),
                        TelephoneNumbers = customerTelephoneNumbers,
                        AddressId = customerAddress.Id,
                        ApplicationUserId = context.Users.FirstOrDefault()?.Id,  
                        CustomerDevices = customerDevices,
                        WorkItems = workItems.Select(wraw => new WorkItem
                        {
                            Date = wraw.Item1,
                            WorkerId = context.Workers.FirstOrDefault(w => w.Name.ToLower() == wraw.Item4)?.Id,
                            Description = wraw.Item3,
                            Price = wraw.Item2,
                            Type = WorkItemType.Unknown
                        }).ToList(),
                        SearchKeywords = name.ToLowerInvariant().Trim()
                    };

                    context.Customers.Add(customer);
                    context.SaveChanges();

                    context.Dispose();
                    // Construct something :D
                }
            }
            Console.WriteLine(counter);
            Console.WriteLine("Match count: " + matchCounter);
            Console.WriteLine("UNIQUE ADDRESS PLACES: " + addressPlaces.Distinct().Count());
            Console.WriteLine("UNIQUE PLACES: " + places.Distinct().Count());
            var totalLocations = places.Distinct().Union(addressPlaces.Distinct()).Distinct();
            Console.WriteLine("TOTAL UNIQUE LOCATIONS: " + totalLocations.Count());



            Console.ReadLine();
        }
    }
}
