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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DimensionesController : ControllerBase
    {
        private readonly IDimenssionService _dimensionService;
        private readonly ILogger<DimensionesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="dimensionService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public DimensionesController(IDimenssionService dimensionService, ILogger<DimensionesController> logger, IMapper mapper)
        {
            _dimensionService = dimensionService ?? throw new ArgumentNullException(nameof(dimensionService));
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
            var dimensiones = await _dimensionService.GetActiveDimenssionsAsync();
            return Ok(_mapper.MapList<DimensionDto>(dimensiones));
        }
    }
}
