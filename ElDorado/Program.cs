using Gold.ConsoleMenu;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ElDorado
{
    class Program
    {
        static void Main(string[] args)
        {
            MenuGenerator.Run();
        }
    }
}
