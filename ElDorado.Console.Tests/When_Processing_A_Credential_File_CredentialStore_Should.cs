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
    public class When_Processing_A_Credential_File_CredentialStore_Should
    {
        private const string CredentialFileContents = "ID:an-id\nSecret:a-secret";

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Match_First_Line_Key_To_Value()
        {
            var target = new CredentialStore(CredentialFileContents);

            //target["ID"].ShouldBe("an-id");
        }
}
    public struct CredentialStore
    {
        //public string this[string i]
        //{
        //    get { return "an-id"; }
        //}
        public CredentialStore(string credentialFileContents)
        {
            
        }
    }
}
