using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace ElDorado.Metrics
{
    public class MozInquisitor
    {
        public struct MozRecord
        {
            public decimal DomainAuthority { get; private set; }
            public int LinkingDomains { get; private set; }

            public MozRecord(decimal domainAuthority, int linkingDomains)
            {
                DomainAuthority = domainAuthority;
                LinkingDomains = linkingDomains;
            }
        }

        private readonly CredentialStore _credentials;
        private readonly SimpleWebClient _simpleWebClient;

        public DateTime CurrentTime { get; set; } = DateTime.Now;

        public int Timeout { get; set; } = 0;

        public MozInquisitor(CredentialStore credentials, SimpleWebClient simpleWebClient)
        {
            _simpleWebClient = simpleWebClient;
            _credentials = credentials;
        }

        public virtual MozRecord GetMozStats(string host)
        {
            Thread.Sleep(Timeout * 1000);
            try
            {
                var authenticatedUrl = BuildUrl(host);
                var rawText = _simpleWebClient.GetRawText(authenticatedUrl);

                dynamic mozRowJson = JsonConvert.DeserializeObject(rawText);
                return new MozRecord(Math.Round((decimal)mozRowJson.pda, 2), (int)mozRowJson.pid);
            }
            catch
            {
                return new MozRecord(0, 0);
            }
        }

        public string BuildUrl(string baseUrl)
        {
            var accessId = _credentials["ID"];
            var secret = _credentials["Secret"];
            var expiration = CurrentTime.AddMinutes(5).ToUnixTime();

            var bytesToHash = Encoding.ASCII.GetBytes($"{accessId}\n{expiration}");
            var signatureCode = GetSignatureCode(bytesToHash, secret);

            return $"http://lsapi.seomoz.com/linkscape/url-metrics/{baseUrl}?Cols=288230376151711743&AccessID={accessId}&Expires={expiration}&Signature={signatureCode}";
        }

        private static String GetSignatureCode(byte[] bytesToHash, string secret)
        {
            using (HMACSHA1 hashCalculator = new HMACSHA1(Encoding.ASCII.GetBytes(secret)))
            {
                var hash = hashCalculator.ComputeHash(bytesToHash);
                var hashedBytes = Convert.ToBase64String(hash);

                return HttpUtility.UrlEncode(hashedBytes);
            }
        }


    }
}
