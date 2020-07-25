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
    public class SexosController : ControllerBase
    {
        private readonly ICrudServiceBase<Sexo> _sexoService;
        private readonly ILogger<SexosController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="sexoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public SexosController(ICrudServiceBase<Sexo> sexoService, ILogger<SexosController> logger, IMapper mapper)
        {
            _sexoService = sexoService ?? throw new ArgumentNullException(nameof(sexoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetSexos")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<SexoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SexoDto>>> GetSexos()
        {
            var sexos = await _sexoService.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<SexoDto>(sexos));
        }

        /// <summary>
        /// Get all stored items mapped as Masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetSexosAsMasterData")]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetSexosAsMasterData()
        {
            var sexos = await _sexoService.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<MasterDataDto>(sexos));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSexo")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(SexoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SexoDto>> GetSexo(int id)
        {
            var sexo = await _sexoService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<SexoDto>(sexo);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="sexoDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(SexoDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostSexo")]
        public async Task<ActionResult<SexoDto>> PostSexo(SexoDto sexoDto)
        {
            var sexo = _mapper.Map<Sexo>(sexoDto);
            var result = _sexoService.Add(sexo);

            _logger.LogInformation($"Se ha añadido un nuevo sexo: {sexo.Descripcion}");

            await _sexoService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSexo), new { id = result.Id }, _mapper.Map<SexoDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="sexoDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutSexo")]
        public async Task<IActionResult> PutSexo(int id, SexoDto sexoDto)
        {
            if (id != sexoDto.Id)
            {
                return BadRequest();
            }

            var sexo = await _sexoService.GetFirstOrDefault(x => x.Id == id);
            if (sexo == null)
            {
                return NotFound();
            }

            _logger.LogInformation($"Se actualiza el sexo {sexo.Descripcion} : {sexoDto.Descripcion}");
            _mapper.Map(sexoDto, sexo);

            _sexoService.Update(sexo);
            await _sexoService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteSexo")]
        public async Task<IActionResult> DeleteSexo(int id)
        {
            var sexo = await _sexoService.GetFirstOrDefault(x => x.Id == id);
            if (sexo == null)
            {
                return NotFound();
            }

            _logger.LogWarning($"Se elimina el sexo {sexo.Descripcion}");
            _sexoService.Remove(sexo);
            await _sexoService.SaveChangesAsync();

            return NoContent();
        }
    }
}
