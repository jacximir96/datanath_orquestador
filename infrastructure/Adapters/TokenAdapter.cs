using domain.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Adapters
{
    public class TokenAdapter : ITokenAdapter
    {
        
        public string GenerateToken(string username, int expireMinutes = 20)
        {
            JwtSecurityTokenHandler securityTokenHandler = null;
            JwtSecurityToken token = null;
            try
            {
                var claims = new List<Claim>{
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, "Administrator")
                        //new Claim(ClaimTypes., username),
                        //new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                           };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ResourceInfra.secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    token = new JwtSecurityToken(
                    issuer: ResourceInfra.issuer,
                    audience: ResourceInfra.audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(expireMinutes)),
                    signingCredentials: creds
                );
                securityTokenHandler = new JwtSecurityTokenHandler();
                
            }
            catch (Exception e) 
            {
                e.Message.ToString();
            }
            return securityTokenHandler.WriteToken(token);
        }
    }
}
