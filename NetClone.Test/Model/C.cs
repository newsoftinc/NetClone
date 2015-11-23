using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsoft.NetClone.Test.Model
{
    public class C
    {
        public Guid Id { get; set; }

        public C()
        {
        }

        public D D { get; set; }
        public int F1 { get; set; }
    }
}
