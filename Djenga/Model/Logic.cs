
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB.Structure;
using System.Diagnostics;
using System.Windows;

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
                double width = 0.656168;


                //converted to mm
                length = UnitsConversion.FootToMmInt(length);
                height = UnitsConversion.FootToMmInt(height);
                width = UnitsConversion.FootToMmInt(width);

                
                Joint veriticalJoint = new Joint(mortarThickness,height,width);
                Joint horizontalJoint = new Joint(mortarThickness,length,width);

                veriticalJoint.GetVolume();
                horizontalJoint.GetVolume();

                Block fullBlock = new Block(masonryLength,masonryHeight,masonryWidth);
                Block toothBlock = new Block(masonryLength/2, masonryHeight, masonryWidth);
                Block stackBlock = new Block(masonryLength * (2/3), masonryHeight, masonryWidth); // avargely 2/3rds of the normal block

                // Intantiate ingredients for creating firs and alternate course
                Course firstCourse = new Course(fullBlock,toothBlock,stackBlock,veriticalJoint,horizontalJoint,length,"First Course");
                Course secondCourse = new Course(fullBlock, toothBlock, stackBlock, veriticalJoint, horizontalJoint, length, "Alternate Course");

                // Create  courses by adding individual elements i.e blocks and mortar
                Debug.WriteLine($"First Course");
                firstCourse.AddCourseElements(true);
                Debug.WriteLine($"Second Course");
                secondCourse.AddCourseElements(false);


                HoopIronStrip hoopIron = new HoopIronStrip(length);
                DpcStrip dpcStrip = new DpcStrip(width, length);

                Cement cement = new Cement();
                Sand sand = new Sand();
                Mortar mortar = new Mortar(cement, sand);


                // Intantiate ingredients for creating an abstract wall
                Ukuta ukuta = new Ukuta(firstCourse, secondCourse, hoopIron, dpcStrip,mortar);
                ukuta.AddCourses(height);


                // stones
                Items.Add(new Display
                {
                    Description = fullBlock.Name,
                    Unit = fullBlock.Unit,
                    Quantity = ukuta.TotalBlocks(),
                    Rate = fullBlock.Rate,
                    Amount = fullBlock.Amount
                });

                // cement
                Items.Add(new Display
                {
                    Description = cement.Name,
                    Unit = cement.Unit,
                    Quantity = ukuta.TotalCementWeight(),
                    Rate = cement.Rate,
                    Amount = cement.Amount,

                });


                // sand
                Items.Add(new Display
                {
                    Description = sand.Name,
                    Unit = sand.Unit,
                    Quantity = ukuta.TotalSandWeight(),
                    Rate = sand.Rate,
                    Amount = sand.Amount,

                });


                // Hoop Iron

                Items.Add(new Display
                {
                    Description = hoopIron.Name,
                    Unit = hoopIron.Unit,
                    Quantity = ukuta.TotalHoopIronRolls(),
                    Rate = hoopIron.Rate,
                    Amount = hoopIron.Amount(),

                });



                // DPC
                Items.Add(new Display
                {
                    Description = dpcStrip.Name,
                    Unit = dpcStrip.Unit,
                    Quantity = ukuta.TotalDpcRolls(),
                    Rate = dpcStrip.Rate,
                    Amount = dpcStrip.Amount(),

                });
            }

        }
    }
}


