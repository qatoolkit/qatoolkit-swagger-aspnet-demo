using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QAToolKit.Swagger.AspNet.Demo.Extensions;
using QAToolKit.Swagger.AspNet.Demo.Models;
using QAToolKit.Swagger.AspNet.Demo.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QAToolKit.Swagger.AspNet.Demo.Controllers.v2
{
    [Route("api/bicycles")]
    [ApiVersion("2.0")]
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

        /// <summary>
        /// Get All Bikes
        /// </summary>
        /// <param name="bicycleType">Nullable BikeType enum</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Bicycle>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Get all bikes by filter",
            Description = "Get all bikes. TEST TAGS -> [@integrationtest,@loadtest]",
            OperationId = "GetAllBikes",
            Tags = new[] { "Public" }
        )]
        public async Task<IActionResult> GetAllBikes([FromQuery] BicycleType? bicycleType)
        {
            var bikes = _bikeService.GetAll(bicycleType);

            return Ok(bikes);
        }

        /// <summary>
        /// Get a bike
        /// </summary>
        /// <param name="id">Bicycle Id</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetBike")]
        [ProducesResponseType(typeof(Bicycle), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Get bike by id",
            Description = "Get bicycle by id. TEST TAGS -> [@integrationtest,@loadtest]",
            OperationId = "GetBike",
            Tags = new[] { "Public" }
        )]
        public async Task<IActionResult> GetBike(int id)
        {
            var bike = _bikeService.GetById(id);

            return Ok(bike);
        }


        /// <summary>
        /// Create new bicycle
        /// </summary>
        /// <param name="bicycle">Bicycle object to create</param>
        /// <param name="apiKey">ApiKey to access the endpoint</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Bicycle), StatusCodes.Status201Created)]
        [SwaggerOperation(
            Summary = "Create new bike",
            Description = "Add new bike. TEST TAGS -> [@integrationtest,@loadtest]",
            OperationId = "NewBike",
            Tags = new[] { "Public" }
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(ApiKeyAuthorizationFilterAttribute))]
        public async Task<IActionResult> NewBike([FromBody] Bicycle bicycle, [FromQuery] string apiKey)
        {
            var bike = _bikeService.CreateNew(bicycle);

            return Created(new Uri(Url.Link(nameof(GetBike), new { id = bike.Id })), bike);
        }

        /// <summary>
        /// Update a bicycle
        /// </summary>
        /// <param name="id">Bicycle id</param>
        /// <param name="bicycle">Bicycle object to update</param>
        /// <param name="apiKey">ApiKey to access the endpoint</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Bicycle), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Update a bike",
            Description = "Update a bike. TEST TAGS -> [@integrationtest,@loadtest,@apikey]",
            OperationId = "UpdateBike",
            Tags = new[] { "Public" }
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(ApiKeyAuthorizationFilterAttribute))]
        public async Task<IActionResult> UpdateBike(int id, [FromBody] Bicycle bicycle, [FromQuery] string apiKey)
        {
            var bike = _bikeService.Update(id, bicycle);

            return Ok(bike);
        }

        /// <summary>
        /// Delete a bicycle
        /// </summary>
        /// <param name="id">Bicycle Id</param>
        /// <param name="apiKey">ApiKey to access the endpoint</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Delete a bike",
            Description = "Delete a bike by id. TEST TAGS -> [@integrationtest,@loadtest,@apikey]",
            OperationId = "DeleteBike",
            Tags = new[] { "Public" }
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(ApiKeyAuthorizationFilterAttribute))]
        public async Task<IActionResult> DeleteBike(int id, [FromQuery] string apiKey)
        {
            _bikeService.Delete(id);

            return NoContent();
        }
    }
}
