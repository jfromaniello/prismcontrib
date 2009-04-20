using System.Windows;
using Infragistics.Windows.DockManager;

namespace CompositeWPFContrib.Composite.Wpf.Infragistics.Extensions
{
    /// <summary>
    /// Provides Method Extensions to <see cref="FrameworkElement"/> 
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Finds the parent <see cref="XamDockManager"/> of a given< <see cref="FrameworkElement"/> child
        /// </summary>
        /// <param name="child"><see cref="FrameworkElement"/></param>
        /// <returns><see cref="XamDockManager"/></returns>
        public static XamDockManager FindDockManager(this FrameworkElement child)
        {
            if (child is SplitPane)
                return XamDockManager.GetDockManager(child);

            if (child.Parent != null)
            {
                if (child.Parent is XamDockManager)
                {
                    return (XamDockManager)child.Parent;
                }

                return FindDockManager((FrameworkElement)child.Parent);
            }

            return null;
        }

        public static Window FindWindow(this FrameworkElement child)
        {
            if (child.Parent != null)
            {
                if (child.Parent is Window)
                {
                    return (Window)child.Parent;
                }

                return FindWindow((FrameworkElement)child.Parent);
            }

            return null;
        }
    }
}
