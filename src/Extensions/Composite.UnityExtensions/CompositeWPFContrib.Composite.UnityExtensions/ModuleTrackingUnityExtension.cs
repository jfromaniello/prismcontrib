using System;
using CompositeWPFContrib.Composite.Services;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace CompositeWPFContrib.Composite.UnityExtensions
{
    /// <summary>
    /// Unity extension to keep track of loaded modules.
    /// Register this extension to allow the application to offer module status information
    /// through the module status service
    /// </summary>
    public class ModuleTrackingUnityExtension : UnityContainerExtension
    {
        /// <summary>
        /// Initializes the module tracking extension
        /// </summary>
        protected override void Initialize()
        {
            // Register a special builder strategy that keeps track loaded module types
            // when they are created through the container resolving logic
            Context.Strategies.Add(new ModuleTrackingBuildStrategy(Container),
                                   UnityBuildStage.PostInitialization);

            // Wire-up the instance registration event to capture modules
            // that are loaded this way
            Context.RegisteringInstance += new EventHandler<RegisterInstanceEventArgs>(OnRegisteringInstance);
        }

        /// <summary>
        /// Gets called when a new instance of a registered type is created.
        /// This is used to filter out module types and keep track of them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRegisteringInstance(object sender, RegisterInstanceEventArgs e)
        {
            if (e.RegisteredType.GetInterface(typeof(IModule).FullName) != null)
            {
                IModuleStatusService service = Container.Resolve<IModuleStatusService>();
                service.RegisterLoadedModule(e.RegisteredType);
            }
        }
    }
}