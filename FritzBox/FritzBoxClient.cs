using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using RaspPiTest.FritzBox.Model;

namespace RaspPiTest.FritzBox
{
    public class FritzBoxClient
    {
        private const string Username = "pi";
        private const string Password = "waterrat";

        private bool _isRetrying;
        private string _sessionId;
        private bool IsInitialized => !string.IsNullOrEmpty(_sessionId);

        public async Task<T> ReadPageAsync<T>(string url) where T : class
        {
            if (!IsInitialized) _sessionId = await GetSessionIdAsync(Username, Password);

            using (var client = new HttpClient())
            {
                url = url.Contains("?") ? 
                    $"{url}&sid={_sessionId}" : 
                    $"{url}?sid={_sessionId}";

                var response = await client.GetAsync(new Uri(url));
                try
                {
                    response.EnsureSuccessStatusCode();

                    if (typeof(T) == typeof(string))
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return result as T;
                    }

                    using (var responseContent = await response.Content.ReadAsStreamAsync())
                    {
                        if(responseContent.Length == 0) return default(T);
                        XDocument xDoc = XDocument.Load(responseContent);
                        responseContent.Seek(0, SeekOrigin.Begin);

                        var serializer = new XmlSerializer(typeof(T));
                        return (T) serializer.Deserialize(responseContent);
                    }
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                catch
                {
                    if (_isRetrying)
                    {
                        _isRetrying = false;
                        throw;
                    }
                    _isRetrying = true;
                    _sessionId = string.Empty;
                    return await ReadPageAsync<T>(url);
                }
            }
        }

        private async Task<string> GetSessionIdAsync(string username, string password)
        {
            using (var client = new HttpClient())
            using (var xmlStream = await client.GetStreamAsync(@"http://fritz.box/login_sid.lua"))
            using (var reader = XmlReader.Create(xmlStream))
            {
                var sessionInfo = new SessionInfoResponse();
                sessionInfo.ReadXml(reader);
                if (sessionInfo.IsSessionIdSet) return sessionInfo.SessionId;
                using (var loginStream = await client.GetStreamAsync(@"http://fritz.box/login_sid.lua?username=" + username + "&response=" + SolveChallenge(sessionInfo.Challenge, password)))
                using (var loginReader = XmlReader.Create(loginStream))
                {
                    sessionInfo.ReadXml(loginReader);
                    return sessionInfo.SessionId;
                }
            }
        }

        private static string SolveChallenge(string challenge, string password)
            => $"{challenge}-{Md5Hash(challenge + "-" + password)}";

        private static string Md5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.Unicode.GetBytes(input));
                return string.Join(string.Empty, result.Select(p => p.ToString("x2")));
            }
        }
    }
}