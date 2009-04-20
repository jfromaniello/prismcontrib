Structure of the package
Composite WPF Contribution
http://www.codeplex.com/CompositeWPFContrib
===============================================

- Library
- Samples
	- PrismImageSearch: Application that demonstrates how to use PrismAG
	- Presentation Model with DataTemplates (http://blogs.southworks.net/jdominguez/2008/09/presentation-model-with-datatemplates-in-compositewpf-prism-sample/)
	- DSASample: Application that demonstrates how to use the Disconnected Service Agent with CompositeWPF (http://blogs.southworks.net/mconverti/2008/08/11/composite-wpf-with-dsa-sample/)
	- WPFQuickStart: CAB QuickStart migrated to CompositeWPF (http://blogs.southworks.net/mconverti/2008/09/07/wpf-quickstart-shipped-with-scsf-regenerated-using-composite-wpf-prism/)
	- DialogWorkspace: Application that demonstrates how to use the Dialog Workspace.
- src
	- Extensions
		- Composite: UI agnostic Composite Extensions
			- CompositeWPFContrib.Composite
				* Composite ModuleEnumerator (http://blogs.infosupport.com/blogs/willemm/archive/2008/07/08/Combining-module-enumerators-in-CompositeWPF.aspx)
				* Module Status Service (http://blogs.infosupport.com/blogs/willemm/archive/2008/07/17/Module-status-service-for-CompositeWPF.aspx)
				* Extended Module Loader Service (http://blogs.infosupport.com/blogs/willemm/archive/2008/07/28/CompositeWPF-_1320_-Installing-modules-on-demand.aspx)
		    - CompositeWPFContrib.Composite.Tests
		- Composite.WPF: WPF Composite.WPF extensions
			- CompositeWPFContrib.Composite.Wpf 
				* OutlookBar control (http://blogs.southworks.net/ejadib/2008/07/22/use-the-outlookbar-in-your-compositewpf-prism-applications/)
				* OutlookBar RegionAdapter
				* ToolBarPanel RegionAdapter
				* Dialog Workspace (http://blogs.infosupport.com/blogs/willemm/archive/2008/09/05/Introducing-the-dialog-workspace-for-Composite-WPF.aspx)
			- CompositeWPFContrib.Composite.Wpf.Tests
		- Composite.UnityExtensions: Unity Composite.UnityExtensions extensions
			- CompositeWPFContrib.Composite.UnityExtensions
				* ModuleTrackingBuildStrategy
				* ModuleTrackingUnityExtension
			- CompositeWPFContrib.Composite.UnityExtensions.Tests
	- Extensions.Infragistics
	  Note: To use this, you must have installed NetAdvantage for WPF (http://www.infragistics.com/dotnet/netadvantage/wpf.aspx#Overview) 
		- Composite.Wpf.Infragistics: WPF Composite.WPF extensions for Infragistics controls
			- CompositeWPFContrib.Composite.Wpf.Infragistics
				* Tab Group Pane RegionAdapter (http://claudiopi.blogspot.com/2008/07/infragistics-tabgrouppane-region.html)	
	- Silverlight
		- Lib
		- PrismAG: CompositeWPF for Silverlight
- VisualStudio Templates
	- CompositeWPF: Visual Studio templates and Installer for shell project in the CompositeWPF (http://blogs.southworks.net/ejadib/2008/07/03/composite-application-guidance-for-wpf-visual-studio-templates/.)
		- Releases
		- Templates
		- Vsi