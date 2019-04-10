using ElDorado.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests
{
    [TestClass]
    public class When_Searching_ForAlexaRank_AlexaDataIniquisitor_Should
    {
        private const int DaedTechRank = 488542;
        private const string BaseSite = "daedtech.com";
        private static readonly string Response = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\n<!--Need more Alexa data? Find our APIs here: https://aws.amazon.com/alexa/ -->\n<ALEXA VER =\"0.9\" URL=\"{BaseSite}/\" HOME=\"0\" AID=\"=\" IDN=\"daedtech.com/\">\n<RLS PREFIX =\"http://\" more=\"0\">\n</RLS>\n<SD TITLE =\"A\" FLAGS=\"\" HOST=\"daedtech.com\">\n</SD>\n<SD><POPULARITY URL =\"daedtech.com/\" TEXT=\"{DaedTechRank}\" SOURCE=\"panel\"/><REACH RANK=\"405492\"/><RANK DELTA=\"+139728\"/><COUNTRY CODE=\"US\" NAME=\"United States\" RANK=\"300332\"/></SD></ALEXA>";

        private SimpleWebClient Client = Mock.Create<SimpleWebClient>();

        private AlexaDataInquisitor Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Client.Arrange(cl => cl.GetRawResultOfBasicGetRequest("http://data.alexa.com/data?cli=10&dat=snbamz&url=daedtech.com")).Returns(Response);
            Target = new AlexaDataInquisitor(Client);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_For_Nonsense()
        {
            var rank = Target.GetGlobalRank(string.Empty);

            rank.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_()
        {
            var rank = Target.GetGlobalRank(BaseSite);

            rank.ShouldBe(DaedTechRank);
        }

    }

}
