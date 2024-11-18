﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OMA_App.ErrorServices;

namespace OMA_App.ViewModels
{
    public partial class IslandModalViewModel : BaseViewModel
    {
        [ObservableProperty]
        private TurbineDTO turbineObj;

        [ObservableProperty]
        private ObservableCollection<AttributeDTO> attributeDTOs = new();

        private readonly OMAClient _client;
        private readonly Action _closePopupAction;

        public IslandModalViewModel(int turbineId, Action closePopupAction, OMAClient client, ErrorService errorService)
            : base(errorService)
        {
            _closePopupAction = closePopupAction;
            _client = client;
            GetTurbineAndData(turbineId);
        }

        public async Task GetTurbineAndData(int id)
        {
            try
            {
                var turbine = await _client.GetTurbineAsync(id);
                var attributeDTOstemp = await _client.GetAttributeDataByTurbineIdAsync(turbine.TurbineID);

                TurbineObj = turbine;
                AttributeDTOs.Clear();
                foreach (var data in attributeDTOstemp)
                {
                    AttributeDTOs.Add(data);
                }
            }
            catch (ApiException apiEx)
            {
                await _errorService.ShowErrorAlert(apiEx);
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"An error occurred while fetching turbine data: {e.Message}");
            }
        }

        [RelayCommand]
        private async Task ChangeStateTurbine()
        {
            var result = await Application.Current.MainPage.DisplayActionSheet("What state do you wanna set the Turbine To?", "Cancel", null, "On", "Off", "Service");
            var value = result switch
            {
                "On" => 1,
                "Off" => 2,
                "Service" => 3,
                _ => 0,
            };

            if (value > 0)
                try
                {
                    await _client.ActionTurbineAsync("ChangeState", value, TurbineObj.IslandID);

                }
                catch (ApiException apiEx)
                {
                    await _errorService.ShowErrorAlert(apiEx);
                }
                catch (Exception e)
                {
                    await _errorService.DisplayAlertAsync("Error", $"An error occurred while Changing turbine State: {e.Message}");
                }


        }

        [RelayCommand]
        private Task Close()
        {
            _closePopupAction?.Invoke();
            return Task.CompletedTask;
        }
    }
}
