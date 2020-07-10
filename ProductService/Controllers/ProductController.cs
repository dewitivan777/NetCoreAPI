using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("ProductService/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<ProductEntity> _repository;

        public ProductController(IRepository<ProductEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// List all Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _repository.ListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {

            var result = await _repository.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Find Products by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetByName/{name}")]
        [ProducesResponseType(typeof(List<ProductEntity>), 200)]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _repository.ListAsync(t => t.Name.ToLower() == name.ToLower());

            return Ok(result);
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductEntity product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.Id = Guid.NewGuid().ToString("N");

            var result = await _repository.Add(product);

            if (result == false)
            {
                return BadRequest("create failed");
            }

            return Ok(product);
        }

        /// <summary>
        /// Edit Product
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody]ProductEntity product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.Id = id;

            var result = await _repository.Update(product);

            if (result == false)
            {
                return BadRequest("edit failed");
            }

            return Ok(product);
        }

        /// <summary>
        /// Delete Products by id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            var result = await _repository.Delete(id);
            if (result == false)
            {
                return BadRequest("delete failed");
            }

            return Ok(id);
        }
    }
}
