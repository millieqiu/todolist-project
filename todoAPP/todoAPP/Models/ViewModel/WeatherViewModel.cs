namespace todoAPP.ViewModel;

public class WeatherViewModel
{
    public Records records { get; set; }
}

public class Records
{
    public List<Station> Station { get; set; }
}

public class Station
{
    public WeatherElement WeatherElement { get; set; }
}

public class WeatherElement
{
    public string Weather { get; set; }
}

