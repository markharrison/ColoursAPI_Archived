using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ColoursAPI.Services;
using ColoursAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ColoursAPI.Controllers
{
    [Route("colours")]
    [ApiController]
    public class ColoursController : ControllerBase
    {

        private readonly ColoursService _ColoursService;
        public ColoursController(ColoursService ColoursService)
        {
            _ColoursService = ColoursService;
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "Get colours",
            Description = "Returns all colours.",
            OperationId = "GetColours",
            Tags = new[] { "Colours" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - returns list of colours", typeof(List<ColoursItem>))]
        public async Task<IActionResult> GetAllAsync()
        {
            List<ColoursItem> _ColoursList;
            _ColoursList = await _ColoursService.GetAll();
            return Ok(_ColoursList);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Update / create colours",
            Description = "Updates colours - creates colour if it doesn't exist",
            OperationId = "UpdateColours",
            Tags = new[] { "Colours" }
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Success - colours updated/created", typeof(ColoursItem))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateAsync(
            [FromBody, SwaggerRequestBody("Colours to update", Required = true)] List<ColoursItem> coloursItems)
        {
            List<ColoursItem> _ColoursInserted = new() { };

            foreach (ColoursItem coloursItem in coloursItems) // Loop through List with foreach
            {
                if (coloursItem.Name == null || coloursItem.Name.Length == 0)
                {
                    return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Missing a Colour Name" });
                }

                ColoursItem coloursItemReturn = await _ColoursService.UpdateById(0, coloursItem);
                _ColoursInserted.Add(coloursItemReturn);

            }

            List<ColoursItem> _ColoursList = await _ColoursService.GetAll();

            return Created("Colours/", _ColoursInserted);
        }

        [HttpDelete]
        [SwaggerOperation(
            Summary = "Delete colours",
            Description = "Deletes all colours.",
            OperationId = "DeletesColours",
            Tags = new[] { "Colours" }
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Success - all colours deleted", typeof(ColoursItem))]
        public async Task<IActionResult> DeleteAllAsync()
        {

            await _ColoursService.DeleteAll();

            return NoContent();
        }

        [HttpGet("{colourId}")]
        [SwaggerOperation(
            Summary = "Get colour by id",
            Description = "Returns colour specified by {colourId} (must be between 1 and 1000).",
            OperationId = "GetColourById",
            Tags = new[] { "Colours" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - returns colour", typeof(ColoursItem))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", typeof(ProblemDetails))]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute, SwaggerParameter("Id of Colour to return", Required = true)] int colourId)
        {
            if (colourId < 1 || colourId > 1000)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "{colourId} must be between 1 and 1000" });
            }

            ColoursItem _ColoursItem = await _ColoursService.GetById(colourId);
            if (_ColoursItem == null)
            {
                return NotFound(new ProblemDetails { Status = 404, Title = "Not Found - {colourId}: " + colourId });
            }

            return Ok(_ColoursItem);
        }

        [HttpPost("{colourId}")]
        [SwaggerOperation(
            Summary = "Update / create colour by id",
            Description = "Updates colour specified by {colourId} (must be between 1 and 1000);  use {colourId} = 0 to insert new color",
            OperationId = "UpdateColourById",
            Tags = new[] { "Colours" }
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Success - colour created/updated", typeof(ColoursItem))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateByIdAsync(
                    [FromRoute, SwaggerParameter("Id of Colour to update", Required = true)] int colourId,
                    [FromBody, SwaggerRequestBody("Colours to update", Required = true)] ColoursItem coloursItemUpdate)
        {
            if (colourId < 0 || colourId > 1000)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Unprocessable Entity - {id} must be between 0 and 1000" });
            }
            if (coloursItemUpdate.Name == null || coloursItemUpdate.Name.Length == 0)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Unprocessable Entity - Needs a Colour Name" });
            }
            if (coloursItemUpdate.Id != colourId)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Unprocessable Entity - payload Id doesnt match {colourId}" });
            }

            ColoursItem coloursItemReturn = await _ColoursService.UpdateById(colourId, coloursItemUpdate);

            return Created("Colours/" + colourId, coloursItemReturn);

        }

        [HttpDelete("{colourId}")]
        [SwaggerOperation(
            Summary = "Delete colour by id",
            Description = "Deletes colour specified by {colourId} (must be between 1 and 1000).",
            OperationId = "DeleteColourById",
            Tags = new[] { "Colours" }
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Success - colour deleted", typeof(ColoursItem))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteByIdAsync(
            [FromRoute, SwaggerParameter("Id of Colour to delete", Required = true)] int colourId)
        {
            if (colourId < 1 || colourId > 1000)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Unprocessable Entity - {id} must be between 1 and 1000" });
            }

            await _ColoursService.DeleteById(colourId);

            return NoContent();
        }

        [Route("findbyname")]
        [HttpGet]
        [SwaggerOperation(
             Summary = "Get colour by name",
             Description = "Returns colour specified by {colourName} ",
             OperationId = "GetColourByName",
             Tags = new[] { "Colours" }
         )]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - returns colour", typeof(ColoursItem))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ProblemDetails))]
        public async Task<IActionResult> GetByNameAsync(
            [FromQuery, SwaggerParameter("Name of Colour to return", Required = true)] string colourName)
        {

            ColoursItem _ColoursItem = await _ColoursService.GetByName(colourName);
            if (_ColoursItem == null)
            {
                return NotFound(new ProblemDetails { Status = 404, Title = "Not Found - {colourName}: " + colourName });
            }

            return Ok(_ColoursItem);
        }


        [Route("random")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get random colour",
            Description = "Returns random colour.",
            OperationId = "GetRandomColour",
            Tags = new[] { "Colours" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - returns random colour", typeof(ColoursItem))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ProblemDetails))]
        public async Task<IActionResult> RandomAsync()
        {
            List<ColoursItem> _ColoursList = await _ColoursService.GetAll();
            if (_ColoursList.Count == 0)
            {
                return NotFound(new ProblemDetails { Status = 404, Title = "Not Found - no colors exist"});
            }

            Random rnd = new Random();
            int rndInt = rnd.Next(_ColoursList.Count);

            return Ok(_ColoursList[rndInt]);
        }

        [Route("reset")]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Reset colours",
            Description = "Reset colours to default.",
            OperationId = "ResetColours",
            Tags = new[] { "Colours" }
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Success - colours reset", typeof(ColoursItem))]
        public async Task<IActionResult> ResetAsync()
        {

            await _ColoursService.Reset();

            List<ColoursItem> _ColoursList = await _ColoursService.GetAll();

            return Created("Colours/", _ColoursList);
        }

    }
}
