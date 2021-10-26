using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthModel model)
        {
            var user = _userRepository.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthModel model)
        {
            bool isUnique = _userRepository.IsUniqueUser(model.Username);

            if (!isUnique)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            var user = _userRepository.Register(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Error while registering." });
            }

            return Ok();
        }
    }
}
