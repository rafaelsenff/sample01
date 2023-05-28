namespace BaseRabbitMq
{
    public interface IRabbitEventPublisher
    {
        Task<bool> SendEvent<T>(T evento) where T : BaseEvent;
    }
}