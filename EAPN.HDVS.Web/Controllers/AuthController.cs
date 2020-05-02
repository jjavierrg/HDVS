using AutoMapper;
using EAPN.HDVS.Application.Services.User;
using EAPN.HDVS.Web.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IUsuarioService _usuarioService;
        private ILogger<AuthController> _logger;
        private IMapper _mapper;

        public AuthController(IUsuarioService usuarioService, ILogger<AuthController> logger, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _logger = logger;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserTokenDto>> AuthenticateAsync([FromBody] LoginAttempDto loginAttempDto)
        {
            var token = await _usuarioService.LoginAsync(loginAttempDto.Email, loginAttempDto.Password);

            if (token == null)
                return Unauthorized();

            return _mapper.Map<UserTokenDto>(token);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserTokenDto>> RefreshTokenAsync([FromBody]RefreshTokenAttempDto refreshTokenAttempDto)
        {
            var token = await _usuarioService.RefreshTokenAsync(refreshTokenAttempDto.RefreshToken, refreshTokenAttempDto.UserId);

            if (token == null)
                return Unauthorized();

            return _mapper.Map<UserTokenDto>(token);
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await _usuarioService.LogoutAsync(User);
            return NoContent();
        }
    }
}
