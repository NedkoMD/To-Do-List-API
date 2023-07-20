using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Business.Services;
using ToDoList.Business.Validations;
using ToDoList.Data.Constants;
using ToDoList.Domain.Entities;

namespace ToDoList.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : Controller
    {
        private readonly IToDoListService _todoListService;
        private readonly Validations _validations;

        public ToDoListController(IToDoListService todoService, Validations validations)
        {
            _todoListService = todoService;
            _validations = validations;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _todoListService.GetAllItemsAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(Item item)
        {
            var priority = await _todoListService.GetPriorityByIdAsync(item.PriorityId);
            if (priority == null)
            {
                return BadRequest(Consts.InvalidPriorityId);
            }

            item.Priority = priority;

            await _todoListService.AddItemAsync(item);

            return CreatedAtAction(nameof(GetAllItems), null, item);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            var existingItem = await _todoListService.GetItemByIdAsync(id, item);

            await _todoListService.UpdateItemAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                await _todoListService.DeleteItemAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
