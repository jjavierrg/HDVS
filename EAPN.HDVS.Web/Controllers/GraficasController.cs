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
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    /// <summary>
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GraficasController : ControllerBase
    {
        private readonly IFilterPaginable<Ficha> _filterPaginator;
        private readonly IReadServiceBase<Ficha> _fichaService;
        private readonly IReadServiceBase<Rango> _rangoService;
        private readonly ILogger<FichasController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="fichaService"></param>
        /// <param name="rangoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="filterPaginator"></param>
        public GraficasController(IReadServiceBase<Ficha> fichaService, IReadServiceBase<Rango> rangoService, ILogger<FichasController> logger, IMapper mapper, IFilterPaginable<Ficha> filterPaginator)
        {
            _fichaService = fichaService ?? throw new ArgumentNullException(nameof(fichaService));
            _rangoService = rangoService ?? throw new ArgumentNullException(nameof(rangoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        /// <summary>
        /// Get chart data acording to user query
        /// </summary>
        /// <param name="graphQuery">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetChartData")]
        [AuthorizePermission(Permissions.STATS_ACCESS)]
        [ProducesResponseType(typeof(IEnumerable<DatosGraficaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<DatosGraficaDTO>>> GetChartData([FromBody] GraphQuery graphQuery)
        {
            if (graphQuery.Query == null)
                return BadRequest();

            if (graphQuery.GlobalData && !User.HasPermission(Permissions.APP_SUPERADMIN))
                return Unauthorized();

            _logger.LogInformation($"Realiza una búsqueda de datos para gráficas con los siguientes filtros: ${graphQuery.Query.FilterParameters}");
            IQueryable<Ficha> basequery = _fichaService.Repository.EntitySet
                .Include(x => x.Sexo).Include(x => x.Genero)
                .Include(x => x.Municipio).Include(x => x.Provincia)
                .Include(x => x.Nacionalidad).Include(x => x.Origen)
                .Include(x => x.Padron).Include(x => x.SituacionAdministrativa)
                .Include(x => x.Seguimientos).ThenInclude(x => x.Indicadores).ThenInclude(x => x.Indicador);

            if (!graphQuery.GlobalData)
                basequery = basequery.Where(x => x.OrganizacionId == User.GetUserOrganizacionId());

            var result = await _filterPaginator.Execute(basequery, graphQuery.Query);
            var rangos = await _rangoService.GetListAsync();

            var data = result.Data.Select(x => new DatosGraficaDTO
            {
                OrganizacionId = x.OrganizacionId,
                FechaNacimiento = x.FechaNacimiento,
                SexoId = x.SexoId,
                GeneroId = x.GeneroId,
                MunicipioId = x.MunicipioId,
                ProvinciaId = x.ProvinciaId,
                PadronId = x.PadronId,
                NacionalidadId = x.NacionalidadId,
                OrigenId = x.OrigenId,
                SituacionAdministrativaId = x.SituacionAdministrativaId,
                Sexo = x.Sexo?.Descripcion,
                Genero = x.Genero?.Descripcion,
                Municipio = x.Municipio?.Nombre,
                Provincia = x.Provincia?.Nombre,
                Padron = x.Padron?.Descripcion,
                Nacionalidad = x.Nacionalidad?.Descripcion,
                Origen = x.Origen?.Descripcion,
                SituacionAdministrativa = x.SituacionAdministrativa?.Descripcion,
                RangoId = GetRango(GetPuntuacion(x), rangos)?.Id,
                Rango = GetRango(GetPuntuacion(x), rangos)?.Descripcion
            });

            if (graphQuery.Rangos != null && graphQuery.Rangos.Any())
                data = data.Where(x => x.RangoId.HasValue && graphQuery.Rangos.Any(r => r == x.RangoId.Value));

            return Ok(data);
        }

        private int? GetPuntuacion(Ficha ficha)
        {
            if (ficha == null || ficha.Seguimientos == null || !ficha.Seguimientos.Any())
                return null;

            var lastSeguimiento = ficha.Seguimientos.OrderByDescending(x => x.Fecha).FirstOrDefault();
            if (lastSeguimiento == null)
                return null;

            return lastSeguimiento.Indicadores.Where(x => x.Indicador?.Activo ?? false).Sum(x => x.Indicador?.Puntuacion ?? 0);
        }

        private Rango GetRango(int? puntuacion, IEnumerable<Rango> rangos)
        {
            if (!puntuacion.HasValue || rangos == null || !rangos.Any())
                return null;

            return rangos.FirstOrDefault(x => x.Minimo <= puntuacion && (!x.Maximo.HasValue || x.Maximo.Value >= puntuacion));
        }
    }
}
