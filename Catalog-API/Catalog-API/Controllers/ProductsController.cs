using AutoMapper;
using Catalog_API.DTOs;
using Catalog_API.Models;
using Catalog_API.Pagination;
using Catalog_API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Catalog_API.Controllers;

[EnableCors("EnableApiGet")]//Adds EnableApiGet pollicy defined in Program.cs in all methods of controller
[Authorize]//Adds authentication required in all endpoints off this controller
[ApiController]// Configure attribute routing requirement, ModelState validation, ModelBinding parameter inference (automatically adding [FromBody] to POST methods) and Automatic HTTP 400 responses.
[Route("[controller]")]//Sets controller route
public class ProductsController : Controller
{
    private readonly IUnityOfWork _unity;
    private readonly IMapper _mapper;

    public ProductsController(IUnityOfWork unity, IMapper mapper)
    {
        _unity = unity;
        _mapper = mapper;
    }

    [HttpGet("price")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> ListProductsByPriceAsync()
    {
        var products = await _unity.ProductRepository.ListProductsByPriceAsync();
        if (products is not null)
        {
            var productsDTO = _mapper.Map<List<ProductDTO>>(products);
            return productsDTO;
        }
        return NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductParameters parameters)
    {
        var products = await _unity.ProductRepository.GetProductsAsync(parameters);
        if (products is not null)
        {
            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            var productDTO = _mapper.Map<List<ProductDTO>>(products);
            return productDTO;
        }
        return NotFound();
    }

    [HttpGet("{id:int}", Name = "GetProduct")]//Specifies that this route will receive an id attribute of type integer and adds an internal name for this route. In this example this route name is being used in the CreatedAtRouteResult method
    public async Task<ActionResult<ProductDTO>> GetByIdAsync(int id)
    {
        var product = await _unity.ProductRepository.GetByIdAsync(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound();
        }
        var productDTO = _mapper.Map<ProductDTO>(product);
        return productDTO;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(ProductDTO productDTO)//IActionResult in the return type means that this method will only return ActionResult, that is, responses with status codes and without objects in the Request Body
    {
        var product = _mapper.Map<Product>(productDTO);
        _unity.ProductRepository.Add(product);
        await _unity.CommitAsync();
        productDTO = _mapper.Map<ProductDTO>(product);
        return CreatedAtRoute("GetProduct", new { id = productDTO.ProductId }, productDTO);
        /* This method returns the code 201 (created result) and also adds a "location" field
         * in the response header, with the URI used to query the created object (good practices of REST).
         * The "GetProduct" parameter corresponds to the name of the route that will be called
         * to query the created object*/
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDTO>> PutAsync(int id, ProductDTO productDTO)
    {
        if (id != productDTO.ProductId) return BadRequest();

        var product = _mapper.Map<Product>(productDTO);
        _unity.ProductRepository.Update(product);
        await _unity.CommitAsync();
        return productDTO;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductDTO>> DeleteAsync(int id)
    {
        var product = await _unity.ProductRepository.GetByIdAsync(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound();
        }
        _unity.ProductRepository.Delete(product);
        await _unity.CommitAsync();
        var productDTO = _mapper.Map<ProductDTO>(product);
        return productDTO;
    }
}
