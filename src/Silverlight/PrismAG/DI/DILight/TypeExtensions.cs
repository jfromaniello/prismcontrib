using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DILight
{
    static public class TypeExtensions
    {
        static public bool IsCreatable(this Type type)
        {
            return ((!type.IsInterface) && (!type.IsAbstract) && (type.IsPublic));
        }
    }
}
