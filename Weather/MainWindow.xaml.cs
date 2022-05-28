﻿using System;
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
using System.Globalization;

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
        private void DownloadData(string city)
        {
            if(Weathers.Count>0)
                Weathers.Clear();
            pole.Text = "";
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            try
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture("PL-pl");
                string json = client.DownloadString($"https://api.openweathermap.org/data/2.5/forecast?q={city}&?format=json&appid=ec8a2f661ea7e9b22ad91606f3197452");
                WeatherTable table = JsonSerializer.Deserialize<WeatherTable>(json);
                msc.Content = table.city.name;
                foreach (var item in table.list)
                {
                    Weathers.Add(item.dt_txt, item);
                } 
                DateTime time = ConvertToDateTime(table.list[0].dt);
                //timeApp= ConvertToDateTime(table.list[0].dt);
                firstTimeApp = table.list[0].dt_txt;
                InputDay(time);
                InputIcon();
                firstValueSlider();
                ButtonVisibility();
                ContenButtons(time);
                ButtonEnabled();
                InputData(buttonFirst.Content.ToString());
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
            if (!buttonFirst.IsEnabled)
            {
                if (e.NewValue < timeApp.Hour)
                    slider.Value = e.OldValue;
                else
                    slider.Value = e.NewValue;
                NewValueSlider(e,buttonFirst); 
            }
            if (!buttonSecond.IsEnabled)
            {
                NewValueSlider(e,buttonSecond); 
            }
        }

        private void NewValueSlider(RoutedPropertyChangedEventArgs<double> e,Button button)
        {
            DateTime time = DateTime.Parse(button.Content.ToString());
            if (e.NewValue > e.OldValue)
                time = time.AddHours(3);
            else
                time = time.AddHours(-3);
            string timeStr= DayToString(time);
            button.Content = timeStr;
            InputData(timeStr);
        }

        private void ContenButtons(DateTime time)
        {
            buttonFirst.Content = DayToStringMidday(time);
            buttonSecond.Content = DayToStringMidday(time.AddDays(1));
            buttonThird.Content = DayToStringMidday(time.AddDays(2));
            buttonFour.Content = DayToStringMidday(time.AddDays(3));
        }
        private void firstValueSlider()
        {
            timeApp = DateTime.Parse(Weathers[firstTimeApp].dt_txt);
            slider.Value= timeApp.Hour;
            buttonSecond.IsEnabled = false;
            buttonThird.IsEnabled = false;
            buttonFour.IsEnabled = false;

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
        public string DayToString(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:00:00");
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
            public double temp { get; set; }
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
        private void ButtonEnabled()
        {
            buttonFirst.IsEnabled = true; 
            buttonSecond.IsEnabled = true;
            buttonThird.IsEnabled = true;
            buttonFour.IsEnabled = true;
        }
        private void ClickButtonSecond(object sender, RoutedEventArgs e)
        {
            ContenButtons(timeApp);
            slider.Value = 12;
            ButtonEnabled();
            buttonSecond.IsEnabled = false;
            InputData(buttonSecond.Content.ToString());   
        }
        private void ClickButtonThird(object sender, RoutedEventArgs e)
        {
            ContenButtons(timeApp);
            slider.Value = 12;
            ButtonEnabled();
            buttonThird.IsEnabled = false;
            InputData(buttonThird.Content.ToString());
        }
        private void ClickButtonFour(object sender, RoutedEventArgs e)
        {
            ContenButtons(timeApp);
            slider.Value = 12;
            ButtonEnabled();
            buttonFour.IsEnabled = false;
            InputData(buttonFour.Content.ToString());
        }
        private void buttonFirstClick(object sender, RoutedEventArgs e)
        {
            firstValueSlider();
            ContenButtons(timeApp);
            ButtonEnabled();
            buttonFirst.IsEnabled = false;
            InputData(buttonFirst.Content.ToString());
        }
        private void InputData(string timeStr)
        {
            foreach (var item in Weathers)
            {
                if (item.Key==timeStr)
                {
                    int temp = (int)(item.Value.main.temp - 273.15);
                    tempValue.Content = temp + " ";
                    humidityValue.Content = item.Value.main.humidity;
                    cloudsValue.Content = item.Value.clouds.all;
                    pressureValue.Content=item.Value.main.pressure;
                    //windValue.Content=item.Value
                }
            }
        }

        public class Wind
        {
            public double spped { get; set; }
            public int deg { get; set; }

        }
        

    }
}
