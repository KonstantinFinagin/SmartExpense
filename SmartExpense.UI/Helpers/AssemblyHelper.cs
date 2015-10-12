using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartExpense.UI.Helpers
{
    using System.Reflection;

    public static class AssemblyHelper
    {
        public static Type GetViewType(this Assembly assembly, string viewTypeName)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.FullName == viewTypeName) return type;
            }
            return null;

        }
    }
}
