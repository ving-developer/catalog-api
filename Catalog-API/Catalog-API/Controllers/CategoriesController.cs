using AutoMapper;
using Catalog_API.DTOs;
using Catalog_API.Models;
using Catalog_API.Pagination;
using Catalog_API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Catalog_API.Controllers;

[Authorize]//Adds authentication required in all endpoints off this controller
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

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProductsAsync()
    {
        var categories = await _unity.CategoryRepository.GetCategoryProductsAsync();
        if (categories is not null)
        {
            //Mapping categories (a List<Category>) to a List<CategoryDTO>
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDTO;
        }
        return NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAsync([FromQuery] CategoryParameters parameters)
    {
        var categories = await _unity.CategoryRepository.GetCategoriesAsync(parameters);
        if (categories is null)
        {
            return NotFound();
        }

        var metadata = new
        {
            categories.TotalCount,
            categories.PageSize,
            categories.CurrentPage,
            categories.TotalPages,
            categories.HasNext,
            categories.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);
        return categoriesDTO;
    }

    [HttpGet("{id:int}", Name = "GetCategoryById")]
    public async Task<ActionResult<CategoryDTO>> GetByIdAsync(int id)
    {
        var category = await _unity.CategoryRepository.GetByIdAsync(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }

        var categoryDTO = _mapper.Map<CategoryDTO>(category);
        return categoryDTO;
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> PostAsync(CategoryDTO categoryDTO)
    {
        var category = _mapper.Map<Category>(categoryDTO);
        _unity.CategoryRepository.Add(category);
        await _unity.CommitAsync();
        categoryDTO = _mapper.Map<CategoryDTO>(category);
        return new CreatedAtRouteResult("GetCategoryById",
            new { id = categoryDTO.CategoryId }, categoryDTO);
        /* This method returns code 201 (created result) and also adds a
         * "location" field in the response Header, with the URI used to
         * query the created category (good practices of REST).
         * "GetCategoryById" parameter corresponds to the internal
         * name of the route that will be called to query the created category*/
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> PutAsync(int id, CategoryDTO categoryDTO)
    {
        if (id != categoryDTO.CategoryId)
        {
            return BadRequest();
        }

        var category = _mapper.Map<Category>(categoryDTO);
        _unity.CategoryRepository.Update(category);
        await _unity.CommitAsync();
        return categoryDTO;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> DeleteAsync(int id)
    {
        var categoria = await _unity.CategoryRepository.GetByIdAsync(c => c.CategoryId == id);
        if (categoria is not null)
        {
            _unity.CategoryRepository.Delete(categoria);
            await _unity.CommitAsync();
            var categoryDTO = _mapper.Map<CategoryDTO>(categoria);
            return categoryDTO;
        }

        return NotFound();
    }
}
