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

namespace EAPN.HDVS.Web.Contperfillers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesController : ControllerBase
    {
        private readonly ICrudServiceBase<Perfil> _perfilService;
        private readonly ILogger<PerfilesController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="perfilService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public PerfilesController(ICrudServiceBase<Perfil> perfilService, ILogger<PerfilesController> logger, IMapper mapper)
        {
            _perfilService = perfilService ?? throw new ArgumentNullException(nameof(perfilService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items with related data
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetPerfiles")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<PerfilDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PerfilDto>> GetPerfiles()
        {
            var perfiles = await _perfilService.GetListAsync(includes: q => q.Include(x => x.Permisos).ThenInclude(x => x.Permiso).Include(x => x.Usuarios), orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<PerfilDto>(perfiles));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetPerfilesAsMasterData")]
        [AuthorizePermission(Permissions.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetPerfilesAsMasterData()
        {
            var perfiles = await _perfilService.GetListAsync(GetSuperadminExclusionFilter(), orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<MasterDataDto>(perfiles));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPerfil")]
        [AuthorizePermission(Permissions.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PerfilDto>> GetPerfil(int id)
        {
            var query = _perfilService.Repository.EntitySet;
            var filter = GetSuperadminExclusionFilter();
            if (filter != null)
                query.Where(filter);

            var perfil = await query.Include(x => x.Permisos).ThenInclude(x => x.Permiso).FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<PerfilDto>(perfil);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="perfilDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostPerfil")]
        public async Task<ActionResult<PerfilDto>> PostPerfil(PerfilDto perfilDto)
        {
            var perfil = _mapper.Map<Perfil>(perfilDto);
            var result = _perfilService.Add(perfil);

            await _perfilService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPerfil), new { id = result.Id }, _mapper.Map<PerfilDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="perfilDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutPerfil")]
        public async Task<IActionResult> PutPerfil(int id, PerfilDto perfilDto)
        {
            if (id != perfilDto.Id)
                return BadRequest();

            var perfil = await _perfilService.GetFirstOrDefault(x => x.Id == id, q => q.Include(x => x.Permisos));
            if (perfil == null)
                return NotFound();

            _mapper.Map(perfilDto, perfil);

            _perfilService.Update(perfil);
            await _perfilService.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeletePerfil")]
        public async Task<IActionResult> DeletePerfil(int id)
        {
            var perfil = await _perfilService.GetFirstOrDefault(x => x.Id == id);
            if (perfil == null)
                return NotFound();

            _perfilService.Remove(perfil);
            await _perfilService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Profiles with superadmin permission exclusion filter
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Perfil, bool>> GetSuperadminExclusionFilter()
        {
            Expression<Func<Perfil, bool>> filter = null;
            if (!User.HasSuperAdminPermission())
                filter = x => !x.Permisos.Any(r => r.Permiso.Clave == Permissions.APP_SUPERADMIN);

            return filter;
        }
    }
}
