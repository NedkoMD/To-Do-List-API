using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;

namespace ToDoList.Data.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ToDoListDbContext _todoListDbContext;

        public ToDoListRepository(ToDoListDbContext todolistDbContext)
        {
            _todoListDbContext = todolistDbContext;
        }

        public async Task<ICollection<Item>> GetAllAsync()
        {
            return await _todoListDbContext.Items
                .Include(item => item.Priority)
                .OrderByDescending(item => item.Priority.Id)
                .ToListAsync();
        }

        public async Task<Item> GetByIdAsync(int id)
        {
            return await _todoListDbContext.Items.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task AddAsync(Item item)
        {
            _todoListDbContext.Items.Add(item);
            await _todoListDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Item item)
        {
            var existingItem = await _todoListDbContext.Items.FirstOrDefaultAsync(i => i.Id == item.Id);

            existingItem.Title = item.Title;
            existingItem.Description = item.Description;
            existingItem.Priority = item.Priority;
            existingItem.Status = item.Status;
            await _todoListDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var itemToRemove = await _todoListDbContext.Items.FirstOrDefaultAsync(item => item.Id == id);

            _todoListDbContext.Items.Remove(itemToRemove);
            await _todoListDbContext.SaveChangesAsync();
        }

        public async Task<Priority> GetPriorityByIdAsync(int priorityId)
        {
            return await _todoListDbContext.Priorities.FirstOrDefaultAsync(p => p.Id == priorityId);
        }
    }
}
