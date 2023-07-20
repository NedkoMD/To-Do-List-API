using ToDoList.Data.Constants;
using ToDoList.Data.Repositories;
using ToDoList.Domain.Entities;

namespace ToDoList.Business.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _todoListRepository;
        private readonly Validations.Validations _validations;

        public ToDoListService(IToDoListRepository todoListRepository, Validations.Validations validations)
        {
            _todoListRepository = todoListRepository;
            _validations = validations;
        }

        public async Task<ICollection<object>> GetAllItemsAsync()
        {
            var items = await _todoListRepository.GetAllAsync();

            return items.Select(item => new
            {
                item.Id,
                item.Title,
                item.Description,
                Status = ((ItemStatus)item.Status).ToString(),
                Priority = new
                {
                    item.Priority?.Title
                }
            }).ToList<object>();
        }

        public async Task<Item> GetItemByIdAsync(int id, Item item)
        {
            var existingItem = await _todoListRepository.GetByIdAsync(id);

            if (existingItem == null)
            {
                return null;
            }

            existingItem.Title = item.Title;
            existingItem.Description = item.Description;
            existingItem.Status = item.Status;
            existingItem.PriorityId = item.PriorityId;

            return await _todoListRepository.GetByIdAsync(id);
        }

        public async Task AddItemAsync(Item item)
        {
            if (!_validations.IsValidStatus(item.Status))
            {
                throw new ArgumentException(Consts.InvalidStatusId);
            }

            if (!_validations.IsValidPriority(item.PriorityId))
            {
                throw new ArgumentException(Consts.InvalidPriorityId);
            }

            await _todoListRepository.AddAsync(item);
        }

        public async Task UpdateItemAsync(Item item)
        {
            if (!_validations.IsValidStatus(item.Status))
            {
                throw new ArgumentException(Consts.InvalidStatusId);
            }

            if (!_validations.IsValidPriority(item.PriorityId))
            {
                throw new ArgumentException(Consts.InvalidPriorityId);
            }

            if(item == null)
            {
                return;
            }

            await _todoListRepository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            var existingItem = await _todoListRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                throw new Exception(Consts.ItemNotFound);
            }

            await _todoListRepository.DeleteAsync(id);
        }

        public async Task<Priority> GetPriorityByIdAsync(int priorityId)
        {
            return await _todoListRepository.GetPriorityByIdAsync(priorityId);
        }
    }
}
