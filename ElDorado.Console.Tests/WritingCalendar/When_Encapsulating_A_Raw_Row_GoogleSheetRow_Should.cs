using ElDorado.Console.WritingCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElDorado.Console.Tests.WritingCalendar
{

    [TestClass]
    public class When_Encapsulating_A_Raw_Row_GoogleSheetRow_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_EmptyString_For_Out_Of_Range_Index()
        {
            var row = new GoogleSheetRow(new List<object>());

            row.Item(0).ShouldBe(string.Empty);
        }
}
}
