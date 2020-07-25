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
    /// <summary>
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GraficasController : ControllerBase
    {
        private readonly IFilterPaginable<Ficha> _filterPaginator;
        private readonly IReadServiceBase<Ficha> _fichaService;
        private readonly ILogger<FichasController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="fichaService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="filterPaginator"></param>
        public GraficasController(IReadServiceBase<Ficha> fichaService, ILogger<FichasController> logger, IMapper mapper, IFilterPaginable<Ficha> filterPaginator)
        {
            _fichaService = fichaService ?? throw new ArgumentNullException(nameof(fichaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        /// <summary>
        /// Get chart data acording to user query
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetChartData")]
        [AuthorizePermission(Permissions.STATS_ACCESS)]
        [ProducesResponseType(typeof(IEnumerable<DatosGraficaDTO>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<DatosGraficaDTO>> GetChartData([FromBody] QueryData query)
        {
            _logger.LogInformation($"Realiza una búsqueda de datos para gráficas con los siguientes filtros: ${query.FilterParameters}");
            IQueryable<Ficha> basequery = _fichaService.Repository.EntitySet
                .Include(x => x.Sexo).Include(x => x.Genero)
                .Include(x => x.Municipio).Include(x => x.Provincia)
                .Include(x => x.Nacionalidad).Include(x => x.Origen)
                .Include(x => x.Padron).Include(x => x.SituacionAdministrativa)
                .Include(x => x.Seguimientos).ThenInclude(x => x.Indicadores).ThenInclude(x => x.Indicador);

            var result = await _filterPaginator.Execute(basequery, query);  
            return _mapper.MapList<DatosGraficaDTO>(result.Data);
        }
    }
}
