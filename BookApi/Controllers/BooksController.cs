namespace BaseApi.Controllers
{
    using BaseApi.Controllers.Common;
    using BaseApi.Domain.Books;
    using BaseMongoDb.Services.Interfaces;
    using BaseRabbitMq;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public partial class BooksController : ApiBaseCrudController<Book>
    {
        private readonly IRabbitEventPublisher _eventPublisher;

        public BooksController(IMongoService<Book> booksService, IRabbitEventPublisher eventPublisher) : base(booksService)
        {
            _eventPublisher = eventPublisher;
        }

        [HttpPut("{id:length(24)}/upvote")]
        public async Task<IActionResult> Upvote(string id)
        {
            var entity = await _baseMongoService.GetAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            entity.Votes++;

            await _baseMongoService.UpdateAsync(id, entity);
            await _eventPublisher.SendEvent(new BookVotedEvent { Book = entity });

            return NoContent();
        }             
    }
}
