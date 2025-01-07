using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using VFi.NetDevPack.Events;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Messaging;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.CMS.Bus;

public sealed class MediatorHandler : NetDevPack.Mediator.IMediatorHandler
{
    private readonly IMediator _mediator;
    private readonly IEventStore _eventStore;
    protected readonly Publisher _publisher;
    public MediatorHandler(IEventStore eventStore, IMediator mediator, Publisher publisher)
    {
        _eventStore = eventStore;
        _mediator = mediator;
        _publisher = publisher;
    }

    public async Task PublishEvent<T>(T @event) where T : Event
    {
        if (!@event.MessageType.Equals("DomainNotification"))
            _eventStore?.Save(@event);
        _ = _mediator.Publish(@event);
    }
    public async Task PublishEvent<T>(T @event, PublishStrategy strategy, CancellationToken cancellationToken) where T : Event
    {
        // if (!@event.MessageType.Equals("DomainNotification"))
        //     _eventStore?.Save(@event);

        _ = _publisher.Publish(@event, strategy);
    }
    public async Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>
    {
        return await _mediator.Send(query);
        //throw new NotImplementedException();
    }

    public async Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(request);
    }

    public async Task<ValidationResult> SendCommand<T>(T command) where T : Command
    {
        return await _mediator.Send(command);
    }

}
