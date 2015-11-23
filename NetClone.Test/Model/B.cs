using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsoft.NetClone.Test.Model
{
    public class B
    {
        public Guid Id { get; set; }

        public B()
        {
        }
        public C C { get; set; }
        public D D { get; set; }
        public int F1 { get; set; }
    }
}
