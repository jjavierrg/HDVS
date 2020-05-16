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

namespace EAPN.HDVS.Web.Contfichalers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FichasController : ControllerBase
    {
        private readonly ICrudServiceBase<Ficha> _fichaService;
        private readonly ILogger<FichasController> _logger;
        private readonly IMapper _mapper;
        private readonly IFilterPaginable<Ficha> _filterPaginator;

        /// <summary>
        /// </summary>
        /// <param name="fichaService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="filterPaginator"></param>
        public FichasController(ICrudServiceBase<Ficha> fichaService, ILogger<FichasController> logger, IMapper mapper, IFilterPaginable<Ficha> filterPaginator)
        {
            _fichaService = fichaService ?? throw new ArgumentNullException(nameof(fichaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        /// <summary>
        /// Get all stored items with related data
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetFichas")]
        [AuthorizePermission(Permissions.PESONALCARD_READ)]
        [ProducesResponseType(typeof(IEnumerable<FichaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FichaDto>> GetFichas()
        {
            var fichas = await GetBaseQueryable().ToListAsync();
            var fichasIds = fichas.Select(x => x.Id);

            if (fichasIds.Any())
                _logger.LogInformation($"[Fichas] Se obtiene un listado con las siguientes fichas: [{string.Join(", ", fichasIds)}]");

            return Ok(_mapper.MapList<FichaDto>(fichas));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetFicha")]
        [AuthorizePermission(Permissions.PESONALCARD_READ)]
        [ProducesResponseType(typeof(FichaDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<FichaDto>> GetFicha(int id)
        {
            var query = GetBaseQueryable();
            var ficha = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (ficha != null)
                _logger.LogInformation($"[Fichas] Se accede a la ficha [{ficha.Id}]");
            else
                _logger.LogWarning($"[Fichas] Se ha intentado acceder a una ficha no válida [id: {id}]");

            return _mapper.Map<FichaDto>(ficha);
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetFichasFiltered")]
        [AuthorizePermission(Permissions.PESONALCARD_READ)]
        [ProducesResponseType(typeof(QueryResult<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<FichaDto>>> GetUsuariosFiltered([FromBody]QueryData query)
        {
            _logger.LogInformation($"Realiza una búsqueda de fichas con los siguientes filtros: ${query.FilterParameters}");
         
            var result = await _filterPaginator.Execute(GetBaseQueryable(), query);
            var fichasIds = result.Data?.Select(x => x.Id);

            if (fichasIds.Any())
                _logger.LogInformation($"[Fichas] Se obtiene un listado mediante búsqueda filtrada con las siguientes fichas: [{string.Join(", ", fichasIds)}]");

            return _mapper.MapQueryResult<Ficha, FichaDto>(result);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="fichaDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.PESONALCARD_WRITE)]
        [ProducesResponseType(typeof(FichaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "PostFicha")]
        public async Task<ActionResult<FichaDto>> PostFicha(FichaDto fichaDto)
        {
            var asociacionId = User.GetUserAsociacionId();
            if (fichaDto.AsociacionId != asociacionId)
            {
                _logger.LogCritical($"[Fichas] Se ha intentado crear para otra asociación [Asociacion_id de destino: {fichaDto.AsociacionId}]");
                return BadRequest();
            }

            var ficha = _mapper.Map<Ficha>(fichaDto);
            var result = _fichaService.Add(ficha);            

            await _fichaService.SaveChangesAsync();

            if (ficha != null)
                _logger.LogInformation($"[Fichas] Se ha creado la ficha [id: {ficha.Id}]");

            return CreatedAtAction(nameof(GetFicha), new { id = result.Id }, _mapper.Map<FichaDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="fichaDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.PESONALCARD_WRITE)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutFicha")]
        public async Task<IActionResult> PutFicha(int id, FichaDto fichaDto)
        {
            if (id != fichaDto.Id)
                return BadRequest();

            var asociacionId = User.GetUserAsociacionId();
            if (fichaDto.AsociacionId != asociacionId)
            {
                _logger.LogCritical($"[Fichas] Se ha intentado actualizar una ficha de otra asociación [Asociacion_id de destino: {fichaDto.AsociacionId}]");
                return BadRequest();
            }

            var query = GetBaseQueryable();
            var ficha = await query.SingleOrDefaultAsync(x => x.Id == id);
            
            if (ficha == null)
            {
                _logger.LogCritical($"[Fichas] Se ha intentado actualizar una ficha no válida [id: {fichaDto.Id}]");
                return NotFound();
            }

            _mapper.Map(fichaDto, ficha);

            _fichaService.Update(ficha);
            await _fichaService.SaveChangesAsync();
            
            _logger.LogInformation($"[Fichas] Se ha actualizado la ficha [id: {ficha.Id}]");
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
        [HttpDelete("{id}", Name = "DeleteFicha")]
        public async Task<IActionResult> DeleteFicha(int id)
        {
            var asociacionId = User.GetUserAsociacionId();

            var ficha = await _fichaService.GetFirstOrDefault(x => x.Id == id);
            if (ficha == null)
            {
                _logger.LogCritical($"[Fichas] Se ha intentado eliminar una ficha no válida [id: {id}]");
                return NotFound();
            }

            _fichaService.Remove(ficha);
            await _fichaService.SaveChangesAsync();

            _logger.LogInformation($"[Fichas] Se ha eliminado la ficha [id: {ficha.Id}]");
            return NoContent();
        }

        /// <summary>
        /// Base query. Handles security and asociacion scope
        /// </summary>
        /// <returns></returns>
        private IQueryable<Ficha> GetBaseQueryable()
        {
            var asociacionId = User.GetUserAsociacionId();
            var query = _fichaService.Repository.EntitySet;
            query = query.Where(x => x.AsociacionId == asociacionId);

            return query;
        }
    }
}
