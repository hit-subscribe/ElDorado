using ElDorado.Domain;
using ElDorado.WritingCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.WritingCalendar
{
    [TestClass]
    public class When_Writing_To_A_ClientPostPlan_ClientSpreadsheetService_Should
    {
        private const string Title = "A Blog Post";
        private const string Mission = "To boldly go...";
        private static readonly DateTime PubDate = new DateTime(2018, 12, 1);

        private BlogPost Post { get; } = new BlogPost() { Title = Title, Mission = Mission, TargetPublicationDate = PubDate };

        private GoogleSheet Sheet { get; } = Mock.Create<GoogleSheet>();

        private ClientSpreadsheetService Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new ClientSpreadsheetService(Sheet);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Column_0_To_Title()
        {
            Target.AddPosts(Post.AsEnumerable());

            Sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(grid => grid[0][0].ToString() == Title)));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Column_3_To_Mission()
        {
            Target.AddPosts(Post.AsEnumerable());

            Sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(grid => grid[0][3].ToString() == Mission)));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Column_4_To_PublicationDate()
        {
            Target.AddPosts(Post.AsEnumerable());

            Sheet.Assert(s => s.UpdateSpreadsheet(Arg.AnyString, Arg.Matches<IList<IList<object>>>(grid => grid[0][4].ToString() == PubDate.ToString())));
        }
    }
}
