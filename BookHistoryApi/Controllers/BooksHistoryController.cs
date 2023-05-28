using BaseMongoDb.Services.Interfaces;
using BookHistoryApi.Domain;
using BookHistoryApi.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookHistoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksHistoryController : ControllerBase
    {
        protected readonly IMongoService<BookHistory> _bookHistoryRepository;

        public BooksHistoryController(IMongoService<BookHistory> bookHistoryRepository)
        {
            _bookHistoryRepository = bookHistoryRepository;
        }

        [HttpGet]
        public async Task<List<BookHistoryDto>> Get()
        {
            var listHistory = await _bookHistoryRepository.GetAsync();
            return listHistory.OrderByDescending(x => x.Created).Take(10).Select(a => new BookHistoryDto(a)).ToList();
        }           
    }
}