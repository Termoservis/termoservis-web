using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Termoservis.BLL;
using Termoservis.Contracts.Services;
using Termoservis.DAL;
using Termoservis.DAL.Repositories;
using Termoservis.Models;
using Termoservis.Web.Services;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;

namespace Termoservis.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container

        private static readonly Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
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
            return container.Value;
        }

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;

        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ILoggingService, LoggingService>(new PerRequestLifetimeManager());
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterFactory<IAuthenticationManager>(
                c => HttpContext.Current.GetOwinContext().Authentication);
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
