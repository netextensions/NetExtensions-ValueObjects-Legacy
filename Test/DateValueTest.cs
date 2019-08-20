using System;
using NetExtensions.ValueObjects.Legacy;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class DateValueTest
    {
        
        [TestCase("")]
        [TestCase("20180518")]
        [TestCase("19000101")]
        [TestCase("20401230")]
        [TestCase("18481230")]
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
        
        [TestCase("", "empty date")]
        [TestCase("180518","19180518")]
        [TestCase("401230","19401230")]
        [TestCase("991230","19991230")]
        [TestCase("560101","19560101")]
        [TestCase("170101","20170101")]
        [TestCase("20170101","20170101")]
        public void TestEqualityCentury_PreviousCentury(string stringValue, string expected)
        {
            var fromYearBelongsToPreviousCentury = 1918;
            var eq1 = DateValue.Create(stringValue, fromYearBelongsToPreviousCentury);
            var eq2 = DateValue.Create(stringValue, fromYearBelongsToPreviousCentury);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
            Assert.AreEqual( expected,eq1.Value.Match(x=>x.Date.ToString("yyyyMMdd"), error=> error.First()));

        }
        
        [TestCase("", "empty date",true, "yyMMdd")]
        [TestCase("180518","19180518",true, "yyMMdd")]
        [TestCase("401230","19401230", true, "yyMMdd")]
        [TestCase("991230","19991230", true, "yyMMdd")]
        [TestCase("560101","19560101", true, "yyMMdd")]
        [TestCase("170101","19170101", true, "yyMMdd")]
        public void TestEqualityCentury_PreviousCentury_lastCentury_True(string stringValue, string expected,bool lastCentury, string format)
        {

            var eq1 = DateValue.Create(stringValue, lastCentury, format);
            var eq2 = DateValue.Create(stringValue, lastCentury,format);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
            Assert.AreEqual( expected,eq1.Value.Match(x=>x.Date.ToString("yyyyMMdd"), error=> error.First()));

        }


        [TestCase("", "empty date", false, "yyMMdd")]
        [TestCase("180518", "20180518", false, "yyMMdd")]
        [TestCase("401230", "20401230", false, "yyMMdd")]
        [TestCase("991230", "20991230", false, "yyMMdd")]
        [TestCase("560101", "20560101", false, "yyMMdd")]
        [TestCase("170101", "20170101", false, "yyMMdd")]
        public void TestEqualityCentury_PreviousCentury_lastCentury_False(string stringValue, string expected, bool lastCentury, string format)
        {

            var eq1 = DateValue.Create(stringValue, lastCentury, format);
            var eq2 = DateValue.Create(stringValue, lastCentury, format);
            Assert.AreEqual(eq1, eq2);
            Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
            Assert.AreEqual(expected, eq1.Value.Match(x => x.Date.ToString("yyyyMMdd"), error => error.First()));

        }


    }
}