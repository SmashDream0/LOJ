using LaboratoryOnlineJournal.FormatChecker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOJ_test
{
    [TestClass]
    public class FormatCheckerTest
    {
        [TestMethod]
        public void TXTTest()
        {
            var checker = new TXTFormatChecker(Encoding.UTF32);

            var sb = new StringBuilder();
            sb.AppendLine(checker.Mark);
            sb.AppendLine("Just for test");
            var bytes = Encoding.UTF32.GetBytes(sb.ToString());

            Assert.AreEqual(true, checker.Check(bytes));
            Assert.AreEqual(false, checker.Check(TestData1.MessageBytes));
        }

        [TestMethod]
        public void OldTest()
        {
            var txtChecker = new TXTFormatChecker(Encoding.UTF32);

            var checker = new OldFormatChecker(Encoding.UTF32);

            var sb = new StringBuilder();
            sb.AppendLine(txtChecker.Mark);
            sb.AppendLine("Just for test");
            var bytes = Encoding.UTF32.GetBytes(sb.ToString());

            Assert.AreEqual(true, checker.Check(TestData1.MessageBytes));
            Assert.AreEqual(false, checker.Check(bytes));
        }
    }
}