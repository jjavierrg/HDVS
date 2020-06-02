using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Queries;
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
    public class SeguimientosController : ControllerBase
    {
        private readonly ICrudServiceBase<Seguimiento> _seguimientoService;
        private readonly ICrudServiceBase<Ficha> _fichaService;
        private readonly ILogger<SeguimientosController> _logger;
        private readonly IMapper _mapper;
        private readonly IFilterPaginable<Seguimiento> _filterPaginator;

        /// <summary>
        /// </summary>
        /// <param name="seguimientoService"></param>
        /// <param name="fichaService"></param>
        /// <param name="adjuntoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="filterPaginator"></param>
        public SeguimientosController(ICrudServiceBase<Seguimiento> seguimientoService, ICrudServiceBase<Ficha> fichaService, ICrudServiceBase<Adjunto> adjuntoService, ILogger<SeguimientosController> logger, IMapper mapper, IFilterPaginable<Seguimiento> filterPaginator)
        {
            _seguimientoService = seguimientoService ?? throw new ArgumentNullException(nameof(seguimientoService));
            _fichaService = fichaService ?? throw new ArgumentNullException(nameof(fichaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        /// <summary>
        /// Get all stored items with related data
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetSeguimientos")]
        [AuthorizePermission(Permissions.PERSONALINDICATORS_READ)]
        [ProducesResponseType(typeof(IEnumerable<SeguimientoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SeguimientoDto>> GetSeguimientos()
        {
            var seguimientos = await GetBaseQueryable().ToListAsync();
            var seguimientosIds = seguimientos.Select(x => x.Id);

            if (seguimientosIds.Any())
                _logger.LogInformation($"[Seguimientos] Se obtiene un listado con las siguientes seguimientos: [{string.Join(", ", seguimientosIds)}]");

            return Ok(_mapper.MapList<SeguimientoDto>(seguimientos));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSeguimiento")]
        [AuthorizePermission(Permissions.PERSONALCARD_READ)]
        [ProducesResponseType(typeof(SeguimientoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SeguimientoDto>> GetSeguimiento(int id)
        {
            var query = GetBaseQueryable();

            var seguimiento = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (seguimiento != null)
                _logger.LogInformation($"[Seguimientos] Se accede al seguimiento [{seguimiento.Id}]");
            else
                _logger.LogWarning($"[Seguimientos] Se ha intentado acceder a un seguimiento no válido [id: {id}]");

            return _mapper.Map<SeguimientoDto>(seguimiento);
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetSeguimientosFiltered")]
        [AuthorizePermission(Permissions.PERSONALCARD_READ)]
        [ProducesResponseType(typeof(QueryResult<SeguimientoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<SeguimientoDto>>> GetSeguimientosFiltered([FromBody]QueryData query)
        {
            _logger.LogInformation($"Realiza una búsqueda de seguimientos con los siguientes filtros: ${query.FilterParameters}");

            var result = await _filterPaginator.Execute(GetBaseQueryable(), query);
            var seguimientosIds = result.Data?.Select(x => x.Id);

            if (seguimientosIds.Any())
                _logger.LogInformation($"[Seguimientos] Se obtiene un listado mediante búsqueda filtrada con las siguientes seguimientos: [{string.Join(", ", seguimientosIds)}]");

            return _mapper.MapQueryResult<Seguimiento, SeguimientoDto>(result);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="seguimientoDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.PERSONALCARD_WRITE)]
        [ProducesResponseType(typeof(SeguimientoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "PostSeguimiento")]
        public async Task<ActionResult<SeguimientoDto>> PostSeguimiento(SeguimientoDto seguimientoDto)
        {
            var organizacionId = User.GetUserOrganizacionId();
            var ficha = await _fichaService.GetFirstOrDefault(x => x.Id == seguimientoDto.FichaId);
            if (ficha == null)
            {
                return BadRequest();
            }

            if (ficha.OrganizacionId != organizacionId)
            {
                _logger.LogCritical($"[Seguimientos] Se ha intentado crear un seguimiento para una ficha de otra organizacion [Ficha de destino: {seguimientoDto.FichaId}]");
                return BadRequest();
            }

            var seguimiento = _mapper.Map<Seguimiento>(seguimientoDto);
            var result = _seguimientoService.Add(seguimiento);

            await _seguimientoService.SaveChangesAsync();

            if (seguimiento != null)
                _logger.LogInformation($"[Seguimientos] Se ha creado el seguimiento [id: {seguimiento.Id}] para la ficha {ficha.Id}");

            return CreatedAtAction(nameof(GetSeguimiento), new { id = result.Id }, _mapper.Map<SeguimientoDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="seguimientoDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.PERSONALCARD_WRITE)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutSeguimiento")]
        public async Task<IActionResult> PutSeguimiento(int id, SeguimientoDto seguimientoDto)
        {
            if (id != seguimientoDto.Id)
                return BadRequest();

            var organizacionId = User.GetUserOrganizacionId();
            var seguimiento = await _seguimientoService.GetSingleOrDefault(x => x.Id == id && x.Ficha.OrganizacionId == organizacionId, q => q.Include(x => x.Indicadores).Include(x => x.Ficha));

            if (seguimiento == null)
            {
                _logger.LogCritical($"[Seguimientos] Se ha intentado actualizar un seguimiento no válido [id: {seguimientoDto.Id}]");
                return NotFound();
            }

            if (seguimiento.Ficha.OrganizacionId != organizacionId)
            {
                _logger.LogCritical($"[Seguimientos] Se ha intentado actualizar un seguimiento de una ficha de otra organización [Ficha_Id solicitada: {seguimientoDto.FichaId}]");
                return BadRequest();
            }

            _mapper.Map(seguimientoDto, seguimiento);
            _seguimientoService.Update(seguimiento);
            await _seguimientoService.SaveChangesAsync();

            _logger.LogInformation($"[Seguimientos] Se ha actualizado el seguimiento [id: {seguimiento.Id}]");
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
        [HttpDelete("{id}", Name = "DeleteSeguimiento")]
        public async Task<IActionResult> DeleteSeguimiento(int id)
        {
            var organizacionId = User.GetUserOrganizacionId();

            var seguimiento = await _seguimientoService.GetFirstOrDefault(x => x.Id == id, q => q.Include(x => x.Ficha));
            if (seguimiento == null)
            {
                _logger.LogCritical($"[Seguimientos] Se ha intentado eliminar un seguimiento no válido [id: {id}]");
                return NotFound();
            }

            if (seguimiento.Ficha.OrganizacionId != organizacionId)
            {
                _logger.LogCritical($"[Seguimientos] Se ha intentado eliminar un seguimiento de una organización a la que no se tenía acceso [id: {id}]");
                return NotFound();
            }

            _seguimientoService.Remove(seguimiento);
            await _seguimientoService.SaveChangesAsync();

            _logger.LogInformation($"[Seguimientos] Se ha eliminado el seguimiento [id: {seguimiento.Id}]");
            return NoContent();
        }

        /// <summary>
        /// Base query. Handles security and organizacion scope
        /// </summary>
        /// <returns></returns>
        private IQueryable<Seguimiento> GetBaseQueryable()
        {
            var organizacionId = User.GetUserOrganizacionId();
            var query = _seguimientoService.Repository.EntitySet;
            query = query.Where(x => x.Ficha.OrganizacionId == organizacionId);
            query = query.Include(x => x.Ficha).Include(x => x.Tecnico);
            query = query.Include(x => x.Indicadores).ThenInclude(x => x.Indicador);

            return query;
        }
    }
}
