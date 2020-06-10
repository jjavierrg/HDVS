using AutoMapper;
using EAPN.HDVS.Application.Services.User;
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
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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
        [AuthorizePermission(Permissions.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetListAsync(GetSuperadminExclusionFilter(), q => q.Include(x => x.Perfiles).ThenInclude(x => x.Perfil).Include(x => x.PermisosAdicionales).ThenInclude(x => x.Permiso).Include(x => x.Organizacion), q => q.OrderBy(x => x.Email));
            _logger.LogInformation("Obtiene el listado de usuarios");
            return Ok(_mapper.MapList<UsuarioDto>(usuarios));
        }

        /// <summary>
        /// Get all stored user from current partner
        /// </summary>
        /// <returns></returns>
        [HttpGet("materdata", Name = "GetUsuariosAsMasterdata")]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetUsuariosAsMasterdata()
        {
            var usuarios = await _usuarioService.GetListAsync(x => x.OrganizacionId == User.GetUserOrganizacionId(), orderBy: q => q.OrderBy(x => x.Nombre).ThenBy(x => x.Apellidos));
            _logger.LogInformation("Obtiene el listado de usuarios (masterdata)");
            return Ok(_mapper.MapList<MasterDataDto>(usuarios));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUsuario")]
        [AuthorizePermission(Permissions.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var query = _usuarioService.Repository.EntitySet.Where(x => x.Id == id);
            var filter = GetSuperadminExclusionFilter();
            if (filter != null)
                query = query.Where(filter);

            query = query.Include(x => x.Perfiles).ThenInclude(x => x.Perfil).Include(x => x.PermisosAdicionales).ThenInclude(x => x.Permiso);
            var usuario = await query.FirstOrDefaultAsync();
            
            if (usuario != null)
                _logger.LogInformation($"Accede a la información del usuario: {usuario.Email}[Id: {usuario.Id}]");

            return _mapper.Map<UsuarioDto>(usuario);
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetUsuariosFiltered")]
        [AuthorizePermission(Permissions.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(QueryResult<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<UsuarioDto>>> GetUsuariosFiltered([FromBody]QueryData query)
        {
            var result = await _filterPaginator.Execute(_usuarioService.Repository.EntitySet, query);
            _logger.LogInformation($"Realiza una búsqueda de usuarios con los siguientes filtros: ${query.FilterParameters}");

            return _mapper.MapQueryResult<Usuario, UsuarioDto>(result);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="usuarioDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.USERMANAGEMENT_WRITE)]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status403Forbidden)]
        [HttpPost(Name = "PostUsuario")]
        public async Task<ActionResult<UsuarioDto>> PostUsuario(UsuarioDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            var organizacionId = usuario.OrganizacionId;

            // If user is not superadmin, cannot create user of other partner
            if (organizacionId != User.GetUserOrganizacionId() && !User.HasSuperAdminPermission())
            {
                _logger.LogCritical($"Creación de usuario {usuario.Email}[Id: {usuario.Id}] no autorizada: Se ha intentado crear un usuario de otra organización. Organización_id solicitada: {usuarioDto.OrganizacionId}");
                return Forbid();
            }

            var result = await _usuarioService.CreateAsync(usuario, usuarioDto.Clave);
            _logger.LogInformation($"Crea el usuario {usuario.Email}[Id: {usuario.Id}]");

            return CreatedAtAction(nameof(GetUsuario), new { id = result.Id }, _mapper.Map<UsuarioDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="usuarioDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.USERMANAGEMENT_WRITE)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutUsuario")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.Id)
                return BadRequest();

            var usuario = await _usuarioService.GetSingleOrDefault(x => x.Id == id, q => q.Include(x => x.Perfiles).Include(x => x.PermisosAdicionales));
            var organizacionId = usuario.OrganizacionId;

            // If user is not superadmin, cannot change user partner
            if (organizacionId != usuarioDto.OrganizacionId && !User.HasSuperAdminPermission())
            {
                _logger.LogCritical($"Cambio de organización para el usuario {usuario.Email}[Id: {usuario.Id}] no autorizado: OrganizaciónID Original:{organizacionId} -> OrganizaciónID Solicitado: {usuarioDto.OrganizacionId}");
                return Forbid();
            }
            else if (organizacionId != User.GetUserOrganizacionId() && !User.HasSuperAdminPermission())
            {
                _logger.LogCritical($"Actualización para el usuario {usuario.Email}[Id: {usuario.Id}] no autorizado: El usuario pertenece a otra organización. OrganizaciónID Solicitado: {usuarioDto.OrganizacionId}");
                return Forbid();
            }

            _mapper.Map(usuarioDto, usuario);
            _logger.LogInformation($"Actuliza el usuario {usuario.Email}[Id: {usuario.Id}]");

            _usuarioService.UpdateWithtPass(usuario);
            await _usuarioService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.USERMANAGEMENT_DELETE)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteUsuario")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var query = _usuarioService.Repository.EntitySet.Where(x => x.Id == id);
            var securityCheck = await query.FirstOrDefaultAsync();
            
            // If user is not superadmin, cannot create delete of other partners
            if (securityCheck.OrganizacionId != User.GetUserOrganizacionId() && !User.HasSuperAdminPermission())
            {
                _logger.LogCritical($"Eliminación del usuario {securityCheck.Email}[Id: {securityCheck.Id}] no autorizada: El usuario pertenece a otra organización [id: {securityCheck.OrganizacionId}].");
                return Forbid();
            }

            var filter = GetSuperadminExclusionFilter();
            if (filter != null)
                query = query.Where(filter);

            var usuario = await query.FirstOrDefaultAsync();
            if (usuario == null)
                return NotFound();

            _logger.LogInformation($"Elimina {usuario.Email}[Id: {usuario.Id}]");
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
            if (!User.HasSuperAdminPermission())
                filter = x => x.OrganizacionId == User.GetUserOrganizacionId() && !(x.PermisosAdicionales.Any(r => r.Permiso.Clave == Permissions.APP_SUPERADMIN) || x.Perfiles.Any(p => p.Perfil.Permisos.Any(r => r.Permiso.Clave == Permissions.APP_SUPERADMIN)));

            return filter;
        }
    }
}
