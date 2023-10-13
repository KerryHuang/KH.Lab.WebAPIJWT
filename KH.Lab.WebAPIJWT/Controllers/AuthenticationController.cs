using KH.Lab.WebAPIJWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KH.Lab.WebAPIJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login user)
        {
            if (user is null)
            {
                return BadRequest("Invalid user request!!!");
            }
            if (user.UserName == "string" && user.Password == "string")
            {
                var key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JsonWebTokenKeys:IssuerSigningKey"]);
                DateTime expireTime = DateTime.Now.AddMinutes(double.Parse(ConfigurationManager.AppSetting["JsonWebTokenKeys:ExpirationTime"]));

                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JsonWebTokenKeys:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JsonWebTokenKeys:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: new DateTimeOffset(expireTime).DateTime,
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new JWTTokenResponse { Token = tokenString });
            }
            return Unauthorized();
        }
    }
}
