using Hr.Application.Common;
using Hr.Application.DTOs.Authentication;
using Hr.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hr.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager , 
            IConfiguration config , 
            RoleManager<IdentityRole> roleManager
            ,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.config = config;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                if (userDto.EmailOrUserName != null)
                {
                      user = await userManager.FindByNameAsync(userDto.EmailOrUserName);
                    if (user == null)
                    {
                        user = await userManager.FindByEmailAsync(userDto.EmailOrUserName);
                    }
                }
                
               
                if (user != null)
                {
                    bool result = await userManager.CheckPasswordAsync(user, userDto.Password);
                    if (result)
                    {

                        //Claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, userDto.EmailOrUserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        //get role
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            //if (role == SD.Roles.SuperAdmin.ToString())
                            //{
                            //    claims.Add(new Claim(ClaimTypes.Role, role));
                            //}
                            //else
                            //{
                                var permissions = await roleManager.GetClaimsAsync(await roleManager.FindByNameAsync(role));
                                foreach (var permission in permissions)
                                {
                                    claims.Add(new Claim(permission.Type, permission.Value));
                                }
                               
                            //}
                           
                        }

                        SecurityKey securityKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));

                        SigningCredentials signincred =
                            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                        //Create token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],//url web api
                            audience: config["JWT:ValidAudiance"],//url consumer angular
                            claims: claims,
                            expires: DateTime.Now.AddHours(24),
                            signingCredentials: signincred
                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = mytoken.ValidTo
                        });
                    }

                }
                return Unauthorized( new { message ="Email or User Name Not Exist "});
            }
            return Unauthorized(new { message = "Invaild Email or Password" });
        }

        [HttpPost("Logout")]
        [Authorize]  
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();  
            return NoContent();  
        }
    }
    

}
