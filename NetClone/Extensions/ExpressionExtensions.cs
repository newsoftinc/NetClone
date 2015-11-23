using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Newsoft.NetClone.Extensions
{
    public static class ExpressionExtensions
    {
        public static string GetPath<T,TMember>(this Expression<Func<T, TMember>> expr)
        {
            var stack = new Stack<string>();

            MemberExpression me;
            switch (expr.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expr.Body as UnaryExpression;
                    me = ((ue != null) ? ue.Operand : null) as MemberExpression;
                    break;
                default:
                    me = expr.Body as MemberExpression;

                    if (me == null && expr.NodeType == ExpressionType.Lambda)
                    {
                        var methCallExpr = expr.Body as MethodCallExpression;
                        
                        var leftOperand = methCallExpr.Arguments[0] as MemberExpression;
                        dynamic rightOperand = methCallExpr.Arguments[1];

                        me = rightOperand.Body as MemberExpression;
                        while (me != null)
                        {
                            stack.Push(me.Member.Name);
                            me = me.Expression as MemberExpression;
                        }

                        stack.Push(leftOperand.Member.Name + "[*]");
                        return string.Join(".", stack.ToArray()); 
                    }
                    break;
            }

            while (me != null)
            {
                stack.Push(me.Member.Name);
                me = me.Expression as MemberExpression;
            }

            return string.Join(".", stack.ToArray());
        }
        //public static string GetPropertyNavigation(this Expression body)
        //{
        //    return ParseMemberPath(body.GetMemberInfo());
        //}
        //private static MemberExpression GetLamdaMember(Expression body)
        //{
        //    if (body.NodeType == ExpressionType.Convert)
        //    {
        //        return ((UnaryExpression)body).Operand as MemberExpression;
        //    }
        //    else if (body.NodeType == ExpressionType.MemberAccess)
        //    {
        //        return body as MemberExpression;
        //    }
        //    // unhandled.
        //    throw new ArgumentException("method");
        //}
        //public static string ParseMemberPath(List<MemberExpression> expressions)
        //{
        //    string ret = "";
        //    for (var i = 0; i < expressions.Count; i++)
        //    {
        //        if (i != 0)
        //        {
        //            ret += ".";
        //        }

        //        ret += expressions[i].Member.Name;
        //    }

        //    return ret;
        //}
        //public static List<MemberExpression> GetMemberInfo(this Expression method)
        //{
        //    // cast the lamba expression.
        //    LambdaExpression lambda = method as LambdaExpression;
        //    if (lambda == null)
        //        throw new ArgumentNullException("method");

        //    // return value.
        //    var ret = new List<MemberExpression>();

        //    // top.
        //    ret.Insert(0, GetLamdaMember(lambda.Body));

        //    // each parent.
        //    while(ret[0].Expression.NodeType != ExpressionType.Parameter)
        //    {
        //        ret.Insert(0, GetLamdaMember(ret[0].Expression));
        //    } 

        //    return ret;
        //}
    }
}
