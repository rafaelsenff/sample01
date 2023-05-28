namespace BaseApi.Controllers.Common
{
    using BaseMongoDb.Models;
    using BaseMongoDb.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public abstract class ApiBaseCrudController<T> : ControllerBase
        where T : BaseMongoEntity
    {
        protected readonly IMongoService<T> _baseMongoService;

        public ApiBaseCrudController(IMongoService<T> baseMongoService) =>
            _baseMongoService = baseMongoService;

        [HttpGet]
        public async Task<List<T>> Get() =>
            await _baseMongoService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<T>> Get(string id)
        {
            var entity = await _baseMongoService.GetAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            return entity;
        }

        [HttpPost]
        public async Task<IActionResult> Post(T newEntity)
        {
            await _baseMongoService.CreateAsync(newEntity);

            return CreatedAtAction(nameof(Get), new { id = newEntity.Id }, newEntity);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, T updatedEntity)
        {
            var entity = await _baseMongoService.GetAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            updatedEntity.Id = entity.Id;

            await _baseMongoService.UpdateAsync(id, updatedEntity);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _baseMongoService.GetAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            await _baseMongoService.RemoveAsync(id);

            return NoContent();
        }       
    }
}