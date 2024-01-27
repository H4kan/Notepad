using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notepad.Api.Admin
{
    public static class CreateAdminToken
    {

        public static string Create(string jwtKey)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "admin")
            };

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));


            var cred = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha512Signature);

            // admin token is unexpirable
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddYears(99),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
