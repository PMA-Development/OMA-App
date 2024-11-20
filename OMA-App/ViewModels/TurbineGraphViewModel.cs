using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.ErrorServices;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class TurbineGraphViewModel : BaseViewModel
    {
        private readonly OMAClient _omaClient;

        [ObservableProperty]
        private DateTime startDate = DateTime.Today;

        [ObservableProperty]
        private DateTime endDate = DateTime.Today;

        public ObservableCollection<DeviceDataGraphDTO> TemperatureData { get; } = new();
        public ObservableCollection<DeviceDataGraphDTO> HumidityData { get; } = new();
        public ObservableCollection<DeviceDataGraphDTO> VoltageData { get; } = new();
        public ObservableCollection<DeviceDataGraphDTO> AmpData { get; } = new();

        [ObservableProperty]
        private int iD;

        public TurbineGraphViewModel(ErrorService errorService, IConnectivity connectivity, OMAClient omaClient, int id) : base(errorService, connectivity)
        {
            _omaClient = omaClient;
            ID = id;
        }

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "OK");
                    return;
                }


                var graphData = await _omaClient.DeviceDataGraphByTurbineId(ID, StartDate, EndDate);

                if (graphData != null)
                {
                    TemperatureData.Clear();
                    HumidityData.Clear();
                    VoltageData.Clear();
                    AmpData.Clear();
                    foreach (var item in graphData)
                    {
              

                        switch (item.Name)
                        {
                            case "Temperature":
                                TemperatureData.Add(item);
                                break;
                            case "Humidity":
                                HumidityData.Add(item);
                                break;
                            case "Voltage":
                                VoltageData.Add(item);
                                break;
                            case "AMP":
                                AmpData.Add(item);
                                break;
                            default:
                              
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task Return()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
