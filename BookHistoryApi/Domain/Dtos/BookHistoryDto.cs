namespace BookHistoryApi.Domain.Dtos
{
    public class BookHistoryDto
    {
        public BookHistoryDto(BookHistory bookHistory)
        {
            BookName = bookHistory.BookName;
            Author = bookHistory.Author;
            EventDate = bookHistory.Created;
        }

        public string BookName { get; set; }
        public string Author { get; set; }
        public DateTime EventDate { get; set; }
    }
}
