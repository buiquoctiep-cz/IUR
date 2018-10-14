using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    static class WeatherInfo
    {
        private static string apiKey = "23d75aac127dab330e3341f31c9f1b47";
        private static string apiUrl = "http://api.openweathermap.org/data/2.5/";
        private static string country = "Czechia";
        private static string language = "en";
        private static string units = "metric";

        /// <summary>
        /// Set key and url for api calls
        /// </summary>
        public static void SetApi()
        {
            WeatherNet.ClientSettings.ApiKey = apiKey;
            WeatherNet.ClientSettings.ApiUrl = apiUrl;
        }
        
        /// <summary>
        /// Return current weather results
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static WeatherNet.Model.CurrentWeatherResult CurrentWeather(string cityName)
        {
            return WeatherNet.Clients.CurrentWeather.GetByCityName(cityName, country, language, units).Item;
        }

        /// <summary>
        /// Return a five day forecast
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static List<WeatherNet.Model.FiveDaysForecastResult> FiveDayForecast(string cityName)
        {
            return WeatherNet.Clients.FiveDaysForecast.GetByCityName(cityName, country, language, units).Items;
        }

        public static bool CityIsValid(string cityName)
        {
            var result = WeatherNet.Clients.CurrentWeather.GetByCityName(cityName, country, language, units);

            if (result.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
