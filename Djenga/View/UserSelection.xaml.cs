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

namespace Djenga.View
{
    /// <summary>
    /// Interaction logic for UserSelection.xaml
    /// </summary>
    public partial class UserSelection : Window
    {

        // store external command data reference
        ExternalCommandData _commandData;

        public UserSelection(ExternalCommandData commandData)
        {
            InitializeComponent();
            _commandData = commandData;
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

            // parameters

            double masonryHeight   = Convert.ToDouble(tbMasonryHeight.Text);
            double masonryWidth    = Convert.ToDouble(tbMasonrywidth.Text);
            double masonryLength   = Convert.ToDouble(tbMasonryLength.Text);
            double mortarThickness = Convert.ToDouble(tbMortarThickness.Text);

            // select elements

            Document doc = _commandData.Application.ActiveUIDocument.Document;

            // select elements
             Logic logic = new Logic();
             IList<Element> walls = logic.SelectAllWalls(doc);

            foreach (Element wall in walls)
            {
                double length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                double area = wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
                double volume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                double height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
                double width = wall.GetTypeId();

                // create the following objects

                Course firstCourse = new Course(width, length, height, mortarThickness, masonryHeight, masonryLength, masonryWidth);
                Course secondCourse = new Course(width, length, height, mortarThickness, masonryHeight, masonryLength, masonryWidth);
                HoopIron hoopiron = new HoopIron(length);


                firstCourse.AddCourseElements(true);
                secondCourse.AddCourseElements(false);

                AbstractWall abstractWall = new AbstractWall(firstCourse,secondCourse,hoopiron);

                abstractWall.AddCourses(height);

                Calculator abstractCalculator = new Calculator(abstractWall);

                double stonesPieces = abstractCalculator.AddStones();
                double mortarVolume = abstractCalculator.AddMortar();
                double hoopIronLength = abstractCalculator.AddHoopIron();



             
        }
    }
}
