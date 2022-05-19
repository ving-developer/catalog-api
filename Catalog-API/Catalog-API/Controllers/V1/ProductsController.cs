using AutoMapper;
using Catalog_API.DTOs;
using Catalog_API.Models;
using Catalog_API.Pagination;
using Catalog_API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Catalog_API.Controllers.V1;

/// <summary>
/// Products CRUD
/// </summary>
[EnableCors("EnableApiGet")]//Adds EnableApiGet pollicy defined in Program.cs in all methods of controller
[Authorize]//Adds authentication required in all endpoints off this controller
[ApiController]// Configure attribute routing requirement, ModelState validation, ModelBinding parameter inference (automatically adding [FromBody] to POST methods) and Automatic HTTP 400 responses.
[Route("api/v{v:apiVersion}/[controller]")]//Sets controller route
[ApiVersion("1.0")]//maps the API version to this Controller's endpoints
[ApiVersion("2.0")]//maps the API version to this Controller's endpoints
[ApiConventionType(typeof(DefaultApiConventions))]//Automatically apply the possible return types and status codes based on REST conventions for each HTPP method, for all Controller actions
[Produces(MediaTypeNames.Application.Json)]//Informs that this controller only returns json as a result
[Consumes(MediaTypeNames.Application.Json)]//Informs that this controller only accepts json in its requests
public class ProductsController : ControllerBase
{
    private readonly IUnityOfWork _unity;
    private readonly IMapper _mapper;

    public ProductsController(IUnityOfWork unity, IMapper mapper)
    {
        _unity = unity;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all products ordered by price
    /// </summary>
    /// <returns>All products ordered by price</returns>
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

    /// <summary>
    /// Receives PageNumber and PageSize parameters, fetching paginated products
    /// </summary>
    /// <remarks>
    /// PageNumber
    ///
    ///         1
    ///         
    /// PageSize
    /// 
    ///         5
    /// </remarks>
    /// 
    /// <param name="parameters">PageNumber and PageSize</param>
    /// <returns>Paginated products</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAsync([FromQuery] ProductParameters parameters)
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
            Response?.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            var productDTO = _mapper.Map<List<ProductDTO>>(products);
            return productDTO;
        }
        return NotFound();
    }

    /// <summary>
    /// Searches for a product by its Id and returns it
    /// </summary>
    /// <remarks>
    /// id
    ///
    ///         1
    /// </remarks>
    /// <param name="id">Id of searched Product</param>
    /// <returns>Product</returns>
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

    /// <summary>
    /// Register a new product
    /// </summary>
    /// <remarks>
    /// Request body example:
    ///
    ///     POST /Products
    ///     {
    ///         "productId": 0,
    ///         "name": "string",
    ///         "description": "string",
    ///         "price": 0,
    ///         "imageUrl": "string",
    ///         "categoryId": 0
    ///     }
    ///
    /// </remarks>
    /// <param name="productDTO">Product to be registered</param>
    /// <returns>Product Created</returns>
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

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <remarks>
    /// Request body example:
    ///
    ///     {
    ///         "productId": 0,
    ///         "name": "string",
    ///         "description": "string",
    ///         "price": 0,
    ///         "imageUrl": "string",
    ///         "categoryId": 0
    ///     }
    /// </remarks>
    /// <param name="productDTO">Product to be updated</param>
    /// <returns>Updated Product</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDTO>> PutAsync(int id, ProductDTO productDTO)
    {
        if (id != productDTO.ProductId) return BadRequest();

        var product = _mapper.Map<Product>(productDTO);
        _unity.ProductRepository.Update(product);
        await _unity.CommitAsync();
        return NoContent();
    }

    /// <summary>
    /// Delete an existing product
    /// </summary>
    /// <remarks>
    /// Id:
    ///
    ///     1
    /// </remarks>
    /// <param name="id">product id to be deleted</param>
    /// <returns>Deleted Product</returns>
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
        return Ok(productDTO);
    }
}
