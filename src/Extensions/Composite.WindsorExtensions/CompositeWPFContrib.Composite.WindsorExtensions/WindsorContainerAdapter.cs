using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace CompositeWPFContrib.Composite.WindsorExtensions
{
	/// <summary>
	/// Adapts the behavior of the Windsor container to the common
	/// IServiceLocator
	/// </summary>
	public class WindsorServiceLocatorAdapter : ServiceLocatorImplBase
	{
		private readonly IWindsorContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindsorServiceLocatorAdapter"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public WindsorServiceLocatorAdapter(IWindsorContainer container)
		{
			this.container = container;
		}

		/// <summary>
		///             When implemented by inheriting classes, this method will do the actual work of resolving
		///             the requested service instance.
		/// </summary>
		/// <param name="serviceType">Type of instance requested.</param>
		/// <param name="key">Name of registered service you want. May be null.</param>
		/// <returns>
		/// The requested service instance.
		/// </returns>
		protected override object DoGetInstance(Type serviceType, string key)
		{
			//TODO odd patch I know, drop this when castle release windsor and core.
			
			if(!container.Kernel.HasComponent(serviceType))
			{
				if(!string.IsNullOrEmpty(key ))
				{
					container.Kernel.AddComponent(key, serviceType, LifestyleType.Transient);
				}else
				{
					container.Register(Component.For(serviceType).LifeStyle.Transient);
				}
			}

			if (key != null)
				return container.Resolve(key, serviceType);
			return container.Resolve(serviceType);
		}

		/// <summary>
		///             When implemented by inheriting classes, this method will do the actual work of
		///             resolving all the requested service instances.
		/// </summary>
		/// <param name="serviceType">Type of service requested.</param>
		/// <returns>
		/// Sequence of service instance objects.
		/// </returns>
		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return (object[])container.ResolveAll(serviceType);
		}
	}
}
