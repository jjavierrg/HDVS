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

namespace EAPN.HDVS.Web.Contorganizacionlers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizacionesController : ControllerBase
    {
        private readonly ICrudServiceBase<Organizacion> _organizacionService;
        private readonly ILogger<OrganizacionesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="organizacionService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public OrganizacionesController(ICrudServiceBase<Organizacion> organizacionService, ILogger<OrganizacionesController> logger, IMapper mapper)
        {
            _organizacionService = organizacionService ?? throw new ArgumentNullException(nameof(organizacionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetOrganizaciones")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<OrganizacionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrganizacionDto>>> GetOrganizaciones()
        {
            var organizaciones = await _organizacionService.GetListAsync(includes: q => q.Include(x => x.Usuarios), orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<OrganizacionDto>(organizaciones));
        }

        /// <summary>
        /// Get all stored items mapped as Masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetOrganizacionesAsMasterData")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetOrganizacionesAsMasterData()
        {
            var organizaciones = await _organizacionService.GetListAsync(orderBy: q => q.OrderBy(x => x.Nombre));
            return Ok(_mapper.MapList<MasterDataDto>(organizaciones));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetOrganizacion")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(OrganizacionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrganizacionDto>> GetOrganizacion(int id)
        {
            var organizacion = await _organizacionService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<OrganizacionDto>(organizacion);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="organizacionDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(OrganizacionDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostOrganizacion")]
        public async Task<ActionResult<OrganizacionDto>> PostOrganizacion(OrganizacionDto organizacionDto)
        {
            var organizacion = _mapper.Map<Organizacion>(organizacionDto);
            organizacion.FechaAlta = DateTime.Now;
            var result = _organizacionService.Add(organizacion);

            await _organizacionService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrganizacion), new { id = result.Id }, _mapper.Map<OrganizacionDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="organizacionDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutOrganizacion")]
        public async Task<IActionResult> PutOrganizacion(int id, OrganizacionDto organizacionDto)
        {
            if (id != organizacionDto.Id)
                return BadRequest();

            var organizacion = await _organizacionService.GetFirstOrDefault(x => x.Id == id);
            if (organizacion == null)
                return NotFound();

            _mapper.Map(organizacionDto, organizacion);

            _organizacionService.Update(organizacion);
            await _organizacionService.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeleteOrganizacion")]
        public async Task<IActionResult> DeleteOrganizacion(int id)
        {
            var organizacion = await _organizacionService.GetFirstOrDefault(x => x.Id == id);
            if (organizacion == null)
                return NotFound();

            _organizacionService.Remove(organizacion);
            await _organizacionService.SaveChangesAsync();

            return NoContent();
        }
    }
}
