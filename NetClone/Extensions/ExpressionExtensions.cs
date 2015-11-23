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
        /// <summary>
        /// Get the object path of a given expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
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
      
    }
}
