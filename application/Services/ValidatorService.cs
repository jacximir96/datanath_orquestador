using application.Interfaces;
using domain.Entities;
using domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace application.Services
{
    public class ValidatorService
    {
        private GenericResponse response = null;
        private readonly IConfiguration _config;
        private readonly ITemplateLogDomain _templateLogDomain;
        private readonly ITemplateDomain _templateDomain;

        public ValidatorService(IConfiguration config, 
            ITemplateLogDomain templateLogDomain, 
            ITemplateDomain templateDomain) 
        { 
          response = new GenericResponse();
          _config = config;
          _templateLogDomain = templateLogDomain;
          _templateDomain = templateDomain;
        }

        public GenericResponse GetResponse(string message, bool isSuccess)
        {
            GenericResponse response = new GenericResponse();
            if (isSuccess)
            {
                response.Message = message;
                response.Error = false;
            }
            else
            {
                response.Message = message;         
                response.Error = true;
            }

            return response;
        }

    }
    }


