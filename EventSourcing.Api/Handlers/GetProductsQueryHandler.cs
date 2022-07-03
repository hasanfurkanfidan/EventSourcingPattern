using EventSourcing.Api.Dtos;
using EventSourcing.Api.Models;
using EventSourcing.Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing.Api.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly AppDbContext _appDbContext;
        public GetProductsQueryHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _appDbContext.Products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                UserId = product.UserId,
                Stock = product.Stock
            }).ToListAsync();

            return products;
        }
    }
}
