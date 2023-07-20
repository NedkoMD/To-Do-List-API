using ToDoList.Domain.Entities;

namespace ToDoList.Data.Repositories
{
    public interface IToDoListRepository
    {
        Task<ICollection<Item>> GetAllAsync();
        Task<Item> GetByIdAsync(int id);
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(int id);
        Task<Priority> GetPriorityByIdAsync(int priorityId);
    }
}
