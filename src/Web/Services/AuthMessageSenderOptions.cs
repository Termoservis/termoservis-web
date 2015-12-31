using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services
{
	/// <summary>
	/// Authentication message sender options.
	/// </summary>
	public class AuthMessageSenderOptions
    {
		/// <summary>
		/// Gets or sets the SendGrid user.
		/// </summary>
		/// <value>
		/// The SendGrid user.
		/// </value>
		public string SendGridUser { get; set; }

		/// <summary>
		/// Gets or sets the SendGrid key.
		/// </summary>
		/// <value>
		/// The SendGrid key.
		/// </value>
		public string SendGridKey { get; set; }
    }
}
