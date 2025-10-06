using application.Interfaces;
using domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using System.Runtime.Versioning;

namespace orchestrator.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
      
        
        private readonly ITemplate _template;
        private readonly IValidator<Template> _validatorTemplate;
        private readonly IValidator<Target> _validatiorTarget;       
        private List<string> errors;

        public TemplateController(ITemplate template, 
            IValidator<Template> validatorTemplate, 
            IValidator<Target> validatiorTarget
            ) 
        {          
            _template = template;
            _validatorTemplate = validatorTemplate;
            _validatiorTarget = validatiorTarget;
            this.errors= new List<string>();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ReceiveTemplate(Template template)
        {
            
            var validationTemplate= _validatorTemplate.Validate(template);
            var validationTarget = _validatiorTarget.Validate(template.target);

            if (!validationTemplate.IsValid || !validationTarget.IsValid) 
            {
                SetErrors(validationTarget?.Errors);
                SetErrors(validationTemplate?.Errors);
                return Ok(GetResponse("La plantilla contiene campos vacios", false, this.errors));
            }
                
            var response=await _template.ReceiveTemplate(template);
            return Ok(response);
        }

        public GenericResponse GetResponse(string message, bool isSuccess, List<string> errors)
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
                response.Error = true;
                response.Errors = errors;
            }

            return response;
        }

        private void SetErrors(List<ValidationFailure> errors)
        {
            errors.ForEach(e => {
            this.errors.Add(e.ErrorCode + "-"+e.ErrorMessage);
            });
        }

    }
}
