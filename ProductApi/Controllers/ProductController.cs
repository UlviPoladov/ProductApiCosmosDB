using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly CosmosDbService _service;

        public ProductController(CosmosDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetProductsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _service.GetProductAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            await _service.AddProductAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();
            await _service.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
