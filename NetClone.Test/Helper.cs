using Newsoft.NetClone.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsoft.NetClone.Test
{
   public static  class Helper
    {
        public static A InitTestObject()
        {
            var a = new A()
            {
                F1 = 1,
                Id = Guid.NewGuid(),
                Bs = new List<B>() {
                    new B()
                    {
                        F1 = 10,
                        Id = Guid.NewGuid(),
                        C = new C()
                        {
                            F1 = 11,
                            Id = Guid.NewGuid(),
                            D = new D()
                            {
                                F1 = 12,
                                Id = Guid.NewGuid(),
                            }
                        }
                    },
                    new B()
                    {
                        F1 = 20,
                        Id = Guid.NewGuid(),
                        C = new C()
                        {
                            F1 = 21,
                            Id = Guid.NewGuid(),
                            D = new D()
                            {
                                F1 = 22,
                                Id = Guid.NewGuid(),
                            }
                        }
                    }
                },
                B = new B()
                {
                    F1 = 2,
                    Id = Guid.NewGuid(),
                    C = new C()
                    {
                        F1 = 3,
                        Id = Guid.NewGuid(),
                        D = new D()
                        {
                            F1 = 8,
                            Id = Guid.NewGuid(),
                        }
                    },
                    D = new D()
                    {
                        F1 = 4,
                        Id = Guid.NewGuid(),
                    },
                },
                C = new C()
                {
                    F1 = 5,
                    Id = Guid.NewGuid(),
                    D = new D()
                    {
                        F1 = 6,
                        Id = Guid.NewGuid(),
                    },
                },
                D = new D()
                {
                    F1 = 7,
                    Id = Guid.NewGuid(),
                },

            };
            a.B.C.F1 = 1234;

            return a;
        }
    }
}
