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
using System.Collections.ObjectModel;

namespace Weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> collectionCities;
        private DateTime tommorowDate;
        private DateTime afterTommorowDate;
        private string previousCity;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            /// default values for cities
            /// bind the collection to combobox and pass to modal windows
            collectionCities = new ObservableCollection<string>()
            {
                "Prague", "Brno", "Ostrava"
            };
            Binding bindingCities = new Binding();

            bindingCities.Source = collectionCities;
            ComboBoxCities.SetBinding(ComboBox.ItemsSourceProperty, bindingCities);
            ComboBoxCities.SelectedIndex = 0;

            /// Prepare api and get weather info
            WeatherInfo.SetApi();
            UpdateDates();
            UpdateForPlace(collectionCities[0]);
            previousCity = collectionCities[0];
        }

        /// <summary>
        /// Update date values
        /// </summary>
        private void UpdateDates()
        {
            tommorowDate = DateTime.Today.AddDays(1);
            afterTommorowDate = DateTime.Today.AddDays(2);

            /// update day after tommorow label
            string dayOfTheWeek = afterTommorowDate.DayOfWeek.ToString();
            string dateShort = afterTommorowDate.ToString("dd.MM.yyyy");
            TabItemAfterTommorow.Header = dayOfTheWeek + " " + dateShort;
        }

        /// <summary>
        /// Update temperature and humidity
        /// </summary>
        private void SetWeatherLabelsError()
        {
            LabelHumidity0.Content = "N/A";
            LabelHumidity1.Content = "N/A";
            LabelHumidity2.Content = "N/A";
            LabelTemperature0.Content = "N/A";
            LabelTemperature1.Content = "N/A";
            LabelTemperature2.Content = "N/A";
        }

        /// <summary>
        /// Update temperature and humidity
        /// </summary>
        /// <param name="cityName"></param>
        private void UpdateForPlace(string cityName)
        {
            if (cityName == null)
            {
                SetWeatherLabelsError();
                return;
            }
            
            UpdateDates();

            /// current weather
            var current = WeatherInfo.CurrentWeather(cityName);

            if (current == null)
            {
                SetWeatherLabelsError();
                return;
            }
           
            if (LabelTemperature0 != null)
                LabelTemperature0.Content = Math.Ceiling(current.Temp).ToString() + " °C";
            if (LabelHumidity0 != null)
                LabelHumidity0.Content = current.Humidity.ToString() + " %";

            /// tommorow's and aftertommorow's weather (average temperature and humidity) using 
            /// data from a five day forecast, one made every 3 hours (total 40 items)
            var fiveDays = WeatherInfo.FiveDayForecast(cityName);
            if (fiveDays == null)
            {
                SetWeatherLabelsError();
                return;
            }

            double tommorowTemperature = 0;
            double tommorowHumidity = 0;
            double afterTommorowTemperature = 0;
            double afterTommorowHumidity = 0;

            /// calculate
            foreach (var item in fiveDays)
            {
                if (item.Date.ToShortDateString() == tommorowDate.Date.ToShortDateString())
                {
                    tommorowTemperature += item.Temp;
                    tommorowHumidity += item.Humidity;
                }
                else if (item.Date.ToShortDateString() == afterTommorowDate.Date.ToShortDateString())
                {
                    afterTommorowTemperature += item.Temp;
                    afterTommorowHumidity += item.Humidity;
                }
            }
            tommorowTemperature /= 8;
            tommorowHumidity /= 8;
            afterTommorowTemperature /= 8;
            afterTommorowHumidity /= 8;

            /// set label values
            if (LabelTemperature1 != null)
                LabelTemperature1.Content = Math.Ceiling(tommorowTemperature).ToString() + " °C";
            if (LabelHumidity1 != null)
                LabelHumidity1.Content = Math.Ceiling(tommorowHumidity).ToString() + " %";

            if (LabelTemperature2 != null)
                LabelTemperature2.Content = Math.Ceiling(afterTommorowTemperature).ToString() + " °C";
            if (LabelHumidity2 != null)
                LabelHumidity2.Content = Math.Ceiling(afterTommorowHumidity).ToString() + " %";

        }

        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cityName = null;

            if (ComboBoxCities.SelectedIndex != -1)
            {
                cityName = ComboBoxCities.SelectedItem.ToString();
            }
            else
            {
                if (collectionCities.Contains(previousCity))
                {
                    cityName = previousCity;
                    ComboBoxCities.SelectedItem = previousCity;
                }
                else
                {
                    ComboBoxCities.SelectedIndex = 0;
                    cityName = ComboBoxCities.SelectedItem.ToString();
                }

            }

            UpdateForPlace(cityName);
            previousCity = cityName;
        }

        /// <summary>
        /// Open window for managing collection of cities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowManageCities manageCitiesWindow = new WindowManageCities(collectionCities);
            manageCitiesWindow.ShowDialog();
        }

        /*
        /// <summary>
        /// Close all windows when the main one shuts
        /// Unnecessary, using dialog windows keep for future
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        */
    }
}
