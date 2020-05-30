using AutoMapper;
using EAPN.HDVS.Application.Core.Services;
using EAPN.HDVS.Entities;
using EAPN.HDVS.Infrastructure.Core.Repository;
using EAPN.HDVS.Web.Dto;
using EAPN.HDVS.Web.Extensions;
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
        private readonly IReadRepository<TipoAdjunto> _adjuntoRepository;
        private readonly ILogger<AdjuntosController> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string _attachmentsFolder;

        /// <summary>
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="adjuntoService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="adjuntoRepository"></param>
        /// <param name="configuration"></param>
        public AdjuntosController(IWebHostEnvironment hostingEnvironment,
                                  ICrudServiceBase<Adjunto> adjuntoService,
                                  ILogger<AdjuntosController> logger,
                                  IMapper mapper,
                                  IReadRepository<TipoAdjunto> adjuntoRepository,
                                  IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _adjuntoService = adjuntoService ?? throw new ArgumentNullException(nameof(adjuntoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _adjuntoRepository = adjuntoRepository ?? throw new ArgumentNullException(nameof(adjuntoRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _attachmentsFolder = _configuration.GetValue<string>("AttachmentsFolder");
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetAdjunto")]
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
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("display/{id}", Name = "DisplayAdjunto")]
        [Produces("application/octet-stream")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<FileStreamResult> DisplayAdjunto(int id)
        {
            var adjunto = await _adjuntoService.GetFirstOrDefault(x => x.Id == id, q => q.Include(x => x.Tipo));

            // Verificamos restricciones
            if (adjunto.OrganizacionId.HasValue && adjunto.OrganizacionId.Value != User.GetUserOrganizacionId())
                return null;

            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, _attachmentsFolder, adjunto.FullPath, adjunto.Alias);
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

            var tipo = await _adjuntoRepository.GetFirstOrDefault(x => x.Id == subidaAdjuntoDto.TipoId);
            var extension = Path.GetExtension(subidaAdjuntoDto.File.FileName);
            var adjunto = new Adjunto {
                FechaAlta = DateTime.Now,
                NombreOriginal = subidaAdjuntoDto.File.FileName,
                Tamano = subidaAdjuntoDto.File.Length,
                UsuarioId = User.GetUserId(),
                OrganizacionId = subidaAdjuntoDto.OrganizacionId,
                Alias = $"{Guid.NewGuid()}{extension}",
                TipoId = subidaAdjuntoDto.TipoId,
                FichaId = subidaAdjuntoDto.FichaId,
                Tipo = tipo
            };

            try
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, _attachmentsFolder, adjunto.FullPath);
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

            // remove for avoid EF exception
            adjunto.Tipo = null;

            _adjuntoService.Add(adjunto);
            await _adjuntoService.SaveChangesAsync();

            adjunto.Tipo = tipo;
            return CreatedAtAction(nameof(GetAdjunto), new { id = adjunto.Id }, _mapper.Map<AdjuntoDto>(adjunto));
        }

        /// <summary>
        /// Delete existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}", Name = "DeleteAdjunto")]
        public async Task<IActionResult> DeleteAdjunto(int id)
        {
            var adjunto = await _adjuntoService.GetFirstOrDefault(x => x.Id == id);
            if (adjunto == null)
                return NotFound();

            _logger.LogWarning($"Se elimina el adjunto {adjunto.NombreOriginal}");

            var filePath = Path.Combine(_attachmentsFolder, adjunto.FullPath, adjunto.Alias);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _adjuntoService.Remove(adjunto);
            await _adjuntoService.SaveChangesAsync();

            return NoContent();
        }
    }
}
