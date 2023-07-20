using ToDoList.Domain.Entities;

namespace ToDoList.Business.Services
{
    public interface IToDoListService
    {
        Task<ICollection<object>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id, Item item);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(int id);
        Task<Priority> GetPriorityByIdAsync(int priorityId);
    }
}
