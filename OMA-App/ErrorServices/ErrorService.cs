using OMA_App.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.ErrorServices
{
    public class ErrorService
    {
        public async Task ShowErrorAlert(ApiException apiEx)
        {
            string message = apiEx.StatusCode switch
            {
                400 => "Invalid input. Please check your data.",
                401 => "You are not authorized to perform this action.",
                500 => "An error occurred on the server. Please try again later.",
                _ => $"Unexpected error: {apiEx.Message}",
            };
            await DisplayAlertAsync("Error", message);
        }


        public async Task DisplayAlertAsync(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}
