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
    public class ObjectCloner
    {
        protected Dictionary<string, MemberMapping> memberMappings = new Dictionary<string, MemberMapping>();
        /// <summary>
        /// For member settigns 
        /// </summary>
        /// <param name="member">The path for the member. use [*] for IEnumerable members (A.Items[*].C)</param>
        /// <returns></returns>
        public MemberMapping ForMember(string member)
        {
            var memberMapping = GetMemberMapping(member);

            return memberMapping;
        }



        /// <summary>
        /// Get a member mapping from the provided navigation path
        /// </summary>
        /// <param name="navigationPath"></param>
        /// <returns></returns>
        protected MemberMapping GetMemberMapping(string navigationPath)
        {
            var propertyNav = navigationPath;

            if (!memberMappings.ContainsKey(propertyNav))
            {
                memberMappings.Add(propertyNav, new MemberMapping(propertyNav));
            }
            var mapping = memberMappings[propertyNav];

            return mapping as MemberMapping;
            //return memberMappings.ContainsKey(navigationPath) ? memberMappings[navigationPath] : null;
        }


        /// <summary>
        /// Perform every integrity validation.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsValid()
        {
            var brokenDescendants = ValidateBrokenDescendants();

            if (!string.IsNullOrEmpty(brokenDescendants))
                return false;

            return true;

        }
        /// <summary>
        /// Checks if any child nodes are inconsistent due to ancestors that define a reference instead of a clone.
        /// ForMember(member=>B.C).CloneMode(CloneMode.Copy)
        /// ForMember(member=>B).CloneMode(CloneMode.AsReference)
        /// This should throw an error as we cannot clone desendant C of reference B (inconsistent).
        /// </summary>
        /// <returns></returns>
        private string ValidateBrokenDescendants(bool throwOnError = true)
        {
            var referenceMappings = memberMappings.Values.Where(V => V.Mode == CloneMode.AsReference);
            foreach (MemberMapping memberMapping in referenceMappings)
            {
                var childPaths = memberMappings
                    .Where(
                    V =>
                    V.Key != memberMapping.MemberPath && //Not the current member
                    V.Key.StartsWith(memberMapping.MemberPath) &&  //Is child of the current member
                    V.Value.Mode == CloneMode.Copy); //Is configured to be cloned;

                if (childPaths.Any())
                {
                    var msg = "One or many ancestors of a node configured as Copy are configured AsReference. ";
                    var messages = childPaths.Select(V => $"{V.Value.MemberPath}");

                    foreach (var message in messages)
                        msg += message + "\r\n";

                    if (throwOnError)
                        throw new Exception(msg);
                    else return msg;
                }
            }
            return string.Empty;

        }
        /// <summary>
        /// Prepare the JObject , cleanup paths and serialize the object
        /// </summary>
        /// <param name="root">The instance to clone</param>
        /// <param name="paths">Paths to be excluded from the serialization</param>
        /// <returns></returns>
        protected string SerializeAndSelectTokens(object root, string[] paths, JsonSerializer serializer = null)
        {
            if (serializer == null)
                serializer = JsonSerializer.CreateDefault();

            var obj = JObject.FromObject(root, serializer);

            obj.RemovePaths(paths);

            Debug.WriteLine(obj.ToString().Length);

            var json = obj.ToString(Formatting.None);

            return json;
        }


        /// <summary>
        /// Clone an instance of an object.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public T Clone<T>(T instance, JsonSerializer serializer = null)
            where T : class
        {
            var breakChilds = IsValid();

            var jsonResult = Serialize(instance, serializer);

            // determine final type.
            var finalType = instance.GetType();
            if (finalType.IsEFProxy())
                finalType = finalType.GetEFProxyRealType();

            var cloneResult = (T)JsonConvert.DeserializeObject(jsonResult, finalType);

            var resultWithReferences = RestoreReferences(instance, cloneResult);

            return cloneResult;
        }

        /// <summary>
        /// Will map every member defined AsReference from their source object to their target path.
        /// </summary>
        /// <param name="origin">Object to be cloned</param>
        /// <param name="target">Cloned object</param>
        /// <returns></returns>
        protected T RestoreReferences<T>(T origin, T target)
            where T : class
        {
            var byRefMembers = memberMappings.Where(V => V.Value.Mode == CloneMode.AsReference).Select(v => v.Value);

            foreach (MemberMapping byRefMember in byRefMembers)
            {
                byRefMember.Map(origin, target);
            }

            return target;
        }

        /// <summary>
        /// Will serialize object to create a new reference.
        /// Member mappings defined with mode AsReference or Ignore will be removed from mapping and managed later following their respective logic.
        /// </summary>
        /// <param name="obj">The instance to clone</param>
        /// <returns></returns>
        protected string Serialize<T>(T obj, JsonSerializer serializer = null)
            where T : class
        {
            var paths = memberMappings.Values.Where(V => V.Mode == CloneMode.AsReference || V.Mode == CloneMode.Ignore)
                .Select(V => V.ToJsonToken()).ToArray();
            var result = SerializeAndSelectTokens(obj, paths, serializer);

            return result;

        }

    }
}
