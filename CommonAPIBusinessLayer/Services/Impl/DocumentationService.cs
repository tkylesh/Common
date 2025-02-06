using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using Newtonsoft.Json;
using System.Configuration;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class DocumentationService : IDocumentationService
    {
        public void GetAlfaMembershipIdCard(byte[] pdf, int quoteId, string policyNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DocumentationAPIBaseUri"] + "documents/alfaIdCard");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Newtonsoft.Json.Linq.JObject raterVars = new Newtonsoft.Json.Linq.JObject
                {
                    { "quoteId", quoteId },
                    { "policyNumber", policyNumber },
                    { "pdf", pdf }
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(raterVars), Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(client.BaseAddress.ToString(), stringContent).Result;
            }
        }
    }
}
