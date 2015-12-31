using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;
using Web.Validators;
using IConfigurationProvider = Web.Services.Configuration.IConfigurationProvider;

namespace Web
{
	public class Startup
    {
		/// <summary>
		/// Gets or sets the configuration.
		/// </summary>
		/// <value>
		/// The configuration.
		/// </value>
		public IConfigurationRoot Configuration { get; }

		/// <summary>
		/// Gets the configuration provider.
		/// </summary>
		/// <value>
		/// The configuration provider.
		/// </value>
		public IConfigurationProvider ConfigurationProvider { get; }

		/// <summary>
		/// Handles application startup and hosting environment preparation.
		/// </summary>
		/// <param name="env">The hosting environment.</param>
		public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			// Add user secrets to the configuration
	        if (env.IsDevelopment())
		        builder.AddUserSecrets();

			// Add environment variables
	        builder.AddEnvironmentVariables();

			// Build the configuration
            Configuration = builder.Build();

			// Instantiate configuration provider 
			this.ConfigurationProvider = new Services.Configuration.ConfigurationProvider(this.Configuration);

        }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services">The services collection.</param>
		// ReSharper disable once UnusedMember.Global
		public void ConfigureServices(IServiceCollection services)
        {
			// Register configuration instance and configuration provider
			services.AddInstance(this.Configuration);
			services.AddInstance(this.ConfigurationProvider);

            // Register Entity Framework
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(this.ConfigurationProvider.Data.DefaultConnection.ConnectionString));

			// Register Identity Framework
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
				.AddUserValidator<TermoservisUserValidator>();

			// Register MVC 
            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();

			// Configure
			services.Configure<AuthMessageSender>(this.Configuration);
        }

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app">The application builder.</param>
		/// <param name="env">The hosting environment.</param>
		/// <param name="loggerFactory">The logger factory.</param>
		// ReSharper disable once UnusedMember.Global
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
			// Configure logger
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

			if (env.IsDevelopment())
            {
				// Configure debugging tools
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
	            app.UseRuntimeInfoPage();
            }
            else
            {
				// Add exception handling to the pipeline
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
	            try
	            {
		            // Run migrations in production
		            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			            serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
	            }
	            catch(Exception ex)
	            {
		            loggerFactory.CreateLogger<Startup>().LogError("Failed to run production migrations.", ex);
	            }
            }

			// Add IIS level handler to the pipeline
            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());
			
			// Add Static files hosting to the pipeline
            app.UseStaticFiles();

			// Add Identity Framework to the pipeline
            app.UseIdentity();
			
			// Configure MVC routing
            app.UseMvc(routes =>
            {
				// Areas support
	            routes.MapRoute(
		            name: "areaRoute",
		            template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

				// Default routing
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

		/// <summary>
		/// Entry point for the application.
		/// </summary>
		/// <param name="args">The application start arguments.</param>
		// ReSharper disable once UnusedMember.Global
		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
