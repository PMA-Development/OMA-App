﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.MessageClass
{
    public class LoginStateChangedMessage : ValueChangedMessage<bool>
    {
        public LoginStateChangedMessage(bool isLoggedIn) : base(isLoggedIn) { }
    }
}
