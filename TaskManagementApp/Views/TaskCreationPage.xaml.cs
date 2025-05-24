using TaskManagementApp.ViewModels;

namespace TaskManagementApp.Views;

public partial class TaskCreationPage : ContentPage
{
	public TaskCreationPage(TaskCreationViewModel viewModel) // Inject ViewModel
	{
		InitializeComponent();
		BindingContext = viewModel; // Set injected ViewModel
	}

    // Event handler for the Save button will be added later,
    // typically interacting with a ViewModel.
    // Example:
    // private async void SaveTaskButton_Clicked(object sender, EventArgs e)
    // {
    //     // Logic to gather data from entries/pickers
    //     // and call a service or ViewModel method to save the task.
    //     // Example:
    //     // var newTask = new Models.TaskModel
    //     // {
    //     //     Title = TitleEntry.Text,
    //     //     Description = DescriptionEditor.Text,
    //     //     Duration = DurationTimePicker.Time, // Or parse from DurationEntry.Text
    //     //     Repetition = RepetitionEntry.Text, // Or RepetitionPicker.SelectedItem as string
    //     //     Priority = PriorityEntry.Text, // Or PriorityPicker.SelectedItem as string
    //     //     Category = CategoryEntry.Text // Or CategoryPicker.SelectedItem as string
    //     // };
    //     //
    //     // if (BindingContext is ViewModels.TaskCreationViewModel vm)
    //     // {
    //     //     await vm.SaveTaskAsync(newTask);
    //     // }
    //     //
    //     // await DisplayAlert("Success", "Task saved successfully!", "OK");
    //     // await Navigation.PopAsync(); // Go back to the previous page
    // }
}
