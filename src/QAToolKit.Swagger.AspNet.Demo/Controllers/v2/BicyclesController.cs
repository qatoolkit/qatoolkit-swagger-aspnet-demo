using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QAToolKit.Swagger.AspNet.Demo.Extensions;
using QAToolKit.Swagger.AspNet.Demo.Models;
using QAToolKit.Swagger.AspNet.Demo.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QAToolKit.Swagger.AspNet.Demo.Controllers.v2
{
    [Route("api/bicycles")]
    [ApiVersion("2.0")]
    [ApiController]
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
        [Consumes("application/json")]
        [Produces("application/json")]
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
        [Consumes("application/json")]
        [Produces("application/json")]
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
            Description = "Add new bike. TEST TAGS -> [@integrationtest,@loadtest,@apikey,@customer]",
            OperationId = "NewBike",
            Tags = new[] { "Public" }
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(ApiKeyAuthorizationFilterAttribute))]
        [Consumes("application/json")]
        [Produces("application/json")]
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
        [Consumes("application/json")]
        [Produces("application/json")]
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
            Description = "Delete a bike by id. TEST TAGS -> [@integrationtest,@loadtest,@apikey,@administrator]",
            OperationId = "DeleteBike",
            Tags = new[] { "Public" }
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(ApiKeyAuthorizationFilterAttribute))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteBike(int id, [FromQuery] string apiKey)
        {
            _bikeService.Delete(id);

            return NoContent();
        }

        /// <summary>
        /// Upload a bicycle image
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image">Bicycle image</param>
        /// <returns></returns>
        [HttpPost("{id}/images")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Upload a bike image",
            Description = "Upload a bike image. TEST TAGS -> [@integrationtest,@loadtest,@apikey,@administrator]",
            OperationId = "BicycleImageUpload",
            Tags = new[] { "Public" }
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BicycleImageUpload(int id, [FromForm] BicycleImage image)
        {
            if (image.FileContent == null || image.FileContent.Length == 0 || image.FileContent.Length > 3 * 1024 * 1024)
                return BadRequest("Invalid file");

            string Content;
            using (MemoryStream ms = new MemoryStream())
            {
                await image.FileContent.CopyToAsync(ms).ConfigureAwait(false);
                Content = Encoding.ASCII.GetString(ms.ToArray());
            }

            return Ok($"File name: {image.FileName}, length: {image.FileContent.Length}");
        }

        /// <summary>
        /// Upload a bicycle image
        /// </summary>
        /// <param name="id"></param>
        /// <param name="brochure">Bicycle image</param>
        /// <returns></returns>
        [HttpPost("{id}/brochures")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Upload a bike brochure",
            Description = "Upload a bike brochure. TEST TAGS -> [@integrationtest,@loadtest,@apikey,@administrator]",
            OperationId = "BicycleBrochureUpload",
            Tags = new[] { "Public" }
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BicycleBrochureUpload(int id, [FromForm] BicycleBrochure brochure)
        {
            if (brochure.Image.FileContent == null || brochure.Image.FileContent.Length == 0 || brochure.Image.FileContent.Length > 3 * 1024 * 1024)
                return BadRequest("Invalid file");

            string Content;
            using (MemoryStream ms = new MemoryStream())
            {
                await brochure.Image.FileContent.CopyToAsync(ms).ConfigureAwait(false);
                Content = Encoding.ASCII.GetString(ms.ToArray());
            }

            return Ok($"File name: {brochure.Metadata.Name}, image name: {brochure.Image.FileName}, length: {brochure.Image.FileContent.Length}");
        }
    }
}
