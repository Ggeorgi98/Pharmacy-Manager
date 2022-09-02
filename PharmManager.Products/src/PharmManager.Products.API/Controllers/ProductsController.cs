using Microsoft.AspNetCore.Mvc;
using PharmManager.Products.Domain;
using PharmManager.Products.Domain.Dtos;
using PharmManager.Products.Domain.Services;
using PharmManager.Products.DomainServices;
using System.Net;

namespace PharmManager.Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
            _productsService.ValidationDictionary = new ValidationDictionary(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var response = await _productsService.GetByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(PagedResults<ProductDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PagedResults<ProductDto>>> GetPagedAllProducts()
        {
            var pager = new Paginator
            {
                CurrentPage = 1,
                PageSize = int.MaxValue
            };

            var result = await _productsService.GetListAsync(pager, x => true, null, true);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> CreateProduct([FromBody] ProductDto model)
        {
            var product = await _productsService.AddAsync(model);

            if (product == null)
            {
                return BadRequest(_productsService.ValidationDictionary.GetModelState());
            }

            return Created(product.Id.ToString(), Request.Path);
        }
    }
}
