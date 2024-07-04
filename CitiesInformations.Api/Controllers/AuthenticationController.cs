using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CitiesInformations.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }

            public string? Password { get; set; }
        }


        public class CityInfoUser
        {
            public int UserId { get; set; }

            public string UserName { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string City { get; set; }


            public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
            {
                UserId = userId;

                UserName = userName;

                FirstName = firstName;

                LastName = lastName;
                City = city;

            }
        }

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
            AuthenticationRequestBody authenticationRequestBody)
        {
            // step one validate the username/password

            var user = ValidateUserCredentials(authenticationRequestBody.UserName,
                authenticationRequestBody.Password);


            if(user == null)
            {
                return Unauthorized();
            }


            // step two Create a Token

            var securityKey = new SymmetricSecurityKey(

                Convert.FromBase64String(configuration["Authentication :SecretForKey"]));
            var signingCredentials =new SigningCredentials(securityKey,
                
                SecurityAlgorithms.HmacSha256);


            // the claims that 

            var claimsForToken = new List<Claim>();

            claimsForToken.Add(new Claim("Sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("FamilyName", user.LastName));
            claimsForToken.Add(new Claim("City", user.City));


            var jwtSecurityToken = new JwtSecurityToken(

                configuration["Authentication:Issure"],
                configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
                );

            var tokenReturn = new JwtSecurityTokenHandler().
                WriteToken(jwtSecurityToken);

            return Ok(tokenReturn);
        }

        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            // we don't have a user DB or table

            return new CityInfoUser(

                1,
                userName ?? "",
                "Kevin",
                "Dock",
                "Antwerp"

            );
            
        }
    }
}
