using BaseMongoDb.Models;

namespace BaseApi.Domain.Books
{
    public class Book : BaseMongoEntity
    {
        public string BookName { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public int Votes { get; set; } = 0;
    }
}
