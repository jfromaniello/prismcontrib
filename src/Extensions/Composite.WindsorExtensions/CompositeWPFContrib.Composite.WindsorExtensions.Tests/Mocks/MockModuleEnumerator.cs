using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class MockModuleEnumerator : IModuleEnumerator
    {
        public bool GetStartupLoadedModulesCalled;

        public ModuleInfo[] StartupLoadedModules = new ModuleInfo[0];

        public ModuleInfo[] GetModules()
        {
            return null;
        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            GetStartupLoadedModulesCalled = true;
            return StartupLoadedModules;
        }

        public ModuleInfo GetModule(string moduleName)
        {
            return null;
        }
    }
}
