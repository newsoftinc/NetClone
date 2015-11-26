using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newsoft.NetClone.Test.Model;
using Newsoft.NetClone;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace NetClone.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void CollectionMemberNotClonedWhenIgnored()
        {
            var clone = new ObjectCloner<A>();

            var a = InitTestObject();

            clone.ForMember(V => V.Bs.Select(V0=>V0.C)).CloneMode(CloneMode.Ignore);

            var aprime = clone.Clone(a);


            Assert.IsTrue(aprime.Bs.First().C == null);
        }
        [TestMethod]
        public void CollectionMemberClonedWhenCopy()
        {
            var clone = new ObjectCloner<A>();

            var a = InitTestObject();

            clone.ForMember(V => V.Bs.Select(V0 => V0.C)).CloneMode(CloneMode.Copy);

            var aprime = clone.Clone(a);


            Assert.IsTrue(aprime.Bs.First().C != null);
        }
        [TestMethod]
        public void CollectionMemberAssignedByRef()
        {
            var clone = new ObjectCloner<A>();

            var a = InitTestObject();

            clone.ForMember(V => V.Bs).CloneMode(CloneMode.AsReference);

            var aprime = clone.Clone(a);

            var leftMember = a.Bs.First();
            var rightMember = aprime.Bs.First();


            leftMember.F1 = 34343434;

            Assert.AreEqual(leftMember.F1, rightMember.F1);
        }
        [TestMethod]
        public void DefaultIsCopy()
        {
            var clone = new Newsoft.NetClone.  ObjectCloner<A>();

            var a = InitTestObject();

            var aprime = clone.Clone(a);

            a.B.F1 = 12345;
            aprime.B.F1 = 6789;

            Assert.AreNotEqual(a.B.F1, aprime.B.F1);
        }
        [TestMethod]
        public void IsReference()
        {
            var clone = new ObjectCloner<A>();

            var a = InitTestObject();

            clone.ForMember(V => V.B.C).CloneMode(CloneMode.AsReference);

            var aprime = clone.Clone(a);

            a.B.C.F1 = 12345;

            Assert.AreEqual(a.B.C.F1, aprime.B.C.F1);
        }
        [TestMethod]
        public void CollectionMemberIsReference()
        {
            var clone = new ObjectCloner<A>();
            clone.ForMember(V => V.Bs).CloneMode(CloneMode.Copy);
            clone.ForMember(V => V.Bs.Select(V0 => V0.C)).CloneMode(CloneMode.AsReference);

            var a = InitTestObject();
            var aprime = clone.Clone(a);

            var memberSrc = a.Bs.First();
            var memberDst = aprime.Bs.First();

            memberSrc.F1 = 1234;
            memberDst.F1 = 5678;


            Assert.AreNotEqual(memberDst.F1, memberSrc.F1);
        }
        [TestMethod]
        public void CollectionIsCopy()
        {
            var clone = new ObjectCloner<A>();
            clone.ForMember(V => V.Bs).CloneMode(CloneMode.Copy);
            clone.ForMember(V => V.Bs.Select(V0 => V0.C)).CloneMode(CloneMode.AsReference);

            var a = InitTestObject();
            var aprime = clone.Clone(a);


            var memberSrc = a.Bs.First();
            var memberDst = aprime.Bs.First();

            memberSrc.F1 = 1234;
            memberDst.F1 = 5678;

            Assert.AreNotEqual(memberDst.F1, memberSrc.F1);
        }
        [TestMethod]
        public void ThrowWhenReferenceDescendantSetAsCopy()
        {
            var clone = new ObjectCloner<A>();
            clone.ForMember(V => V.B).CloneMode(CloneMode.AsReference);
            clone.ForMember(V => V.B.C).CloneMode(CloneMode.Copy);

            var hasTrown = false;
            try
            {
                var a = InitTestObject();
                var aprime = clone.Clone(a);
            }
            catch
            {
                hasTrown = true;
            }

            Assert.IsTrue(hasTrown);
        }
        public A InitTestObject()
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