using BaseMongoDb.Models;
using BookHistoryApi.Domain.Eventos;

namespace BookHistoryApi.Domain
{
    public class BookHistory : BaseMongoEntity
    {
        public BookHistory(Book book)
        {
            BookName = book.BookName;
            Author = book.Author;            
        }

        public string BookName { get; set; }

        public string Author { get; set; }
    }
}
