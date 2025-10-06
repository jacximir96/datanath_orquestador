using domain.Entities;
using domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace infrastructure.Adapters
{
    public class TemplateAdapter : ITemplateLogDomain, ITemplateDomain
    {
        private readonly ILogger<TemplateAdapter> _logger;
        private readonly IConfiguration _configuration;

        public TemplateAdapter(ILogger<TemplateAdapter> logger, IConfiguration configuration) 
        { 
            _logger = logger;
            _configuration = configuration;
        }

        public void GenerateLog(string message)
        {
            try
            {
                _logger.LogWarning(message); 
               
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
            }
        }

        public async Task<GenericResponse> SendDataManager(Template template) 
        {
            GenericResponse resDomain = null;
            try
            {
                using (var client = new HttpClient()) 
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, _configuration.GetSection("urldatamanager").Value);
                    var json = JsonConvert.SerializeObject(template);
                    var content = new StringContent(json, null, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var res = await response.Content.ReadAsStringAsync();
                    resDomain = JsonConvert.DeserializeObject<GenericResponse>(res);
                }
                    
            }
            catch (Exception e) 
            {
                resDomain = new GenericResponse();
                resDomain.Error = true;
                resDomain.Message = e.Message;
                resDomain.Code = 400;              
            }

            return resDomain;
        }

    }
}
