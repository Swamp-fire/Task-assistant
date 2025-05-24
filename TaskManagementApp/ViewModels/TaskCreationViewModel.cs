using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskManagementApp.Models;
using TaskManagementApp.Services;
using System.Threading.Tasks; // For async operations

namespace TaskManagementApp.ViewModels
{
    public class TaskCreationViewModel : INotifyPropertyChanged
    {
        // Backing fields
        private string _title;
        private string _description;
        private TimeSpan _duration;
        private string _repetition;
        private string _priority;
        private string _category;

        private readonly ITaskService _taskService;

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Public properties
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                    // Notify that SaveTaskCommand's CanExecute might have changed
                    ((Command)SaveTaskCommand).ChangeCanExecute();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Repetition
        {
            get => _repetition;
            set
            {
                if (_repetition != value)
                {
                    _repetition = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Priority
        {
            get => _priority;
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged();
                }
            }
        }

        // Command
        public ICommand SaveTaskCommand { get; }

        // Constructor
        public TaskCreationViewModel(ITaskService taskService) // Inject ITaskService
        {
            _taskService = taskService;

            // Initialize properties
            Title = string.Empty;
            Description = string.Empty;
            Duration = TimeSpan.Zero; // Default duration
            Repetition = string.Empty;
            Priority = string.Empty; // Or a default like "Medium"
            Category = string.Empty; // Or a default like "Personal"

            // Initialize command with CanExecute
            SaveTaskCommand = new Command(async () => await OnSaveTaskAsync(), CanSaveTask);
        }

        private bool CanSaveTask()
        {
            return !string.IsNullOrWhiteSpace(Title);
        }

        private async Task OnSaveTaskAsync()
        {
            if (!CanSaveTask()) return;

            var newTask = new TaskModel
            {
                Title = this.Title,
                Description = this.Description,
                Duration = this.Duration,
                Repetition = this.Repetition,
                Priority = this.Priority,
                Category = this.Category,
                CreationDate = DateTime.Now // CreationDate is set by default in TaskModel
            };

            try
            {
                await _taskService.AddTaskAsync(newTask);
                Console.WriteLine("Task saved successfully!");
                // Optional: Clear form or navigate
                ClearForm();
                // Example navigation (if AppShell and routing are set up):
                // await Shell.Current.GoToAsync(".."); // Go back
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving task: {ex.Message}");
                // Optional: Display an error message to the user
                // await App.Current.MainPage.DisplayAlert("Error", "Failed to save task.", "OK");
            }
        }

        private void ClearForm()
        {
            Title = string.Empty;
            Description = string.Empty;
            Duration = TimeSpan.Zero;
            Repetition = string.Empty;
            Priority = string.Empty;
            Category = string.Empty;
        }
    }
}
