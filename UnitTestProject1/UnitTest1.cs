using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ClassLibrary2.SuperBilard bil = new ClassLibrary2.SuperBilard();
            int result = bil.add(3, 2);
            Assert.AreEqual(5, result);
        }
    }
}
