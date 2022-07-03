using EventSourcing.Api.Commands;
using EventSourcing.Api.EventStores;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing.Api.Handlers
{
    public class ChangeProductNameCommandHandler : IRequestHandler<ChangeProductNameCommand>
    {
        private readonly ProductStream _productStream;
        public ChangeProductNameCommandHandler(ProductStream productStream)
        {
            _productStream = productStream;
        }
        public async Task<Unit> Handle(ChangeProductNameCommand request, CancellationToken cancellationToken)
        {
            _productStream.NameChanged(request.ChangeProductNameDto);
            await _productStream.SaveAsync();
            return Unit.Value;
        }
    }
}
