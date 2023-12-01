using Autodesk.Revit.UI;
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
using Autodesk.Revit.DB;
using Djenga.Model;
using System.Collections.ObjectModel;

namespace Djenga.View
{
    /// <summary>
    /// Interaction logic for UserSelection.xaml
    /// </summary>
    public partial class UserSelection : Window
    {

        // store external command data reference
        ExternalCommandData CommandData { get; }
       

        public UserSelection(ExternalCommandData commandData)
        {
            InitializeComponent();
            CommandData = commandData;
        }

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

        private void Click_btnDjenga(object sender, RoutedEventArgs e)
        {
            // logic runs here on click of the button

            // User parameters

            double masonryHeight = Convert.ToDouble(tbMasonryHeight.Text);
            double masonryWidth = Convert.ToDouble(tbMasonrywidth.Text);
            double masonryLength = Convert.ToDouble(tbMasonryLength.Text);
            double mortarThickness = Convert.ToDouble(tbMortarThickness.Text);

            // Revit parameters
            UIDocument uidoc = CommandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            

            // close window before selecting walls
            this.Close();

            // select elements
            Logic logic = new Logic();
            IList<Element> walls = logic.SelectMultipleWalls(uidoc, doc);

            // Abstract material data from walls and store it on display
            logic.AbstractWallMaterialData(walls, mortarThickness, masonryHeight, masonryLength, masonryWidth);


            UserDisplay displayForm = new UserDisplay(logic.Items);
            displayForm.ShowDialog();

        }
    }
}
