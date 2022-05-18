using AutoMapper;
using Catalog_API.DTOs;
using Catalog_API.Models;
using Catalog_API.Pagination;
using Catalog_API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Catalog_API.Controllers.V1;

/// <summary>
/// Categories CRUD
/// </summary>
[Authorize]//Adds authentication required in all endpoints off this controller
[ApiController]// Configure attribute routing requirement, ModelState validation, ModelBinding parameter inference (automatically adding [FromBody] to POST methods) and Automatic HTTP 400 responses.
[Route("api/v{v:apiVersion}/[controller]")]//Sets controller route
[ApiVersion("1.0")]//maps the API version to this Controller's endpoints
[ApiVersion("2.0")]//maps the API version to this Controller's endpoints
[ApiConventionType(typeof(DefaultApiConventions))]//Automatically apply the possible return types and status codes based on REST conventions for each HTPP method, for all Controller actions
[Produces(MediaTypeNames.Application.Json)]//Informs that this controller only returns json as a result
[Consumes(MediaTypeNames.Application.Json)]//Informs that this controller only accepts json in its requests
public class CategoriesController : Controller
{
    private readonly IUnityOfWork _unity;
    private readonly IMapper _mapper;

    public CategoriesController(IUnityOfWork unity, IMapper mapper)
    {
        _unity = unity;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all the categories and the first products of each of them
    /// </summary>
    /// <returns>All categories and their first products</returns>
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

    /// <summary>
    /// Receives PageNumber and PageSize parameters, fetching paginated categories
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
    /// <returns>Paginated categories</returns>
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

    /// <summary>
    /// Searches for a category by its Id and returns it
    /// </summary>
    /// <remarks>
    /// id
    ///
    ///         1
    /// </remarks>
    /// <param name="id">Id of searched Category</param>
    /// <returns>Category</returns>
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

    /// <summary>
    /// Register a new category
    /// </summary>
    /// <remarks>
    /// Request body example:
    ///
    ///     POST /Categories
    ///     {
    ///         "categoryId": 0,
    ///         "name": "Same category",
    ///         "imageUrl": "category.jpg"
    ///     }
    ///
    /// </remarks>
    /// <param name="categoryDTO">Category to be registered</param>
    /// <returns>Created Category</returns>
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

    /// <summary>
    /// Update an existing category
    /// </summary>
    /// <remarks>
    /// Request body example:
    ///
    ///     PUT /Categories/{id}
    ///     {
    ///         "categoryId": 0,
    ///         "name": "Updated Category",
    ///         "imageUrl": "category.jpg"
    ///     }
    ///
    /// </remarks>
    /// <param name="categoryDTO">Category to be updated</param>
    /// <returns>Updated Category</returns>
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

    /// <summary>
    /// Delete an existing category
    /// </summary>
    /// <remarks>
    /// Id:
    ///
    ///     1
    /// </remarks>
    /// <param name="id">Category id to be deleted</param>
    /// <returns>Deleted Category</returns>
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
