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
    public class ProvinciasController : ControllerBase
    {
        private readonly ICrudServiceBase<Provincia> _provinciaService;
        private readonly ILogger<ProvinciasController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="provinciaService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ProvinciasController(ICrudServiceBase<Provincia> provinciaService, ILogger<ProvinciasController> logger, IMapper mapper)
        {
            _provinciaService = provinciaService ?? throw new ArgumentNullException(nameof(provinciaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetProvincias")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<ProvinciaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProvinciaDto>>> GetProvincias()
        {
            var provincias = await _provinciaService.GetListAsync(orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<ProvinciaDto>(provincias));
        }

        /// <summary>
        /// Get all stored items mapped as Masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetProvinciasAsMasterData")]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetProvinciasAsMasterData()
        {
            var provincias = await _provinciaService.GetListAsync(orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<MasterDataDto>(provincias));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetProvincia")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(ProvinciaDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProvinciaDto>> GetProvincia(int id)
        {
            var provincia = await _provinciaService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<ProvinciaDto>(provincia);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="provinciaDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(ProvinciaDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostProvincia")]
        public async Task<ActionResult<ProvinciaDto>> PostProvincia(ProvinciaDto provinciaDto)
        {
            var provincia = _mapper.Map<Provincia>(provinciaDto);
            var result = _provinciaService.Add(provincia);

            _logger.LogInformation($"Se ha añadido un nuevo provincia: {provincia.Nombre}");

            await _provinciaService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProvincia), new { id = result.Id }, _mapper.Map<ProvinciaDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="provinciaDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutProvincia")]
        public async Task<IActionResult> PutProvincia(int id, ProvinciaDto provinciaDto)
        {
            if (id != provinciaDto.Id)
            {
                return BadRequest();
            }

            var provincia = await _provinciaService.GetFirstOrDefault(x => x.Id == id);
            if (provincia == null)
            {
                return NotFound();
            }

            _logger.LogInformation($"Se actualiza la provincia {provincia.Nombre} : {provinciaDto.Nombre}");
            _mapper.Map(provinciaDto, provincia);

            _provinciaService.Update(provincia);
            await _provinciaService.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeleteProvincia")]
        public async Task<IActionResult> DeleteProvincia(int id)
        {
            var provincia = await _provinciaService.GetFirstOrDefault(x => x.Id == id);
            if (provincia == null)
            {
                return NotFound();
            }

            _logger.LogWarning($"Se elimina la provincia {provincia.Nombre}");
            _provinciaService.Remove(provincia);
            await _provinciaService.SaveChangesAsync();

            return NoContent();
        }
    }
}
