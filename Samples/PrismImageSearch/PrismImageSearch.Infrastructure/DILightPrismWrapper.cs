using System;
using DILight;
using Prism.Interfaces;

namespace PrismImageSearch.Infrastructure
{
    /// <summary>
    /// Wraps a DILight <see cref="IContainer"/> to allow it to function as a Prism container.
    /// </summary>
    public class DILightPrismWrapper : IPrismContainer
    {
        private IContainer container;

        public DILightPrismWrapper(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        #region IPrismContainer Members

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }

        public object TryResolve(Type type)
        {
            return container.TryResolve(type);
        }

        #endregion
    }
}
