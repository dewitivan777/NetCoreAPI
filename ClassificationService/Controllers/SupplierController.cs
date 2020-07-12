using ClassificationService.Models;
using ClassificationService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassificationService.Controllers
{
    [ApiController]
    [Route("ClassificationService/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly IRepository<SupplierEntity> _repository;

        public SupplierController(IRepository<SupplierEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// List all Suppliers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _repository.ListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get Supplier
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
        /// Find supplier by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetByName/{name}")]
        [ProducesResponseType(typeof(List<SupplierEntity>), 200)]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _repository.ListAsync(t => t.Name.ToLower() == name.ToLower());

            return Ok(result);
        }

        /// <summary>
        /// Create supplier
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]SupplierEntity supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            supplier.Id = Guid.NewGuid().ToString("N");

            var result = await _repository.Add(supplier);

            if (result == false)
            {
                return BadRequest("create failed");
            }

            return Ok(supplier);
        }

        /// <summary>
        /// Edit supplier
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody]SupplierEntity supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            supplier.Id = id;

            var result = await _repository.Update(supplier);

            if (result == false)
            {
                return BadRequest("edit failed");
            }

            return Ok(supplier);
        }

        /// <summary>
        /// Delete supplier by id
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