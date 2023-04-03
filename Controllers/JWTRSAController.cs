using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Authentication_JWT_RSA.Controllers
{
    public class JWTRSAController: ControllerBase
    {
        private readonly SecurityOptions options;

        public JWTRSAController(SecurityOptions options)
        {
            this.options = options;
        }

        [HttpGet(nameof(Generate))]
        public async Task<IActionResult> Generate()
        {
            var rsa = RSA.Create();
            string key = await System.IO.File.ReadAllTextAsync(options.PrivateKeyFilePath);
            rsa.FromXmlString(key);

            var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            var jwt = new JwtSecurityToken(new JwtHeader(credentials), new JwtPayload(issuer:"webapi",audience:"webapi",claims: new List<Claim>(),notBefore: DateTime.Now,expires: DateTime.Now.AddHours(3)));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(token);
        }

        [Authorize]
        [HttpGet(nameof(Verify))]
        public async Task<IActionResult> Verify()
        {
            return Ok();
        }
    }
}
