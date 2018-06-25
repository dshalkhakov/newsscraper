using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace newsscraper.Tests
{
    [TestClass]
    public class DateTimeParsingTests
    {
        [TestMethod]
        public void ParseDate_WhenGivenToday_ReturnsProperDate()
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 11, 0);
            Assert.AreEqual(date, newsscraper.Program.ParseDateTime("сегодня, 19:11"));
        }

        [TestMethod]
        public void ParseDate_WhenGivenYesterday_ReturnsProperDate()
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 11, 0).AddDays(-1);
            Assert.AreEqual(date, newsscraper.Program.ParseDateTime("вчера, 19:11"));
        }

        [TestMethod]
        public void ParseDate_WhenGivenDateWithGentitiveMonth_ReturnsProperDate()
        {
            var date = new DateTime(2018, 6, 6, 6, 40, 0);
            Assert.AreEqual(date, newsscraper.Program.ParseDateTime("6 июня, 6:40"));
        }

        [TestMethod]
        public void ParseDate_WhenGivenGarbage_ReturnsMinDate()
        {
            var date = DateTime.MinValue;
            Assert.AreEqual(date, newsscraper.Program.ParseDateTime("garbage text"));
        }

        [TestMethod]
        public void ParseDate_WhenGivenTextLikeDate_ReturnsMinDate()
        {
            var date = DateTime.MinValue;
            Assert.AreEqual(date, newsscraper.Program.ParseDateTime("date, time"));
        }

        [TestMethod]
        public void ParseDate_WhenGivenKazakhDate_ReturnsProperDate()
        {
            var date = new DateTime(2018, 6, 6, 6, 40, 0);
            Assert.AreEqual(date, newsscraper.Program.ParseDateTime("6 Маусым, 6:40"));
        }
    }
}
