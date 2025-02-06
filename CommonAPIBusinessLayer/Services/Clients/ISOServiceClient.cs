using CommonAPIBusinessLayer.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIBusinessLayer.Services.Clients
{
    public class ISOServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ISOConfig _config;

        public ISOServiceClient(HttpClient httpClient, IOptions<ISOConfig> configOptions)
        {
            _httpClient = httpClient;
            _config = configOptions.Value;
        }

        public async Task<string> CallISOAsync(string requestXml)
        {
            var content = new StringContent(requestXml, Encoding.UTF8, "text/xml");
            _httpClient.DefaultRequestHeaders.Add("SOAPAction", "SomeAction"); // Set correct SOAP action

            var response = await _httpClient.PostAsync(_config.ISOListener, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
