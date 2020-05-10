using AutoMapper;
using EAPN.HDVS.Application.Services.User;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Queries;
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
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;
        private readonly IMapper _mapper;
        private readonly IFilterPaginable<Usuario> _filterPaginator;

        /// <summary>
        /// </summary>
        /// <param name="usuarioService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="filterPaginator"></param>
        public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger, IMapper mapper, IFilterPaginable<Usuario> filterPaginator)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetUsuarios")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetListAsync(GetSuperadminExclusionFilter(), q => q.Include(x => x.Perfiles).ThenInclude(x => x.Perfil).Include(x => x.RolesAdicionales).ThenInclude(x => x.Rol), q => q.OrderBy(x => x.Email));
            return Ok(_mapper.MapList<UsuarioDto>(usuarios));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUsuario")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var query = _usuarioService.Repository.EntitySet.Where(x => x.Id == id);
            var filter = GetSuperadminExclusionFilter();
            if (filter != null)
                query = query.Where(filter);

            query = query.Include(x => x.Perfiles).ThenInclude(x => x.Perfil).Include(x => x.RolesAdicionales).ThenInclude(x => x.Rol);

            return _mapper.Map<UsuarioDto>(await query.FirstOrDefaultAsync());
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetUsuariosFiltered")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(QueryResult<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<UsuarioDto>>> GetUsuariosFiltered([FromBody]IQueryData query)
        {
            var result = await _filterPaginator.Execute(_usuarioService.Repository.EntitySet, query);
            return _mapper.MapQueryResult<Usuario, UsuarioDto>(result);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="usuarioDto">Item data</param>
        /// <returns></returns>
        [Authorize(Roles = Roles.USERMANAGEMENT_WRITE)]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostUsuario")]
        public async Task<ActionResult<UsuarioDto>> PostUsuario(UsuarioDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            var result = await _usuarioService.CreateAsync(usuario, usuarioDto.Clave);

            return CreatedAtAction(nameof(GetUsuario), new { id = result.Id }, _mapper.Map<UsuarioDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="usuarioDto">Item data</param>
        /// <returns></returns>
        [Authorize(Roles = Roles.USERMANAGEMENT_WRITE)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutUsuario")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.Id)
                return BadRequest();

            var usuario = _mapper.Map<Usuario>(usuarioDto);
            _usuarioService.UpdateWithtPass(usuario);
            await _usuarioService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [Authorize(Roles = Roles.USERMANAGEMENT_DELETE)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteUsuario")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var query = _usuarioService.Repository.EntitySet.Where(x => x.Id == id);
            var filter = GetSuperadminExclusionFilter();
            if (filter != null)
                query = query.Where(filter);

            var usuario = await query.FirstOrDefaultAsync();
            if (usuario == null)
                return NotFound();

            _usuarioService.Remove(usuario);
            await _usuarioService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Users with superadmin permission exclusion filter
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Usuario, bool>> GetSuperadminExclusionFilter()
        {
            Expression<Func<Usuario, bool>> filter = null;
            if (!User.IsInRole(Roles.USERMANAGEMENT_ADMIN))
                filter = x => !(x.RolesAdicionales.Any(r => r.Rol.Permiso == Roles.USERMANAGEMENT_ADMIN) || x.Perfiles.Any(p => p.Perfil.Roles.Any(r => r.Rol.Permiso == Roles.USERMANAGEMENT_ADMIN)));

            return filter;
        }
    }
}
