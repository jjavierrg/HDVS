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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IReadServiceBase<LogEntry> _logEntryService;
        private readonly IMapper _mapper;
        private readonly IFilterPaginable<LogEntry> _filterPaginator;

        /// <summary>
        /// </summary>
        /// <param name="logEntryService"></param>
        /// <param name="mapper"></param>
        /// <param name="filterPaginator"></param>
        public LogController(IReadServiceBase<LogEntry> logEntryService, IMapper mapper, IFilterPaginable<LogEntry> filterPaginator)
        {
            _logEntryService = logEntryService ?? throw new ArgumentNullException(nameof(logEntryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetLogs")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<LogEntryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LogEntryDto>>> GetLogs()
        {
            var entries = await _logEntryService.GetListAsync(includes: q => q.Include(x => x.Usuario).ThenInclude(x => x.Organizacion), orderBy: q => q.OrderByDescending(x => x.Date));
            return Ok(_mapper.MapList<LogEntryDto>(entries));
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetLogsFiltered")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(QueryResult<LogEntryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<LogEntryDto>>> GetLogsFiltered([FromBody] QueryData query)
        {
            IQueryable<LogEntry> basequery = _logEntryService.Repository.EntitySet.Include(x => x.Usuario).ThenInclude(x => x.Organizacion);
            var result = await _filterPaginator.Execute(basequery, query);
            return _mapper.MapQueryResult<LogEntry, LogEntryDto>(result);
        }
    }
}
