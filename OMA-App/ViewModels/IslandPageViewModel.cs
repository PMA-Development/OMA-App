﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OMA_App.API;
using OMA_App.Models;
using OMA_App.ErrorServices;
using CommunityToolkit.Maui.Views;
using OMA_App.Views;
using OMA_App.Modals;

namespace OMA_App.ViewModels
{
    [QueryProperty(nameof(IslandIdString), "IslandId")]
    public partial class IslandPageViewModel : BaseViewModel
    {
        private readonly OMAClient _client;

        [ObservableProperty]
        private int islandId;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private ObservableCollection<TurbineTask> turbinesTasks = new();

        private ICollection<TurbineTask> _allTurbinesTasks;

        public IslandPageViewModel(OMAClient client, ErrorService errorService,IConnectivity connectivity)
            : base(errorService, connectivity)
        {
            _client = client;
            _allTurbinesTasks = new ObservableCollection<TurbineTask>();
        }

        private string islandIdString;
        public string IslandIdString
        {
            get => islandIdString;
            set
            {
                islandIdString = value;
                if (int.TryParse(value, out var id))
                {
                    IslandId = id;
                    GetTurbinesWithTasks();
                }
            }
        }

        private async Task GetTurbinesWithTasks()
        {
            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "OK");
                    return;
                }

                var turbines = await _client.GetTurbinesIslandAsync(islandId);
                var allUncompletedTasks = await _client.GetUncompletedTasksAsync();
                _allTurbinesTasks.Clear();
                if (turbines.Count != null && allUncompletedTasks.Count != null)
                {
                    var turbineIdsInIsland = turbines.Select(t => t.TurbineID).ToHashSet();
                    var tasksByTurbine = allUncompletedTasks
                        .Where(task => turbineIdsInIsland.Contains(task.TurbineID))
                        .GroupBy(task => task.TurbineID)
                        .ToDictionary(group => group.Key, group => group.ToList());

                    foreach (var turbine in turbines)
                    {
                        var turbineTasks = tasksByTurbine.ContainsKey(turbine.TurbineID)
                            ? tasksByTurbine[turbine.TurbineID]
                            : new List<TaskDTO>();

                        var turbineTask = new TurbineTask
                        {
                            Title = turbine.Title,
                            TurbineId = turbine.TurbineID,
                            TaskDTOs = new ObservableCollection<TaskDTO>(turbineTasks)
                        };

                        _allTurbinesTasks.Add(turbineTask);
                    }
                    PerformSearch();


                }
               
            }
            catch (ApiException apiEx)
            {
                await _errorService.ShowErrorAlert(apiEx);
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"Failed to load turbines or tasks: {e.Message}");
            }
        }

        [RelayCommand]
        private async Task Refresh()
        {
            IsRefreshing = true;
             GetTurbinesWithTasks();
            IsRefreshing = false;
        }
        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                TurbinesTasks = new ObservableCollection<TurbineTask>(_allTurbinesTasks);
            }
            else
            {
                var filteredTurbines = _allTurbinesTasks
                    .Where(t => t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                TurbinesTasks = new ObservableCollection<TurbineTask>(filteredTurbines);
            }
        }

        [RelayCommand]
        private async Task Return()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async void OpenTurbine(int turbineID)
        {
            try
            {
                await Application.Current.MainPage.ShowPopupAsync(new IslandModal(turbineID, _client,_errorService, _connectivity));
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"Failed to open turbine details: {e.Message}");
            }
        }
    }
}
