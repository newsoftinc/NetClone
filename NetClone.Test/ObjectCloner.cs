using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newsoft.NetClone.Test.Model;
using Newsoft.NetClone;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Newsoft.NetClone.Test;

namespace NetClone.Test
{
    [TestClass]
    public class ObjectCloneTest
    {

        [TestMethod]
        public void CollectionMemberNotClonedWhenIgnored()
        {
            var clone = new ObjectCloner();

            var a = Helper.InitTestObject();

            clone.ForMember("Bs[*].C").CloneMode(CloneMode.Ignore);

            var aprime = clone.Clone(a);

            Assert.IsTrue(aprime.Bs.First().C == null);
        }
        [TestMethod]
        public void CollectionMemberClonedWhenCopy()
        {
            var clone = new ObjectCloner();

            var a = Helper.InitTestObject();

            clone.ForMember("Bs[*].C").CloneMode(CloneMode.Copy);

            var aprime = clone.Clone(a);

            Assert.IsTrue(aprime.Bs.First().C != null);
        }
        [TestMethod]
        public void CollectionMemberAssignedByRef()
        {
            var clone = new ObjectCloner();

            var a = Helper.InitTestObject();

            clone.ForMember("Bs").CloneMode(CloneMode.AsReference);
            
            var aprime = clone.Clone(a);

            var leftMember = a.Bs.First();
            var rightMember = aprime.Bs.First();


            leftMember.F1 = 34343434;

            Assert.AreEqual(leftMember.F1, rightMember.F1);
        }
        [TestMethod]
        public void DefaultIsCopy()
        {
            var clone = new ObjectCloner();

            var a = Helper.InitTestObject();

            var aprime = clone.Clone(a);

            a.B.F1 = 12345;
            aprime.B.F1 = 6789;

            Assert.AreNotEqual(a.B.F1, aprime.B.F1);
        }
        [TestMethod]
        public void IsReference()
        {
            var clone = new ObjectCloner();

            var a = Helper.InitTestObject();

            clone.ForMember("B.C").CloneMode(CloneMode.AsReference);
            
            var aprime = clone.Clone(a);

            a.B.C.F1 = 12345;

            Assert.AreEqual(a.B.C.F1, aprime.B.C.F1);
        }
        [TestMethod]
        public void CollectionMemberIsReference()
        {
            var clone = new ObjectCloner();
            clone.ForMember("Bs[*]").CloneMode(CloneMode.Copy);
            clone.ForMember("Bs[*].C").CloneMode(CloneMode.AsReference);
            
            var a = Helper.InitTestObject();
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
            var clone = new ObjectCloner();
            clone.ForMember("Bs[*]").CloneMode(CloneMode.Copy);
            clone.ForMember("Bs[*].C").CloneMode(CloneMode.AsReference);

            var a = Helper.InitTestObject();
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
            var clone = new ObjectCloner();
            clone.ForMember("Bs[*]").CloneMode(CloneMode.AsReference);
            clone.ForMember("Bs[*].C").CloneMode(CloneMode.Copy);

            var hasTrown = false;
            try
            {
                var a = Helper.InitTestObject();
                var aprime = clone.Clone(a);
            }
            catch
            {
                hasTrown = true;
            }

            Assert.IsTrue(hasTrown);
        }
    }

}