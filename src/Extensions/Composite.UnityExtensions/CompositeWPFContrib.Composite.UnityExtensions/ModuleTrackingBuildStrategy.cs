using System;
using CompositeWPFContrib.Composite.Services;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace CompositeWPFContrib.Composite.UnityExtensions
{
    /// <summary>
    /// Builder strategy that registers any module types that are build
    /// by the unity container
    /// </summary>
    public class ModuleTrackingBuildStrategy : BuilderStrategy
    {
        IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the ModuleTrackingBuildStrategy class.
        /// </summary>
        /// <param name="container"></param>
        public ModuleTrackingBuildStrategy(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Register the loaded type after a new instance if build for it
        /// </summary>
        /// <param name="context"></param>
        public override void PostBuildUp(IBuilderContext context)
        {
            Type buildType = BuildKey.GetType(context.BuildKey);

            if (buildType.GetInterface(typeof(IModule).FullName) != null)
            {
                IModuleStatusService service = _container.Resolve<IModuleStatusService>();
                service.RegisterLoadedModule(buildType);
            }

            base.PostBuildUp(context);
        }
    }
}