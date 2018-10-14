using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Weather
{
    /// <summary>
    /// Interaction logic for WindowAddCity.xaml
    /// </summary>
    public partial class WindowAddCity : Window
    {
        ObservableCollection<string> collectionCitiesTemp;

        /// <summary>
        /// Constructor
        /// Expects collection of cities
        /// </summary>
        /// <param name="collectionCitiesTemp"></param>
        public WindowAddCity(ObservableCollection<string> collectionCitiesTemp)
        {
            if (collectionCitiesTemp == null)
            {
                MessageBox.Show("Error: Collection of cities is null.");
                this.Close();
                return;
            }

            InitializeComponent();
            this.collectionCitiesTemp = collectionCitiesTemp;
        }

        /// <summary>
        /// Ok button clicked
        /// Adds the city to collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            string newCity = TextBoxCity.Text;

            if (!WeatherInfo.CityIsValid(newCity))
            {
                TextBoxCity.Text = "Not a valid city name";
                return;
            }

            if (!collectionCitiesTemp.Contains(newCity))
            {
                collectionCitiesTemp.Add(newCity);
            }

            this.Close();
        }

        /// <summary>
        /// Storno button clicked
        /// Closes the window without doing anything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStorno_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
