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
    public class When_Converting_DateTimes_SafeToDateTime_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Null_For_Null_Input()
        {
            ((DateTime?)null).SafeToDateTime().ShouldBe(null);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Matching_Date()
        {
            "12/25/2018".SafeToDateTime().ShouldBe(DateTime.Parse("12/25/2018"));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Null_For_Empty_String()
        {
            string.Empty.SafeToDateTime().ShouldBe(null);
        }
}
}
