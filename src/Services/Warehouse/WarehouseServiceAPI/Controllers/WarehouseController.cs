using Microsoft.AspNetCore.Mvc;
using WarehouseServiceAPI.Models;
using WarehouseServiceAPI.Services;

namespace WarehouseServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class WarehouseController(IPartService partService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetParts() => Ok(await partService.GetAllParts());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPart(int id)
        {
            var part = await partService.GetPartById(id);
            return part is null ? NotFound(id) : Ok(part);
        }

        [HttpPost]
        public async Task<IActionResult> AddPart(Part part)
        {
            var result = await partService.AddPart(part);
            return result.IsSuccess ? Ok() : BadRequest(result.Exception!.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePart(Part part)
        {
            var result = await partService.UpdatePart(part);
            return result.IsSuccess ? Ok() : BadRequest(result.Exception!.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart(int id)
        {
            var result = await partService.DeletePart(id);
            return result.IsSuccess ? Ok() : BadRequest(result.Exception!.Message);
        }
    }
}
