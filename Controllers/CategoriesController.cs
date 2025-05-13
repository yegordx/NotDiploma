using Diploma.Entities;
using Diploma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Controllers;


[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _service;

    public CategoriesController(CategoriesService service)
    {
        _service = service;
    }

    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        var created = await _service.CreateAsync(category);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpPost("batch")]
    public async Task<IActionResult> CreateBatch([FromBody] List<Category> categories)
    {
        if (categories == null || !categories.Any())
            return BadRequest(new { message = "Список категорій порожній" });

        await _service.CreateBatchAsync(categories);
        return Ok(new { message = $"{categories.Count} категорій додано" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Category category)
    {
        var updated = await _service.UpdateAsync(id, category);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? Ok(new { message = "Категорію видалено" }) : NotFound();
    }
}

// ctrl+k+u to uncommend
//[
//  {
//    "id": "00000001-0000-0000-0000-000000000001",
//    "categoryName": "Овочі",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000002",
//    "categoryName": "Фрукти",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000003",
//    "categoryName": "Ягоди",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000004",
//    "categoryName": "Молочні продукти",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000005",
//    "categoryName": "Хліб та випічка",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000006",
//    "categoryName": "Крупи та макарони",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000007",
//    "categoryName": "М'ясо та ковбаси",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000008",
//    "categoryName": "Риба та морепродукти",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-00000000000a",
//    "categoryName": "Консерви та соління",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-00000000000b",
//    "categoryName": "Приправи та олії",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-00000000000c",
//    "categoryName": "Солодощі",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-00000000000d",
//    "categoryName": "Готові страви",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-00000000000e",
//    "categoryName": "Дитяче харчування",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-00000000000f",
//    "categoryName": "Напої",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000010",
//    "categoryName": "Кава, чай, какао",
//    "products": []
//  },
//  {
//    "id": "00000001-0000-0000-0000-000000000011",
//    "categoryName": "Дієтичні та еко продукти",
//    "products": []
//  }
//]