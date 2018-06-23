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
    public class When_Loose_Matching_LooselyMatches_Should
    {
        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_For_Empty_And_Empty()
        {
            string.Empty.TrelloCardLooselyMatches(string.Empty).ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_False_For_A_And_B()
        {
            "A".TrelloCardLooselyMatches("B").ShouldBeFalse();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_For_Upper_And_Lower_Case_A()
        {
            "A".TrelloCardLooselyMatches("a").ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_For_Upper_And_Lower_Case_A_Reversed()
        {
            "a".TrelloCardLooselyMatches("A").ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_The_Target_Contains_The_Match_Candidate()
        {
            "A ".TrelloCardLooselyMatches("A").ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_Removing_Extraneous_Double_Spaces_From_Candidate()
        {
            "A Post".TrelloCardLooselyMatches("A  Post").ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_Removing_The_2X_Distinction()
        {
            "A Post (2x)".TrelloCardLooselyMatches("A Post").ShouldBeTrue();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_True_When_Spreadsheet_Has_Trailing_Whitespace()
        {
            "[N] Barriers to Successful Enterprise Release Management".TrelloCardLooselyMatches("[N] Barriers to Successful Enterprise Release Management");
        }

    }
}
