using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Shared.Roles;
using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Extensions;
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
    public class RolesController : ControllerBase
    {
        private readonly IReadServiceBase<Rol> _rolService;
        private readonly ILogger<RolesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="rolService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public RolesController(IReadServiceBase<Rol> rolService, ILogger<RolesController> logger, IMapper mapper)
        {
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetRoles")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RolDto>>> GetRoles()
        {
            var roles = await _rolService.GetListAsync(GetSuperadminExclusionFilter(), orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<RolDto>(roles));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetRol")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(RolDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<RolDto>> GetRol(int id)
        {
            var query = _rolService.Repository.EntitySet.Where(x => x.Id == id);
            var filter = GetSuperadminExclusionFilter();
            if (filter != null)
                query = query.Where(filter);

            var rol = await query.FirstOrDefaultAsync();
            return _mapper.Map<RolDto>(rol);
        }

        /// <summary>
        /// Role with superadmin permission exclusion filter
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Rol, bool>> GetSuperadminExclusionFilter()
        {
            Expression<Func<Rol, bool>> filter = null;
            if (!User.IsInRole(Roles.USERMANAGEMENT_ADMIN))
                filter = x => x.Permiso != Roles.USERMANAGEMENT_ADMIN;

            return filter;
        }
    }
}
