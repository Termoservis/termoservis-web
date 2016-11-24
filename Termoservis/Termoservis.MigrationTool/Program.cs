using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace Termoservis.MigrationTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new CsvReader(new StreamReader("C:\\Users\\aleks\\Desktop\\termoserviskorisnici.csv", Encoding.UTF8));
            int counter = 0;
            int matchCounter = 0;
            var addressPlaces = new List<string>();
            var places = new List<string>();
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

                    
                    var address = reader[2].Trim();
                    var addressCorrected = address.ToLower().Replace(". ", ".").Replace("-", " ").Replace("??", "").Replace("sveti ", "sv.");
                    var addressPlace = addressCorrected.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()?.Trim();
                    if (addressPlace != null && !string.IsNullOrWhiteSpace(addressPlace) && addressPlace != addressCorrected && !addressPlace.Any(char.IsDigit) && !addressPlace.Contains("bb"))
                    {
                        addressPlaces.Add(addressPlace);
                        Console.WriteLine(addressPlace); //matchCounter++;
                        matchCounter++;
                        if (matchCounter % 20 == 0)
                        {
                            Console.WriteLine("------------- " + matchCounter);
                            //Console.ReadKey();
                        }
                    }

                    var place = reader[3].Trim().ToLower().Replace(". ", ".").Replace("-", " ").Replace("??", "").Replace("sveti ", "sv.");
                    if (!string.IsNullOrWhiteSpace(place))
                        places.Add(place);

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
                    
                    var service = reader[14];


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
                    DateTime creationDate;
                    if (!DateTime.TryParse(creation, out creationDate))
                    {
                        // TODO Handle no creation date
                        Console.WriteLine("INVALID CREATION DATE" + creation);
                    }
                    //Console.WriteLine(name);
                }
            }
            Console.WriteLine(counter);
            Console.WriteLine(matchCounter);
            Console.WriteLine("UNIQUE ADDRESS PLACES: " + addressPlaces.Distinct().Count());
            Console.WriteLine("UNIQUE PLACES: " + places.Distinct().Count());
            var totalLocations = places.Distinct().Union(addressPlaces.Distinct()).Distinct();
            Console.WriteLine("TOTAL UNIQUE LOCATIONS: " + totalLocations.Count());



            Console.ReadLine();
        }
    }
}
