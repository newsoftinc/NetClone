using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newsoft.NetClone.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Newsoft.NetClone;
using Newtonsoft.Json.Linq;
using System.Diagnostics;


namespace Newsoft.NetClone
{
    public class ObjectCloner<T> : ObjectCloner
        where T : class
    {
        /// <summary>
        /// Fluent for member settigns 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public MemberMapping<T, dynamic> ForMember(Expression<Func<T, dynamic>> member)
        {
            var memberMapping = GetMemberMapping(member);

            return memberMapping;
        }

        /// <summary>
        /// Will get the member mapping of a member expression. If not exists is created, else return the existing memberMapping
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        private MemberMapping<T, TMember> GetMemberMapping<TMember>(Expression<Func<T, TMember>> member)
        {
            var propertyNav = member.GetPath();// member.GetPropertyNavigation();

            if (!memberMappings.ContainsKey(propertyNav))
            {
                memberMappings.Add(propertyNav, new MemberMapping<T, TMember>(member));
            }
            var mapping = memberMappings[propertyNav];

            return mapping as MemberMapping<T, TMember>;
        }


        /// <summary>
        /// Clone an instance of an object.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public T Clone(T instance, JsonSerializer serializer = null)
        {
            return base.Clone<T>(instance, serializer);
        }


    }
}
