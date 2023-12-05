
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

        // displayMaterials
        private Cement dispCement = new Cement();
        private Sand dispSand = new Sand();
        private Stone dispStone = new Stone();
        private HoopIron dispHoopIron = new HoopIron();
        private DampProofCourse dispDPC = new DampProofCourse();



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

            //sum quantities

            double quantityCement = 0;
            double quantitySand = 0;
            double quantityBlocks = 0;
            double quantityHoopIron = 0;
            double quantityDpc = 0;

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


                Joint veriticalJoint = new Joint(mortarThickness, height, width);
                Joint horizontalJoint = new Joint(mortarThickness, length, width);

                veriticalJoint.GetVolume();
                horizontalJoint.GetVolume();

                Block fullBlock = new Block(masonryLength, masonryHeight, masonryWidth);
                Block toothBlock = new Block(masonryLength / 2, masonryHeight, masonryWidth);
                Block stackBlock = new Block(masonryLength * (2 / 3), masonryHeight, masonryWidth); // avargely 2/3rds of the normal block

                // Intantiate ingredients for creating firs and alternate course
                Course firstCourse = new Course(fullBlock, toothBlock, stackBlock, veriticalJoint, horizontalJoint, length, "First Course");
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
                Ukuta ukuta = new Ukuta(firstCourse, secondCourse, hoopIron, dpcStrip, mortar);
                ukuta.AddCourses(height);

                quantityBlocks += ukuta.TotalBlocks();
                quantityCement += ukuta.TotalCementWeight();
                quantitySand += ukuta.TotalSandWeight();
                quantityDpc += ukuta.TotalDpcRolls();
                quantityHoopIron += ukuta.TotalHoopIronRolls();


            }

            // stones
            Items.Add(new Display
            {
                Description = dispStone.Name,
                Unit = dispStone.Unit,
                Quantity = quantityBlocks,
                Rate = dispStone.Rate,
                Amount = dispStone.Amount
            });

            // cement
            Items.Add(new Display
            {
                Description = dispCement.Name,
                Unit = dispCement.Unit,
                Quantity = UnitsConversion.gramsToKg(quantityCement),
                Rate = dispCement.Rate,
                Amount = dispCement.Amount,

            });


            // sand
            Items.Add(new Display
            {
                Description = dispSand.Name,
                Unit = dispSand.Unit,
                Quantity = UnitsConversion.gramsToKg(quantitySand),
                Rate = dispSand.Rate,
                Amount = dispSand.Amount,

            });


            // Hoop Iron

            Items.Add(new Display
            {
                Description = dispHoopIron.Name,
                Unit = dispHoopIron.Unit,
                Quantity = quantityHoopIron,
                Rate = dispHoopIron.Rate,
                Amount = dispHoopIron.Amount(),

            });



            // DPC
            Items.Add(new Display
            {
                Description = dispDPC.Name,
                Unit = dispDPC.Unit,
                Quantity = quantityDpc,
                Rate = dispDPC.Rate,
                Amount = dispDPC.Amount(),

            });
            

        }
    }
}


