using Microsoft.AspNetCore.Mvc;
using System.Net;
using WarehouseServiceAPI.Models;
using WarehouseServiceAPI.Services;

namespace WarehouseServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class WarehouseController(IPartService partService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Part>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetParts() => Ok(await partService.GetAllPartsAsync());

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Part), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPart(int id)
        {
            var part = await partService.GetPartByIdAsync(id);
            return part is null ? NotFound(id) : Ok(part);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddPart(Part part)
        {
            var (Result, NewPartId) = await partService.AddPartAsync(part);
            return Result.IsSuccess ? Ok(NewPartId) : BadRequest(Result.Exception.Message);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdatePart(Part part)
        {
            var result = await partService.UpdatePartAsync(part);
            return result.IsSuccess ? Ok() : BadRequest(result.Exception.Message);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePart(int id)
        {
            var result = await partService.DeletePartAsync(id);
            return result.IsSuccess ? Ok() : BadRequest(result.Exception!.Message);
        }
    }
}
