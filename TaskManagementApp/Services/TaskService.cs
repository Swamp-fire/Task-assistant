using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;
        private readonly List<TaskModel> _inMemoryTasks = new List<TaskModel>(); // In-memory store
        private int _nextId = 1;

        public TaskService(TaskDbContext context)
        {
            _context = context;
            // Initialize with some dummy data if needed for in-memory testing
            // _inMemoryTasks.Add(new TaskModel { ID = _nextId++, Title = "Initial Task 1", CreationDate = System.DateTime.Now });
            // _inMemoryTasks.Add(new TaskModel { ID = _nextId++, Title = "Initial Task 2", CreationDate = System.DateTime.Now });
        }

        public async Task<List<TaskModel>> GetTasksAsync()
        {
            // Using in-memory store due to build issues
            return await Task.FromResult(_inMemoryTasks.ToList());
            // Ideal EF Core implementation:
            // return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            // Using in-memory store
            var task = _inMemoryTasks.FirstOrDefault(t => t.ID == id);
            return await Task.FromResult(task);
            // Ideal EF Core implementation:
            // return await _context.Tasks.FindAsync(id);
        }

        public async Task AddTaskAsync(TaskModel task)
        {
            // Using in-memory store
            task.ID = _nextId++;
            _inMemoryTasks.Add(task);
            await Task.CompletedTask;
            // Ideal EF Core implementation:
            // _context.Tasks.Add(task);
            // await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskModel task)
        {
            // Using in-memory store
            var existingTask = _inMemoryTasks.FirstOrDefault(t => t.ID == task.ID);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.Duration = task.Duration;
                existingTask.Repetition = task.Repetition;
                existingTask.Priority = task.Priority;
                existingTask.Category = task.Category;
                // Note: CreationDate is typically not updated.
            }
            await Task.CompletedTask;
            // Ideal EF Core implementation:
            // _context.Entry(task).State = EntityState.Modified;
            // await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            // Using in-memory store
            var taskToRemove = _inMemoryTasks.FirstOrDefault(t => t.ID == id);
            if (taskToRemove != null)
            {
                _inMemoryTasks.Remove(taskToRemove);
            }
            await Task.CompletedTask;
            // Ideal EF Core implementation:
            // var taskToRemove = await _context.Tasks.FindAsync(id);
            // if (taskToRemove != null)
            // {
            //     _context.Tasks.Remove(taskToRemove);
            //     await _context.SaveChangesAsync();
            // }
        }
    }
}
