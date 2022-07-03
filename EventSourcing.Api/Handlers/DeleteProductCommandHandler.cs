﻿using EventSourcing.Api.Commands;
using EventSourcing.Api.EventStores;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing.Api.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly ProductStream _productStream;
        public DeleteProductCommandHandler(ProductStream productStream)
        {
            _productStream = productStream;
        }
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _productStream.Deleted(request.Id);
            await _productStream.SaveAsync();
            return Unit.Value;
        }
    }
}
