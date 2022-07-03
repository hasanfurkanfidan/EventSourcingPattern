using EventSourcing.Api.Dtos;
using MediatR;
using System.Collections.Generic;

namespace EventSourcing.Api.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {
    }
}
