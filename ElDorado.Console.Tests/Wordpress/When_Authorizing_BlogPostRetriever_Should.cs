﻿using ElDorado.Wordpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ElDorado.Console.Tests.Wordpress
{
    [TestClass]
    public class When_Authorizing_BlogPostRetriever_Should
    {
        private const string Username = "erik";
        private const string Password = "badpass";
        private const string Token = "tok";
        private static readonly string RawResponseJson = $"{{\"token\":\"{Token}\"}}";

        private SimpleWebClient Client { get; set; } = Mock.Create<SimpleWebClient>();

        private BlogPostRetriever Target;

        [TestInitialize]
        public void BeforeEachTest()
        {
            Client.Arrange(cl => cl.GetRawResultOfBasicPostRequest($"{BlogPostRetriever.AuthEndpoint}?username={Username}&password={Password}")).Returns(RawResponseJson);

            Target = new BlogPostRetriever(Client);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Set_The_Token_To_Response_From_Basic_PostRequest()
        {
            Target.AuthorizeUser(Username, Password);

            Target.Token.ShouldBe(Token);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Throw_An_Authorization_Exception_On_An_Invalid_Json_Response()
        {
            Client.Arrange(cl => cl.GetRawResultOfBasicPostRequest($"{BlogPostRetriever.AuthEndpoint}?username={Username}&password={Password}")).Returns("asdf");

            Should.Throw<WordpressAuthorizationException>(() => Target.AuthorizeUser(Username, Password));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Encode_The_Password()
        {
            const string password = "badpass&";

            Client.Arrange(cl => cl.GetRawResultOfBasicPostRequest(Arg.AnyString)).Returns(RawResponseJson);

            Target.AuthorizeUser(Username, password);

            Client.Assert(cl => cl.GetRawResultOfBasicPostRequest($"{BlogPostRetriever.AuthEndpoint}?username={Username}&password=badpass%26"), Occurs.Once());
        }
}
}
