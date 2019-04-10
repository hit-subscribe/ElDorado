using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElDorado.Metrics
{
    public class AlexaDataInquisitor
    {
        private SimpleWebClient _client;

        public AlexaDataInquisitor(SimpleWebClient client)
        {
            _client = client;
        }

        public virtual int GetGlobalRank(string baseSiteUrl)
        {
            var rawXml =_client.GetRawResultOfBasicGetRequest($"http://data.alexa.com/data?cli=10&dat=snbamz&url={baseSiteUrl}");
            try
            {
                return GetRankFromXml(rawXml);
            }
            catch
            {
                return 0;
            }
        }

        private static int GetRankFromXml(string rawXml)
        {
            var document = XDocument.Parse(rawXml);
            return int.Parse(document.Descendants("POPULARITY").First().Attributes("TEXT").First().Value);
        }
    }
}
