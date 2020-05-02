using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaspPiTest.FritzBox.Model;

namespace RaspPiTest.FritzBox
{
    public class FritzBoxClient
    {
        private readonly FritzBoxConnection _connection;

        public FritzBoxClient(ILoggerFactory loggerFactory, IOptions<FritzBoxConnection> options)
        {
            _logger = loggerFactory.CreateLogger<FritzBoxClient>();
            _connection = options.Value;
        }

        private bool _isRetrying;
        private string _sessionId;
        private readonly ILogger _logger;
        private bool IsInitialized => !string.IsNullOrEmpty(_sessionId);

        public async Task<T> ReadPageAsync<T>(string url) where T : class
        {
            _logger.LogDebug("reading Fritz!Box-Page at {url}, expecting {type}", url, typeof(T).Name);
            if (!IsInitialized) _sessionId = await GetSessionIdAsync(_logger, _connection.Username, _connection.Password);

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
                        var serializer = new XmlSerializer(typeof(T));
                        return (T) serializer.Deserialize(responseContent);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, "Fehler beim Auslesen der Antwort der Fritz!Box.");
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

        private static async Task<string> GetSessionIdAsync(ILogger logger, string username, string password)
        {
            logger.LogDebug("Versuche Anmeldung an Fritz!Box...");
            using (var client = new HttpClient())
            using (var xmlStream = await client.GetStreamAsync(@"http://fritz.box/login_sid.lua"))
            using (var reader = XmlReader.Create(xmlStream))
            {
                var sessionInfo = new SessionInfoResponse();
                sessionInfo.ReadXml(reader);
                if (sessionInfo.IsSessionIdSet) return sessionInfo.SessionId;
                using (var loginStream = await client.GetStreamAsync($@"http://fritz.box/login_sid.lua?username={username}&response={SolveChallenge(sessionInfo.Challenge, password)}"))
                using (var loginReader = XmlReader.Create(loginStream))
                {
                    sessionInfo.ReadXml(loginReader);
                    logger.LogDebug("Anmeldung erfolgreich. ({@session})", sessionInfo);
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