using APIMusicPlayLists.Core.Entities;
using APIMusicPlayLists.Core.Interfaces.IServices;
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
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _logger;
        private IDeviceServices _service;

        public DeviceController(ILogger<DeviceController> logger, IDeviceServices service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/<DeviceController>
        // GET: api/Device
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> Get()
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

        // GET api/<DeviceController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> Get(int id)
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

        // POST api/<DeviceController>
        [HttpPost]
        public async Task<ActionResult<ResultDTO>> Post([FromBody] DeviceDTO data)
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

        // PUT api/<DeviceController>/5
        [HttpPut]
        public async Task<ActionResult<ResultDTO>> Put([FromBody] DeviceDTO data)
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

        // DELETE api/<DeviceController>/5
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

        // GET api/<PlayListController>/5
        [HttpGet("uniqueid/{id}")]
        public async Task<ActionResult<Device>> GetByUniqueDeviceID(string id)
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
    }
}
