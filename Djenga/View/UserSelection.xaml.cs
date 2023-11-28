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
        public ObservableCollection<Display> Items { get; set; }

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
            Items = new ObservableCollection<Display>();

            // close window before selecting walls
            this.Close();

            // select elements
            Logic logic = new Logic();
            IList<Element> walls = logic.SelectMultipleWalls(uidoc, doc);

            foreach (Element wall in walls)
            {
                double length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
 
                double height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
                double width = 0.656168; // to be fixed , set to 200mm thick

                // create the following objects

                Course firstCourse = new Course(width, length, height, mortarThickness, masonryHeight, masonryLength, masonryWidth);
                Course secondCourse = new Course(width, length, height, mortarThickness, masonryHeight, masonryLength, masonryWidth);
                HoopIron hoopiron = new HoopIron(length);


                firstCourse.AddCourseElements(true);
                secondCourse.AddCourseElements(false);

                AbstractWall abstractWall = new AbstractWall(firstCourse, secondCourse, hoopiron);

                abstractWall.AddCourses(height);

                Calculator abstractCalculator = new Calculator(abstractWall);

                double stonesPieces = abstractCalculator.AddStones();
               
                double hoopIronLength = abstractCalculator.AddHoopIron();

                // stones
                Items.Add(new Display
                {
                    Description = abstractWall.FirstCourse.StoneFull.Name,
                    Unit = abstractWall.FirstCourse.StoneFull.Unit,
                    Quantity = stonesPieces,
                    Rate = abstractWall.FirstCourse.StoneFull.Rate,
                    Amount = abstractWall.FirstCourse.StoneFull.Amount()
                });

                // hoop iron
                Items.Add(new Display
                {
                    Description = abstractWall.HoopIronPiece.Name,
                    Unit = abstractWall.HoopIronPiece.Unit,
                    Quantity = abstractWall.HoopIronPiece.GetNoOfRolls(hoopIronLength),
                    Rate = abstractWall.HoopIronPiece.Rate,
                    Amount = abstractWall.HoopIronPiece.Amount()

                });
            }
            UserDisplay displayForm = new UserDisplay(Items);
            displayForm.ShowDialog();

        }
    }
}
