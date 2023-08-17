using System;
namespace todoAPP.ViewModel
{

    public class WeatherViewModel
    {
        public Records records { get; set; }
    }

    public class Records
    {
        public List<Location> location { get; set; }
    }

    public class Location
    {
        public Time time { get; set; }
        public List<WeatherElement> weatherElement { get; set; }
    }

    public class Time
    {
        public string obsTime { get; set; }
    }

    public class WeatherElement
    {
        public string elementName { get; set; }
        public string elementValue { get; set; }
    }
}

