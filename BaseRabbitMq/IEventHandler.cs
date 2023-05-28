namespace BaseRabbitMq
{
    public interface IEventHandler<TEvent>
    {
        void Handle(TEvent eventData);
    }
}