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


            // For every wall
            foreach (Element wall in walls)
            {
                // abstract wall data
                double length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                double height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
                double width = 0.656168; // to be fixed , set to 200mm thick

                // Intantiate ingredients for creating wall courses
                Course firstCourse = new Course(width, length, mortarThickness, masonryHeight, masonryLength, masonryWidth);
                Course secondCourse = new Course(width, length, mortarThickness, masonryHeight, masonryLength, masonryWidth);
                HoopIron hoopiron = new HoopIron(length);
                DpcStrip dpcStrip = new DpcStrip(width, length);

                // Create  courses by adding individual elements i.e blocks and mortar
                firstCourse.AddCourseElements(true);
                secondCourse.AddCourseElements(false);

                // Intantiate ingredients for creating an abstract wall
                Ukuta ukuta = new Ukuta(firstCourse, secondCourse, hoopiron, dpcStrip);
                ukuta.AddCourses(height);

                // stones
                Items.Add(new Display
                {
                    Description = ukuta.FirstCourse.FullBlock.Name,
                    Unit = ukuta.FirstCourse.FullBlock.Unit,
                    Quantity = ukuta.TotalBlocks(),
                    Rate = ukuta.FirstCourse.FullBlock.Rate,
                    Amount = ukuta.FirstCourse.FullBlock.Amount()
                });

                // cement
                Items.Add(new Display
                {
                    Description = ukuta.FirstCourse.HorizontalJoint.Mortar.Cement.Name,
                    Unit = ukuta.FirstCourse.HorizontalJoint.Mortar.Cement.Unit,
                    Quantity = ukuta.TotalCementWeight(),
                    Rate = ukuta.FirstCourse.HorizontalJoint.Mortar.Cement.Rate,
                    Amount = ukuta.FirstCourse.HorizontalJoint.Mortar.Cement.Amount(),

                });


                // sand
                Items.Add(new Display
                {
                    Description = ukuta.FirstCourse.HorizontalJoint.Mortar.Sand.Name,
                    Unit = ukuta.FirstCourse.HorizontalJoint.Mortar.Sand.Unit,
                    Quantity = ukuta.TotalSandWeight(),
                    Rate = ukuta.FirstCourse.HorizontalJoint.Mortar.Sand.Rate,
                    Amount = ukuta.FirstCourse.HorizontalJoint.Mortar.Sand.Amount(),

                });


                // Hoop Iron

                Items.Add(new Display
                {
                    Description = ukuta.FirstCourse.HorizontalJoint.Mortar.Sand.Name,
                    Unit = ukuta.FirstCourse.HorizontalJoint.Mortar.Sand.Unit,
                    Quantity = ukuta.TotalSandWeight(),
                    Rate = ukuta.FirstCourse.HorizontalJoint.Mortar.Sand.Rate,
                    Amount = ukuta.FirstCourse.HorizontalJoint.Mortar.Sand.Amount(),

                });



                // DPC
                Items.Add(new Display
                {
                    Description = ukuta.DpcStrip.Name,
                    Unit = ukuta.DpcStrip.Unit,
                    Quantity = ukuta.TotalDpcRolls(),
                    Rate = ukuta.DpcStrip.Rate,
                    Amount = ukuta.DpcStrip.Amount(),

                });


            }
            UserDisplay displayForm = new UserDisplay(Items);
            displayForm.ShowDialog();

        }
    }
}
