using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Tests.ExtensionTests
{
    [TestClass]
    public class When_Padding_The_Results_Of_A_GoogleSheet_Pad_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_The_Same_List_For_A_1_By_1_Integer_List()
        {
            IList<IList<object>> paddingTarget = new List<IList<object>>() { new List<object>() { 1 } };

            var paddingResult = paddingTarget.Pad();

            paddingResult[0][0].ShouldBe(1);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_2_By_2_List_For_A_2_By_1_Integer_List()
        {
            IList<IList<object>> paddingTarget = new List<IList<object>>() { new List<object>() { "A1" }, new List<object>() { "B1","B2" } };

            var paddingResult = paddingTarget.Pad();

            paddingResult[0][1].ShouldBe(string.Empty);
        }
}
}
