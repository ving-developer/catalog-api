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

    public ProductsController(IUnityOfWork unity)
    {
        _unity = unity;
    }

    [Authorize]
    [HttpGet("price")]
    public ActionResult<IEnumerable<Product>> ListProductsByPrice()
    {
        return _unity.ProductRepository.ListProductsByPrice().ToList();
    }

    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _unity.ProductRepository.Get().Take(10).ToList();// The Take() method will define a limit of products to be searched
        if (products is null)
        {
            return NotFound();
        }
        return products;
    }

    [Authorize]
    [HttpGet("{id:int}", Name = "GetProduct")]//Specifies that this route will receive an id attribute of type integer and adds an internal name for this route. In this example this route name is being used in the CreatedAtRouteResult method
    public ActionResult<Product> Get(int id)
    {
        var product = _unity.ProductRepository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound();
        }
        return product;
    }

    [Authorize]
    [HttpPost]
    public IActionResult Post(Product product)//IActionResult in the return type means that this method will only return ActionResult, that is, responses with status codes and without objects in the Request Body
    {
        _unity.ProductRepository.Add(product);
        _unity.Commit();

        return CreatedAtRoute("GetProduct", new { id = product.ProductId}, product);
        /* This method returns the code 201 (created result) and also adds a "location" field
         * in the response header, with the URI used to query the created object (good practices of REST).
         * The "GetProduct" parameter corresponds to the name of the route that will be called
         * to query the created object*/
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public ActionResult<Product> Put(int id, Product product)
    {
        if (id != product.ProductId) return BadRequest();

        _unity.ProductRepository.Update(product);
        _unity.Commit();
        return Ok(product);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public ActionResult<Product> Delete(int id)
    {
        var product = _unity.ProductRepository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound();
        }
        _unity.ProductRepository.Delete(product);
        _unity.Commit();
        return Ok(product);
    }
}
