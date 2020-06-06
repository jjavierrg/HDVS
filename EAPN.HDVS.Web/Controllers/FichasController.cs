using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Queries;
using EAPN.HDVS.Infrastructure.Core.Repository;
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
    public class FichasController : ControllerBase
    {
        private readonly ICrudServiceBase<Ficha> _fichaService;
        private readonly ICrudServiceBase<Adjunto> _adjuntoService;
        private readonly ILogger<FichasController> _logger;
        private readonly IMapper _mapper;
        private readonly IFilterPaginable<Ficha> _filterPaginator;

        /// <summary>
        /// </summary>
        /// <param name="fichaService"></param>
        /// <param name="adjuntoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="filterPaginator"></param>
        public FichasController(ICrudServiceBase<Ficha> fichaService, ICrudServiceBase<Adjunto> adjuntoService, ILogger<FichasController> logger, IMapper mapper, IFilterPaginable<Ficha> filterPaginator)
        {
            _fichaService = fichaService ?? throw new ArgumentNullException(nameof(fichaService));
            _adjuntoService = adjuntoService ?? throw new ArgumentNullException(nameof(adjuntoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        /// <summary>
        /// Get all stored items with related data
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetFichas")]
        [AuthorizePermission(Permissions.PERSONALCARD_READ)]
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
        [AuthorizePermission(Permissions.PERSONALCARD_READ)]
        [ProducesResponseType(typeof(FichaDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<FichaDto>> GetFicha(int id)
        {
            var query = GetBaseQueryable();

            if (User.HasPermission(Permissions.PERSONALATTACHMENTS_READ))
                query = query.Include(x => x.Adjuntos);

            if (User.HasPermission(Permissions.PERSONALINDICATORS_READ))
                query = query.Include(x => x.Seguimientos).ThenInclude(x => x.Tecnico)
                    .Include(x => x.Seguimientos).ThenInclude(x => x.Indicadores).ThenInclude(x => x.Indicador);

            var ficha = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (ficha != null)
                _logger.LogInformation($"[Fichas] Se accede a la ficha [{ficha.Id}]");
            else
                _logger.LogWarning($"[Fichas] Se ha intentado acceder a una ficha no válida [id: {id}]");

            return _mapper.Map<FichaDto>(ficha);
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}/datos", Name = "GetDatosFicha")]
        [AuthorizePermission(Permissions.PERSONALCARD_READ)]
        [ProducesResponseType(typeof(DatosFichaDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<DatosFichaDto>> GetDatosFicha(int id)
        {
            var query = GetBaseQueryable();

            if (User.HasPermission(Permissions.PERSONALATTACHMENTS_READ))
                query = query.Include(x => x.Adjuntos);

            if (User.HasPermission(Permissions.PERSONALINDICATORS_READ))
                query = query.Include(x => x.Seguimientos).ThenInclude(x => x.Tecnico)
                    .Include(x => x.Organizacion).Include(x => x.Tecnico)
                    .Include(x => x.Sexo).Include(x => x.Genero)
                    .Include(x => x.Municipio).Include(x => x.Provincia)
                    .Include(x => x.Nacionalidad).Include(x => x.Origen).Include(x => x.Padron);

            var ficha = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (ficha != null)
                _logger.LogInformation($"[Fichas] Se accede a la ficha [{ficha.Id}]");
            else
                _logger.LogWarning($"[Fichas] Se ha intentado acceder a una ficha no válida [id: {id}]");

            return _mapper.Map<DatosFichaDto>(ficha);
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetFichasFiltered")]
        [AuthorizePermission(Permissions.PERSONALCARD_READ)]
        [ProducesResponseType(typeof(QueryResult<FichaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<FichaDto>>> GetFichasFiltered([FromBody]QueryData query)
        {
            _logger.LogInformation($"Realiza una búsqueda de fichas con los siguientes filtros: ${query.FilterParameters}");

            var result = await _filterPaginator.Execute(GetBaseQueryable(), query);
            var fichasIds = result.Data?.Select(x => x.Id);

            if (fichasIds.Any())
                _logger.LogInformation($"[Fichas] Se obtiene un listado mediante búsqueda filtrada con las siguientes fichas: [{string.Join(", ", fichasIds)}]");

            return _mapper.MapQueryResult<Ficha, FichaDto>(result);
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("vistaprevia/filtered", Name = "GetVistaPeviaFichas")]
        [AuthorizePermission(Permissions.PERSONALCARD_READ)]
        [ProducesResponseType(typeof(QueryResult<VistaPreviaFichaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<VistaPreviaFichaDto>>> GetVistaPeviaFichas([FromBody]QueryData query)
        {
            _logger.LogInformation($"Solicita una vista previa de fichas con los siguiente parámetros: ${query.FilterParameters}");

            var result = await _filterPaginator.Execute(_fichaService.Repository.EntitySet.Include(x => x.Organizacion).Include(x => x.Tecnico), query);
            var fichasIds = result.Data?.Select(x => x.Id);

            if (fichasIds.Any())
                _logger.LogInformation($"[Fichas] Se obtiene un vista previa de las siguientes fichas: [{string.Join(", ", fichasIds)}]");

            return _mapper.MapQueryResult<Ficha, VistaPreviaFichaDto>(result);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="fichaDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.PERSONALCARD_WRITE)]
        [ProducesResponseType(typeof(FichaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "PostFicha")]
        public async Task<ActionResult<FichaDto>> PostFicha(FichaDto fichaDto)
        {
            var organizacionId = User.GetUserOrganizacionId();
            if (fichaDto.OrganizacionId != organizacionId)
            {
                _logger.LogCritical($"[Fichas] Se ha intentado crear una ficha para otra organizacion [Organizacion_id de destino: {fichaDto.OrganizacionId}]");
                return BadRequest();
            }

            var adjuntos = fichaDto.Adjuntos;
            fichaDto.Adjuntos = null;
            var ficha = _mapper.Map<Ficha>(fichaDto);
            var result = _fichaService.Add(ficha);

            await _fichaService.SaveChangesAsync();

            // update foto information
            if (ficha.FotoId.HasValue)
            {
                var foto = await _adjuntoService.GetFirstOrDefault(x => x.Id == ficha.FotoId.Value);
                if (foto != null)
                {
                    foto.FichaId = ficha.Id;
                    foto.OrganizacionId = ficha.OrganizacionId;
                    _adjuntoService.Update(foto);
                    await _adjuntoService.SaveChangesAsync();
                }
            }

            // update attachment information
            if (adjuntos != null && adjuntos.Any())
            {
                var ids = adjuntos.Select(x => x.Id);
                var adjuntosDb = await _adjuntoService.GetListAsync(x => ids.Contains(x.Id));
                foreach (var adjunto in adjuntosDb)
                {
                    adjunto.FichaId = ficha.Id;
                    adjunto.OrganizacionId = ficha.OrganizacionId;
                    _adjuntoService.Update(adjunto);
                }
                await _adjuntoService.SaveChangesAsync();
            }

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
        [AuthorizePermission(Permissions.PERSONALCARD_WRITE)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutFicha")]
        public async Task<IActionResult> PutFicha(int id, FichaDto fichaDto)
        {
            if (id != fichaDto.Id)
                return BadRequest();

            var organizacionId = User.GetUserOrganizacionId();
            if (fichaDto.OrganizacionId != organizacionId)
            {
                _logger.LogCritical($"[Fichas] Se ha intentado actualizar una ficha de otra organización [Organizacion_Id de destino: {fichaDto.OrganizacionId}]");
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
            var organizacionId = User.GetUserOrganizacionId();

            var ficha = await _fichaService.GetFirstOrDefault(x => x.Id == id);
            if (ficha == null)
            {
                _logger.LogCritical($"[Fichas] Se ha intentado eliminar una ficha no válida [id: {id}]");
                return NotFound();
            }

            if (ficha.OrganizacionId != organizacionId)
            {
                _logger.LogCritical($"[Fichas] Se ha intentado eliminar una ficha de una organización a la que no se tenía acceso [id: {id}]");
                return NotFound();
            }

            _fichaService.Remove(ficha);
            await _fichaService.SaveChangesAsync();

            _logger.LogInformation($"[Fichas] Se ha eliminado la ficha [id: {ficha.Id}]");
            return NoContent();
        }

        /// <summary>
        /// Base query. Handles security and organizacion scope
        /// </summary>
        /// <returns></returns>
        private IQueryable<Ficha> GetBaseQueryable()
        {
            var organizacionId = User.GetUserOrganizacionId();
            var query = _fichaService.Repository.EntitySet;
            query = query.Where(x => x.OrganizacionId == organizacionId);

            return query;
        }
    }
}
