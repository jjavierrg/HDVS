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
    public class PaisesController : ControllerBase
    {
        private readonly ICrudServiceBase<Pais> _paisService;
        private readonly ILogger<PaisesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="paisService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public PaisesController(ICrudServiceBase<Pais> paisService, ILogger<PaisesController> logger, IMapper mapper)
        {
            _paisService = paisService ?? throw new ArgumentNullException(nameof(paisService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetPaises")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<PaisDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PaisDto>>> GetPaises()
        {
            var paises = await _paisService.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<PaisDto>(paises));
        }

        /// <summary>
        /// Get all stored items mapped as Masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetPaisesAsMasterData")]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetPaisesAsMasterData()
        {
            var paises = await _paisService.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<MasterDataDto>(paises));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPais")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(PaisDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaisDto>> GetPais(int id)
        {
            var pais = await _paisService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<PaisDto>(pais);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="paisDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(PaisDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostPais")]
        public async Task<ActionResult<PaisDto>> PostPais(PaisDto paisDto)
        {
            var pais = _mapper.Map<Pais>(paisDto);
            var result = _paisService.Add(pais);

            _logger.LogInformation($"Se ha añadido un nuevo pais: {pais.Descripcion}");

            await _paisService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPais), new { id = result.Id }, _mapper.Map<PaisDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="paisDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutPais")]
        public async Task<IActionResult> PutPais(int id, PaisDto paisDto)
        {
            if (id != paisDto.Id)
                return BadRequest();

            var pais = await _paisService.GetFirstOrDefault(x => x.Id == id);
            if (pais == null)
                return NotFound();

            _logger.LogInformation($"Se actualiza el pais {pais.Descripcion} : {paisDto.Descripcion}");
            _mapper.Map(paisDto, pais);

            _paisService.Update(pais);
            await _paisService.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeletePais")]
        public async Task<IActionResult> DeletePais(int id)
        {
            var pais = await _paisService.GetFirstOrDefault(x => x.Id == id);
            if (pais == null)
                return NotFound();

            _logger.LogWarning($"Se elimina el pais {pais.Descripcion}");
            _paisService.Remove(pais);
            await _paisService.SaveChangesAsync();

            return NoContent();
        }
    }
}
