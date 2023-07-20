using Moq;
using ToDoList.Business.Services;
using ToDoList.Business.Validations;
using ToDoList.Data.Repositories;
using ToDoList.Domain.Entities;

namespace ToDoListTests
{
    public class ToDoListServiceTests
    {
        private Mock<IToDoListRepository> _todoListRepository;
        private Mock<Validations> _validations;

        [SetUp]
        public void Setup()
        {
            _todoListRepository = new Mock<IToDoListRepository>();
            _validations = new Mock<Validations>();
        }

        [Test]
        public async Task GetAllItemsAsync_ShouldReturnListOfItems()
        {
            // Arrange
            var expectedItems = new List<Item>
        {
            new Item { Id = 1, Title = "Item 1", Description = "Description 1", Status = (short)ItemStatus.Pending, PriorityId = 1 },
            new Item { Id = 2, Title = "Item 2", Description = "Description 2", Status = (short)ItemStatus.Complete, PriorityId = 2 }
        };

            _todoListRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedItems);

            var service = new ToDoListService(_todoListRepository.Object, _validations.Object);

            // Act
            var result = await service.GetAllItemsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedItems.Count, result.Count);

            foreach (var item in result)
            {
                Assert.IsNotNull(item.GetType().GetProperty("Id").GetValue(item));
                Assert.IsNotNull(item.GetType().GetProperty("Title").GetValue(item));
                Assert.IsNotNull(item.GetType().GetProperty("Description").GetValue(item));
                Assert.IsNotNull(item.GetType().GetProperty("Status").GetValue(item));
                Assert.IsNotNull(item.GetType().GetProperty("Priority").GetValue(item));
            }
        }

        [Test]
        public async Task GetItemByIdAsync_ShouldUpdateItemAndReturnIt()
        {
            // Arrange
            var todoListRepositoryMock = new Mock<IToDoListRepository>();
            var existingItem = new Item { Id = 1, Title = "Existing Item", Description = "Existing Description", Status = (short)ItemStatus.Pending, PriorityId = 1 };
            var newItem = new Item { Id = 1, Title = "Updated Item", Description = "Updated Description", Status = (short)ItemStatus.Complete, PriorityId = 2 };

            todoListRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingItem);

            var service = new ToDoListService(todoListRepositoryMock.Object, _validations.Object);

            // Act
            var result = await service.GetItemByIdAsync(1, newItem);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newItem.Title, result.Title);
            Assert.AreEqual(newItem.Description, result.Description);
            Assert.AreEqual(newItem.Status, result.Status);
            Assert.AreEqual(newItem.PriorityId, result.PriorityId);
        }

        [Test]
        public async Task GetItemByIdAsync_ShouldReturnNullWhenItemNotFound()
        {
            // Arrange
            var todoListRepositoryMock = new Mock<IToDoListRepository>();
            todoListRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Item)null);

            var service = new ToDoListService(todoListRepositoryMock.Object, _validations.Object);

            // Act
            var result = await service.GetItemByIdAsync(1, new Item { Id = 1 });

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetItemByIdAsync_ShouldNotUpdateItemWhenItemNotFound()
        {
            // Arrange
            int nonExistingItemId = 999;
            Item existingItem = null; // Simulating the scenario when the item is not found.
            Item updatedItem = null; // Simulating the scenario when the item to update is null.

            _todoListRepository.Setup(repo => repo.GetByIdAsync(nonExistingItemId)).ReturnsAsync(existingItem);

            var service = new ToDoListService(_todoListRepository.Object, _validations.Object);

            // Act
            var result = await service.GetItemByIdAsync(nonExistingItemId, updatedItem);

            // Assert
            Assert.IsNull(result); // The method should return null when item is not found.
            _todoListRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public async Task GetAllItemsAsync_ShouldReturnEmptyListWhenRepositoryReturnsEmpty()
        {
            // Arrange
            var todoListRepositoryMock = new Mock<IToDoListRepository>();
            var expectedItems = new List<Item>(); // Empty list

            todoListRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedItems);

            var service = new ToDoListService(todoListRepositoryMock.Object, _validations.Object);

            // Act
            var result = await service.GetAllItemsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        public async Task AddItemAsync_ShouldAddNewItem()
        {
            // Arrange
            var todoListRepositoryMock = new Mock<IToDoListRepository>();
            var newItem = new Item { Title = "New Item", Description = "New Description", Status = (short)ItemStatus.Pending, PriorityId = 1 };

            var service = new ToDoListService(todoListRepositoryMock.Object, _validations.Object);

            // Act
            await service.AddItemAsync(newItem);

            // Assert
            todoListRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public async Task AddItemAsync_ShouldThrowExceptionWhenStatusIsInvalid()
        {
            // Arrange
            var invalidItem = new Item { Title = "Invalid Item", Description = "Invalid Description", Status = 99, PriorityId = 1 };

            var validations = new Validations();
            var service = new ToDoListService(_todoListRepository.Object, validations);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.AddItemAsync(invalidItem));
        }

        [Test]
        public async Task AddItemAsync_ShouldThrowExceptionWhenPriorityIsInvalid()
        {
            // Arrange
            var invalidItem = new Item { Title = "Invalid Item", Description = "Invalid Description", Status = (short)ItemStatus.Pending, PriorityId = 99 };

            var validations = new Validations();
            var service = new ToDoListService(_todoListRepository.Object, validations);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await service.AddItemAsync(invalidItem));
        }

        [Test]
        public async Task UpdateItemAsync_ShouldUpdateExistingItem()
        {
            // Arrange
            var todoListRepositoryMock = new Mock<IToDoListRepository>();
            var existingItem = new Item { Id = 1, Title = "Existing Item", Description = "Existing Description", Status = (short)ItemStatus.Pending, PriorityId = 1 };

            todoListRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingItem);

            var service = new ToDoListService(todoListRepositoryMock.Object, _validations.Object);

            // Act
            await service.UpdateItemAsync(existingItem);

            // Assert
            todoListRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public async Task UpdateItemAsync_ShouldThrowExceptionWhenStatusIsInvalid()
        {
            // Arrange
            var existingItem = new Item { Id = 1, Title = "Existing Item", Description = "Existing Description", Status = 1, PriorityId = 1 };
            var invalidItem = new Item { Id = 1, Title = "Invalid Item", Description = "Invalid Description", Status = 99, PriorityId = 1 };

            var validations = new Validations();
            var service = new ToDoListService(_todoListRepository.Object, validations);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.UpdateItemAsync(invalidItem);
            });
        }

        [Test]
        public async Task UpdateItemAsync_ShouldThrowExceptionWhenPriorityIsInvalid()
        {
            // Arrange
            var existingItem = new Item { Id = 1, Title = "Existing Item", Description = "Existing Description", Status = (short)ItemStatus.Pending, PriorityId = 1 };
            var invalidItem = new Item { Id = 1, Title = "Invalid Item", Description = "Invalid Description", Status = (short)ItemStatus.Pending, PriorityId = 99 };

            var validations = new Validations();
            var service = new ToDoListService(_todoListRepository.Object, validations);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.UpdateItemAsync(invalidItem);
            });
        }


        [Test]
        public async Task DeleteItemAsync_ShouldDeleteExistingItem()
        {
            // Arrange
            var todoListRepositoryMock = new Mock<IToDoListRepository>();
            var existingItem = new Item { Id = 1, Title = "Existing Item", Description = "Existing Description", Status = (short)ItemStatus.Pending, PriorityId = 1 };

            todoListRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingItem);

            var service = new ToDoListService(todoListRepositoryMock.Object, _validations.Object);

            // Act
            await service.DeleteItemAsync(1);

            // Assert
            todoListRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task DeleteItemAsync_ShouldThrowExceptionWhenItemNotFound()
        {
            // Arrange
            int nonExistingItemId = 999;
            _todoListRepository.Setup(repo => repo.GetByIdAsync(nonExistingItemId)).ReturnsAsync((Item)null);

            var service = new ToDoListService(_todoListRepository.Object, _validations.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await service.DeleteItemAsync(nonExistingItemId));
        }


        [Test]
        public async Task GetPriorityByIdAsync_ShouldReturnPriority()
        {
            // Arrange
            var todoListRepositoryMock = new Mock<IToDoListRepository>();
            var expectedPriority = new Priority { Id = 1, Title = "High" };

            todoListRepositoryMock.Setup(repo => repo.GetPriorityByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedPriority);

            var service = new ToDoListService(todoListRepositoryMock.Object, _validations.Object);

            // Act
            var result = await service.GetPriorityByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPriority.Id, result.Id);
            Assert.AreEqual(expectedPriority.Title, result.Title);
        }

        [Test]
        public async Task GetPriorityByIdAsync_ShouldReturnNullWhenPriorityNotFound()
        {
            // Arrange
            int nonExistingPriorityId = 999;
            _todoListRepository.Setup(repo => repo.GetPriorityByIdAsync(nonExistingPriorityId)).ReturnsAsync((Priority)null);

            var service = new ToDoListService(_todoListRepository.Object, _validations.Object);

            // Act
            var result = await service.GetPriorityByIdAsync(nonExistingPriorityId);

            // Assert
            Assert.IsNull(result);
        }
    }
}