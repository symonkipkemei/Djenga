
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Djenga.Model
{
    public class Logic
    {

        public ObservableCollection<Display> Items { get; set; }

        public IList<Element> SelectMultipleWalls(UIDocument uidoc, Document doc)
        {
            IList<Element> walls = new List<Element>();
            IList<Reference> references = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);
            foreach (Reference reference in references)
            {
                Element wall = doc.GetElement(reference);
                walls.Add(wall);
            }

            return walls;
        }

        public IList<Element> SelectAllWalls(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> walls = collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElements();
            return walls;
        }


        public void AbstractWallMaterialData(IList<Element> walls, double mortarThickness, double masonryHeight, double masonryLength, double masonryWidth)
        {
            Items = new ObservableCollection<Display>();

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
                HoopIronStrip hoopiron = new HoopIronStrip(length);
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
                    Description = ukuta.HoopIronStrip.Name,
                    Unit = ukuta.HoopIronStrip.Unit,
                    Quantity = ukuta.TotalHoopIronRolls(),
                    Rate = ukuta.HoopIronStrip.Rate,
                    Amount = ukuta.HoopIronStrip.Amount(),

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

        }
    }
}


