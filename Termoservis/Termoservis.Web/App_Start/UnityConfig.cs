using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Unity;
using Termoservis.Contracts.Services;
using Termoservis.DAL;
using Termoservis.DAL.Repositories;
using Termoservis.Models;
using Termoservis.Web.Services;
using Termoservis.BLL;
using Unity.AspNet.Mvc;
using Unity.Injection;

namespace Termoservis.Web
{
	/// <summary>
	/// Specifies the Unity configuration for the main container.
	/// </summary>
	// ReSharper disable once ClassNeverInstantiated.Global
	public class UnityConfig
    {  
        /// <summary>
        /// The container
        /// </summary>
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
	        container.RegisterType<ILoggingService, LoggingService>(new PerRequestLifetimeManager());
			container.RegisterType<ApplicationDbContext>();
			container.RegisterType<ApplicationSignInManager>();
			container.RegisterType<ApplicationUserManager>();
			container.RegisterType<IAuthenticationManager>(
				new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
			container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
		        new InjectionConstructor(typeof(ApplicationDbContext)));

			// Register repositories
	        container.RegisterType<ICountriesRepository, CountriesRepository>();
			container.RegisterType<IPlacesRepository, PlacesRepository>();
	        container.RegisterType<IAddressesRepository, AddressesRepository>();
			container.RegisterType<ITelephoneNumbersRepository, TelephoneNumbersRepository>();
			container.RegisterType<ICustomersRepository, CustomersRepository>();
            container.RegisterType<ICustomerDevicesRepository, CustomerDevicesRepository>();
            container.RegisterType<IWorkItemsRepository, WorkItemsRepository>();

            // Register services
            container.RegisterType<ICustomerService, CustomerService>();
        }
    }
}
