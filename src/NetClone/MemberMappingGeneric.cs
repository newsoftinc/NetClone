using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newsoft.NetClone.Extensions;

namespace Newsoft.NetClone
{
    public class MemberMapping<TSource, TMember> : MemberMapping
    {
        public MemberMapping(Expression<Func<TSource, TMember>> member)
        {
            SourceType = typeof(TSource);
            MemberType = typeof(TMember);

            MemberPath = member.GetPath();
        }

        public MemberMapping<TSource, TMember> CloneMode(CloneMode clonemode = NetClone.CloneMode.Default)
        {
            Mode = clonemode;
            return this;
        }
    }
}
