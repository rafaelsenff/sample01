using BaseApi.Domain.Books;
using BaseRabbitMq;

public class BookVotedEvent : BaseEvent
{
    public Book Book { get; set; }
}
