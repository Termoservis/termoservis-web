using Serilog;
using Termoservis.Contracts.Services;

namespace Termoservis.Web.Services
{
	/// <summary>
	/// The logging service.
	/// </summary>
	/// <seealso cref="ILoggingService" />
	// ReSharper disable once ClassNeverInstantiated.Global
	public class LoggingService : ILoggingService
	{
		private static readonly object LoggerLocker = new object();
		private ILogger logger;


		/// <summary>
		/// Gets the logger.
		/// </summary>
		/// <typeparam name="T">The logger context.</typeparam>
		/// <returns>
		/// Returns new instance of logger for given context.
		/// </returns>
		public ILogger GetLogger<T>()
		{
			this.InitializeLogger();
			return this.logger.ForContext<T>();
		}

		/// <summary>
		/// Initializes the logger.
		/// </summary>
		private void InitializeLogger()
		{
			if (this.logger != null)
				return;

			lock (LoggerLocker)
			{
				if (this.logger == null)
					this.logger = ConfigureLogger();
			}
		}

		/// <summary>
		/// Configures the logger.
		/// </summary>
		/// <returns>Returns configured logger.</returns>
		private static ILogger ConfigureLogger()
		{
			return new LoggerConfiguration()
				.WriteTo.ColoredConsole()
				.CreateLogger();
		}
	}
}