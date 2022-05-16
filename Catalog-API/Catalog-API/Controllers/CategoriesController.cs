using AutoMapper;
using Catalog_API.DTOs;
using Catalog_API.Models;
using Catalog_API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog_API.Controllers;

[ApiController]// Configure attribute routing requirement, ModelState validation, ModelBinding parameter inference (automatically adding [FromBody] to POST methods) and Automatic HTTP 400 responses.
[Route("[controller]")]//Sets controller route
public class CategoriesController : Controller
{
    private readonly IUnityOfWork _unity;
    private readonly IMapper _mapper;

    public CategoriesController(IUnityOfWork unity, IMapper mapper)
    {
        _unity = unity;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("products")]
    public ActionResult<IEnumerable<CategoryDTO>> GetCategoriesProducts()
    {
        var categories = _unity.CategoryRepository.GetCategoryProducts().ToList();
        if (categories is not null)
        {
            //Mapping categories (a List<Category>) to a List<CategoryDTO>
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDTO;
        }
        return NotFound();
    }

    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<CategoryDTO>> Get()
    {
        var categories = _unity.CategoryRepository.Get().ToList();
        if (categories is null)
        {
            return NotFound();
        }

        var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);
        return categoriesDTO;
    }

    [Authorize]
    [HttpGet("{id:int}", Name = "GetCategoryById")]
    public ActionResult<CategoryDTO> Get(int id)
    {
        var category = _unity.CategoryRepository.GetById(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }

        var categoryDTO = _mapper.Map<CategoryDTO>(category);
        return categoryDTO;
    }

    [Authorize]
    [HttpPost]
    public ActionResult<CategoryDTO> Post(CategoryDTO categoryDTO)
    {
        var category = _mapper.Map<Category>(categoryDTO);
        _unity.CategoryRepository.Add(category);
        _unity.Commit();
        categoryDTO = _mapper.Map<CategoryDTO>(category);
        return new CreatedAtRouteResult("GetCategoryById",
            new { id = categoryDTO.CategoryId }, categoryDTO);
        /* This method returns code 201 (created result) and also adds a
         * "location" field in the response Header, with the URI used to
         * query the created category (good practices of REST).
         * "GetCategoryById" parameter corresponds to the internal
         * name of the route that will be called to query the created category*/
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDTO)
    {
        if (id != categoryDTO.CategoryId)
        {
            return BadRequest();
        }

        var category = _mapper.Map<Category>(categoryDTO);
        _unity.CategoryRepository.Update(category);
        _unity.Commit();
        return categoryDTO;
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public ActionResult<CategoryDTO> Delete(int id)
    {
        var categoria = _unity.CategoryRepository.GetById(c => c.CategoryId == id);
        if (categoria is not null)
        {
            _unity.CategoryRepository.Delete(categoria);
            _unity.Commit();
            var categoryDTO = _mapper.Map<CategoryDTO>(categoria);
            return categoryDTO;
        }

        return NotFound();
    }
}
