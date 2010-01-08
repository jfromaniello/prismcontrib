using Castle.Windsor;
using CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests
{
	//Im using the official version of ServiceLocator castle adapter.
    [TestClass]
	[Ignore]
    public class WindsorContainerAdapterTests
	{
	//    [TestMethod]
	//    public void CanCreateAdapter()
	//    {
	//        var container = new WindsorContainer();
	//        var adapter = new WindsorContainerAdapter(container);

	//        Assert.IsNotNull(adapter);
	//    }

	//    [TestMethod]
	//    public void CanResolveServicesOnContainerAdapter()
	//    {
	//        var container = new WindsorContainer();
	//        var adapter = new WindsorContainerAdapter(container);

	//        var service = adapter.Resolve(typeof(MockService));
	//        Assert.IsNotNull(service);
	//    }

	//    [TestMethod]
	//    public void CanTryResolveServiceOnContainerAdapter()
	//    {
	//        var container = new WindsorContainer();
	//        var adapter = new WindsorContainerAdapter(container);

	//        var service = adapter.TryResolve(typeof(MockService));
	//        Assert.IsNotNull(service);
	//    }
    }
}
