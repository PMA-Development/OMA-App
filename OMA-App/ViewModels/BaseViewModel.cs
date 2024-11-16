using CommunityToolkit.Mvvm.ComponentModel;
using OMA_App.ErrorServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        protected ErrorService _errorService { get; }
        public BaseViewModel(ErrorService errorService)
        {
            _errorService = errorService;
        }
    }
}
