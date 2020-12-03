using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ColoursAPI.Services;
using ColoursAPI.Models;

namespace ColoursAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ColoursController : ControllerBase
    {

        private readonly ColoursService _ColoursService;
        public ColoursController(ColoursService ColoursService)
        {
            _ColoursService = ColoursService;
        }

        [HttpGet(Name = "GetColours")]
        [ProducesResponseType(typeof(List<ColoursItem>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<ColoursItem> _ColoursList;
            _ColoursList = await _ColoursService.GetAll();
            return Ok(_ColoursList);
        }

        [HttpGet("{id}", Name = "GetColoursById")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ColoursItem), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id < 0 || id > 1000)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Unprocessable Entity - {id} must be between 0 and 1000" });
            }

            ColoursItem _ColoursItem = await _ColoursService.GetById(id);
            if (_ColoursItem == null)
            {
                return NotFound(new ProblemDetails { Status = 404, Title = "Not Found" });
            }

            return Ok(_ColoursItem);
        }

        [HttpPost(Name = "UpdateColours")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] ColoursItem coloursItemUpdate)
        {
            if (coloursItemUpdate.Name == null || coloursItemUpdate.Name.Length == 0)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Unprocessable Entity - Needs a Colour Name" });
            }

            ColoursItem coloursItemReturn = await _ColoursService.UpdateById(0, coloursItemUpdate);

            return Created("Colours/" + coloursItemReturn.Id, coloursItemReturn);
        }

        [HttpPut("{id}", Name = "UpdateColoursById")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateByIdAsync(int id, [FromBody] ColoursItem coloursItemUpdate)
        {
            if (id < 0 || id > 1000)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Unprocessable Entity - {id} must be between 0 and 1000" });
            }
            if (coloursItemUpdate.Name == null || coloursItemUpdate.Name.Length == 0)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 422, Title = "Unprocessable Entity - Needs a Colour Name" });
            }

            if (coloursItemUpdate.Id != id)
            {
                return UnprocessableEntity(new ProblemDetails { Status = 400, Title = "Unprocessable Entity - payload Id doesnt match parameter Id" });
            }

            ColoursItem coloursItemReturn = await _ColoursService.UpdateById(id, coloursItemUpdate);

            return Created("Colours/" + id, coloursItemReturn);

        }

        [HttpDelete("{id}", Name = "DeleteColoursById")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            if (id < 0 || id > 1000)
            {
                return UnprocessableEntity( new ProblemDetails { Status = 422, Title = "Unprocessable Entity - {id} must be between 0 and 1000" });
            }

            await _ColoursService.DeleteById(id);

            return NoContent();
        }

        [HttpDelete(Name = "DeleteColours")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAllAsync()
        {

            await _ColoursService.DeleteAll();

            return NoContent();
        }

        [Route("Random")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRandomAsync()
        {
            List<ColoursItem> _listRandomColors = new List<ColoursItem> {
                new ColoursItem {Id = 1, Name = "blue" },
                new ColoursItem {Id = 2, Name = "darkblue" },
                new ColoursItem {Id = 3, Name = "lightblue" }
            };

            await Task.Run(() => { });

            Random rnd = new Random();
            int rndInt = rnd.Next(_listRandomColors.Count);

            return Ok(_listRandomColors[rndInt]);
        }
    }
}
