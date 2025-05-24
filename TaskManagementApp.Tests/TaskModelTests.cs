using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaskManagementApp.Models;

namespace TaskManagementApp.Tests
{
    [TestClass]
    public class TaskModelTests
    {
        [TestMethod]
        public void TaskModel_DefaultValues_CreationDateIsSet()
        {
            // Arrange & Act
            var task = new TaskModel();
            var now = DateTime.Now;

            // Assert
            Assert.IsNotNull(task.CreationDate);
            // Check if CreationDate is recent (e.g., within the last 5 seconds)
            Assert.IsTrue((now - task.CreationDate).TotalSeconds < 5, "CreationDate should be set to a recent time.");
        }

        [TestMethod]
        public void TaskModel_DefaultValues_InitializesCorrectly()
        {
            // Arrange & Act
            var task = new TaskModel();

            // Assert
            Assert.AreEqual(0, task.ID, "Default ID should be 0.");
            Assert.IsNull(task.Title, "Default Title should be null."); // Or string.Empty if initialized that way
            Assert.IsNull(task.Description, "Default Description should be null.");
            Assert.IsNull(task.Duration, "Default Duration should be null.");
            Assert.IsNull(task.Repetition, "Default Repetition should be null.");
            Assert.IsNull(task.Priority, "Default Priority should be null.");
            Assert.IsNull(task.Category, "Default Category should be null.");
        }

        [TestMethod]
        public void TaskModel_PropertyAssignments_SetAndGetCorrectly()
        {
            // Arrange
            var task = new TaskModel();
            var testTitle = "Test Task Title";
            var testDescription = "This is a test description.";
            var testDuration = TimeSpan.FromHours(1.5);
            var testCreationDate = new DateTime(2023, 1, 15, 10, 30, 0); // Specific date for testing
            var testRepetition = "Daily";
            var testPriority = "High";
            var testCategory = "Work";

            // Act
            task.ID = 101;
            task.Title = testTitle;
            task.Description = testDescription;
            task.Duration = testDuration;
            task.CreationDate = testCreationDate; // Manually set for this test case
            task.Repetition = testRepetition;
            task.Priority = testPriority;
            task.Category = testCategory;

            // Assert
            Assert.AreEqual(101, task.ID);
            Assert.AreEqual(testTitle, task.Title);
            Assert.AreEqual(testDescription, task.Description);
            Assert.AreEqual(testDuration, task.Duration);
            Assert.AreEqual(testCreationDate, task.CreationDate);
            Assert.AreEqual(testRepetition, task.Repetition);
            Assert.AreEqual(testPriority, task.Priority);
            Assert.AreEqual(testCategory, task.Category);
        }
    }
}
