using Catalog_API.Models;
using Catalog_API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog_API.Controllers;

[ApiController]// Configure attribute routing requirement, ModelState validation, ModelBinding parameter inference (automatically adding [FromBody] to POST methods) and Automatic HTTP 400 responses.
[Route("[controller]")]//Sets controller route
public class CategoriesController : Controller
{
    private readonly IUnityOfWork _unity;

    public CategoriesController(IUnityOfWork unity)
    {
        _unity = unity;
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
    {
        var categories = _unity.CategoryRepository.GetCategoryProducts().ToList();
        if (categories is not null)
        {
            return categories;
        }
        return NotFound();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _unity.CategoryRepository.Get().ToList();
        if (categories is null)
        {
            return NotFound();
        }

        return categories;
    }

    [HttpGet("{id:int}", Name = "GetCategoryById")]
    public ActionResult<Category> Get(int id)
    {
        var category = _unity.CategoryRepository.GetById(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        _unity.CategoryRepository.Add(category);
        _unity.Commit();
        return new CreatedAtRouteResult("GetCategoryById",
            new { id = category.CategoryId }, category);
        /* This method returns code 201 (created result) and also adds a
         * "location" field in the response Header, with the URI used to
         * query the created category (good practices of REST).
         * "GetCategoryById" parameter corresponds to the internal
         * name of the route that will be called to query the created category*/
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }

        _unity.CategoryRepository.Update(category);
        _unity.Commit();
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoria = _unity.CategoryRepository.GetById(c => c.CategoryId == id);
        if (categoria is not null)
        {
            _unity.CategoryRepository.Delete(categoria);
            _unity.Commit();
            return Ok(categoria);
        }

        return NotFound();
    }
}
