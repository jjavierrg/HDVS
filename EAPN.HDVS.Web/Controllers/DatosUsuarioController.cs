using AutoMapper;
using EAPN.HDVS.Application.Services.User;
using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DatosUsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<DatosUsuarioController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="usuarioService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public DatosUsuarioController(IUsuarioService usuarioService, ILogger<DatosUsuarioController> logger, IMapper mapper)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get the user data
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetDatosUsuario")]
        [ProducesResponseType(typeof(DatosUsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DatosUsuarioDto>> GetDatosUsuario()
        {
            var userId = User.GetUserId();
            var usuario = await _usuarioService.GetFirstOrDefault(x => x.Id == userId, q => q.Include(x => x.Organizacion));

            if (usuario == null)
            {
                return NotFound();
            }

            return _mapper.Map<DatosUsuarioDto>(usuario);
        }

        /// <summary>
        /// Get the user data
        /// </summary>
        /// <returns></returns>
        [HttpPut(Name = "PutDatosUsuario")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> PutDatosUsuario([FromBody] DatosUsuarioDto datosUsuarioDto)
        {
            var userId = User.GetUserId();
            var usuario = await _usuarioService.GetFirstOrDefault(x => x.Id == userId);

            if (usuario == null)
            {
                _logger.LogCritical($"Intento de modificación de datos de un usuario inexistente");
                return NotFound();
            }

            usuario.Nombre = datosUsuarioDto.Nombre;
            usuario.FotoId = datosUsuarioDto.FotoId;
            usuario.Apellidos = datosUsuarioDto.Apellidos;
            var result = await _usuarioService.UpdateUserData(usuario, datosUsuarioDto.ClaveActual, datosUsuarioDto.NuevaClave);

            if (!result)
            {
                _logger.LogCritical($"El usuario ha intentado actualizado sus datos pero no ha sido posible. (Probablemente la contraseña actual no sea válida)");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(datosUsuarioDto.NuevaClave))
            {
                _logger.LogInformation($"El usuario ha actualizado su clave");
            }
            else
            {
                _logger.LogInformation($"El usuario ha actualizado sus datos personales");
            }

            return result;
        }
    }
}
