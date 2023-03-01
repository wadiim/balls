using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class ProgramTests
    {
        private const string answer = "Hello World!";
        [TestMethod]
        public void TestMain()
        {
            using (var i = new StringWriter())
            {
                Console.SetOut(i);
                TPW.Program.Main(null);

                var result = i.ToString().Trim();
                Assert.AreEqual(answer, result);
            }
        }
    }
}
