using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Shared.Permissions;
using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Extensions;
using EAPN.HDVS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly ICrudServiceBase<Configuracion> _configuracionService;
        private readonly ILogger<ConfiguracionesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="configuracionService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ConfiguracionesController(ICrudServiceBase<Configuracion> configuracionService, ILogger<ConfiguracionesController> logger, IMapper mapper)
        {
            _configuracionService = configuracionService ?? throw new ArgumentNullException(nameof(configuracionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetConfiguraciones")]
        [ProducesResponseType(typeof(IEnumerable<ConfiguracionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ConfiguracionDto>>> GetConfiguraciones()
        {
            var configuraciones = await _configuracionService.GetListAsync();
            return Ok(_mapper.MapList<ConfiguracionDto>(configuraciones));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetConfiguracion")]
        [ProducesResponseType(typeof(ConfiguracionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ConfiguracionDto>> GetConfiguracion(int id)
        {
            var configuracion = await _configuracionService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<ConfiguracionDto>(configuracion);
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="configuracionDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutConfiguracion")]
        public async Task<IActionResult> PutConfiguracion(int id, ConfiguracionDto configuracionDto)
        {
            if (id != configuracionDto.Id)
                return BadRequest();

            var configuracion = await _configuracionService.GetFirstOrDefault(x => x.Id == id);
            if (configuracion == null)
                return NotFound();

            _logger.LogInformation($"Se actualiza la configuracion");
            _mapper.Map(configuracionDto, configuracion);

            _configuracionService.Update(configuracion);
            await _configuracionService.SaveChangesAsync();

            return NoContent();
        }
    }
}
