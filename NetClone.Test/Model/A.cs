using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsoft.NetClone.Test.Model
{
    public class A
    {
        public Guid Id { get; set; }

        public A()
        {
        }
        public B B { get; set; } 
        public C C { get; set; } 
        public D D { get; set; }

        public List<B> Bs { get; set; }
        public int F1 { get; set; }
    }
}
