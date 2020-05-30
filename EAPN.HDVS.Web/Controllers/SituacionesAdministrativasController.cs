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
    public class SituacionesAdministrativasController : ControllerBase
    {
        private readonly ICrudServiceBase<SituacionAdministrativa> _situacionesAdministrativaservice;
        private readonly ILogger<SituacionesAdministrativasController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary>
        /// <param name="situacionesAdministrativaservice"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public SituacionesAdministrativasController(ICrudServiceBase<SituacionAdministrativa> situacionesAdministrativaservice, ILogger<SituacionesAdministrativasController> logger, IMapper mapper)
        {
            _situacionesAdministrativaservice = situacionesAdministrativaservice ?? throw new ArgumentNullException(nameof(situacionesAdministrativaservice));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all stored items
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetSituacionesAdministrativas")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(IEnumerable<SituacionAdministrativaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SituacionAdministrativaDto>>> GetSituacionesAdministrativas()
        {
            var situacionesAdministrativas = await _situacionesAdministrativaservice.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<SituacionAdministrativaDto>(situacionesAdministrativas));
        }

        /// <summary>
        /// Get all stored items mapped as Masterdata
        /// </summary>
        /// <returns></returns>
        [HttpGet("masterdata", Name = "GetSituacionesAdministrativasAsMasterData")]
        [ProducesResponseType(typeof(IEnumerable<MasterDataDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MasterDataDto>>> GetSituacionesAdministrativasAsMasterData()
        {
            var situacionesAdministrativas = await _situacionesAdministrativaservice.GetListAsync(orderBy: q => q.OrderBy(x => x.Descripcion));
            return Ok(_mapper.MapList<MasterDataDto>(situacionesAdministrativas));
        }

        /// <summary>
        /// Get the item with the specified identifier
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSituacionAdministrativa")]
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(SituacionAdministrativaDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SituacionAdministrativaDto>> GetSituacionAdministrativa(int id)
        {
            var situacionAdministrativa = await _situacionesAdministrativaservice.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<SituacionAdministrativaDto>(situacionAdministrativa);
        }

        /// <summary>
        /// Add new item to collection
        /// </summary>
        /// <param name="situacionAdministrativaDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(typeof(SituacionAdministrativaDto), StatusCodes.Status201Created)]
        [HttpPost(Name = "PostSituacionAdministrativa")]
        public async Task<ActionResult<SituacionAdministrativaDto>> PostSituacionAdministrativa(SituacionAdministrativaDto situacionAdministrativaDto)
        {
            var situacionAdministrativa = _mapper.Map<SituacionAdministrativa>(situacionAdministrativaDto);
            var result = _situacionesAdministrativaservice.Add(situacionAdministrativa);

            _logger.LogInformation($"Se ha añadido un nuevo situacionAdministrativa: {situacionAdministrativa.Descripcion}");

            await _situacionesAdministrativaservice.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSituacionAdministrativa), new { id = result.Id }, _mapper.Map<SituacionAdministrativaDto>(result));
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="id">Item identifier</param>
        /// <param name="situacionAdministrativaDto">Item data</param>
        /// <returns></returns>
        [AuthorizePermission(Permissions.APP_SUPERADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}", Name = "PutSituacionAdministrativa")]
        public async Task<IActionResult> PutSituacionAdministrativa(int id, SituacionAdministrativaDto situacionAdministrativaDto)
        {
            if (id != situacionAdministrativaDto.Id)
                return BadRequest();

            var situacionAdministrativa = await _situacionesAdministrativaservice.GetFirstOrDefault(x => x.Id == id);
            if (situacionAdministrativa == null)
                return NotFound();

            _logger.LogInformation($"Se actualiza el situacionAdministrativa {situacionAdministrativa.Descripcion} : {situacionAdministrativaDto.Descripcion}");
            _mapper.Map(situacionAdministrativaDto, situacionAdministrativa);

            _situacionesAdministrativaservice.Update(situacionAdministrativa);
            await _situacionesAdministrativaservice.SaveChangesAsync();

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
        [HttpDelete("{id}", Name = "DeleteSituacionAdministrativa")]
        public async Task<IActionResult> DeleteSituacionAdministrativa(int id)
        {
            var situacionAdministrativa = await _situacionesAdministrativaservice.GetFirstOrDefault(x => x.Id == id);
            if (situacionAdministrativa == null)
                return NotFound();

            _logger.LogWarning($"Se elimina el situacionAdministrativa {situacionAdministrativa.Descripcion}");
            _situacionesAdministrativaservice.Remove(situacionAdministrativa);
            await _situacionesAdministrativaservice.SaveChangesAsync();

            return NoContent();
        }
    }
}
