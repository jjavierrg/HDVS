using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Queries;
using EAPN.HDVS.Infrastructure.Core.Repository;
using EAPN.HDVS.Shared.Permissions;
using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Extensions;
using EAPN.HDVS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EAPN.HDVS.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdjuntosController : ControllerBase
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly ICrudServiceBase<Adjunto> _adjuntoService;
        private readonly ICrudServiceBase<TipoAdjunto> _adjuntoTipoService;
        private readonly ILogger<AdjuntosController> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string _attachmentsFolder;
        private readonly IFilterPaginable<Adjunto> _filterPaginator;

        /// <summary>
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="adjuntoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        public AdjuntosController(IWebHostEnvironment hostingEnvironment,
                                  ICrudServiceBase<Adjunto> adjuntoService,
                                  ICrudServiceBase<TipoAdjunto> adjuntoTipoService,
                                  ILogger<AdjuntosController> logger,
                                  IMapper mapper,
                                  IConfiguration configuration,
                                  IFilterPaginable<Adjunto> filterPaginator)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _adjuntoService = adjuntoService ?? throw new ArgumentNullException(nameof(adjuntoService));
            _adjuntoTipoService = adjuntoTipoService ?? throw new ArgumentNullException(nameof(adjuntoTipoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _filterPaginator = filterPaginator ?? throw new ArgumentNullException(nameof(filterPaginator));

            _attachmentsFolder = _configuration.GetValue<string>("AttachmentsFolder");
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetAdjunto")]
        [AuthorizePermission(Permissions.PERSONALATTACHMENTS_READ)]
        [ProducesResponseType(typeof(AdjuntoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<AdjuntoDto>> GetAdjunto(int id)
        {
            var adjunto = await _adjuntoService.GetFirstOrDefault(x => x.Id == id);

            // Verificamos restricciones
            if (adjunto.OrganizacionId.HasValue && adjunto.OrganizacionId.Value != User.GetUserOrganizacionId())
                return NotFound();

            return _mapper.Map<AdjuntoDto>(adjunto);
        }

        /// <summary>
        /// Get all items with a specific criteria filter
        /// </summary>
        /// <param name="query">Query criteria filter</param>
        /// <returns></returns>
        [HttpPost("filtered", Name = "GetAdjuntosFiltered")]
        [AuthorizePermission(Permissions.PERSONALATTACHMENTS_READ)]
        [ProducesResponseType(typeof(QueryResult<AdjuntoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<AdjuntoDto>>> GetAdjuntosFiltered([FromBody]QueryData query)
        {
            var queryCollection = _adjuntoService.Repository.EntitySet.Where(x => !x.OrganizacionId.HasValue || x.OrganizacionId.Value == User.GetUserOrganizacionId());
            var result = await _filterPaginator.Execute(queryCollection, query);

            return _mapper.MapQueryResult<Adjunto, AdjuntoDto>(result);
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("file/{id}", Name = "GetFile")]
        [Produces("application/octet-stream")]
        [AuthorizePermission(Permissions.PERSONALATTACHMENTS_READ)]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<FileStreamResult> GetFile(int id)
        {
            var adjunto = await _adjuntoService.GetFirstOrDefault(x => x.Id == id, q => q.Include(x => x.Tipo));

            if (adjunto == null)
                return null;

            // Verificamos restricciones
            if (adjunto.OrganizacionId.HasValue && adjunto.OrganizacionId.Value != User.GetUserOrganizacionId())
                return null;

            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, _attachmentsFolder, adjunto.Tipo?.Carpeta, adjunto.Alias);
            var fileStream = System.IO.File.OpenRead(filePath);

            var pvd = new FileExtensionContentTypeProvider();
            bool isKnownType = pvd.TryGetContentType(adjunto.Alias, out string mimeType);

            return File(fileStream, isKnownType ? mimeType : "application/octet-stream");
        }

        /// <summary>
        /// Endpoint to view public images or profile images
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("view/{id}", Name = "ViewFile")]
        [Produces("application/octet-stream")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<FileStreamResult> ViewImage(int id)
        {
            var adjunto = await _adjuntoService.GetFirstOrDefault(x => x.Id == id, q => q.Include(x => x.Tipo).Include(x => x.FotoUsuario).Include(x => x.FotoFicha));

            if (adjunto == null)
                return null;

            // Verificamos restricciones
            if (adjunto.OrganizacionId.HasValue && adjunto.OrganizacionId.Value != User.GetUserOrganizacionId() && !User.HasSuperAdminPermission())
                return null;

            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, _attachmentsFolder, adjunto.Tipo?.Carpeta, adjunto.Alias);
            var fileStream = System.IO.File.OpenRead(filePath);

            var pvd = new FileExtensionContentTypeProvider();
            bool isKnownType = pvd.TryGetContentType(adjunto.Alias, out string mimeType);

            return File(fileStream, isKnownType ? mimeType : "application/octet-stream"); 
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="subidaAdjuntoDto">Item data</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(AdjuntoDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostAdjunto")]
        public async Task<ActionResult<AdjuntoDto>> PostAdjunto([FromForm] SubidaAdjuntoDto subidaAdjuntoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (subidaAdjuntoDto.File.Length == 0)
                return NoContent();

            // user with no specific permission, only can update their user image
            if (!User.HasPermission(Permissions.PERSONALATTACHMENTS_WRITE) &&
                (subidaAdjuntoDto.FichaId.HasValue 
                || !subidaAdjuntoDto.OrganizacionId.HasValue 
                || subidaAdjuntoDto.OrganizacionId != User.GetUserOrganizacionId()))
                return BadRequest();

            var tipo = await _adjuntoTipoService.GetFirstOrDefault(x => x.Id == subidaAdjuntoDto.TipoId);
            var extension = Path.GetExtension(subidaAdjuntoDto.File.FileName);
            var adjunto = new Adjunto
            {
                FechaAlta = DateTime.Now,
                NombreOriginal = subidaAdjuntoDto.File.FileName,
                Tamano = subidaAdjuntoDto.File.Length,
                UsuarioId = User.GetUserId(),
                OrganizacionId = subidaAdjuntoDto.OrganizacionId,
                Alias = $"{Guid.NewGuid()}{extension}",
                TipoId = subidaAdjuntoDto.TipoId,
                FichaId = subidaAdjuntoDto.FichaId,
            };

            try
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, _attachmentsFolder, tipo?.Carpeta);
                var di = Directory.CreateDirectory(filePath);
                filePath = Path.Combine(filePath, adjunto.Alias);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await subidaAdjuntoDto.File.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading files");
                throw ex;
            }

            _adjuntoService.Add(adjunto);
            await _adjuntoService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdjunto), new { id = adjunto.Id }, _mapper.Map<AdjuntoDto>(adjunto));
        }

        /// <summary>
        /// Delete existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthorizePermission(Permissions.PERSONALATTACHMENTS_DELETE)]
        [HttpDelete("{id}", Name = "DeleteAdjunto")]
        public async Task<IActionResult> DeleteAdjunto(int id)
        {
            var adjunto = await _adjuntoService.GetFirstOrDefault(x => x.Id == id, q => q.Include(x => x.Tipo));
            if (adjunto == null)
                return NotFound();

            _logger.LogWarning($"Se elimina el adjunto {adjunto.NombreOriginal}");

            _adjuntoService.Remove(adjunto);
            await _adjuntoService.SaveChangesAsync();

            var filePath = Path.Combine(_attachmentsFolder, adjunto.Tipo?.Carpeta, adjunto.Alias);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return NoContent();
        }
    }
}
