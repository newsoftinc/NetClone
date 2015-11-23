using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsoft.NetClone.Extensions
{
    public static class JsonNetExtensions
    {
        /// <summary>
        /// Make a memberMapping path standard with the JToken syntax.
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static string ToJsonToken(this MemberMapping mapping)
        {
            return $"$.{ mapping.MemberPath}";
        }

        /// <summary>
        /// Remove all JOBject tokens exept the provided path.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paths"></param>
        public static void RemoveAllExcept(this JObject obj, IEnumerable<string> paths)
        {
            if (obj == null || paths == null)
                throw new NullReferenceException();
            var keepers = new HashSet<JToken>(paths.SelectMany(path => obj.SelectTokens(path)));
            var keepersAndParents = new HashSet<JToken>(keepers.SelectMany(t => t.AncestorsAndSelf()));
            // Keep any token that is a keeper, or a child of a keeper, or a parent of a keeper
            // I.e. if you have a path ""$.A.B" and it turns out that B is an object, then everything
            // under B should be kept.
            foreach (var token in obj.DescendantsAndSelfReversed().Where(t => !keepersAndParents.Contains(t) && !t.AncestorsAndSelf().Any(p => keepers.Contains(p))))
                token.RemoveFromLowestPossibleParent();
        }

        /// <summary>
        /// Remove provided JToken paths
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paths"></param>
        public static void RemovePaths(this JObject obj, IEnumerable<string> paths)
        {
            if (obj == null || paths == null)
                throw new NullReferenceException();
            var toRemove = new HashSet<JToken>(paths.SelectMany(path => obj.SelectTokens(path)));

            var toRemoveAndParents = new HashSet<JToken>(toRemove.SelectMany(t => t.AncestorsAndSelf()));

            // Remove any token that is to be removed, or a child of an element to remove, or a parent of an element to be removed
            // I.e. if you have a path ""$.A.B" and it turns out that B is an object, then everything
            // under B should be removed.
            foreach (var token in obj.DescendantsAndSelfReversed().Where(t => toRemoveAndParents.Contains(t) && t.AncestorsAndSelf().Any(p => toRemove.Contains(p))))
                token.RemoveFromLowestPossibleParent();
        }

        /// <summary>
        /// Remove the lowest possible parent
        /// </summary>
        /// <param name="node"></param>
        public static void RemoveFromLowestPossibleParent(this JToken node)
        {
            if (node == null)
                throw new ArgumentNullException();
            var contained = node.AncestorsAndSelf().Where(t => t.Parent is JArray || t.Parent is JObject).FirstOrDefault();
            if (contained != null)
                contained.Remove();
        }


        /// <summary>
        /// All descendants and the node itself, in reverse order.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<JToken> DescendantsAndSelfReversed(this JToken node)
        {
            if (node == null)
                throw new ArgumentNullException();
            return RecursiveEnumerableExtensions.Traverse(node, t => ListReversed(t as JContainer));
        }


        /// <summary>
        /// Iterate backwards through a list without throwing an exception if the list is modified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        static IEnumerable<T> ListReversed<T>(this IList<T> list)
        {
            if (list == null)
                yield break;
            for (int i = list.Count - 1; i >= 0; i--)
                yield return list[i];
        }
    }
    

    public static class RecursiveEnumerableExtensions
    {
        public static IEnumerable<T> Traverse<T>(
            T root,
            Func<T, IEnumerable<T>> children)
        {
            yield return root;

            var stack = new Stack<IEnumerator<T>>();
            stack.Push(children(root).GetEnumerator());

            while (stack.Count != 0)
            {
                var enumerator = stack.Peek();
                if (!enumerator.MoveNext())
                {
                    stack.Pop();
                    enumerator.Dispose();
                }
                else
                {
                    yield return enumerator.Current;
                    stack.Push(children(enumerator.Current).GetEnumerator());
                }
            }
        }
    }
}
