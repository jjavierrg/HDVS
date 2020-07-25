using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Application.Services.Dimension;
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
    public class DimensionesController : ControllerBase
    {
        private readonly IDimensionService _dimensionService;
        private readonly IReadServiceBase<Categoria> _categoryService;
        private readonly ILogger<DimensionesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="dimensionService"></param>
        /// <param name="categoryService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public DimensionesController(IDimensionService dimensionService, IReadServiceBase<Categoria> categoryService, ILogger<DimensionesController> logger, IMapper mapper)
        {
            _dimensionService = dimensionService ?? throw new ArgumentNullException(nameof(dimensionService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items with related data
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetDimensiones")]
        [AuthorizePermission(Permissions.PERSONALCARD_ACCESS)]
        [ProducesResponseType(typeof(IEnumerable<DimensionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<DimensionDto>> GetDimensiones()
        {
            var dimensiones = await _dimensionService.GetActiveDimensionsAsync();
            return Ok(_mapper.MapList<DimensionDto>(dimensiones));
        }

        /// <summary>
        /// Get all stored items with related data
        /// </summary>
        /// <param name="ids">Categorias ids that dimensions must contains</param>
        /// <returns></returns>
        [HttpGet("categorias", Name = "GetDimensionesByCategorias")]
        [AuthorizePermission(Permissions.PERSONALCARD_ACCESS)]
        [ProducesResponseType(typeof(IEnumerable<DimensionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<DimensionDto>> GetDimensionesByCategorias([FromQuery] IEnumerable<int> ids)
        {
            var categorias = await _categoryService.GetListAsync(x => ids.Contains(x.Id), q => q.Include(x => x.Dimension), q => q.OrderBy(x => x.Dimension.Orden).ThenBy(x => x.Orden));
            var dimensiones = categorias.GroupBy(x => x.Dimension).ToList();
            dimensiones.ForEach(x => x.Key.Categorias = x.OrderBy(c => c.Orden).ToList());

            return Ok(_mapper.MapList<DimensionDto>(dimensiones.Select(x => x.Key).OrderBy(x => x.Orden)));
        }
    }
}
