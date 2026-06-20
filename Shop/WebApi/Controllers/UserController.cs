using Database.Entities;
using Database.Repositories;
using DTO.DTOEntities;
using DTO.Entities;
using DTO.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;

        private readonly IAppMappingProfile _appMappingProfile;

        private readonly ITokenControl _tokenControl;

        private readonly IPasswordHasher _passwordHasher;

        private readonly IConfiguration _configuration;

        public UserController(UserRepository userRepository, IAppMappingProfile appMappingProfile, ITokenControl tokenControl, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _appMappingProfile = appMappingProfile;
            _tokenControl = tokenControl;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        [HttpGet("getuser")]
        [Authorize]
        public ActionResult<User> GetUser()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            User user = _userRepository.GetUserWithFullCart(userId)!;
            UserDTO userDTO = _appMappingProfile.UserToUserDTO(user);
            return Ok(userDTO);
        }

        [HttpPost("reg")]
        public IActionResult Registration([FromBody] RegModel regModel)
        {
            User? user = _userRepository.GetByFunc(x => x.Email.CompareTo(regModel.Email) == 0);
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with this email already exsist");
                return BadRequest(ModelState);
            }

            User userDB = _appMappingProfile.RegModelToUser(regModel);
            userDB.Password = _passwordHasher.HashPassword(userDB.Password);

            if (userDB.Email == _configuration.GetValue<string>("AdminEmail"))
            {
                userDB.RoleId = UserRoles.Admin;
            }
            else
            {
                userDB.RoleId = UserRoles.User;
            }

            _userRepository.Add(userDB);

            var authResponse = _tokenControl.CreateToken(userDB.Id);
            return Ok(authResponse);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = _userRepository.GetByFunc(x => x.Email.CompareTo(loginModel.Email) == 0);
            if(user == null)
            {
                ModelState.AddModelError("Email", "User with this email doesn't exsist");
                return BadRequest(ModelState);
            }

            if (!_passwordHasher.IsPasswordsEqual(loginModel.Password, user.Password))
            {
                ModelState.AddModelError("Password", "Password is wrong");
                return BadRequest(ModelState);
            }

            var authResponse = _tokenControl.CreateToken(user.Id);
            return Ok(authResponse);
        }
    }
}
