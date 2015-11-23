using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newsoft.NetClone.Extensions;
using System.Reflection;
using System.Collections;

namespace Newsoft.NetClone
{
    public abstract class MemberMapping
    {
        public string MemberPath { get; set; }
        public Type SourceType { get; set; }
        public Type MemberType { get; set; }

        public CloneMode Mode { get; protected set; }

        public object Map(object source, object dest, string memberPath = null)
        {
            if (memberPath == null)
                memberPath = MemberPath;

            var pathSplit = memberPath.TrimStart('$').TrimStart('.').Split('.');

            //Getting both properties to support proxy (EF).
            PropertyInfo currPropDst;
            PropertyInfo currPropSrc;
            object currSrcVal = source;
            object currDstVal = dest;

            int i = 0;
            foreach (var path in pathSplit)
            {
                if (path.Contains("[*]"))
                {
                    var arrPropName = path.Substring(0,path.Length - 3);
                    var arrPropDst = currDstVal.GetType().GetProperty(arrPropName);
                    var arrPropSrc = currSrcVal.GetType().GetProperty(arrPropName);

                    var srcArr = arrPropSrc.GetValue(currSrcVal) as IEnumerable<dynamic>;
                    var dstArr = arrPropDst.GetValue(currDstVal) as IEnumerable<dynamic>;

                    if (srcArr == null || dstArr == null)
                        throw new InvalidOperationException($"Source or destination array is null for path {path}");

                    if (srcArr.Count() != dstArr.Count())
                        throw new InvalidOperationException($"Source array and destination array are not of the same length for path {path}");

                    var trailingPath = GetTrailingPath(path, pathSplit);

                    int j = 0;
                    foreach (var srcItm in srcArr)
                    {
                        var dstItm = dstArr.ElementAt(j);
                        Map(srcItm, dstItm, trailingPath);
                        j++;
                    }
                    break;
                    // currSrcVal = MapArray(pathSplit, out currPropDst, out currPropSrc, currSrcVal, currDstVal, i, path);
                    return dest;
                }
                else
                {
                    currPropSrc = currSrcVal.GetType().GetProperty(path);
                    currPropDst = currDstVal.GetType().GetProperty(path);
                    currSrcVal = currPropSrc.GetValue(currSrcVal);
                    if (pathSplit.Last() == path)
                    {
                        currPropDst.SetValue(currDstVal, currSrcVal);
                        break;
                    }

                    currDstVal = currPropDst.GetValue(currDstVal);
                }
                i++;
            }
            return dest;
        }

        private string GetTrailingPath(string path, string[] pathSplit)
        {
            var newPath = string.Empty;

            var splitList = pathSplit.ToList();

            for (int i = 0; i <= splitList.Count(); i++)
            {
                if (splitList[i] != path)
                    splitList.RemoveAt(i);

                splitList.RemoveAt(i);

                break;
            }

            var result = string.Empty;
            splitList.ForEach(V => result += $"{V}.");
            return result.TrimEnd('.');

        }

    }
}
