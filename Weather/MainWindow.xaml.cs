using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Xml.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Win32;

namespace Weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string firstTimeApp;
        public DateTime timeApp;
        
        Dictionary<string, List> Weathers = new Dictionary<string, List>();
        private void InputData()
        {
        }
        private void DownloadData(string city)
        {
            if(Weathers.Count>0)
                Weathers.Clear();
            pole.Text = "";
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            try
            {
                string json = client.DownloadString($"https://api.openweathermap.org/data/2.5/forecast?q={city}&?format=json&appid=ec8a2f661ea7e9b22ad91606f3197452");
                WeatherTable table = JsonSerializer.Deserialize<WeatherTable>(json);
                msc.Content = table.city.name;
                foreach (var item in table.list)
                {
                    Weathers.Add(item.dt_txt, item);
                } 
                DateTime time = ConvertToDateTime(table.list[0].dt);
                firstTimeApp = table.list[0].dt_txt;
                InputDay(time);
                InputIcon();
                firstValueSlider();
                ButtonVisibility();
                ContenButtons(time);
            }
            catch (Exception)
            {
                MessageBox.Show("Błędna miejscowość");
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (firstTimeApp is null)
                return;
            if (OnFirstWeather)
            {
                if (e.NewValue<timeApp.Hour)
                {
                    slider.Value = e.OldValue;
                }  
            }
            
        }
        private void ContenButtons(DateTime time)
        {
            buttonFirst.Content = DayToStringMidday(time);
            buttonSecond.Content = DayToStringMidday(time.AddDays(1));
            buttonThird.Content = DayToStringMidday(time.AddDays(2));
            buttonFour.Content = DayToStringMidday(time.AddDays(3));
            
        }
        private bool OnFirstWeather { get; set; } = false;
        private void firstValueSlider()
        {
            timeApp = DateTime.Parse(Weathers[firstTimeApp].dt_txt);
            slider.Value= timeApp.Hour;
            OnFirstWeather = true;

        }
        public MainWindow()
        {
            InitializeComponent();
        }
        public void InputDay(DateTime time)
        {
            first.Text = time.ToLongDateString().ToString();
            time = time.AddDays(1);
            second.Text = time.ToLongDateString().ToString();
            time = time.AddDays(1);
            third.Text = time.ToLongDateString().ToString();
            time = time.AddDays(1);
            four.Text = time.ToLongDateString().ToString();
            time=time.AddDays(-3);
        }
        private void ButtonVisibility()
        {
            buttonFirst.Visibility = Visibility.Visible;
            buttonSecond.Visibility = Visibility.Visible;
            buttonThird.Visibility = Visibility.Visible;
            buttonFour.Visibility = Visibility.Visible;
        }
        public void InputIcon()
        {  
            string icon =Weathers[firstTimeApp].weather[0].icon;
            Image imgfirst = new Image();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri($"http://openweathermap.org/img/wn/{icon}@2x.png", UriKind.RelativeOrAbsolute);
            bi3.EndInit();
            firstImg.Source = bi3;
            AddImg(secondImg,1);
            AddImg(thirdImg,2);
            AddImg(fourImg,3);
        }
        public void AddImg(Image image,int i)
        {
            DateTime time = DateTime.Parse(Weathers[firstTimeApp].dt_txt);
            time = time.AddDays(i);
            string timeStr = DayToStringMidday(time);
            string icon = Weathers[timeStr].weather[0].icon;
            Image imgfirst = new Image();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri($"http://openweathermap.org/img/wn/{icon}@2x.png", UriKind.RelativeOrAbsolute);
            bi3.EndInit();
            image.Source = bi3;
        }
        public string DayToStringMidday(DateTime time)
        {
            return time.ToString("yyyy-MM-dd 12:00:00");
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                DownloadData(pole.Text);
            }

        }
        public static DateTime ConvertToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            dtDateTime = dtDateTime.AddHours(-1);
            return dtDateTime;
        }
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
            public string dt_txt { get; set; }
        }
        public class Main
        {
            public decimal temp { get; set; }
            public int humidity { get; set; }
            public int pressure { get; set; }

        }
        public class Weather
        {
            public string icon { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        private void ClickButtons(object sender, RoutedEventArgs e)
        {
            OnFirstWeather = false;
            MessageBox.Show("AAA");
        }

        private void buttonFirstClick(object sender, RoutedEventArgs e)
        {
            OnFirstWeather = true;
            firstValueSlider();
            MessageBox.Show(Name);
        }

        public class Wind
        {
            public double spped { get; set; }
            public int deg { get; set; }

        }
        

    }
}
