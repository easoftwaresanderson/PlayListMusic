using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Core.Interfaces.IServices;
using APIMusicPlayLists.Infra.Shared.Commands;
using APIMusicPlayLists.Infra.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace APIMusicPlayLists.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlayListController : ControllerBase
    {
        private readonly ILogger<PlayListController> _logger;
        private IPlayListServices _service;
        private IMusicServices _musicServices;

        public PlayListController(ILogger<PlayListController> logger, IPlayListServices service, IMusicServices musicServices)
        {
            _logger = logger;
            _service = service;
            _musicServices = musicServices;
        }

        // GET: api/<PlayListController>
        // GET: api/PlayList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayList>>> Get()
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

        // GET api/<PlayListController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayList>> Get(int id)
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

        // GET api/<PlayListController>/5
        [HttpGet("device/{id}")]
        public async Task<ActionResult<PlayList>> GetByDeviceID(int id)
        {
            try
            {
                var reg = await _service.GetByDeviceIdAsync(id);

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

        // POST api/<PlayListController>
        [HttpPost]
        public async Task<ActionResult<ResultDTO>> Post([FromBody] PlayListDTO data)
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

        // POST api/<PlayListController>
        [HttpPost("favoritesong")]
        public async Task<ActionResult<ResultDTO>> FavoriteSong([FromBody] PlayListFavoriteCommand data)
        {
            try
            {
               
                    var music = await _musicServices.GetByIdAsync(data.MusicId);
                    
                    music.Favorite = data.Favorite;

                    return Ok(await _musicServices.PutAsync(music));


                    var res = await _service.FavoriteSong(data);


                    return Ok(await _musicServices.PutAsync(music));

             

                return Ok(await _service.FavoriteSong(data));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { code = ex.GetHashCode(), message = "Oh no.. something bad happened :(", description = ex.Message });
            }

        }

        // PUT api/<PlayListController>/5
        [HttpPut]
        public async Task<ActionResult<ResultDTO>> Put([FromBody] PlayListDTO data)
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

        // DELETE api/<PlayListController>/5
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
