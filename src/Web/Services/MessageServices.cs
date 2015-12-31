using System.Threading.Tasks;
using Microsoft.Extensions.OptionsModel;

namespace Web.Services
{
	/// <summary>
	/// Authentication message sender.
	/// </summary>
	/// <seealso cref="Web.Services.IEmailSender" />
	// ReSharper disable once ClassNeverInstantiated.Global
	public class AuthMessageSender : IEmailSender
    {
	    private readonly AuthMessageSenderOptions options;


		/// <summary>
		/// Initializes a new instance of the <see cref="AuthMessageSender"/> class.
		/// </summary>
		/// <param name="optionsAccessor">The options accessor.</param>
		public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
		{
			this.options = optionsAccessor.Value;
		}


		/// <summary>
		/// Sends the email from Termoservis Portal do-not-reply account.
		/// </summary>
		/// <param name="email">The destination email address.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="message">The message.</param>
		/// <returns>Returns the awaitable task for sending the email.</returns>
		public Task SendEmailAsync(string email, string subject, string message)
        {
			// Create and populate message
			var myMessage = new SendGrid.SendGridMessage();
			myMessage.AddTo(email);
			myMessage.From = new System.Net.Mail.MailAddress("donotreply@termoservis.hr", "Termoservis Portal");
			myMessage.Subject = subject;
			myMessage.Text = message;
			myMessage.Html = message;

			// Populate SendGrid credentials
			var credentials = new System.Net.NetworkCredential(
				this.options.SendGridUser,
				this.options.SendGridKey);

			// Create a Web transport for sending email.
			var transportWeb = new SendGrid.Web(credentials);

			// Send the email.
			return transportWeb.DeliverAsync(myMessage);
        }
    }
}
