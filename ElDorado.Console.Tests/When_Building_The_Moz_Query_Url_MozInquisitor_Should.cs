using ElDorado.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;

namespace ElDorado.Console.Tests
{
    [TestClass]
    public class When_Building_The_Moz_Query_Url_MozInquisitor_Should
    {
        private const string IdLine = "ID:sample-id";
        private const string SecretLine = "Secret:1234567890";

        private const string BaseUrl = "daedtech.com";

        private static readonly CredentialStore Credentials = new CredentialStore($"{IdLine}\r\n{SecretLine}");

        private MozInquisitor Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new MozInquisitor(Credentials, Mock.Create<SimpleWebClient>());
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Build_A_Url_With_The_Base_MozEndpoint()
        {
            var url = Target.BuildUrl(BaseUrl);

            url.ShouldContain("http://lsapi.seomoz.com/linkscape/url-metrics/");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Build_A_Url_With_The_Target_BaseUrl()
        {
            var url = Target.BuildUrl(BaseUrl);

            url.ShouldContain(BaseUrl);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Build_A_Url_Containing_The_Query_Parameter_For_All_Of_The_Moz_Columns()
        {
            var url = Target.BuildUrl(BaseUrl);

            url.ShouldContain("?Cols=288230376151711743");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Build_A_Url_Containing_An_AccessId_QueryParameter_Value_Pair()
        {
            var url = Target.BuildUrl(BaseUrl);

            url.ShouldContain($"&AccessID={IdLine.Split(':')[1]}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Build_A_Url_Containing_A_Five_Minute_Timeout_In_Seconds()
        {
            var time = new DateTime(2018, 1, 31);
            Target.CurrentTime = time;

            var url = Target.BuildUrl(BaseUrl);

            var unixSecondsFiveMinutesFromNow = time.AddMinutes(5).ToUnixTime();

            url.ShouldContain($"&Expires={unixSecondsFiveMinutesFromNow}");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Build_A_Url_Containing_The_Url_Encoded_Secret()
        {
            var time = new DateTime(2018, 1, 31);
            Target.CurrentTime = time;

            var url = Target.BuildUrl(BaseUrl);

            url.ShouldContain("&Signature=dAm3iMTSkcj3kxwHXODpiWXA7EM%3d");
        }


}

}
