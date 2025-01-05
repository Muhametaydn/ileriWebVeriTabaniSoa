using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WCFService;


public class WeatherService : IWeatherService
{
    public string ReceiveWeather(string description, double degree)
    {

        return ("Received weather : {description} {degree}");
    }
}