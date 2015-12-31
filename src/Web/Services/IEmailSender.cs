using System.Threading.Tasks;

namespace Web.Services
{
	/// <summary>
	/// Email sender.
	/// </summary>
	public interface IEmailSender
    {
		/// <summary>
		/// Sends the email from Termoservis Portal do-not-reply account.
		/// </summary>
		/// <param name="email">The destination email address.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="message">The message.</param>
		/// <returns>Returns the awaitable task for sending the email.</returns>
		Task SendEmailAsync(string email, string subject, string message);
    }
}
