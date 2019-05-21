using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogAPI.Entities;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CatalogAPI.Controllers
{
    [Route("api/CatalogAPI")]
    [Produces("application/json")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogController(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        [HttpGet("FetchOrders")]
        public async Task<IActionResult> FetchListAsync(
            [FromQuery]Guid? Id)
        {
            var result =
                await _catalogRepository.FetchListAsync(
                    Id);

            return Ok(result);
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddAsync(
            [FromBody] Catalog catalog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result =
                await _catalogRepository.AddAsync(
                    catalog);
            return CreatedAtRoute("FetchOrder", new { id = result.Id }, result);
        }

        [HttpGet("{id}", Name = "FetchOrder")]
       
        public async Task<IActionResult> FetchByIdAsync(
          [FromRoute] Guid id)
        {
            if (id == null
                || id == Guid.Empty)
            {
                return BadRequest("Id required.");
            }

            var result =
                await _catalogRepository.FetchByIdAsync(
                    id);

            return Ok(result);
        }
        [HttpDelete("{id}", Name = "DeleteOrder")]
        
        public async Task<IActionResult> DeleteByIdAsync(
          [FromRoute] Guid id)
        {
            if (id == null
                || id == Guid.Empty)
            {
                return BadRequest("Id required.");
            }

            await _catalogRepository.DeleteByIdAsync(
                id);

            return NoContent();
        }

    }
}   
