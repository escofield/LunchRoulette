using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LunchRoletteApi.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = Parameter.ParseParameters("-vote somethinghere -bob anotehr command -bill what are you doing on your phone.");
            list = Parameter.ParseParameters(null);
            list = Parameter.ParseParameters("");
        }
    }
}
