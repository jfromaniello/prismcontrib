using System.Collections.ObjectModel;
using System.Linq;
using CompositeWPFContrib.Composite.Modularity;
using CompositeWPFContrib.Composite.Tests.Mocks;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompositeWPFContrib.Composite.Tests.Modularity
{
    /// <summary>
    /// Checks the logic of the <see cref="CompositeModuleEnumerator"/>
    /// </summary>
    [TestClass]
    public class CompositeModuleEnumeratorFixture
    {
        private Collection<ModuleInfo> _modulesFirstEnumerator;
        private Collection<ModuleInfo> _modulesSecondEnumerator;
        private ModuleEnumeratorMock _firstEnumerator;
        private ModuleEnumeratorMock _secondEnumerator;

        /// <summary>
        /// Initializes the test case
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _modulesFirstEnumerator = new Collection<ModuleInfo>()
                                          {
                                              new ModuleInfo("test.dll","TestModuleType1","Test1",false),
                                              new ModuleInfo("test.dll","TestModuleType2","Test2",false)
                                          };

            _modulesSecondEnumerator = new Collection<ModuleInfo>()
                                           {
                                               new ModuleInfo("test.dll","TestModuleType3","Test3",true),
                                               new ModuleInfo("test.dll","TestModuleType4","Test4",false)
                                           };

            _firstEnumerator = new ModuleEnumeratorMock(_modulesFirstEnumerator);
            _secondEnumerator = new ModuleEnumeratorMock(_modulesSecondEnumerator);
        }

        /// <summary>
        /// Checks if the retrieving single modules from different module enumerators works
        /// </summary>
        [TestMethod]
        public void TestGetSingleExistingModuleFromTwoEnumerators()
        {
            CompositeModuleEnumerator compositeEnumerator = new CompositeModuleEnumerator();
            compositeEnumerator.ChildEnumerators.Add(_firstEnumerator);
            compositeEnumerator.ChildEnumerators.Add(_secondEnumerator);

            Assert.AreSame(_modulesFirstEnumerator[0], compositeEnumerator.GetModule("Test1"));
            Assert.AreSame(_modulesSecondEnumerator[1], compositeEnumerator.GetModule("Test4"));
        }

        /// <summary>
        /// Checks if a null-reference is returned when a module does not exist
        /// on either of the module enumerators.
        /// </summary>
        [TestMethod]
        public void TestGetSingleNonExistingModuleEnumerator()
        {
            CompositeModuleEnumerator compositeEnumerator = new CompositeModuleEnumerator();
            compositeEnumerator.ChildEnumerators.Add(_firstEnumerator);
            compositeEnumerator.ChildEnumerators.Add(_secondEnumerator);

            Assert.IsNull(compositeEnumerator.GetModule("Test5"));
        }

        /// <summary>
        /// Checks if several modules can be retrieved that are enumerated
        /// by different module enumerators
        /// </summary>
        [TestMethod]
        public void TestGetAllModulesFromDifferentEnumerators()
        {
            CompositeModuleEnumerator compositeEnumerator = new CompositeModuleEnumerator();
            compositeEnumerator.ChildEnumerators.Add(_firstEnumerator);
            compositeEnumerator.ChildEnumerators.Add(_secondEnumerator);

            ModuleInfo[] modules = compositeEnumerator.GetModules();

            Assert.AreEqual(4, modules.Count());
            Assert.AreSame(_modulesFirstEnumerator[0], modules[0]);
        }

        /// <summary>
        /// Checks if the startup modules are retrieved correctly
        /// </summary>
        [TestMethod]
        public void TestGetAllStartupModulesDifferentEnumerators()
        {
            CompositeModuleEnumerator compositeEnumerator = new CompositeModuleEnumerator();
            compositeEnumerator.ChildEnumerators.Add(_firstEnumerator);
            compositeEnumerator.ChildEnumerators.Add(_secondEnumerator);

            ModuleInfo[] modules = compositeEnumerator.GetStartupLoadedModules();

            Assert.AreEqual(1, modules.Count());
            Assert.AreSame(_modulesSecondEnumerator[0], modules[0]);
        }
    }
}