using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class MockModuleLoader : IModuleLoader
    {
        public bool InitializeCalled;
        public ModuleInfo[] InitializeArgumentModuleInfos;

        public void Initialize(ModuleInfo[] moduleInfos)
        {
            InitializeCalled = true;
            InitializeArgumentModuleInfos = moduleInfos;
        }
    }
}
