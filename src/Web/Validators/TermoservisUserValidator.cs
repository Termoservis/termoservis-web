using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Web.Models;
using IConfigurationProvider = Web.Services.Configuration.IConfigurationProvider;

namespace Web.Validators
{
	/// <summary>
	/// User validator for Termoservis domain.
	/// </summary>
	public class TermoservisUserValidator : UserValidator<ApplicationUser>
	{
		private readonly IConfigurationProvider configuration;
		private readonly ILogger<TermoservisUserValidator> logger;


		/// <summary>
		/// Initializes a new instance of the <see cref="TermoservisUserValidator"/> class.
		/// </summary>
		/// <param name="configuration">The configuration provider.</param>
		/// <param name="logger">The logger.</param>
		/// <exception cref="ArgumentNullException">
		/// configuration
		/// or
		/// logger
		/// </exception>
		public TermoservisUserValidator(IConfigurationProvider configuration, ILogger<TermoservisUserValidator> logger)
		{
			if (configuration == null) throw new ArgumentNullException(nameof(configuration));
			if (logger == null) throw new ArgumentNullException(nameof(logger));

			this.configuration = configuration;
			this.logger = logger;
		}


		/// <summary>
		/// Validates the user.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="user">The user.</param>
		/// <returns>Returns the <see cref="IdentityResult"/> containing errors if there were any.</returns>
		public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
		{
			// Validate with default user validator
			var baseResult = await base.ValidateAsync(manager, user);

			// Check if base result passed, if it didn't - don't validate further
			if (!baseResult.Succeeded)
				return baseResult;

			// Validate email domain
			var errors = new List<IdentityError>();
			await this.ValidateEmailDomain(manager, user, errors);
			var result = IdentityResult.Failed(errors.ToArray());

			return result;
		}

		/// <summary>
		/// Validates the email domain.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="user">The user.</param>
		/// <param name="errors">The errors.</param>
		/// <exception cref="ArgumentNullException">
		/// manager
		/// or
		/// user
		/// or
		/// errors
		/// </exception>
		/// <exception cref="FormatException">Provided email address is not white-listed. User registration forbidden.</exception>
		private async Task ValidateEmailDomain(UserManager<ApplicationUser> manager, ApplicationUser user, ICollection<IdentityError> errors)
		{
			if (manager == null) throw new ArgumentNullException(nameof(manager));
			if (user == null) throw new ArgumentNullException(nameof(user));
			if (errors == null) throw new ArgumentNullException(nameof(errors));

			// Try to retrieve email address from user
			var email = await manager.GetEmailAsync(user);
			if (string.IsNullOrWhiteSpace(email))
			{
				errors.Add(this.Describer.InvalidEmail(email));
				return;
			}

			// Try to parse as mail address
			try
			{
				// Retrieve email and email domain
				var mailAddress = new MailAddress(email);
				var formattedDomain = mailAddress.Host.Trim().ToLower(CultureInfo.InvariantCulture);

				// Check if given email address domain is white-listed
				var isDomainValid = this.GetWhiteListedDomains().Contains(formattedDomain);

				// If domain is invalid, throw format exception
				if (!isDomainValid)
					throw new FormatException("Provided email address is not white-listed. User registration forbidden.");
			}
			catch (FormatException ex)
			{
				this.logger.LogWarning("Format exception while validating new users email address.", ex);
				errors.Add(this.Describer.InvalidEmail(email));
			}
		}

		/// <summary>
		/// Gets the white listed domains.
		/// </summary>
		/// <returns>Returns the collection of white-listed email domains for new user email validation.</returns>
		private IEnumerable<string> GetWhiteListedDomains()
		{
			return this.configuration.Data.Users.AllowedDomains;
		} 
	}
}