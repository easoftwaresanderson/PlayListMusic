using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Core.Interfaces.IServices;
using APIMusicPlayLists.Infra.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIMusicPlayLists.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly ILogger<MusicController> _logger;
        private IMusicServices _service;

        public MusicController(ILogger<MusicController> logger, IMusicServices service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/<MusicController>
        // GET: api/Music
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicDTO>>> Get()
        {
            try
            {
                var reg = await _service.Get();

                if (reg == null || reg.Count() == 0)
                {
                    return NotFound();
                }

                return Ok(reg);

            }
            catch (Exception ex)
            {
                return new JsonResult(new { code = ex.GetHashCode(), message = "Oh no...something bad happened :(", description = ex.Message });
            }

        }

        // GET api/<MusicController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MusicDTO>> Get(int id)
        {
            try
            {
                var reg = await _service.GetByIdAsync(id);

                if (reg == null)
                {
                    return NotFound();
                }

                return Ok(reg);

            }
            catch (Exception ex)
            {
                return new JsonResult(new { code = ex.GetHashCode(), message = "Oh no.. something bad happened :(", description = ex.Message });
            }
        }

        // POST api/<MusicController>
        [HttpPost]
        public async Task<ActionResult<ResultDTO>> Post([FromBody] MusicDTO data)
        {
            try
            {
                return Ok(await _service.PostAsync(data));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { code = ex.GetHashCode(), message = "Oh no.. something bad happened :(", description = ex.Message });
            }

        }

        // PUT api/<MusicController>/5
        [HttpPut]
        public async Task<ActionResult<ResultDTO>> Put([FromBody] MusicDTO data)
        {
            try
            {
                return Ok(await _service.PutAsync(data));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { code = ex.GetHashCode(), message = "Oh no.. something bad happened :(", description = ex.Message });
            }

        }

        // DELETE api/<MusicController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultDTO>> Delete(int id)
        {
            try
            {
                return Ok(await _service.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { code = ex.GetHashCode(), message = "Oh no.. something bad happened :(", description = ex.Message });
            }
        }
    }
}
