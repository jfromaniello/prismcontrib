using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Modularity;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
	public class MockModuleCatalog : IModuleCatalog
	{
		//public bool GetStartupLoadedModulesCalled;

		//public ModuleInfo[] StartupLoadedModules = new ModuleInfo[0];

		//public ModuleInfo[] GetModules()
		//{
		//    return null;
		//}

		//public ModuleInfo[] GetStartupLoadedModules()
		//{
		//    GetStartupLoadedModulesCalled = true;
		//    return StartupLoadedModules;
		//}

		//public ModuleInfo GetModule(string moduleName)
		//{
		//    return null;
		//}

		public IEnumerable<ModuleInfo> GetDependentModules(ModuleInfo moduleInfo)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ModuleInfo> CompleteListWithDependencies(IEnumerable<ModuleInfo> modules)
		{
			throw new NotImplementedException();
		}

		public void Initialize()
		{
			//throw new NotImplementedException();
			//noop.
		}

		public IEnumerable<ModuleInfo> Modules
		{
			get { throw new NotImplementedException(); }
		}
	}
}
