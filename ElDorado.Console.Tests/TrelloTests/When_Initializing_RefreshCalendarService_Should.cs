using ElDorado.Console.Trello;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;

namespace ElDorado.Console.Tests.TrelloTests
{
    [TestClass]
    public class When_Initializing_RefreshCalendarService_Should
    {
        private static readonly string FileLines = $"TrelloAppKey:Key{Environment.NewLine}TrelloUserToken:Token";

        private RefreshSynchronizer Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            Target = new RefreshSynchronizer();

            Target.Initialize(new CredentialStore(FileLines));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Serializer_To_ManateeSerializer()
        {
            TrelloConfiguration.Serializer.ShouldBeOfType<ManateeSerializer>();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Deserializer_To_ManateeSerializer()
        {
            TrelloConfiguration.Deserializer.ShouldBeOfType<ManateeSerializer>();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_JsonFactory_To_New_Manatee_Factory()
        {
            TrelloConfiguration.JsonFactory.ShouldBeOfType<ManateeFactory>();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_RestClientProvider_To_WebApiClientProvider()
        {
            TrelloConfiguration.RestClientProvider.ShouldBeOfType<WebApiClientProvider>();
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Default_App_Key_To_Passed_In_Credentials()
        {
            TrelloAuthorization.Default.AppKey.ShouldBe("Key");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_Default_UserToken_According_To_Passed_In_CredentialFileLines()
        {
            TrelloAuthorization.Default.UserToken.ShouldBe("Token");
        }
    }
}
