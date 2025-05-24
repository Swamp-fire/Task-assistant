using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TaskManagementApp.Models;
using TaskManagementApp.Services;
using TaskManagementApp.ViewModels;

namespace TaskManagementApp.Tests
{
    // Manual Mock for ITaskService
    public class MockTaskService : ITaskService
    {
        public TaskModel LastAddedTask { get; private set; }
        public int AddTaskAsyncCallCount { get; private set; } = 0;
        public Func<TaskModel, Task> AddTaskAsyncFunc { get; set; }

        public Task AddTaskAsync(TaskModel task)
        {
            AddTaskAsyncCallCount++;
            LastAddedTask = task;
            return AddTaskAsyncFunc != null ? AddTaskAsyncFunc(task) : Task.CompletedTask;
        }

        // Implement other ITaskService methods as needed for tests,
        // though for these specific tests, only AddTaskAsync is critical.
        public Task<List<TaskModel>> GetTasksAsync() => Task.FromResult(new List<TaskModel>());
        public Task<TaskModel> GetTaskByIdAsync(int id) => Task.FromResult<TaskModel>(null);
        public Task UpdateTaskAsync(TaskModel task) => Task.CompletedTask;
        public Task DeleteTaskAsync(int id) => Task.CompletedTask;
    }

    [TestClass]
    public class TaskCreationViewModelTests
    {
        private MockTaskService _mockTaskService;
        private TaskCreationViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockTaskService = new MockTaskService();
            _viewModel = new TaskCreationViewModel(_mockTaskService);
        }

        [TestMethod]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Assert
            Assert.AreEqual(string.Empty, _viewModel.Title, "Default Title should be empty.");
            Assert.AreEqual(string.Empty, _viewModel.Description, "Default Description should be empty.");
            Assert.AreEqual(TimeSpan.Zero, _viewModel.Duration, "Default Duration should be TimeSpan.Zero.");
            Assert.AreEqual(string.Empty, _viewModel.Repetition, "Default Repetition should be empty.");
            Assert.AreEqual(string.Empty, _viewModel.Priority, "Default Priority should be empty.");
            Assert.AreEqual(string.Empty, _viewModel.Category, "Default Category should be empty.");
        }

        [TestMethod]
        public void SaveTaskCommand_CanExecute_ChangesWithTitle()
        {
            // Assert initial state (Title is empty)
            Assert.IsFalse(_viewModel.SaveTaskCommand.CanExecute(null), "Command should not execute when Title is empty.");

            // Set Title to a non-empty string
            _viewModel.Title = "Test Task";
            Assert.IsTrue(_viewModel.SaveTaskCommand.CanExecute(null), "Command should execute when Title is not empty.");

            // Set Title back to empty
            _viewModel.Title = "";
            Assert.IsFalse(_viewModel.SaveTaskCommand.CanExecute(null), "Command should not execute when Title is empty again.");

            // Set Title to whitespace
            _viewModel.Title = "   ";
            Assert.IsFalse(_viewModel.SaveTaskCommand.CanExecute(null), "Command should not execute when Title is whitespace.");
        }

        [TestMethod]
        public async Task SaveTaskCommand_Execute_CallsTaskServiceAddTaskAsync()
        {
            // Arrange
            _viewModel.Title = "Service Call Test";
            _viewModel.Description = "Desc";
            _viewModel.Duration = TimeSpan.FromHours(1);
            _viewModel.Repetition = "Daily";
            _viewModel.Priority = "High";
            _viewModel.Category = "Work";

            // Act
            await (Task)_viewModel.SaveTaskCommand.ExecuteAsync(null); // Assuming ExecuteAsync if command is async

            // Assert
            Assert.AreEqual(1, _mockTaskService.AddTaskAsyncCallCount, "AddTaskAsync should be called once.");
            Assert.IsNotNull(_mockTaskService.LastAddedTask, "LastAddedTask should not be null.");
            Assert.AreEqual("Service Call Test", _mockTaskService.LastAddedTask.Title);
            Assert.AreEqual("Desc", _mockTaskService.LastAddedTask.Description);
            Assert.AreEqual(TimeSpan.FromHours(1), _mockTaskService.LastAddedTask.Duration);
            Assert.AreEqual("Daily", _mockTaskService.LastAddedTask.Repetition);
            Assert.AreEqual("High", _mockTaskService.LastAddedTask.Priority);
            Assert.AreEqual("Work", _mockTaskService.LastAddedTask.Category);
        }
        
        [TestMethod]
        public async Task SaveTaskCommand_Execute_ClearsFormAfterSave()
        {
            // Arrange
            _viewModel.Title = "Clear Form Test";
            _viewModel.Description = "Some Description";
            _viewModel.Duration = TimeSpan.FromMinutes(30);
            _viewModel.Repetition = "Weekly";
            _viewModel.Priority = "Low";
            _viewModel.Category = "Personal";

            // Act
            await (Task)_viewModel.SaveTaskCommand.ExecuteAsync(null);

            // Assert
            Assert.AreEqual(string.Empty, _viewModel.Title, "Title should be cleared after save.");
            Assert.AreEqual(string.Empty, _viewModel.Description, "Description should be cleared after save.");
            Assert.AreEqual(TimeSpan.Zero, _viewModel.Duration, "Duration should be reset after save.");
            Assert.AreEqual(string.Empty, _viewModel.Repetition, "Repetition should be cleared after save.");
            Assert.AreEqual(string.Empty, _viewModel.Priority, "Priority should be cleared after save.");
            Assert.AreEqual(string.Empty, _viewModel.Category, "Category should be cleared after save.");
        }

        [TestMethod]
        public void PropertyChanged_IsRaised_WhenPropertiesChange()
        {
            // Arrange
            var changedProperties = new List<string>();
            _viewModel.PropertyChanged += (sender, args) => { changedProperties.Add(args.PropertyName); };

            // Act & Assert for Title
            _viewModel.Title = "New Title";
            Assert.IsTrue(changedProperties.Contains("Title"), "PropertyChanged should be raised for Title.");
            changedProperties.Clear();

            // Act & Assert for Description
            _viewModel.Description = "New Description";
            Assert.IsTrue(changedProperties.Contains("Description"), "PropertyChanged should be raised for Description.");
            changedProperties.Clear();
            
            // Act & Assert for Duration
            _viewModel.Duration = TimeSpan.FromHours(5);
            Assert.IsTrue(changedProperties.Contains("Duration"), "PropertyChanged should be raised for Duration.");
            changedProperties.Clear();

            // Act & Assert for Repetition
            _viewModel.Repetition = "Monthly";
            Assert.IsTrue(changedProperties.Contains("Repetition"), "PropertyChanged should be raised for Repetition.");
            changedProperties.Clear();

            // Act & Assert for Priority
            _viewModel.Priority = "Urgent";
            Assert.IsTrue(changedProperties.Contains("Priority"), "PropertyChanged should be raised for Priority.");
            changedProperties.Clear();

            // Act & Assert for Category
            _viewModel.Category = "Hobby";
            Assert.IsTrue(changedProperties.Contains("Category"), "PropertyChanged should be raised for Category.");
            changedProperties.Clear();
        }
    }
}
