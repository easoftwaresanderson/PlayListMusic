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
        public async Task<ActionResult<PlayListDTO>> Get(int id)
        {
            try
            {
                var reg = await _service.GetByIdAsync(id);

                if (reg == null)
                {
                    return NotFound();
                }

                if (reg == null)
                {
                    return NotFound();
                }

                var data = MapPlayListDTO(reg);

                return Ok(data);

            }
            catch (Exception ex)
            {
                return new JsonResult(new { code = ex.GetHashCode(), message = "Oh no.. something bad happened :(", description = ex.Message });
            }
        }

        // GET api/<PlayListController>/5
        [HttpGet("device/{DeviceId}")]
        public async Task<ActionResult<PlayListDTO>> GetByDeviceID(int DeviceId)
        {
            try
            {
                var reg = await _service.GetByDeviceIdAsync(DeviceId);

                if (reg == null)
                {
                    return NotFound();
                }

                var data = MapPlayListDTO(reg);

                return Ok(data);

            }
            catch (Exception ex)
            {
                return new JsonResult(new { code = ex.GetHashCode(), message = "Oh no.. something bad happened :(", description = ex.Message });
            }
        }

        private PlayListDTO MapPlayListDTO(PlayList reg)
        {
            var data = new PlayListDTO
            {
                Id = reg.Id,
                PlayListName = reg.PlayListName,
            };

            if (reg.Device != null)
            {
                data.Device = new DeviceDTO
                {
                    Id = reg.Device.Id,
                    DeviceType = reg.Device.DeviceType,
                    Idiom = reg.Device.Idiom,
                    Manufacturer = reg.Device.Manufacturer,
                    Model = reg.Device.Model,
                    Name = reg.Device.Name,
                    Platform = reg.Device.Platform,
                    UniqueID = reg.Device.UniqueID,
                    VersionString = reg.Device.VersionString
                };
            }


            data.Musics = new List<MusicDTO>();

            if (reg.Musics != null)
            {
                foreach (Music music in reg.Musics)
                {
                    MusicDTO musicDTO = new MusicDTO
                    {
                        Id = music.Id,
                        MusicName = music.MusicName,
                        AlbumImage = music.AlbumImage,
                        AlbumName = music.AlbumName,
                        AlbumNotes = music.AlbumNotes,
                        AlbumYear = music.AlbumYear,
                        ArtistName = music.ArtistName,
                        Favorite = music.Favorite
                    };

                    data.Musics.Add(musicDTO);
                }
            }

            return data;
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
        public async Task<ActionResult<ResultDTO>> FavoriteSong([FromBody] PlayListFavoriteCommand command)
        {
            try
            {
                return Ok(await _service.FavoriteSong(command));
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
