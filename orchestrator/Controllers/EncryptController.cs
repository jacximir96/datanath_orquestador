using application.Interfaces;
using domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace orchestrator.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EncryptController : ControllerBase
    {
        private readonly IEncryptService _encryptService;   
        public EncryptController(IEncryptService encryptService) 
        { 
            _encryptService = encryptService;
        }

        [HttpPost]
        public IActionResult Encrypt(User user) 
        {
            try
            {
                var encrypted = _encryptService.Encrypt(user.Password);
                return Ok(encrypted);   
            }
            catch (Exception e)
            { 
                return BadRequest();   
            }
        }

        [HttpPost]
        public IActionResult Decrypt(User user) 
        {
            try
            {
                var decrypted = _encryptService.Decrypt(user.Password);
                return Ok(decrypted);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
