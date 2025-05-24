using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TaskManagementApp.Models;
using TaskManagementApp.Services;
using TaskManagementApp.Data; // Required for TaskDbContext
using Microsoft.EntityFrameworkCore; // Required for DbContextOptions
using System.Linq; // For Linq operations like FirstOrDefault

namespace TaskManagementApp.Tests
{
    [TestClass]
    public class TaskServiceTests
    {
        private TaskService _taskService;
        private DbContextOptions<TaskDbContext> _dbContextOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            // Use a new in-memory database for each test to ensure test isolation.
            // Note: Even though TaskService currently uses an internal list for many operations,
            // its constructor requires a TaskDbContext.
            _dbContextOptions = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // Unique name for each test
                .Options;
            
            var dbContext = new TaskDbContext(_dbContextOptions);
            _taskService = new TaskService(dbContext); 
        }

        [TestMethod]
        public async Task AddTaskAsync_ShouldAddTaskToList()
        {
            // Arrange
            var task = new TaskModel { Title = "Test Add Task" };

            // Act
            await _taskService.AddTaskAsync(task);
            var tasks = await _taskService.GetTasksAsync();
            var addedTask = tasks.FirstOrDefault(t => t.Title == "Test Add Task");

            // Assert
            Assert.IsNotNull(addedTask, "Task was not added to the list.");
            Assert.AreEqual("Test Add Task", addedTask.Title);
            Assert.IsTrue(addedTask.ID != 0, "Task ID should be set by the service.");
        }

        [TestMethod]
        public async Task GetTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var task1 = new TaskModel { Title = "Task 1" };
            var task2 = new TaskModel { Title = "Task 2" };
            await _taskService.AddTaskAsync(task1);
            await _taskService.AddTaskAsync(task2);

            // Act
            var tasks = await _taskService.GetTasksAsync();

            // Assert
            Assert.AreEqual(2, tasks.Count, "Should return all added tasks.");
            Assert.IsTrue(tasks.Any(t => t.Title == "Task 1"));
            Assert.IsTrue(tasks.Any(t => t.Title == "Task 2"));
        }

        [TestMethod]
        public async Task GetTaskByIdAsync_ShouldReturnCorrectTask()
        {
            // Arrange
            var task = new TaskModel { Title = "Find Me" };
            await _taskService.AddTaskAsync(task); // ID will be assigned by AddTaskAsync

            // Act
            // Since ID is assigned internally by the in-memory list, retrieve it first
            var allTasks = await _taskService.GetTasksAsync();
            var addedTask = allTasks.First(t => t.Title == "Find Me");
            var foundTask = await _taskService.GetTaskByIdAsync(addedTask.ID);

            // Assert
            Assert.IsNotNull(foundTask);
            Assert.AreEqual(addedTask.ID, foundTask.ID);
            Assert.AreEqual("Find Me", foundTask.Title);
        }

        [TestMethod]
        public async Task GetTaskByIdAsync_ShouldReturnNullForInvalidId()
        {
            // Act
            var task = await _taskService.GetTaskByIdAsync(-99); // A non-existent ID

            // Assert
            Assert.IsNull(task, "Should return null for a non-existent ID.");
        }

        [TestMethod]
        public async Task UpdateTaskAsync_ShouldUpdateTaskInList()
        {
            // Arrange
            var originalTask = new TaskModel { Title = "Original Title" };
            await _taskService.AddTaskAsync(originalTask);
            
            var tasksAfterAdd = await _taskService.GetTasksAsync();
            var taskToUpdate = tasksAfterAdd.First(t => t.Title == "Original Title");
            
            taskToUpdate.Title = "Updated Title"; // Create a new model for update or modify existing
            taskToUpdate.Description = "Updated Description";

            // Act
            await _taskService.UpdateTaskAsync(taskToUpdate);
            var updatedTask = await _taskService.GetTaskByIdAsync(taskToUpdate.ID);

            // Assert
            Assert.IsNotNull(updatedTask);
            Assert.AreEqual("Updated Title", updatedTask.Title);
            Assert.AreEqual("Updated Description", updatedTask.Description);
        }

        [TestMethod]
        public async Task DeleteTaskAsync_ShouldRemoveTaskFromList()
        {
            // Arrange
            var taskToDelete = new TaskModel { Title = "Delete Me" };
            await _taskService.AddTaskAsync(taskToDelete);

            var tasksAfterAdd = await _taskService.GetTasksAsync();
            var addedTask = tasksAfterAdd.First(t => t.Title == "Delete Me");
            int idToDelete = addedTask.ID;

            // Act
            await _taskService.DeleteTaskAsync(idToDelete);
            var deletedTask = await _taskService.GetTaskByIdAsync(idToDelete);
            var allTasksAfterDelete = await _taskService.GetTasksAsync();

            // Assert
            Assert.IsNull(deletedTask, "Deleted task should not be found by ID.");
            Assert.IsFalse(allTasksAfterDelete.Any(t => t.ID == idToDelete), "Deleted task should not be in the list of all tasks.");
        }
    }
}
