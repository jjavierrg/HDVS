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
using Microsoft.EntityFrameworkCore;
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
    public class MunicipiosController : ControllerBase
    {
        private readonly ICrudServiceBase<Municipio> _municipioService;
        private readonly ILogger<MunicipiosController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="municipioService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public MunicipiosController(ICrudServiceBase<Municipio> municipioService, ILogger<MunicipiosController> logger, IMapper mapper)
        {
            _municipioService = municipioService ?? throw new ArgumentNullException(nameof(municipioService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetMunicipios")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<MunicipioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MunicipioDto>>> GetMunicipios()
        {
            var municipios = await _municipioService.GetListAsync(includes: q=> q.Include(x => x.Provincia), orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<MunicipioDto>(municipios));
        }

        /// <summary>
        /// Get all stored items filtered by provincia Id mapped as Masterdata
        /// </summary>
        /// <param name="provinciaId">Provincia identifier</param>
        /// <returns></returns>
        [HttpGet("provincia/{provinciaId}/masterdata", Name = "GetMunicipiosByProvinciaIdAsMasterData")]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetMunicipiosByProvinciaIdAsMasterData(int provinciaId)
        {
            var municipios = await _municipioService.GetListAsync(x => x.ProvinciaId == provinciaId, orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<MasterDataDto>(municipios));
        }

        /// <summary>
        /// Get all stored items mapped as Masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetMunicipiosAsMasterData")]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetMunicipiosAsMasterData()
        {
            var municipios = await _municipioService.GetListAsync(orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<MasterDataDto>(municipios));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetMunicipio")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(MunicipioDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<MunicipioDto>> GetMunicipio(int id)
        {
            var municipio = await _municipioService.GetFirstOrDefault(x => x.Id == id, q => q.Include(x => x.Provincia));
            return _mapper.Map<MunicipioDto>(municipio);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="municipioDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(MunicipioDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostMunicipio")]
        public async Task<ActionResult<MunicipioDto>> PostMunicipio(MunicipioDto municipioDto)
        {
            var municipio = _mapper.Map<Municipio>(municipioDto);
            var result = _municipioService.Add(municipio);

            _logger.LogInformation($"Se ha añadido un nuevo municipio: {municipio.Nombre}");

            await _municipioService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMunicipio), new { id = result.Id }, _mapper.Map<MunicipioDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="municipioDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutMunicipio")]
        public async Task<IActionResult> PutMunicipio(int id, MunicipioDto municipioDto)
        {
            if (id != municipioDto.Id)
                return BadRequest();

            var municipio = await _municipioService.GetFirstOrDefault(x => x.Id == id);
            if (municipio == null)
                return NotFound();

            _logger.LogInformation($"Se actualiza el municipio {municipio.Nombre} : {municipioDto.Nombre}");
            _mapper.Map(municipioDto, municipio);

            _municipioService.Update(municipio);
            await _municipioService.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeleteMunicipio")]
        public async Task<IActionResult> DeleteMunicipio(int id)
        {
            var municipio = await _municipioService.GetFirstOrDefault(x => x.Id == id);
            if (municipio == null)
                return NotFound();

            _logger.LogWarning($"Se elimina el municipio {municipio.Nombre}");
            _municipioService.Remove(municipio);
            await _municipioService.SaveChangesAsync();

            return NoContent();
        }
    }
}
