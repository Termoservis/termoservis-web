using Serilog;

namespace Termoservis.Contracts.Services
{
	/// <summary>
	/// The logging service.
	/// </summary>
	public interface ILoggingService
	{
		/// <summary>
		/// Gets the logger.
		/// </summary>
		/// <typeparam name="T">The logger context.</typeparam>
		/// <returns>Returns new instance of logger for given context.</returns>
		ILogger GetLogger<T>();
	}
}