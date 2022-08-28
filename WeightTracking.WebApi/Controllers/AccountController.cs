using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WeightTracking.DataAccess.Entities;
using WeightTracking.WebApi.ViewModels;

namespace WeightTracking.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new IdentityUser { UserName = registerVM.UserName };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Errors?.FirstOrDefault()?.Description });

            return Ok(new
            {
                username = user.UserName,
                access_token = GenerateToken(user.UserName)
            });
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user == null)
                return NotFound(new { error = "user not found" });

            var result = await _userManager.CheckPasswordAsync(user, loginVM.Password);

            if (!result)
                return BadRequest(new { error = "wrong password" });

            return Ok(new
            {
                username = user.UserName,
                access_token = GenerateToken(user.UserName)
            });
        }

        private string GenerateToken(string userName)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:DurationInMinutes"])),
                    signingCredentials: signingCredentials);

            var jwtTokenHandler = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return jwtTokenHandler;
        }
    }
}

