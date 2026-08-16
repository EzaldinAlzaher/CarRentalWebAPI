using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRental_WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        // Authentication by token

        [HttpPost("login")]
        public ActionResult<string> Login(AuthRequest auth)
        {
            // validate username and password
            DemoUser user = ValidateUsernameAndPassword(auth.Username, auth.Password);

            if (user == null)
                return Unauthorized("User is not exists");

            // Generate JWT

            // get key 
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                configuration["Authentication:SecurityKey"]
                ));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            // add claims - payload
            var claims = new List<Claim>();

            claims.Add(new Claim("sub", user.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Username));
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));

            claims.Add(new Claim(ClaimTypes.Role, user.Role));


            var securityToken = new JwtSecurityToken(
                configuration["Authentication:Issuer"],
                configuration["Authentication:Audiance"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(int.Parse(configuration["Authentication:ExpiryAsHours"])),
                signingCred
                );


            // serialize token object to string and return

            var serializedToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            // add token to header
            Response.Headers.Add("token", serializedToken);

            return serializedToken;
        }

        private DemoUser? ValidateUsernameAndPassword(string username, string password)
        {
            // نحكي مع قاعدة البيانات ونتحقق من المستخدم ومن ثم نتخذ قرار
            // هذه مجرد محاكاة 

            if (username == configuration["Authentication:Username"]
                && password == configuration["Authentication:Password"])
                return new DemoUser() { UserId = 1, Username = "eziii@gmail.com", Role = "Admin", FirstName = "Ezaldin", LastName = "Alzaher" };
            else
                return null;
        }
    }


    public class AuthRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class DemoUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Role { get; set; }
    }
}
