﻿using Microsoft.Maui.Controls.Xaml;
using System.Collections.ObjectModel;
using TarefasApp.Models;

namespace TarefasApp;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
        InitializeComponent();

        bool isReturningFromPopAsync = Navigation.ModalStack.Count > 0;

        ObservableCollection<Models.Task> tasks = new ObservableCollection<Models.Task>();

        if (!isReturningFromPopAsync)
        {
            tasks.Add(new Models.Task(1, "My First Task", "Description Task 1", DateTime.Now, 1));
            tasks.Add(new Models.Task(2, "My Second Task", "Description Task 2", DateTime.Now, 2));
            tasks.Add(new Models.Task(3, "My Third Task", "Description Task 3", DateTime.Now, 3));
            
            lstTasks.ItemsSource = tasks;
        }

        MessagingCenter.Subscribe<DetailTask, Models.Task>(this, "RemoveTask", (sender, task) =>
        {
            // Remove the task from the list
            tasks.Remove(task);
        });

        MessagingCenter.Subscribe<DetailTask, Models.Task>(this, "EditTask", (sender, task) =>
        {
            tasks.Where(x => x.Id == task.Id).First().Title = task.Title;
            tasks.Where(x => x.Id == task.Id).First().Description = task.Description;
            tasks.Where(x => x.Id == task.Id).First().Created = task.Created;
            tasks.Where(x => x.Id == task.Id).First().Priority = task.Priority;
            
            lstTasks.ItemsSource = null;
            lstTasks.ItemsSource = tasks;
        });
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
        if (e.SelectedItem != null)
        {
            var selectedItem = (Models.Task)e.SelectedItem;

            await Navigation.PushAsync(new DetailTask(selectedItem));

            ((ListView)sender).SelectedItem = null;
        }
    }
}
