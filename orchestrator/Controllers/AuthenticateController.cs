using application.Interfaces;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace orchestrator.Controllers
{
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        
        private readonly ITokenService _tokenService;

        public AuthenticateController(ITokenService tokenService) 
        { 
            _tokenService = tokenService;
        }

      
        [HttpPost]
        public IActionResult GetToken(User user)
        {
            try
            {
                GenericResponse response=_tokenService.GenerateToken(user.Name, user.Password);
                return Ok(response.token);   
            }
            catch(Exception e) 
            {
                e.Message.ToString();
                return BadRequest();
            }
        }
    }
}
