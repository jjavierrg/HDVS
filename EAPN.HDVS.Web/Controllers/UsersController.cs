using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
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
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IFilterPaginable<Usuario> _filterPaginator;

        public UsersController(IUsuarioService usuarioService, ILogger<UsersController> logger, IMapper mapper, IFilterPaginable<Usuario> filterPaginator)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));
        }

        [HttpGet(Name = "GetUsarios")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsarios()
        {
            var usuarios = await _usuarioService.GetListAsync(null, q => q.Include(x => x.Perfiles).ThenInclude(x => x.Perfil).Include(x => x.RolesAdicionales).ThenInclude(x => x.Rol));
            return Ok(_mapper.MapList<UsuarioDto>(usuarios));
        }

        [HttpGet("{id}", Name = "GetUsario")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<UsuarioDto>(usuario);
        }

        [HttpPost("filtered", Name = "GetUsariosFiltered")]
        [Authorize(Roles = Roles.USERMANAGEMENT_READ)]
        [ProducesResponseType(typeof(QueryResult<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<UsuarioDto>>> GetUsariosFiltered([FromBody]IQueryData query)
        {
            var result = await _filterPaginator.Execute(_usuarioService.Repository.EntitySet, query);
            return _mapper.MapQueryResult<Usuario, UsuarioDto>(result);
        }

        [Authorize(Roles = Roles.USERMANAGEMENT_WRITE)]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostUsuario")]
        public async Task<ActionResult<UsuarioDto>> PostUsuario(UsuarioDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            var result = await _usuarioService.CreateAsync(usuario, usuarioDto.Clave);

            return CreatedAtAction(nameof(GetUsuario), new { id = result.Id }, _mapper.Map<UsuarioDto>(result));
        }

        [Authorize(Roles = Roles.USERMANAGEMENT_WRITE)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutUsuario")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.Id)
                return BadRequest();

            var usuario = _mapper.Map<Usuario>(usuarioDto);
            _usuarioService.Update(usuario);
            await _usuarioService.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = Roles.USERMANAGEMENT_DELETE)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteUsuario")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _usuarioService.GetFirstOrDefault(x => x.Id == id);
            if (usuario == null)
                return NotFound();

            _usuarioService.Remove(usuario);
            await _usuarioService.SaveChangesAsync();

            return NoContent();
        }
    }
}
