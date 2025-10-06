using application.Interfaces;
using Azure;
using domain.Entities;
using domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace application.Services
{
    public class TemplateService : ITemplate
    {

        private readonly ITemplateLogDomain _template;
        private readonly ITemplateDomain _templateDomain;
        private readonly IConfiguration _config;
        private readonly INatsManagement _nats;

        public TemplateService(ITemplateLogDomain template,
            ITemplateDomain templateDomain, 
            IConfiguration config,
            INatsManagement nats
            ) 
        { 
            _template = template;
            _templateDomain = templateDomain;  
            _config = config;
            _nats = nats;

        }

        public async Task<GenericResponse> ReceiveTemplate(Template template)
        {
            GenericResponse response = null;
            string syncId=string.Empty;
            try
            {
                _template.GenerateLog($"se validó con exito la plantilla con exito");               
                response=await _templateDomain.SendDataManager(template);
                if (!response.Error) 
                {
                    template.id=response.RequestId;
                    await _nats.SendMessage(template);
                }
                    
               return response;
            }
            catch(Exception e) 
            {
                response = GetResponse("Bad Request", false);
            }
            return await Task.FromResult(response);
        }

        public GenericResponse GetResponse(string message, bool isSuccess) 
        {
            GenericResponse response = new GenericResponse();
            if (isSuccess)
            {
                response.Message = message;
                response.Code = 200;             
            }
            else 
            {
                response.Message = message;
                response.Code = 400;             
            }

            return response;    
        }
    }
}
