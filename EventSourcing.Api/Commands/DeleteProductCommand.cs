using MediatR;
using System;

namespace EventSourcing.Api.Commands
{
    public class DeleteProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
