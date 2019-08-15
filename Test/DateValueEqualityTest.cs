using System;
using NetExtensions.ValueObjects.Legacy;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class DateValueEqualityTest
    {
        
        [TestCase("")]
        [TestCase("20180518")]
        [TestCase("19000101")]
        [TestCase("20401230")]
        public void TestEquality(string stringValue)
        {
            var eq1 = DateValue.Create(stringValue);
            var eq2 = DateValue.Create(stringValue);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }

        [TestCase("20180101010100", "20180101110100")]
        [TestCase("19211212110142", "19211212082023")]
        public void TestEqualityYear(string first, string second)
        {
            var eq1 = DateValue.Create(first);
            var eq2 = DateValue.Create(second);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }

        [Test]
        public void TestEqualityDateTime()
        {
            var first = DateTime.Now;
            var second = DateTime.Now;
            var eq1 = DateValue.Create(first);
            var eq2 = DateValue.Create(second);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }
        
        [Test]
        public void TestEqualityDateTimeNullable_Empty()
        {
            DateTime? first = null;
            DateTime? second = null;
            var eq1 = DateValue.Create(first);
            var eq2 = DateValue.Create(second);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }
        
        [Test]
        public void TestEqualityDateTimeNullable_NotEmpty()
        {
            DateTime? first = DateTime.Now;
            DateTime? second = DateTime.Now;;
            var eq1 = DateValue.Create(first);
            var eq2 = DateValue.Create(second);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }
        
        [TestCase("")]
        [TestCase("180518")]
        [TestCase("000101")]
        [TestCase("401230")]
        [TestCase("991230")]
        public void TestEqualityCentury_Previous(string stringValue)
        {
            var eq1 = DateValue.Create(stringValue, true);
            var eq2 = DateValue.Create(stringValue, true);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }
        
    }
}