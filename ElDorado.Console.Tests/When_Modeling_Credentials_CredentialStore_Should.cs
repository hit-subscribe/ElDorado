using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Tests
{
    [TestClass]
    public class When_Modeling_Credentials_CredentialStore_Should
    {
        public const string CredentialFile = "ID:an-id\r\nSecret:a-secret";

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_The_First_Value_For_The_First_Key()
        {
            var target = new CredentialStore(CredentialFile);

            target["ID"].ShouldBe("an-id");
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_The_Second_Value_For_The_Second_Key()
        {
            var target = new CredentialStore(CredentialFile);

            target["Secret"].ShouldBe("a-secret");
        }
}

}
