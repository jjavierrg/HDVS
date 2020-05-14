using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
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
    public class PermisosController : ControllerBase
    {
        private readonly IReadServiceBase<Permiso> _permisoService;
        private readonly ILogger<PermisosController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="permisoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public PermisosController(IReadServiceBase<Permiso> permisoService, ILogger<PermisosController> logger, IMapper mapper)
        {
            _permisoService = permisoService ?? throw new ArgumentNullException(nameof(permisoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetPermisos")]
        [AuthorizePermission(Permissions.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<PermisoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermisoDto>>> GetPermisos()
        {
            var permisos = await _permisoService.GetListAsync(GetSuperadminExclusionFilter(), orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<PermisoDto>(permisos));
        }

        /// <summary>
        /// Get all stored items as masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetPermisosAsMasterdata")]
        [AuthorizePermission(Permissions.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetPermisosAsMasterdata()
        {
            var permisos = await _permisoService.GetListAsync(GetSuperadminExclusionFilter(), orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<MasterDataDto>(permisos));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPermiso")]
        [AuthorizePermission(Permissions.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(PermisoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PermisoDto>> GetPermiso(int id)
        {
            var query = _permisoService.Repository.EntitySet.Where(x => x.Id == id);
            var filter = GetSuperadminExclusionFilter();
            if (filter != null)
                query = query.Where(filter);

            var permiso = await query.FirstOrDefaultAsync();
            return _mapper.Map<PermisoDto>(permiso);
        }

        /// <summary>
        /// Permissions with superadmin permission exclusion filter
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Permiso, bool>> GetSuperadminExclusionFilter()
        {
            Expression<Func<Permiso, bool>> filter = null;
            if (!User.HasSuperAdminPermission())
                filter = x => x.Clave != Permissions.APP_SUPERADMIN;

            return filter;
        }
    }
}
