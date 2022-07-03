using EventSourcing.Api.Commands;
using EventSourcing.Api.Dtos;
using EventSourcing.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventSourcing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var products = await _mediator.Send(new GetProductsQuery());
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto createProductDto)
        {
            await _mediator.Send(new CreateProductCommand { CreateProductDto = createProductDto });
            return NoContent();
        }

        [HttpPut("name")]
        public async Task<IActionResult> ChangeName(ChangeProductNameDto changeProductNameDto)
        {
            await _mediator.Send(new ChangeProductNameCommand { ChangeProductNameDto = changeProductNameDto });
            return NoContent();
        }

        [HttpPut("price")]
        public async Task<IActionResult> ChangeProductPrice(ChangeProductPriceDto changeProductPriceDto)
        {
            await _mediator.Send(new ChangeProductPriceCommand { ChangeProductPriceDto = changeProductPriceDto });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });
            return NoContent();
        }
    }
}
