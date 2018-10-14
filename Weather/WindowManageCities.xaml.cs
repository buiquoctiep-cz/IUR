using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for WindowManageCities.xaml
    /// </summary>
    
    public partial class WindowManageCities : Window
    {
        private ObservableCollection<string> collectionCities;
        private ObservableCollection<string> collectionCitiesTemp;

        /// <summary>
        /// Constuctor
        /// Expects collection of cities
        /// </summary>
        /// <param name="collectionCities"></param>
        public WindowManageCities(ObservableCollection<string> collectionCities)
        {
            if (collectionCities == null)
            {
                MessageBox.Show("Error: Collection of cities is null.");
                this.Close();
                return;
            }
            InitializeComponent();
            collectionCitiesTemp = new ObservableCollection<string>(collectionCities);
            this.collectionCities = collectionCities;

            Binding binding = new Binding();
            binding.Source = collectionCitiesTemp;
            ListBoxCities.SetBinding(ListBox.ItemsSourceProperty, binding);
        }

        /// <summary>
        /// Open modal window for adding a city
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddCity_Click(object sender, RoutedEventArgs e)
        {
            WindowAddCity WindowAddCity = new WindowAddCity(collectionCitiesTemp);
            WindowAddCity.ShowDialog();
        }

        /// <summary>
        /// Remove a city from the list
        /// Always keep at least one city
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRemoveCities_Click(object sender, RoutedEventArgs e)
        {
            if (collectionCitiesTemp.Count() != 1)
            {
                collectionCitiesTemp.Remove(ListBoxCities.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Update the city collection in main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            /// Remove cities
            foreach (string city in collectionCities.ToList())
            {
                if (!collectionCitiesTemp.Contains(city))
                {
                    collectionCities.Remove(city);
                }
            }

            /// Add new cities
            foreach (string city in collectionCitiesTemp.ToList())
            {
                if (!collectionCities.Contains(city))
                {
                    collectionCities.Add(city);
                }
            }

            this.Close();
        }

        /// <summary>
        /// Storno button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStorno_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
