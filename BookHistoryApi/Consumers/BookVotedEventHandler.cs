using BaseMongoDb.Services.Interfaces;
using BaseRabbitMq;
using BookHistoryApi.Domain;
using BookHistoryApi.Domain.Eventos;

namespace BookHistoryApi.Consumers
{
    public class BookVotedEventHandler : IEventHandler<BookVotedEvent>
    {
        private readonly IMongoService<BookHistory> _bookHistoryRepository;

        public BookVotedEventHandler(IMongoService<BookHistory> bookHistoryRepository)
        {
            _bookHistoryRepository = bookHistoryRepository;
        }

        public void Handle(BookVotedEvent eventData)
        {
            Console.WriteLine($"ReceivedBookEvent: {eventData}");
            _bookHistoryRepository.CreateAsync(new BookHistory(eventData.Book)).GetAwaiter().GetResult();
        }
    }
}
