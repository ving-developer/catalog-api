using AutoMapper;
using Catalog_API.DTOs;
using Catalog_API.Models;
using Catalog_API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog_API.Controllers;

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

    [Authorize]
    [HttpGet("price")]
    public ActionResult<IEnumerable<ProductDTO>> ListProductsByPrice()
    {
        var products = _unity.ProductRepository.ListProductsByPrice().ToList();
        if (products is not null)
        {
            var productsDTO = _mapper.Map<List<ProductDTO>>(products);
            return productsDTO;
        }
        return NotFound();
    }

    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> Get()
    {
        var products = _unity.ProductRepository.Get().Take(10).ToList();// The Take() method will define a limit of products to be searched
        if (products is not null)
        {
            var productDTO = _mapper.Map<List<ProductDTO>>(products);
            return productDTO;
        }
        return NotFound();
    }

    [Authorize]
    [HttpGet("{id:int}", Name = "GetProduct")]//Specifies that this route will receive an id attribute of type integer and adds an internal name for this route. In this example this route name is being used in the CreatedAtRouteResult method
    public ActionResult<ProductDTO> Get(int id)
    {
        var product = _unity.ProductRepository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound();
        }
        var productDTO = _mapper.Map<ProductDTO>(product);
        return productDTO;
    }

    [Authorize]
    [HttpPost]
    public IActionResult Post(ProductDTO productDTO)//IActionResult in the return type means that this method will only return ActionResult, that is, responses with status codes and without objects in the Request Body
    {
        var product = _mapper.Map<Product>(productDTO);
        _unity.ProductRepository.Add(product);
        _unity.Commit();
        productDTO = _mapper.Map<ProductDTO>(product);
        return CreatedAtRoute("GetProduct", new { id = productDTO.ProductId}, productDTO);
        /* This method returns the code 201 (created result) and also adds a "location" field
         * in the response header, with the URI used to query the created object (good practices of REST).
         * The "GetProduct" parameter corresponds to the name of the route that will be called
         * to query the created object*/
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public ActionResult<ProductDTO> Put(int id, ProductDTO productDTO)
    {
        if (id != productDTO.ProductId) return BadRequest();

        var product = _mapper.Map<Product>(productDTO);
        _unity.ProductRepository.Update(product);
        _unity.Commit();
        return productDTO;
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public ActionResult<ProductDTO> Delete(int id)
    {
        var product = _unity.ProductRepository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound();
        }
        _unity.ProductRepository.Delete(product);
        _unity.Commit();
        var productDTO = _mapper.Map<ProductDTO>(product);
        return productDTO;
    }
}
