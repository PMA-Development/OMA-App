using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class IslandModalViewModel : BaseViewModel
    {
        [ObservableProperty]
        private TurbineDTO turbineObj;
        [ObservableProperty]
        private ObservableCollection<DeviceDataDTO> deviceDataDTOs = new();

        private readonly OMAClient _client;
        private readonly Action _closePopupAction;
        public IslandModalViewModel(int turbineId, Action closePopupAction, OMAClient client)
        {
            
            _closePopupAction = closePopupAction;
            _client = client;
            GetTurbineAndData(turbineId);

        }

        public async Task GetTurbineAndData(int Id)
        {
            try
            {
                var turbine = await _client.GetTurbineAsync(Id);
                var deviceData = await _client.GetDeviceDataByTurbineIdAsync(turbine.TurbineID);
                TurbineObj = turbine;
                DeviceDataDTOs.Clear();
                foreach (var data in deviceData)
                {
                    DeviceDataDTOs.Add(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [RelayCommand]
        private async Task Close()
        {
            _closePopupAction?.Invoke();
        }

    }
}
