﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WCFService
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        string ReceiveWeather(string description, double degree);
    }

}