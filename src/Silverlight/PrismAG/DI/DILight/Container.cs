using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Reflection;

namespace DILight
{
    /// <summary>
    /// A simple DI container for Silverlight.
    /// </summary>
    public class Container : IContainer
    {
        #region Member Variables
        /************************************************
		 * Member Variables
		 ***********************************************/
        private Dictionary<Type, object> instances = new Dictionary<Type, object>();
        private Dictionary<Type, Dictionary<string, object>> namedInstances = new Dictionary<Type, Dictionary<string, object>>();
        private Dictionary<Type, Type> typeMappings = new Dictionary<Type, Type>();
        private Dictionary<string, Type> namedTypes = new Dictionary<string, Type>();
        private Dictionary<Type, Dictionary<string, Type>> namedTypeMappings = new Dictionary<Type, Dictionary<string,Type>>();
        #endregion // Member Variables

        #region Internal Methods
        /************************************************
		 * Internal Methods
		 ***********************************************/
        /// <summary>
        /// Creates an instance using dependency injection.
        /// </summary>
        /// <param name="t">
        /// The type of instance to create.
        /// </param>
        /// <param name="name">
        /// An optional name to use to resolve type mappings.
        /// </param>
        /// <param name="throwIfNotCreatable">
        /// <c>true</c> if an exception should be thrown if the type is not creatable; otherwise 
        /// <c>false</c>. If false, <see langword="null"/> will be returned instead.
        /// </param>
        /// <returns>
        /// The created instance or <see langword="null"/> if the instance couldn't be created.
        /// </returns>
        private object CreateInstanceDI(Type t, string name, bool throwIfNotCreatable)
        {
            // See if there is a type map that we should use
            if (!string.IsNullOrEmpty(name))
            {
                if (namedTypeMappings.ContainsKey(t))
                {
                    if (namedTypeMappings[t].ContainsKey(name))
                    {
                        // Replace with mapped type.
                        t = namedTypeMappings[t][name];
                    }
                }
            }
            else
            {
                if (typeMappings.ContainsKey(t))
                {
                    // Replace with mapped type.
                    t = typeMappings[t];
                }
            }

            // Make sure we can create it.
            if (!t.IsCreatable())
            {
                if (throwIfNotCreatable)
                {
                    throw new InvalidOperationException(string.Format(ExceptionStrings.TypeNotCreatableNamed, t));
                }
                else
                {
                    return null;
                }
            }

            // Get all constructor params
            ConstructorInfo[] constructors = t.GetConstructors();
            
            // Make sure there's at least one
            if (constructors.Length < 1)
            {
                throw new InvalidOperationException(string.Format(ExceptionStrings.NoConstructorsNamed, t));
            }
            
            // Just use the first one
            ConstructorInfo constructor = constructors[0];

            // Now get all the parameters
            ParameterInfo[] parameters = constructor.GetParameters();

            // Create value placeholder for parameters
            object[] paramvalues = new object[parameters.Length];

            // If parameters are required, buid them up
            for (int i = 0; i < paramvalues.Length; i++)
            {
                // Resolve parameter
                paramvalues[i] = Resolve(parameters[i].ParameterType);
            }

            // Create and return instance
            return Activator.CreateInstance(t, paramvalues);
        }

        /// <summary>
        /// Get an instance of the default requested type from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <param name="throwIfNotResolvable">
        /// <c>true</c> if an exception should be thrown if the type is not resolvable; otherwise 
        /// <c>false</c>. If false, <see langword="null"/> will be returned instead.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        private object Resolve(Type t, bool throwIfNotResolvable)
        {
            // Validate
            if (t == null) throw new ArgumentNullException("t");

            // Check for instance
            if (instances.ContainsKey(t))
            {
                return instances[t];
            }

            // Create instance using DI
            return CreateInstanceDI(t, null, throwIfNotResolvable);
        }

        /// <summary>
        /// Resolves the named instance from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <param name="name">
        /// The named of the instance to resolve.
        /// </param>
        /// <param name="throwIfNotResolvable">
        /// <c>true</c> if an exception should be thrown if the type is not resolvable; otherwise 
        /// <c>false</c>. If false, <see langword="null"/> will be returned instead.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        private object Resolve(Type t, string name, bool throwIfNotResolvable)
        {
            // Validate
            if (t == null) throw new ArgumentNullException("t");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

            // Type key might not exist
            if (namedInstances.ContainsKey(t))
            {
                if (namedInstances[t].ContainsKey(name))
                {
                    return namedInstances[t][name];
                }
            }

            // Try to create with named mapping
            return CreateInstanceDI(t, name, throwIfNotResolvable);
        }
        #endregion // Internal Methods

        #region Public Methods
        /************************************************
		 * Public Methods
		 ***********************************************/
        /// <summary>
        /// Registers an instance with the container.
        /// </summary>
        /// <typeparam name="TInterface">
        /// The <see cref="Type" /> of instance to register.
        /// </typeparam>
        /// <param name="instance">
        /// The object instance to be returned.
        /// </param>
        public void RegisterInstance<TInterface>(TInterface instance)
        {
            RegisterInstance(typeof(TInterface), instance);
        }

        /// <summary>
        /// Registers an instance with the container.
        /// </summary>
        /// <param name="t">
        /// The <see cref="Type" /> of instance to register.
        /// </param>
        /// <param name="instance">
        /// The object instance to be returned.
        /// </param>
        public void RegisterInstance(Type t, object instance)
        {
            // Must not be null
            if (t == null) throw new ArgumentNullException("t");
            if (instance == null) throw new ArgumentNullException("instance");

            // Must inherit
            if (!t.IsAssignableFrom(instance.GetType()))
            {
                throw new InvalidOperationException(ExceptionStrings.TypeMismatch);
            }

            // Don't allow twice
            if (instances.ContainsKey(t))
            {
                throw new InvalidOperationException(ExceptionStrings.InstanceExists);
            }

            // Store
            instances[t] = instance;
        }

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
        public void RegisterInstance<TInterface>(string name, TInterface instance)
        {
            RegisterInstance(typeof(TInterface), name, instance);
        }

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
        public void RegisterInstance(Type t, string name, object instance)
        {
            // Validate
            if (t == null) throw new ArgumentNullException("t");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");
            if (instance == null) throw new ArgumentNullException("instance");

            // Must inherit
            if (!t.IsAssignableFrom(instance.GetType()))
            {
                throw new InvalidOperationException(ExceptionStrings.TypeMismatch);
            }

            // Don't allow twice
            if (namedInstances.ContainsKey(t))
            {
                if (namedInstances[t].ContainsKey(name))
                {
                    throw new InvalidOperationException(ExceptionStrings.InstanceExists);
                }
            }

            // Register 
            namedInstances[t][name] = instance;
        }

        /// <summary>
        /// Registers a named <see cref="Type"/> within the container.
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="Type" /> of instance to create.
        /// </typeparam>
        /// <param name="name">
        /// The name that will be used to resolve an instance of <paramref name="T"/>.
        /// </param>
        public void RegisterType<T>(string name)
        {
            RegisterType(typeof(T), name);
        }

        /// <summary>
        /// Registers a named <see cref="Type"/> within the container.
        /// </summary>
        /// <param name="t">
        /// The <see cref="Type" /> of instance to create.
        /// </param>
        /// <param name="name">
        /// The name that will be used to resolve an instance of <paramref name="T"/>.
        /// </param>
        public void RegisterType(Type t, string name)
        {
            // Validate
            if (t == null) throw new ArgumentNullException("t");
            if (name == null) throw new ArgumentNullException("name");

            // Don't allow twice
            if (namedTypes.ContainsKey(name))
            {
                throw new InvalidOperationException(ExceptionStrings.TypeExists);
            }

            // Register 
            namedTypes[name] = t;
        }
        
        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <typeparam name="TFrom">
        /// The <see cref="System.Type"/> that will be requested.
        /// </typeparam>
        /// <typeparam name="TTo">
        /// The <see cref="System.Type"/> that will be requested.
        /// </typeparam>
        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            RegisterType(typeof(TFrom), typeof(TTo));
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <param name="from">
        /// The <see cref="System.Type"/> that will be requested.
        /// </param>
        /// <param name="to">
        /// The <see cref="System.Type"/> that will be requested.
        /// </param>
        public void RegisterType(Type from, Type to)
        {
            // Validate
            if (from == null) throw new ArgumentNullException("from");
            if (to == null) throw new ArgumentNullException("to");

            // Ensure inheritance
            if (!(from.IsAssignableFrom(to)))
            {
                throw new InvalidOperationException(ExceptionStrings.TypeMismatch);
            }

            // Ensure not already registered
            if (typeMappings.ContainsKey(from))
            {
                throw new InvalidOperationException(ExceptionStrings.TypeExists);
            }
            
            // Store registration
            typeMappings[from] = to;
        }

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
        public void RegisterType<TFrom, TTo>(string name) where TTo : TFrom
        {
            RegisterType(typeof(TFrom), typeof(TTo), name);
        }

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
        public void RegisterType(Type from, Type to, string name)
        {
            // Validate
            if (from == null) throw new ArgumentNullException("from");
            if (to == null) throw new ArgumentNullException("to");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

            // Ensure inheritance
            if (!(from.IsAssignableFrom(to)))
            {
                throw new InvalidOperationException(ExceptionStrings.TypeMismatch);
            }

            // Ensure not already registered
            if (namedTypeMappings.ContainsKey(from))
            {
                if (namedTypeMappings[from].ContainsKey(name))
                {
                    throw new InvalidOperationException(ExceptionStrings.NameExists);
                }
            }

            // Store registration
            namedTypeMappings[from][name] = to;
        }

        /// <summary>
        /// Get an instance of the default requested type from the container.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="Type"/> of object to get from the container.
        /// </typeparam>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Get an instance of the default requested type from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        public object Resolve(Type t)
        {
            return Resolve(t, true);
        }

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
        public T Resolve<T>(string name)
        {
            return (T)Resolve(typeof(T), name);
        }

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
        public object Resolve(Type t, string name)
        {
            return Resolve(t, name, true);
        }

        /// <summary>
        /// Attempts to resolve the default instance of the requested type from the container.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="Type"/> of object to get from the container.
        /// </typeparam>
        /// <returns>
        /// The retrieved object if found; otherwise <see langword="null"/>.
        /// </returns>
        public T TryResolve<T>()
        {
            return (T)TryResolve(typeof(T));
        }

        /// <summary>
        /// Attempts to resolve the default instance of requested type from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <returns>
        /// The retrieved object if found; otherwise <see langword="null"/>.
        /// </returns>
        public object TryResolve(Type t)
        {
            return Resolve(t, false);
        }

        /// <summary>
        /// Attempts to resolve the named instance from the container.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="Type"/> of object to get from the container.
        /// </typeparam>
        /// <param name="name">
        /// The retrieved object if found; otherwise <see langword="null"/>.
        /// </param>
        /// <returns>
        /// The retrieved object.
        /// </returns>
        public T TryResolve<T>(string name)
        {
            return (T)TryResolve(typeof(T), name);
        }

        /// <summary>
        /// Attempts to resolve the named instance from the container.
        /// </summary>
        /// <param name="t">
        /// <see cref="Type"/> of object to get from the container.
        /// </param>
        /// <param name="name">
        /// The named of the instance to resolve.
        /// </param>
        /// <returns>
        /// The retrieved object if found; otherwise <see langword="null"/>.
        /// </returns>
        public object TryResolve(Type t, string name)
        {
            return Resolve(t, name, false);
        }
        #endregion // Public Methods
    }
}
