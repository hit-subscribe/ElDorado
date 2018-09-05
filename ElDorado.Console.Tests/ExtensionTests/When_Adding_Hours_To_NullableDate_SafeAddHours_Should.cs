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
    public class When_Adding_Hours_To_NullableDate_SafeAddHours_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Add_The_Correct_Hours()
        {
            DateTime? targetDate = (DateTime?)new DateTime(2018, 9, 1);

            targetDate.SafeAddHours(12).ShouldBe(targetDate.Value.AddHours(12));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_Null_For_Null_Input()
        {
            ((DateTime?)null).SafeAddHours(12).ShouldBeNull();
        }
}
}
