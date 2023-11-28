using Djenga.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Djenga.View
{
    /// <summary>
    /// Interaction logic for UserDisplay.xaml
    /// </summary>
    public partial class UserDisplay : Window, INotifyPropertyChanged
    {

        //event
        public event PropertyChangedEventHandler PropertyChanged;


        //property
        private ObservableCollection<Display> viewItems;
        public ObservableCollection<Display> ViewItems
        {
            get { return viewItems; }
            set
            {
                viewItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ViewItems"));
            }
        }

        // constructor
        public UserDisplay(ObservableCollection<Display> collection)
        {
            DataContext = this;
            InitializeComponent();
            ViewItems = collection;

        }


        //methods
        private void Click_btnMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Click_btnClose(object sender, RoutedEventArgs e)
        {
            Close();

        }
        private void Click_leftButtonDown(object sender, RoutedEventArgs e)
        {
            DragMove();
        }
    }
}