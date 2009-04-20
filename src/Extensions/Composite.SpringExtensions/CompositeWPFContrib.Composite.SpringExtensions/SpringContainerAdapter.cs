using System;
using Microsoft.Practices.Composite;
using Spring.Context;
using Spring.Context.Support;

namespace CompositeWPFContrib.Composite.SpringExtensions
{
    /// <summary>
    /// Adapter that modifies the spring container interface in such a way
    /// that it is usable by other components in the composite library without having to modify any code.
    /// </summary>
    public class SpringContainerAdapter: IContainerFacade
    {
        private IApplicationContext _context;

        /// <summary>
        /// Initializes a new instance of the SpringContainerAdapter class.
        /// </summary>
        /// <param name="context"></param>
        public SpringContainerAdapter(IApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Initializes a new instance of the SpringContainerAdapter class.
        /// </summary>
        public SpringContainerAdapter()
            :this(null)
        {
            
        }

        #region IContainerFacade Members

        /// <summary>
        /// <para>
        /// Resolves an object in the container by its type; throws an exception when the object is not found.
        /// </para>
        /// <para>
        /// The object is resolved by using the name of the type as the alias for the spring container.
        /// If no object is found by that alias, the typename is prefixed with the namespace. If that
        /// doesn't work the operation is aborted and an exception is thrown.
        /// </para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            object foundObjectInstance = TryResolve(type);

            if (foundObjectInstance == null)
            {
                throw new InvalidOperationException(
                    Properties.Resources.ObjectNotDefinedException);
            }

            return foundObjectInstance;
        }

        /// <summary>
        /// <para>
        /// Resolves an object in the container by its type; throws an exception when the object is not found.
        /// </para>
        /// <para>
        /// The object is resolved by using the name of the type as the alias for the spring container.
        /// If no object is found by that alias, the typename is prefixed with the namespace. If that
        /// doesn't work the operation is aborted and a null reference is returned.
        /// </para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object TryResolve(Type type)
        {
            // Fallback in case the container wasn't initialized using custom logic
            if (_context == null)
                _context = ContextRegistry.GetContext();

            string[] possibleAliases = new string[] { type.Name, type.Namespace + "." + type.Name };
            object foundObjectInstance = null;

            foreach (string possibleAlias in possibleAliases)
            {
                if (_context.ContainsObjectDefinition(possibleAlias))
                {
                    foundObjectInstance = _context.GetObject(possibleAlias);
                    break;
                }
            }

            return foundObjectInstance;
        }

        #endregion
    }
}
