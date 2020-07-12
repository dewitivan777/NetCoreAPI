using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("OrderService/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<OrderEntity> _repository;

        public OrderController(IRepository<OrderEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// List all orders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _repository.ListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get order
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
        /// Find order by ProductId
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetByProductId/{id}")]
        [ProducesResponseType(typeof(List<OrderEntity>), 200)]
        public async Task<IActionResult> GetByProductId(string id)
        {
            var result = await _repository.ListAsync(t => t.ProductId.ToLower() == id.ToLower());

            return Ok(result);
        }

        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]OrderEntity order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.Id = Guid.NewGuid().ToString("N");
            order.CreatedDate = DateTime.Now;
            order.State = "Processing";

            var result = await _repository.Add(order);

            if (result == false)
            {
                return BadRequest("create failed");
            }

            return Ok(order);
        }

        /// <summary>
        /// Edit order
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody]OrderEntity order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.Id = id;
            order.UpdateDate = DateTime.Now;

            var result = await _repository.Update(order);

            if (result == false)
            {
                return BadRequest("edit failed");
            }

            return Ok(order);
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
