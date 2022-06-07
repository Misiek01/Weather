using System.Collections.Generic;

namespace Weather
{
    public partial class MainWindow
    {
        //Klasy Jsona
        public record WeatherTable
        {
            public City city { get; set; }
            public List<List> list { get; set; }

        }
        public class City
        {
            public string name { get; set; }
        }
        public class List
        {
            public int dt { get; set; }
            public Main main { get; set; }
            public List<Weather> weather { get; set; }
            public Clouds clouds { get; set; }
            public Wind wind { get; set; }
            public string dt_txt { get; set; }
            public double pop { get; set; }
        }
        public class Wind
        {
            public double speed { get; set; }
        }
        public class Main
        {
            public double temp { get; set; }
            public int humidity { get; set; }
            public int pressure { get; set; }

        }
        public class Weather
        {
            public string icon { get; set; }
            public string description { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

    }
}
