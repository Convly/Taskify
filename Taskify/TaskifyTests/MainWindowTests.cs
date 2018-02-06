using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taskify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void GetPriorityFromStringTest()
        {
            Assert.AreEqual(MainWindow.GetPriorityFromString("Prioritary"), 0);
            Assert.AreEqual(MainWindow.GetPriorityFromString("High"), 1);
            Assert.AreEqual(MainWindow.GetPriorityFromString("Medium"), 2);
            Assert.AreEqual(MainWindow.GetPriorityFromString("Low"), 3);
            Assert.AreEqual(MainWindow.GetPriorityFromString("Common"), 4);
        }

        [TestMethod()]
        public void GetPriorityFromIntTest()
        {
            Assert.AreEqual(MainWindow.GetPriorityFromInt(0), "Prioritary");
            Assert.AreEqual(MainWindow.GetPriorityFromInt(1), "High");
            Assert.AreEqual(MainWindow.GetPriorityFromInt(2), "Medium");
            Assert.AreEqual(MainWindow.GetPriorityFromInt(3), "Low");
            Assert.AreEqual(MainWindow.GetPriorityFromInt(4), "Common");
        }
    }
}