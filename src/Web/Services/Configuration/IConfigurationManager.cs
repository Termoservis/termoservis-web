using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Web.Services.Configuration
{
	/// <summary>
	/// Configuration provider.
	/// </summary>
	public class ConfigurationProvider : IConfigurationProvider
	{
		/// <summary>
		/// Gets the root.
		/// </summary>
		/// <value>
		/// The root.
		/// </value>
		public IConfigurationRoot Root { get; }

		/// <summary>
		/// Gets the configuration data.
		/// </summary>
		/// <value>
		/// The configuration data.
		/// </value>
		public ConfigurationData Data { get; }


		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationProvider"/> class.
		/// </summary>
		/// <param name="root">The configuration root.</param>
		/// <exception cref="System.ArgumentNullException">
		/// root
		/// </exception>
		public ConfigurationProvider(IConfigurationRoot root)
		{
			if (root == null) throw new ArgumentNullException(nameof(root));

			this.Root = root;

			// Initialize children
			this.Data = new ConfigurationData(this);
		}
	}

	/// <summary>
	/// Configuration node.
	/// </summary>
	public interface IConfigurationNode
	{
		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>
		/// The parent.
		/// </value>
		IConfigurationNode Parent { get; }

		/// <summary>
		/// Gets the namespace.
		/// </summary>
		/// <value>
		/// The namespace.
		/// </value>
		string Namespace { get; }

		/// <summary>
		/// Gets the full namespace.
		/// </summary>
		/// <value>
		/// The full namespace.
		/// </value>
		string FullNamespace { get; }
	}

	/// <summary>
	/// Configuration node base implementation.
	/// </summary>
	public abstract class ConfigurationNodeBase : IConfigurationNode
	{
		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>
		/// The parent.
		/// </value>
		public IConfigurationNode Parent { get; }

		/// <summary>
		/// Gets the namespace.
		/// </summary>
		/// <value>
		/// The namespace.
		/// </value>
		public string Namespace { get; }

		/// <summary>
		/// Gets the full namespace.
		/// </summary>
		/// <value>
		/// The full namespace.
		/// </value>
		public string FullNamespace => Combine(this.Parent?.FullNamespace ?? string.Empty, this.Namespace);


		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationNode"/> class.
		/// </summary>
		/// <param name="ns">The ns.</param>
		/// <param name="parent">The parent.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		protected ConfigurationNodeBase(string ns, IConfigurationNode parent)
		{
			if (ns == null) throw new ArgumentNullException(nameof(ns));

			this.Namespace = ns;
			this.Parent = parent;
		}

		/// <summary>
		/// Combines child on itself.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <returns>Returns the combination of this instances full namespace and children.</returns>
		public string CombineOnSelf(string child)
		{
			return Combine(this.FullNamespace, child);
		}

		/// <summary>
		/// Combines the specified parent with child.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="child">The child.</param>
		/// <returns>Returns the combination of parent and child separated with ':'.</returns>
		public static string Combine(string parent, string child)
		{
			if (string.IsNullOrEmpty(parent))
				return child;
			return parent + ":" + child;
		}
	}

	/// <summary>
	/// The configuration provider.
	/// </summary>
	public interface IConfigurationProvider
	{
		/// <summary>
		/// Gets the configuration data.
		/// </summary>
		/// <value>
		/// The configuration data.
		/// </value>
		ConfigurationData Data { get; }

		/// <summary>
		/// Gets the root.
		/// </summary>
		/// <value>
		/// The root.
		/// </value>
		IConfigurationRoot Root { get; }
	}

	/// <summary>
	/// Configuration data.
	/// </summary>
	public class ConfigurationData : ConfigurationNodeBase
	{
		private readonly IConfigurationProvider provider;

		/// <summary>
		/// Gets the default connection configuration data.
		/// </summary>
		/// <value>
		/// The default connection configuration data.
		/// </value>
		public ConfigurationDataDefaultConnection DefaultConnection { get; }

		/// <summary>
		/// Gets the users configuration data.
		/// </summary>
		/// <value>
		/// The users configuration data.
		/// </value>
		public ConfigurationDataUser Users { get; }


		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationData" /> class.
		/// </summary>
		/// <param name="provider">The configuration provider.</param>
		public ConfigurationData(IConfigurationProvider provider)
					: base("Data", null)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));

			this.provider = provider;

			// Initialize children
			this.DefaultConnection = new ConfigurationDataDefaultConnection(this.provider, this);
			this.Users = new ConfigurationDataUser(this.provider, this);
		}
	}

	/// <summary>
	/// Default connection configuration data.
	/// </summary>
	public class ConfigurationDataDefaultConnection : ConfigurationNodeBase
	{
		private readonly IConfigurationProvider provider;

		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <value>
		/// The connection string.
		/// </value>
		public string ConnectionString => 
			this.provider.Root[this.CombineOnSelf("ConnectionString")];

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationDataDefaultConnection" /> class.
		/// </summary>
		/// <param name="provider">The configuration provider.</param>
		/// <param name="parent">The parent.</param>
		public ConfigurationDataDefaultConnection(IConfigurationProvider provider, IConfigurationNode parent)
					: base("DefaultConnection", parent)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));

			this.provider = provider;
		}
	}

	/// <summary>
	/// Users configuration data.
	/// </summary>
	public class ConfigurationDataUser : ConfigurationNodeBase
	{
		private readonly IConfigurationProvider provider;

		/// <summary>
		/// Gets the allowed domains.
		/// </summary>
		/// <value>
		/// The allowed domains.
		/// </value>
		public IEnumerable<string> AllowedDomains =>
			this.provider.Root[this.CombineOnSelf("AllowedDomains")]
				.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationDataUser" /> class.
		/// </summary>
		/// <param name="provider">The configuration provider.</param>
		/// <param name="parent">The parent.</param>
		public ConfigurationDataUser(IConfigurationProvider provider, IConfigurationNode parent)
					: base("Users", parent)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));

			this.provider = provider;
		}
	}
}
