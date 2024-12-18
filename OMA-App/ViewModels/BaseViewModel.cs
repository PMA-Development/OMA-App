﻿using CommunityToolkit.Mvvm.ComponentModel;
using OMA_App.ErrorServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isRefreshing;
        protected ErrorService _errorService { get; }
        protected IConnectivity _connectivity;
        public BaseViewModel(ErrorService errorService, IConnectivity connectivity)
        {
            _errorService = errorService;
            _connectivity = connectivity;
        }
    }
}
