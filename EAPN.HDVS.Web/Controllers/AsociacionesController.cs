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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Contasociacionlers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AsociacionesController : ControllerBase
    {
        private readonly ICrudServiceBase<Asociacion> _asociacionService;
        private readonly ILogger<AsociacionesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="asociacionService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public AsociacionesController(ICrudServiceBase<Asociacion> asociacionService, ILogger<AsociacionesController> logger, IMapper mapper)
        {
            _asociacionService = asociacionService ?? throw new ArgumentNullException(nameof(asociacionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAsociaciones")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<AsociacionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AsociacionDto>>> GetAsociaciones()
        {
            var asociaciones = await _asociacionService.GetListAsync(includes: q => q.Include(x => x.Usuarios), orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<AsociacionDto>(asociaciones));
        }

        /// <summary>
        /// Get all stored items mapped as Masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetAsociacionesAsMasterData")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetAsociacionesAsMasterData()
        {
            var asociaciones = await _asociacionService.GetListAsync(orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<MasterDataDto>(asociaciones));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetAsociacion")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(AsociacionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<AsociacionDto>> GetAsociacion(int id)
        {
            var asociacion = await _asociacionService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<AsociacionDto>(asociacion);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="asociacionDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(AsociacionDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostAsociacion")]
        public async Task<ActionResult<AsociacionDto>> PostAsociacion(AsociacionDto asociacionDto)
        {
            var asociacion = _mapper.Map<Asociacion>(asociacionDto);
            var result = _asociacionService.Add(asociacion);

            await _asociacionService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAsociacion), new { id = result.Id }, _mapper.Map<AsociacionDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="asociacionDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutAsociacion")]
        public async Task<IActionResult> PutAsociacion(int id, AsociacionDto asociacionDto)
        {
            if (id != asociacionDto.Id)
                return BadRequest();

            var asociacion = await _asociacionService.GetFirstOrDefault(x => x.Id == id);
            if (asociacion == null)
                return NotFound();

            _mapper.Map(asociacionDto, asociacion);

            _asociacionService.Update(asociacion);
            await _asociacionService.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeleteAsociacion")]
        public async Task<IActionResult> DeleteAsociacion(int id)
        {
            var asociacion = await _asociacionService.GetFirstOrDefault(x => x.Id == id);
            if (asociacion == null)
                return NotFound();

            _asociacionService.Remove(asociacion);
            await _asociacionService.SaveChangesAsync();

            return NoContent();
        }
    }
}
