using DotNetAPI2.Data;
using DotNetAPI2.Dtos;
using DotNetAPI2.Models;
using DotNetAPI2.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetAPI2.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly ApplicationDbContext _db;
    private ApiResponse _response;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private string secretKey;

    public AuthController(ApplicationDbContext db, IConfiguration config,
    UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      _db = db;
      secretKey = config.GetValue<string>("ApiSettings:Secret");
      _response = new ApiResponse();
      _userManager = userManager;
      _roleManager = roleManager;

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
      ApplicationUser userfromdb = _db.ApplicationUsers
        .FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

      bool isValid = await _userManager.CheckPasswordAsync(userfromdb, model.Password);

      if (isValid == false)
      {
        _response.Result = new LoginResponseDto();
        _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        _response.ErrorMessages.Add("Username or password is incorrect");
        return BadRequest(_response);
      }

      //generate JWT Token
      var roles = await _userManager.GetRolesAsync(userfromdb);

      JwtSecurityTokenHandler tokenHandler = new();

      byte[] key = Encoding.ASCII.GetBytes(secretKey);



      SecurityTokenDescriptor tokenDescriptor = new()
      {
        Subject = new ClaimsIdentity(new Claim[]{
          new Claim("fullName", userfromdb.Name),
          new Claim("id", userfromdb.Id.ToString()),
          new Claim(ClaimTypes.Email, userfromdb.UserName.ToString()),
          new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
        )
      };

      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


      LoginResponseDto loginResponse = new()
      {
        Email = userfromdb.Email,
        Token = tokenHandler.WriteToken(token)
      };

      if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
      {
        _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        _response.ErrorMessages.Add("Username or password is incorect");
        return BadRequest(_response);
      }

      _response.StatusCode = System.Net.HttpStatusCode.OK;
      _response.IsSuccess = true;
      _response.Result = loginResponse;
      return Ok(_response);

    }




    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
    {

      ApplicationUser userfromdb = _db.ApplicationUsers
      .FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

      if (userfromdb != null)
      {
        _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        _response.ErrorMessages.Add("Username already exists");
        return BadRequest(_response);
      }

      ApplicationUser newUser = new()
      {
        UserName = model.UserName,
        Email = model.UserName,
        NormalizedEmail = model.UserName.ToUpper(),
        Name = model.Name //this is the custom field we made
      };
      try
      {


        var result = await _userManager.CreateAsync(newUser, model.Password);
        if (result.Succeeded)
        {
          if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
          {
            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
          }

          if (model.Role.ToLower() == SD.Role_Admin)
          {
            await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);

          }
          else
          {
            await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
          }

          _response.StatusCode = System.Net.HttpStatusCode.OK;
          _response.IsSuccess = true;
          return (Ok(_response));
        }
      }
      catch (Exception)
      {

      }
      _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
      _response.IsSuccess = false;
      _response.ErrorMessages.Add("Error while registering.");
      return BadRequest(_response);

    }

  }
}
