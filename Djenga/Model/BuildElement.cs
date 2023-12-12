using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB.Electrical;
using System.Security.Cryptography.X509Certificates;
using Autodesk.Revit.DB;
using System.Diagnostics;
using System.Windows;
using Autodesk.Revit.UI;

namespace Djenga.Model
{


    internal class DpcStrip: DampProofCourse
    {
        // Dimensional Properties
        public double SectionWidth { get; set; }
        public double SectionLength { get; set; }

        internal DpcStrip( double WallWidth,double WallLength)
        {
            double offset = 25; //25mm
            SectionWidth = offset + WallWidth + offset;
            SectionLength =  WallLength;
        }
    }


    internal class HoopIronStrip: HoopIron
    {
        public double StripLength { get; set; }

        internal HoopIronStrip(double WallLength)
        {
            StripLength = WallLength;
        }

    }
    internal class Mortar
    {

        // Object properties

        public Cement Cement { get; set; }
        public Sand Sand { get; set; }

        public int RatioSand {  get; set; }
        public int RatioCement { get; set; }


        //Dimensional properties
        public double Volume {  get; set; }

        internal Mortar( Cement cement, Sand sand, int ratioSand = 4, int ratioCement = 1)
        {
         Cement  = cement; Sand = sand; RatioSand = ratioSand; RatioCement = ratioCement;
        }

        public void GetVolume(double volume)
        {
            Volume = volume;
        }

        public double GetCementVolume()
        {
            double totalRatio = RatioCement + RatioSand;
            double ratio = (RatioCement / totalRatio);
            double cementVolume = ratio * Volume;
            return cementVolume;
        }

        public double GetSandVolume()
        {
            double totalRatio = RatioCement + RatioSand;
            double ratio = (RatioSand / totalRatio);
            double sandVolume = ratio * Volume;
            return sandVolume;
        }
    }


    internal class Joint
    {
        //Dimensional Properties
        public double Thickness { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Volume { get; set; }

        public Mortar Mortar { get; set; }

        internal Joint(double thickness, double length, double width)
        {
            Thickness = thickness;
            Length = length;
            Width = width;
    
        }

        public void GetVolume()
        {
            Volume = Thickness * Length * Width;
        }
     
    }



    internal class Block: Stone
    {
        // Dimensional properties
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }

        public double Volume { get; set; }

        public double Weight { get; set; }


        internal Block(double length, double height, double width)
        {
            Width = width;
            Height = height;
            Length = length;
        }

        public void GetVolume()
        {
            Volume = Width * Height * Length;
        }

        public void GetWeight()
        {
            Weight = Volume * Density;
        }

    }

    internal class Course
    {

        // Material Properties/Objects
        public Block FullBlock { get; set; }
        public Block ToothBlock { get; set; }
        public Block StackBlock { get; set; }
        public Joint VeriticalJoint { get; set; }
        public Joint HorizontalJoint { get; set; }




        //Descriptive Properties
        public double WallLength { get; set; }
        public double CourseHeight { get; set; }
        public string Name { get; set; }
      


        // Collection properties
        public ObservableCollection<Block> FullBlockCollection { get; set; }
        public ObservableCollection<Block> ToothBlockCollection { get; set; }

        public ObservableCollection<Block> StackBlockCollection { get; set; }

        public ObservableCollection<Joint> VerticalJointCollection { get; set; }
        public ObservableCollection<Joint> HorizontalJointCollection { get; set; }
        




        public Course(Block fullblock, Block toothBlock, Block stackBlock, Joint veriticalJoint, Joint horizontalJoint, double wallLength, string name)
        {
            FullBlock = fullblock;ToothBlock = toothBlock; StackBlock = stackBlock; VeriticalJoint = veriticalJoint; 
            HorizontalJoint = horizontalJoint; WallLength = wallLength; Name = name;
        }

        public void GetCourseHeight()
        {
            CourseHeight = FullBlock.Height + HorizontalJoint.Thickness;
        }
      
        public void AddCourseElements(bool firstCourse)
        {
            // instantiate collection properties to enable it store values
            VerticalJointCollection = new ObservableCollection<Joint>();
            HorizontalJointCollection = new ObservableCollection<Joint>();
            FullBlockCollection = new ObservableCollection<Block>();
            ToothBlockCollection = new ObservableCollection<Block>();
            StackBlockCollection = new ObservableCollection<Block>();

            // intantiate course Height property
            GetCourseHeight();

            HorizontalJointCollection.Add(HorizontalJoint);
            
            // start with corner stone
            if (firstCourse)
            {
                // add full stone first
                FullBlockCollection.Add(FullBlock); //at the start
                VerticalJointCollection.Add(VeriticalJoint); // after first course
                FullBlockCollection.Add(FullBlock); // at the end

                WallLength -= FullBlock.Length;
                WallLength -= VeriticalJoint.Thickness;
                WallLength -= FullBlock.Length;
            }

            else
            {
                // add tooth stone
                ToothBlockCollection.Add(ToothBlock); //at the start
                VerticalJointCollection.Add(VeriticalJoint); // after first course
                ToothBlockCollection.Add(ToothBlock); //at the end

                WallLength -= ToothBlock.Length;
                WallLength -=VeriticalJoint.Thickness;
                WallLength -= ToothBlock.Length;
            }

            // cross checking the length of the wall
            double count = 0.0;
            while (count < WallLength)
            {
              
                // est the remaining length
                double rem = WallLength - count;
                Debug.WriteLine($"Remaining length {rem}");

                // if length is less than half of a toothblock length
                if (rem < (ToothBlock.Length/2))
                {
                    // add mortart size
                    VerticalJointCollection.Add(VeriticalJoint);
                    count += rem;
                }

                else if ( rem >= (ToothBlock.Length / 2) &&  rem < (FullBlock.Length + VeriticalJoint.Thickness))
                {
                    // add stack block
                    StackBlockCollection.Add(StackBlock);
                    count += rem;
                }

                else
                {
                    //insert block and mortar
                    FullBlockCollection.Add(FullBlock);
                    VerticalJointCollection.Add(VeriticalJoint);


                    // adjust new dimensions of built masonry
                    count += FullBlock.Length;
                    count += VeriticalJoint.Thickness;

                }
            }
            Debug.WriteLine($"Total Full blocks {FullBlockCollection.Count()}");
            Debug.WriteLine($"Total Tooth blocks {ToothBlockCollection.Count()}");
            Debug.WriteLine($"Total Stack blocks {StackBlockCollection.Count()}");
        }
    }




    internal class Ukuta
    {
        public ObservableCollection<Course> courseOneCollection { get; set; }
        public ObservableCollection<Course> courseTwoCollection { get; set; }
        public ObservableCollection<HoopIron> hoopIronCollection { get; set; }
        public ObservableCollection<DpcStrip> DpcStripCollection { get; set; }


        public Course FirstCourse { get; set; }  
        public Course SecondCourse { get; set; }
        public HoopIronStrip HoopIronStrip { get; set; }
        public DpcStrip DpcStrip {  get; set; }
        public Mortar Mortar { get; set; }


        internal Ukuta(Course firstCourse, Course secondCourse, HoopIronStrip hoopIronStrip,DpcStrip dpcStrip, Mortar mortar)
        {

            // store objects into abstract wall properties
            FirstCourse = firstCourse;
            SecondCourse = secondCourse;
            HoopIronStrip = hoopIronStrip;
            DpcStrip = dpcStrip;
            Mortar = mortar;
        }

        public void AddCourses(double heightOfWall)
        {
            // Instantiate collections

            courseOneCollection = new ObservableCollection<Course>();
            courseTwoCollection = new ObservableCollection<Course>();
            hoopIronCollection = new ObservableCollection<HoopIron>();
            DpcStripCollection = new ObservableCollection<DpcStrip>();


            // instert DPC
            DpcStripCollection.Add(DpcStrip);

            int counter = 0;
            double count = 0.0;
            while (count < heightOfWall)
            {
                // add counter to determine between first and alternate course
                counter++;
                double rem = heightOfWall - count;
                Debug.WriteLine($"Remaining course height: {rem}");
                if (counter% 2 != 0 ) // this is a first course
                {
                    if(rem >= FirstCourse.CourseHeight)
                    {
                        courseOneCollection.Add(FirstCourse);
                        count += FirstCourse.CourseHeight;
                    }

                    else if (rem == 0)
                    {
                        count += rem;
                    }

                    else // this is a course whose height is less than courseheight
                    {
                        courseOneCollection.Add(FirstCourse);
                        count += rem;
                        // The count stops at this point
                    }
                }

                else // This is the second course
                {
                    if (rem >= SecondCourse.CourseHeight)
                    {
                        courseTwoCollection.Add(SecondCourse);
                        hoopIronCollection.Add(HoopIronStrip);
                        count += SecondCourse.CourseHeight;
                    }

                    else if (rem == 0)
                    {
                        count += rem;
                    }


                    else // this is the alternate course
                    {
                        courseTwoCollection.Add(SecondCourse);
                        hoopIronCollection.Add(HoopIronStrip);
                        count += rem;
                        // The count stops at this point
                    }
                }

            }
                // if this length is more than the size of the blocks, keep building.
        }
        


        public double TotalBlocks()
        {
        Debug.WriteLine("No of courses");
        Debug.WriteLine($"No of first course: {courseOneCollection.Count()}");
        Debug.WriteLine($"No of alternate course: {courseTwoCollection.Count()}");

        // Total full blocks
        double firstCourseFullBlocks = FirstCourse.FullBlockCollection.Count() * courseOneCollection.Count();
        double secondCourseFullBlocks = SecondCourse.FullBlockCollection.Count() * courseTwoCollection.Count();
        double totalFullBlocks = firstCourseFullBlocks + secondCourseFullBlocks;
    
        //Total tooth blocks
        double firstCourseToothBlocks = FirstCourse.ToothBlockCollection.Count() * courseOneCollection.Count();
        double secondCourseToothBlocks = SecondCourse.ToothBlockCollection.Count() * courseTwoCollection.Count();
        double totalToothBlocks = firstCourseToothBlocks + secondCourseToothBlocks;


        // stack blocks
        double firstCourseStackBlocks = FirstCourse.StackBlockCollection.Count() * courseOneCollection.Count();
        double secondCourseStackBlocks = SecondCourse.StackBlockCollection.Count() * courseTwoCollection.Count();
        double totalStackBlocks = firstCourseStackBlocks + secondCourseStackBlocks;

        //Total Blocks
        double totalBlocks = totalFullBlocks + totalToothBlocks + totalStackBlocks;

        return totalBlocks;

        }

        public double TotalHorizontalJointsVolume()
        {
            double firstCourseHorizontalJoints = FirstCourse.HorizontalJointCollection.Count();
            double secondCourseHorizontaJoints = SecondCourse.HorizontalJointCollection.Count();
            double totalHorizontalJoints = firstCourseHorizontalJoints + secondCourseHorizontaJoints;

            FirstCourse.HorizontalJoint.GetVolume();
            double hjVolume = FirstCourse.HorizontalJoint.Volume;

            double totalhjVolume = hjVolume * totalHorizontalJoints;
          
            return totalhjVolume;

        }

        public double TotalVerticalJointsVolume()
        {
            double firstCourseVerticalJoints = FirstCourse.VerticalJointCollection.Count();
            double secondCourseVerticalJoints = SecondCourse.VerticalJointCollection.Count();


            // total number of joints

            double totalVerticalJoints = firstCourseVerticalJoints + secondCourseVerticalJoints;

            FirstCourse.VeriticalJoint.GetVolume();
            double vjVolume = FirstCourse.VeriticalJoint.Volume;

            double totalvjVolume = vjVolume * totalVerticalJoints;
            return totalvjVolume;

        }


        // vj = vertical joint
        // hj = horizontal joint
        
        
        public void setMortarVolume()
        {
            double totalVolume = TotalHorizontalJointsVolume() + TotalVerticalJointsVolume();
            Mortar.Volume = totalVolume;
        }
        
        public double TotalCementWeight()
        {
       
            setMortarVolume();
       
            double cementVolume = Mortar.GetCementVolume();
            Mortar.Cement.Volume = cementVolume;
            double cementWeight = Mortar.Cement.GetWeight();


            return cementWeight;
        }


        public double TotalSandWeight()
        {
            // sand weight
            setMortarVolume();

            double sandVolume = Mortar.GetSandVolume();
            Mortar.Sand.Volume = sandVolume;
            double sandWeight = Mortar.Sand.GetWeight();

            return sandWeight;
        }

        public double TotalDpcRolls()
        {
            double totalDpcStripLength = DpcStrip.SectionLength * DpcStripCollection.Count();
            double NoOfSectionsInOneRow = (int)(DpcStrip.Width / DpcStrip.SectionWidth);
            double TotalLengthofdpcPerRow = NoOfSectionsInOneRow * DpcStrip.Length;
            double NoOfRolls = (totalDpcStripLength / TotalLengthofdpcPerRow);


            Debug.Write($"dpc Strip length {HoopIronStrip.StripLength}");
            Debug.Write($"dpc strip length collection {hoopIronCollection.Count()}");
            Debug.Write($"dpc Total length {TotalLengthofdpcPerRow}");
            Debug.Write($"dpc no of rolls {NoOfRolls}");

            if (NoOfRolls > 0 && NoOfRolls < 1) { NoOfRolls = 1; }

            return Convert.ToInt32(NoOfRolls);

        }

        public double TotalHoopIronRolls()
        {
            double totalHoopIronStripLength = HoopIronStrip.StripLength * hoopIronCollection.Count();
            Debug.Write($"Strip length {HoopIronStrip.StripLength}");
            Debug.Write($"strip length collection {hoopIronCollection.Count()}");
            Debug.Write($"Total hoop iron length {totalHoopIronStripLength}");
            double NoOfRolls = (totalHoopIronStripLength / HoopIronStrip.Length);
            Debug.Write($"DEFAULT HOOP IRON LENGTH {HoopIronStrip.Length}");
            Debug.Write($"hoopiron no of rolls {NoOfRolls}");

            if ( NoOfRolls > 0 && NoOfRolls < 1) { NoOfRolls = 1; }



            return Convert.ToInt32(NoOfRolls);
        }


    }
            
}
