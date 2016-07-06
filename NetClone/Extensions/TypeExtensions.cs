using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsoft.NetClone.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsEFProxy(this Type t)
        {
            if (t.AssemblyQualifiedName.Contains("EntityFrameworkDynamicProxies"))
                return true;

            return false;
        }

        public static Type GetEFProxyRealType(this Type t)
        {
            return t.BaseType;
        }
    }
}
