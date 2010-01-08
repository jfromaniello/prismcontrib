using System;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;

namespace CompositeWPFContrib.Composite.WindsorExtensions
{
	//TODO: use this artifact when castle release the vnext of Core and Windsor
	//public class LazyLoader : ILazyComponentLoader
	//{
	//    private readonly string[] allowedAssembliesNames = new[]
	//                                                        {
	//                                                            "Microsoft.Practices.Composite",
	//                                                            "Microsoft.Practices.Composite.Presentation",
	//                                                            "Microsoft.Practices.Composite.Wpf"
	//                                                        };

	//    #region ILazyComponentLoader Members

	//    public IRegistration Load(string key, Type service)
	//    {
	//        if (allowedAssembliesNames.Any(n => service.Assembly.FullName.StartsWith(n)))
	//        {
	//            return Component.For(service).Named(key).LifeStyle.Transient;
	//        }

	//        return null;
	//    }

	//    #endregion
	//}
}