using Microsoft.AspNetCore.Mvc;
using SmartLibraryManager.Services;
using SmartLibraryManager.ViewModels;

namespace SmartLibraryManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SLAController : ControllerBase
    {
        private readonly TokenService _tokenService;
        public SLAController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Post(LoginVM loginVM)
        {
            var user = await _tokenService.SLAUserLogin(loginVM);

            if (user is null)
            {
                return NotFound("Invalid Username/password");
            }

            return Ok(user);
        }
    }
}
