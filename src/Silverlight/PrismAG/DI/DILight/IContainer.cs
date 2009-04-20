using System;

namespace DILight
{
    /// <summary>
    /// The interface for a simple DI container for Silverlight.
    /// </summary>
    public interface IContainer
    {
        #region Public Methods
        /************************************************
		 * Public Methods
		 ***********************************************/
        /// <summary>
        /// Registers a default instance with the container.
        /// </summary>
        /// <typeparam name="TInterface">
        /// The <see cref="Type" /> of instance to register.
        /// </typeparam>
        /// <param name="instance">
        /// The object instance to be returned.
        /// </param>
        void RegisterInstance<TInterface>(TInterface instance);

        /// <summary>
        /// Registers a default instance with the container.
        /// </summary>
        /// <param name="t">
        /// The <see cref="Type" /> of instance to register.
        /// </param>
        /// <param name="instance">
        /// The object instance to be returned.
        /// </param>
        void RegisterInstance(Type t, object instance);

        /// <summary>
        /// Registers a named instance with the container.
        /// </summary>
        /// <param name="t">
        /// The <see cref="Type" /> of instance to register.
        /// </param>
        /// <param name="name">
        /// The name of the instance to be returned.
        /// </param>
        /// <param name="instance">
        /// The instance to return.
        /// </param>
        void RegisterInstance<TInterface>(string name, TInterface instance);

        /// <summary>
        /// Registers a named instance with the container.
        /// </summary>
        /// <param name="t">
        /// The <see cref="Type" /> of instance to register.
        /// </param>
        /// <param name="name">
        /// The name of the instance.
        /// </param>
        /// <param name="instance">
        /// The object instance to be returned.
        /// </param>
        void RegisterInstance(Type t, string name, object instance);

        /// <summary>
        /// Registers a named <see cref="Type"/> within the container.
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="Type" /> of instance to create.
        /// </typeparam>
        /// <param name="name">
        /// The name that will be used to resolve an instance of <paramref name="T"/>.
        /// </param>
        void RegisterType<T>(string name);

        /// <summary>
        /// Registers a named <see cref="Type"/> within the container.
        /// </summary>
        /// <param name="t">
        /// The <see cref="Type" /> of instance to create.
        /// </param>
        /// <param name="name">
        /// The name that will be used to resolve an instance of <paramref name="T"/>.
        /// </param>
        void RegisterType(Type t, string name);

        /// <summary>
        /// Register a default type mapping within the container.
        /// </summary>
        /// <typeparam name="TFrom">
        /// The <see cref="System.Type"/> that will be requested.
        /// </typeparam>
        /// <typeparam name="TTo">
        /// The <see cref="System.Type"/> that will be requested.
        /// </typeparam>
        void RegisterType<TFrom, TTo>() where TTo : TFrom;

        /// <summary>
        /// Registers a default type mapping with the container.
        /// </summary>
        /// <param name="from">
        /// The <see cref="System.Type"/> that will be requested.
        /// </param>
        /// <param name="to">
        /// The <see cref="System.Type"/> that will be requested.
        /// </param>
        void RegisterType(Type from, Type to);

        /// <summary>
        /// Registers a named type mapping with the container.
        /// </summary>
        /// <typeparam name="TFrom">
        /// The <see cref="System.Type"/> that will be requested.
        /// </typeparam>
        /// <typeparam name="TTo">
        /// The <see cref="System.Type"/> that will be requested.
        /// </typeparam>
        /// <param name="name">
        /// The name of the mapping.
        /// </param>
        void RegisterType<TFrom, TTo>(string name) where TTo : TFrom;


        /// <summary>
        /// Registers a named type mapping with the container.
        /// </summary>
        /// <param name="from">
        /// The <see cref="System.Type"/> that will be requested.
        /// </param>
        /// <param name="to">
        /// The <see cref="System.Type"/> that will be requested.
        /// </param>
        /// <param name="name">
        /// The name of the mapping.
        /// </param>
        void RegisterType(Type from, Type to, string name);

        /// <summary>
        /// Resolves the default instance of the requested type from the container.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="Type"/> of object to get from the container.
        /// </typeparam>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves the default instance of requested type from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        object Resolve(Type t);

        /// <summary>
        /// Resolves the named instance from the container.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="Type"/> of object to get from the container.
        /// </typeparam>
        /// <param name="name">
        /// The named of the instance to resolve.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        T Resolve<T>(string name);

        /// <summary>
        /// Resolves the named instance from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <param name="name">
        /// The named of the instance to resolve.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        object Resolve(Type t, string name);

        /// <summary>
        /// Resolves the default instance of the requested type from the container.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="Type"/> of object to get from the container.
        /// </typeparam>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        T TryResolve<T>();

        /// <summary>
        /// Resolves the default instance of requested type from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        object TryResolve(Type t);

        /// <summary>
        /// Resolves the named instance from the container.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="Type"/> of object to get from the container.
        /// </typeparam>
        /// <param name="name">
        /// The named of the instance to resolve.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        T TryResolve<T>(string name);

        /// <summary>
        /// Resolves the named instance from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <param name="name">
        /// The named of the instance to resolve.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        object TryResolve(Type t, string name);
        #endregion // Public Methods
    }
}
