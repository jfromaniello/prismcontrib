using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.UnityExtensions.Tests.Mocks
{
    class MockModuleEnumerator : IModuleEnumerator
    {
        public bool GetStartupLoadedModulesCalled;
        public ModuleInfo[] StartupLoadedModules = new ModuleInfo[0];

        public ModuleInfo[] GetModules()
        {
            throw new System.NotImplementedException();
        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            GetStartupLoadedModulesCalled = true;
            return StartupLoadedModules;
        }

        public ModuleInfo GetModule(string moduleName)
        {
            throw new System.NotImplementedException();
        }
    }
}
