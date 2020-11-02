using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QAToolKit.Swagger.AspNet.Demo.Models;
using QAToolKit.Swagger.AspNet.Demo.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QAToolKit.Swagger.AspNet.Demo.Controllers.v1
{
    [Route("api/bicycles")]
    [ApiVersion("1.0")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(void))]
    public class BicyclesController : ControllerBase
    {
        private readonly ILogger<BicyclesController> _logger;
        private readonly BikeService _bikeService;

        public BicyclesController(ILogger<BicyclesController> logger, BikeService bikeService)
        {
            _logger = logger;
            _bikeService = bikeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Bicycle>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Get all bikes by filter",
            Description = "Get all bikes",
            OperationId = "GetAllBikes",
            Tags = new[] { "Public" }
        )]
        public async Task<IActionResult> GetAllBikes([FromQuery] BicycleType? bicycleType)
        {
            var bikes = _bikeService.GetAll(bicycleType);

            return Ok(bikes);
        }

        [HttpGet("{id}", Name = "GetBike")]
        [ProducesResponseType(typeof(Bicycle), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Get bike by id",
            Description = "Get bicycle by id",
            OperationId = "GetBike",
            Tags = new[] { "Public" }
        )]
        public async Task<IActionResult> GetBike(int id)
        {
            var bike = _bikeService.GetById(id);

            return Ok(bike);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Bicycle), StatusCodes.Status201Created)]
        [SwaggerOperation(
            Summary = "Create new bike",
            Description = "Add new bike",
            OperationId = "NewBike",
            Tags = new[] { "Public" }
        )]
        public async Task<IActionResult> NewBike([FromBody] Bicycle bicycle)
        {
            var bike = _bikeService.CreateNew(bicycle);

            return Created(new Uri(Url.Link(nameof(GetBike), new { id = bike.Id })), bike);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Bicycle), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Update a bike",
            Description = "Update a bike",
            OperationId = "UpdateBike",
            Tags = new[] { "Public" }
        )]
        public async Task<IActionResult> UpdateBike(int id, [FromBody] Bicycle bicycle)
        {
            var bike = _bikeService.Update(id, bicycle);

            return Ok(bike);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Delete a bike",
            Description = "Delete a bike by id",
            OperationId = "DeleteBike",
            Tags = new[] { "Public" }
        )]
        public async Task<IActionResult> DeleteBike(int id)
        {
            _bikeService.Delete(id);

            return NoContent();
        }
    }
}
