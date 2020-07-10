using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassificationService.Models;
using ClassificationService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClassificationService.Controllers
{
    [ApiController]
    [Route("ClassificationService/[controller]")]
    public class CategoryController : ControllerBase
    {
            private readonly IRepository<CategoryEntity> _repository;

            public CategoryController(IRepository<CategoryEntity> repository)
            {
                _repository = repository;
            }

            /// <summary>
            /// List all categories
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
            [ProducesResponseType(typeof(List<CategoryEntity>), 200)]
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
        public async Task<IActionResult> Create([FromBody]CategoryEntity category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            category.Id =  Guid.NewGuid().ToString("N");

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
        public async Task<IActionResult> Edit(string id, [FromBody]CategoryEntity category)
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
