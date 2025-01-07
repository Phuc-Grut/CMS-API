//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;
//using FluentValidation.Results;
//using MediatR;
//using VFi.NetDevPack.Messaging;
//using VFi.NetDevPack.Queries;

//namespace VFi.NetDevPack.Mediator
//{
//    public class MediatorHandler : IMediatorHandler
//    {
//        private readonly IMediator _mediator;

//        public MediatorHandler(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public virtual Task<ValidationResult> SendCommand<T>(T command) where T : Command
//        {
//            return _mediator.Send(command);
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public virtual Task PublishEvent<T>(T @event) where T : Event
//        {
//            return _mediator.Publish(@event);
//        }

//        //public Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>
//        //{
//        //    return   _mediator.Send(query);
//        //   // throw new NotImplementedException();
//        //}

//        public Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
//        {
//           return  _mediator.Send<TResponse>(request, cancellationToken); 
//        }
//    }
//}
