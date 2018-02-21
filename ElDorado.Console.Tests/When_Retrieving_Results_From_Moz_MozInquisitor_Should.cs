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
    public class When_Retrieving_Results_From_Moz_MozInquisitor_Should
    {
        private const int LinkingDomains = 330;
        private const decimal DomainAuthority = 39.52576925096734M;
        private static readonly string CallResult = $"{{\"fed\":16,\"feid\":6,\"fejp\":3.168980105739845,\"fejr\":3.3356279576524235e-14,\"fem\":\"\",\"ffb\":\"\",\"ffspl0\":2,\"ffspl1\":0,\"ffspl10\":0,\"ffspl11\":0,\"ffspl12\":0,\"ffspl13\":0,\"ffspl14\":0,\"ffspl15\":0,\"ffspl16\":0,\"ffspl17\":0,\"ffspl2\":3,\"ffspl3\":2,\"ffspl4\":0,\"ffspl5\":1,\"ffspl6\":0,\"ffspl7\":0,\"ffspl8\":0,\"ffspl9\":1,\"fg+\":\"\",\"fid\":12,\"fipl\":10,\"fjd\":6,\"fjf\":7,\"fjid\":27,\"fjp\":3.7845976941276183,\"fjr\":3.3572934353852237e-13,\"flan\":\"\",\"fmrp\":3.9504394511372887,\"fmrr\":5.4392558715782993e-11,\"fspf\":0,\"fsplc\":1474361857,\"fspp\":\"\",\"fsps\":301,\"fspsc\":1,\"ftrp\":4.055773515118338,\"ftrr\":1.0045050348973165e-09,\"ftw\":\"\",\"fuid\":38,\"fuspl0\":2,\"fuspl1\":0,\"fuspl10\":0,\"fuspl11\":0,\"fuspl12\":0,\"fuspl13\":0,\"fuspl14\":0,\"fuspl15\":0,\"fuspl16\":0,\"fuspl17\":0,\"fuspl2\":7,\"fuspl3\":3,\"fuspl4\":0,\"fuspl5\":2,\"fuspl6\":0,\"fuspl7\":0,\"fuspl8\":0,\"fuspl9\":1,\"pda\":{DomainAuthority},\"pdar\":50.30369632118385,\"ped\":4887,\"peid\":4426,\"pejp\":5.6016373690315655,\"pejr\":3.0609785551050523e-10,\"pfspl0\":60,\"pfspl1\":45,\"pfspl10\":1,\"pfspl11\":0,\"pfspl12\":0,\"pfspl13\":0,\"pfspl14\":0,\"pfspl15\":0,\"pfspl16\":0,\"pfspl17\":0,\"pfspl2\":66,\"pfspl3\":67,\"pfspl4\":42,\"pfspl5\":24,\"pfspl6\":13,\"pfspl7\":4,\"pfspl8\":3,\"pfspl9\":2,\"pib\":236,\"pid\":{LinkingDomains},\"pip\":248,\"pjb\":176,\"pjd\":252,\"pjid\":38973,\"pjip\":186,\"pjp\":6.0093308441427356,\"pjr\":1.4124514124772733e-09,\"pmrp\":5.216003393316547,\"pmrr\":7.168754741599249e-08,\"ptrp\":5.261943490545638,\"ptrr\":1.9239891397924185e-07,\"puid\":39849,\"puspl0\":498,\"puspl1\":443,\"puspl10\":1,\"puspl11\":0,\"puspl12\":0,\"puspl13\":0,\"puspl14\":0,\"puspl15\":0,\"puspl16\":0,\"puspl17\":0,\"puspl2\":844,\"puspl3\":2402,\"puspl4\":203,\"puspl5\":65,\"puspl6\":116,\"puspl7\":239,\"puspl8\":7,\"puspl9\":5,\"ued\":1,\"ueid\":0,\"uemrp\":0,\"uemrr\":0.0,\"ufq\":\"daedtech.com/\",\"ufspl0\":0,\"ufspl1\":0,\"ufspl10\":0,\"ufspl11\":0,\"ufspl12\":0,\"ufspl13\":0,\"ufspl14\":0,\"ufspl15\":0,\"ufspl16\":0,\"ufspl17\":0,\"ufspl2\":1,\"ufspl3\":0,\"ufspl4\":0,\"ufspl5\":0,\"ufspl6\":0,\"ufspl7\":0,\"ufspl8\":0,\"ufspl9\":0,\"uib\":2,\"uid\":2,\"uifq\":2,\"uip\":2,\"uipl\":2,\"ujb\":1,\"ujfq\":1,\"ujid\":1,\"ujp\":1,\"ujpl\":1,\"ulc\":1508464130,\"umrp\":0,\"umrr\":0.0,\"upa\":33.82788143046639,\"upar\":-0.6176345246279871,\"upl\":\"daedtech.com/\",\"ur\":\"\",\"urid\":32551519297,\"urrid\":32551519297,\"us\":13,\"ut\":\"\",\"utrp\":0,\"utrr\":0.0,\"uu\":\"daedtech.com/\",\"uuspl0\":0,\"uuspl1\":0,\"uuspl10\":0,\"uuspl11\":0,\"uuspl12\":0,\"uuspl13\":0,\"uuspl14\":0,\"uuspl15\":0,\"uuspl16\":0,\"uuspl17\":0,\"uuspl2\":1,\"uuspl3\":0,\"uuspl4\":0,\"uuspl5\":0,\"uuspl6\":0,\"uuspl7\":0,\"uuspl8\":0,\"uuspl9\":0}}";

        private const string IdLine = "ID:sample-id";
        private const string SecretLine = "Secret:1234567890";

        private const string BaseUrl = "daedtech.com";

        private static readonly IEnumerable<string> FileLines = new List<string>() { IdLine, SecretLine };

        private SimpleWebClient Client = Mock.Create<SimpleWebClient>();

        private MozInquisitor Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new MozInquisitor(FileLines, Client);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_By_Default_For_DomainAuthority()
        {
            var domainAuthority = Target.GetMozStats(string.Empty).DomainAuthority;

            domainAuthority.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_39_Point_53_For_DomainAuthority_Using_SampleJson()
        {
            Client.Arrange(cl => cl.GetRawText(Arg.AnyString)).Returns(CallResult);

            var domainAuthority = Target.GetMozStats(BaseUrl).DomainAuthority;

            domainAuthority.ShouldBe(Decimal.Round(DomainAuthority, 2));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Call_The_Web_Client_With_The_Generated_Moz_Url_For_Domain_Authority()
        {
            var url = Target.BuildUrl("asdf");

            Target.GetMozStats("asdf");

            Client.Assert(cl => cl.GetRawText(url), Occurs.Once());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_0_By_Default_For_LinkingDomains()
        {
            var linkingDomains = Target.GetMozStats(string.Empty).LinkingDomains;

            linkingDomains.ShouldBe(0);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return()
        {
            Client.Arrange(cl => cl.GetRawText(Arg.AnyString)).Returns(CallResult);

            var linkingDomains = Target.GetMozStats(BaseUrl).LinkingDomains;

            linkingDomains.ShouldBe(LinkingDomains);
        }
}
}
