﻿using AutoMapper;
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
    public class RangosController : ControllerBase
    {
        private readonly ICrudServiceBase<Rango> _rangoService;
        private readonly ILogger<RangosController> _logger;
        private readonly IMapper _mapper;
        private readonly IFilterPaginable<Rango> _filterPaginator;

        /// <summary>
        /// </summary>
        /// <param name="rangoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public RangosController(ICrudServiceBase<Rango> rangoService, ILogger<RangosController> logger, IMapper mapper, IFilterPaginable<Rango> filterPaginator)
        {
            _rangoService = rangoService ?? throw new ArgumentNullException(nameof(rangoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetRangos")]
        [ProducesResponseType(typeof(IEnumerable<RangoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RangoDto>>> GetRangos()
        {
            var rangos = await _rangoService.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<RangoDto>(rangos));
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetRangosFiltered")]
        [ProducesResponseType(typeof(QueryResult<RangoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<RangoDto>>> GetRangosFiltered([FromBody]QueryData query)
        {
            var result = await _filterPaginator.Execute(_rangoService.Repository.EntitySet, query);
            return _mapper.MapQueryResult<Rango, RangoDto>(result);
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetRango")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(RangoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<RangoDto>> GetRango(int id)
        {
            var Rango = await _rangoService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<RangoDto>(Rango);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="rangoDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(RangoDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostRango")]
        public async Task<ActionResult<RangoDto>> PostRango(RangoDto rangoDto)
        {
            var Rango = _mapper.Map<Rango>(rangoDto);
            var result = _rangoService.Add(Rango);

            _logger.LogInformation($"Se ha añadido un nuevo Rango: {Rango.Descripcion}");

            await _rangoService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRango), new { id = result.Id }, _mapper.Map<RangoDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="rangoDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutRango")]
        public async Task<IActionResult> PutRango(int id, RangoDto rangoDto)
        {
            if (id != rangoDto.Id)
                return BadRequest();

            var Rango = await _rangoService.GetFirstOrDefault(x => x.Id == id);
            if (Rango == null)
                return NotFound();

            _logger.LogInformation($"Se actualiza el Rango {Rango.Descripcion} : {rangoDto.Descripcion}");
            _mapper.Map(rangoDto, Rango);

            _rangoService.Update(Rango);
            await _rangoService.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeleteRango")]
        public async Task<IActionResult> DeleteRango(int id)
        {
            var Rango = await _rangoService.GetFirstOrDefault(x => x.Id == id);
            if (Rango == null)
                return NotFound();

            _logger.LogWarning($"Se elimina el Rango {Rango.Descripcion}");
            _rangoService.Remove(Rango);
            await _rangoService.SaveChangesAsync();

            return NoContent();
        }
    }
}
