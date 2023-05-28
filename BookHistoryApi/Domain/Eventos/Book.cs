namespace BookHistoryApi.Domain.Eventos
{
    public class Book
    {
        public string Id { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string ImageUrl { get; set; }

        public int Votes { get; set; }
    }
}
