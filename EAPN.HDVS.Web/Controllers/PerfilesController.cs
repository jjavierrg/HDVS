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

namespace EAPN.HDVS.Web.Contperfillers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesController : ControllerBase
    {
        private readonly IReadServiceBase<Perfil> _perfilService;
        private readonly ILogger<PerfilesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="perfilService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public PerfilesController(IReadServiceBase<Perfil> perfilService, ILogger<PerfilesController> logger, IMapper mapper)
        {
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Obtiene los perfiles almacenados en la aplicación
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetPerfiles")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<PerfilDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PerfilDto>>> GetPerfiles()
        {
            var perfiles = await _perfilService.GetListAsync(GetSuperadminExclusionFilter(), orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<PerfilDto>(perfiles));
        }

        /// <summary>
        /// Obtiene un perfil específico
        /// </summary>
        /// <param name="id">Identificador del perfil a obtener</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPerfil")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PerfilDto>> GetPerfil(int id)
        {
            var query = _perfilService.Repository.EntitySet;
            var filter = GetSuperadminExclusionFilter();
            if (filter != null)
                query.Where(filter);

            var perfil = await query.FirstOrDefaultAsync();
            return _mapper.Map<PerfilDto>(perfil);
        }

        /// <summary>
        /// Obtiene la consulta para filtrar perfiles de superadministrador
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Perfil, bool>> GetSuperadminExclusionFilter()
        {
            Expression<Func<Perfil, bool>> filter = null;
            if (!User.IsInRole(Roles.USERMANAGEMENT_ADMIN))
                filter = x => !x.Roles.Any(r => r.Rol.Permiso == Roles.USERMANAGEMENT_ADMIN);

            return filter;
        }
    }
}
