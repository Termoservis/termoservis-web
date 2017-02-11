using System;
using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Mindscape.Raygun4Net;
using Termoservis.DAL;
using Termoservis.DAL.Migrations;
using Termoservis.Web.ModelBinders;
using WebGrease.Configuration;

namespace Termoservis.Web
{
    /// <summary>
    /// The MVC application entry-point.
    /// </summary>
    /// <seealso cref="HttpApplication" />
    public class MvcApplication : HttpApplication, IRaygunApplication
    {
        private static readonly RaygunClient RaygunClient = new RaygunClient();

		/// <summary>
		/// Application starting point.
		/// </summary>
		protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Migrate database
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());

            // Handle raygun message
            RaygunClient.SendingMessage += RaygunClientOnSendingMessage;

            // Model binders
            var dateTimeBinder = new DateTimeModelBinder("dd.MM.yyyy");
            ModelBinders.Binders.Add(typeof(DateTime), dateTimeBinder);
            ModelBinders.Binders.Add(typeof(DateTime?), dateTimeBinder);
        }

        /// <summary>
        /// Handles the raygun client sending message.
        /// This will set any additional data to the message and ignore the local exceptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RaygunSendingMessageEventArgs"/> instance containing the event data.</param>
        private static void RaygunClientOnSendingMessage(object sender, RaygunSendingMessageEventArgs e)
        {
            SetApplicationVersion(e);
            CancelLocalhostExceptions(e);
        }

        /// <summary>
        /// Cancels the localhost exceptions for Raygun message.
        /// </summary>
        /// <param name="e">The <see cref="RaygunSendingMessageEventArgs"/> instance containing the event data.</param>
        private static void CancelLocalhostExceptions(RaygunSendingMessageEventArgs e)
        {
            if (e.Message.Details.Request.HostName == "localhost")
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Sets the application version to Raygun message.
        /// </summary>
        /// <param name="e">The <see cref="RaygunSendingMessageEventArgs"/> instance containing the event data.</param>
        private static void SetApplicationVersion(RaygunSendingMessageEventArgs e)
        {
            try
            {
                e.Message.Details.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception)
            {
                // Failed to set application version
            }
        }

        /// <summary>
        /// Generates the raygun client.
        /// </summary>
        /// <returns>Returns the Raygun client.</returns>
        public RaygunClient GenerateRaygunClient()
        {
            return RaygunClient;
        }
    }
}
