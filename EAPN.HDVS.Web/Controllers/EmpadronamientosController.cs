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
    public class EmpadronamientosController : ControllerBase
    {
        private readonly ICrudServiceBase<Empadronamiento> _empadronamientoService;
        private readonly ILogger<EmpadronamientosController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="empadronamientoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public EmpadronamientosController(ICrudServiceBase<Empadronamiento> empadronamientoService, ILogger<EmpadronamientosController> logger, IMapper mapper)
        {
            _empadronamientoService = empadronamientoService ?? throw new ArgumentNullException(nameof(empadronamientoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetEmpadronamientos")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<EmpadronamientoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmpadronamientoDto>>> GetEmpadronamientos()
        {
            var empadronamientos = await _empadronamientoService.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<EmpadronamientoDto>(empadronamientos));
        }

        /// <summary>
        /// Get all stored items mapped as Masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetEmpadronamientosAsMasterData")]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetEmpadronamientosAsMasterData()
        {
            var empadronamientos = await _empadronamientoService.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<MasterDataDto>(empadronamientos));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetEmpadronamiento")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(EmpadronamientoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<EmpadronamientoDto>> GetEmpadronamiento(int id)
        {
            var empadronamiento = await _empadronamientoService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<EmpadronamientoDto>(empadronamiento);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="empadronamientoDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(EmpadronamientoDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostEmpadronamiento")]
        public async Task<ActionResult<EmpadronamientoDto>> PostEmpadronamiento(EmpadronamientoDto empadronamientoDto)
        {
            var empadronamiento = _mapper.Map<Empadronamiento>(empadronamientoDto);
            var result = _empadronamientoService.Add(empadronamiento);

            _logger.LogInformation($"Se ha añadido un nuevo empadronamiento: {empadronamiento.Descripcion}");

            await _empadronamientoService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmpadronamiento), new { id = result.Id }, _mapper.Map<EmpadronamientoDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="empadronamientoDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutEmpadronamiento")]
        public async Task<IActionResult> PutEmpadronamiento(int id, EmpadronamientoDto empadronamientoDto)
        {
            if (id != empadronamientoDto.Id)
            {
                return BadRequest();
            }

            var empadronamiento = await _empadronamientoService.GetFirstOrDefault(x => x.Id == id);
            if (empadronamiento == null)
            {
                return NotFound();
            }

            _logger.LogInformation($"Se actualiza el empadronamiento {empadronamiento.Descripcion} : {empadronamientoDto.Descripcion}");
            _mapper.Map(empadronamientoDto, empadronamiento);

            _empadronamientoService.Update(empadronamiento);
            await _empadronamientoService.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeleteEmpadronamiento")]
        public async Task<IActionResult> DeleteEmpadronamiento(int id)
        {
            var empadronamiento = await _empadronamientoService.GetFirstOrDefault(x => x.Id == id);
            if (empadronamiento == null)
            {
                return NotFound();
            }

            _logger.LogWarning($"Se elimina el empadronamiento {empadronamiento.Descripcion}");
            _empadronamientoService.Remove(empadronamiento);
            await _empadronamientoService.SaveChangesAsync();

            return NoContent();
        }
    }
}
