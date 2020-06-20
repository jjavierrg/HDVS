using AutoMapper;
using EAPN.HDVS.Application.Services.User;
using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Dto.Auth;
using EAPN.HDVS.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]
        [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserTokenDto>> AuthenticateAsync([FromBody] LoginAttempDto loginAttempDto)
        {
            var token = await _usuarioService.LoginAsync(loginAttempDto.Email, loginAttempDto.Password);

            //if (token == null)
            //    return Unauthorized();

            return _mapper.Map<UserTokenDto>(token);
        }

        [Authorize]
        [HttpGet("refresh")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserTokenDto>> RefreshTokenAsync()
        {
            var token = await _usuarioService.RefreshTokenAsync(User.GetUserId() ?? 0);

            //if (token == null)
            //    return Unauthorized();

            return _mapper.Map<UserTokenDto>(token);
        }

        [Authorize]
        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await _usuarioService.LogoutAsync(User);
            return NoContent();
        }
    }
}
