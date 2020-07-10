using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<OrderEntity> _repository;

        public OrderController(IRepository<OrderEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// List all Categorys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _repository.ListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get Category
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
        /// Find categories by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetByName/{name}")]
        [ProducesResponseType(typeof(List<OrderEntity>), 200)]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _repository.ListAsync(t => t.Name.ToLower() == name.ToLower());

            return Ok(result);
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]OrderEntity category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            category.Id = Guid.NewGuid().ToString("N");

            var result = await _repository.Add(category);

            if (result == false)
            {
                return BadRequest("create failed");
            }

            return Ok(category);
        }

        /// <summary>
        /// Edit category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody]OrderEntity category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            category.Id = id;

            var result = await _repository.Update(category);

            if (result == false)
            {
                return BadRequest("edit failed");
            }

            return Ok(category);
        }

        /// <summary>
        /// Delete categories by id
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
