using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.UnityExtensions.Tests.Mocks
{
    internal class MockModuleLoader : IModuleLoader
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
