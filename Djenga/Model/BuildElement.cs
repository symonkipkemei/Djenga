using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB.Electrical;
using System.Security.Cryptography.X509Certificates;
using Autodesk.Revit.DB;

namespace Djenga.Model
{


    internal class DpcStrip: DampProofCourse
    {
        // Dimensional Properties
        public double SectionWidth { get; set; }
        public double SectionLength { get; set; }

        internal DpcStrip( double WallWidth,double WallLength)
        {
            double offset = 25;
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
        public double Volume {  get; set; }

        internal Mortar(double volume) 
        {
            Volume = volume;
            RatioSand = 3;
            RatioCement = 1;
            double cementVolume = (RatioSand / (RatioSand + RatioCement)) * Volume;
            double sandVolume = (RatioSand / (RatioSand + RatioCement)) * Volume;
            Cement = new Cement(cementVolume);
            Sand = new Sand(sandVolume);
        }
        
    }


    internal class Joint
    {
        //Dimensional Properties
        public double Thickness { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Volume { get; set; }


        // make up properties
        public Mortar Mortar { get; set; }


        internal Joint()
        {
            Volume = Thickness * Length * Width;
            Mortar = new Mortar(Volume);
        }
     
    }



    internal class Block: Stone
    {
        // Dimensional properties
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }


        public double Volume()
        {
            return Width * Length * Height;
        }

        public double Weight()
        {
            return (Length * Width * Height) * Density;
        }

    }

    internal class Course
    {

        // Material Properties/Objects
        public Block FullBlock { get; set; }
        public Block ToothBlock { get; set; }
        public Joint VeriticalJoint { get; set; }
        public Joint HorizontalJoint { get; set; }


        //Descriptive Properties
        public double WallLength { get; set; }
        public double CourseHeight { get; set; }
        public string Name { get; set; }
      


        // Collection properties
        public ObservableCollection<Block> FullBlockCollection { get; set; }
        public ObservableCollection<Block> ToothBlockCollection { get; set; }
        public ObservableCollection<Joint> VerticalJointCollection { get; set; }
        public ObservableCollection<Joint> HorizontalJointCollection { get; set; }


       
        
        public Course(double wallWidth, double wallLength, double mortarThickness, double masonryHeight, double masonryLength, double masonryWidth)
        {
            // Intanitiate Material objects
            VeriticalJoint = new Joint
            {
                Thickness = mortarThickness,
                Width = wallWidth,
                Length = masonryHeight,
           
            };

            HorizontalJoint = new Joint
            {
                Thickness = mortarThickness,
                Width = wallWidth,
                Length = wallLength,
            };

            FullBlock = new Block
            {
                Width = masonryWidth,
                Length = masonryLength,
                Height = masonryHeight
            };

            ToothBlock = new Block
            {
                Width = masonryWidth,
                Length = masonryLength / 2,
                Height = masonryHeight
            };

            // Intanitiate Dimensional properties
            WallLength = wallLength;
            CourseHeight = FullBlock.Height + HorizontalJoint.Thickness;

            // instantiate collection properties to enable it store values
            VerticalJointCollection = new ObservableCollection<Joint>();
            HorizontalJointCollection = new ObservableCollection<Joint>();
            FullBlockCollection = new ObservableCollection<Block>();
            ToothBlockCollection = new ObservableCollection<Block>();
        }

      
        public void AddCourseElements(bool firstCourse)
        {
            HorizontalJointCollection.Add(HorizontalJoint);
            
            // start with corner stone
            if (firstCourse)
            {
                // add full stone first
                FullBlockCollection.Add(FullBlock);
                WallLength -= FullBlock.Length;
            }

            else
            {
                // add tooth stone
                ToothBlockCollection.Add(ToothBlock);
                WallLength -= ToothBlock.Length;
            }

            // cross checking the length of the wall
            double count = 0.0;
            while (count < WallLength)
            {

                //insert block and mortar
                FullBlockCollection.Add(FullBlock);
                VerticalJointCollection.Add(VeriticalJoint);


                // adjust new dimensions of built masonry
                count += FullBlock.Length;
                count += VeriticalJoint.Thickness;

                // est the remaining length
                double rem = WallLength - count;

                if (rem < FullBlock.Length && rem > ToothBlock.Length)
                {
                    //use a bigger block
                    FullBlockCollection.Add(FullBlock);
                    count += rem;
                }

                else if (rem < ToothBlock.Length)
                {
                    //use  a tooth block
                    ToothBlockCollection.Add(FullBlock);
                    count += rem;
                }
                // if this length is more than the size of the blocks, keep building.
            }
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
       

        internal Ukuta(Course firstCourse, Course secondCourse, HoopIronStrip hoopIronStrip,DpcStrip dpcStrip)
        {

            // store objects into abstract wall properties
            FirstCourse = firstCourse;
            SecondCourse = secondCourse;
            HoopIronStrip = hoopIronStrip;
            DpcStrip = dpcStrip;

            // Instantiate collections

            courseOneCollection = new ObservableCollection<Course>();
            courseTwoCollection = new ObservableCollection<Course>();
            hoopIronCollection = new ObservableCollection<HoopIron>();
            DpcStripCollection = new ObservableCollection<DpcStrip>();

        }

        public void AddCourses(double heightOfWall)
        {

            // instert DPC

            DpcStripCollection.Add(DpcStrip);
            double count = 0.0;
            while (count < heightOfWall)
            {
                //Insert the first course
                courseOneCollection.Add(FirstCourse);
                count += FirstCourse.CourseHeight;

                double rem = heightOfWall - count;

                if (rem < FirstCourse.CourseHeight)
                {
                    courseTwoCollection.Add(SecondCourse);
                    hoopIronCollection.Add(HoopIronStrip);
                    count += rem;
                }
                   
                else 
                {
                    courseTwoCollection.Add(SecondCourse);
                    hoopIronCollection.Add(HoopIronStrip);

                    count += FirstCourse.CourseHeight;

                    if (rem < SecondCourse.CourseHeight)
                    {
                        courseOneCollection.Add(FirstCourse);
                        count += rem;
                    }
               
                }
            }
                // if this length is more than the size of the blocks, keep building.
        }
        


        public double TotalBlocks()
        {
        // Total full blocks
        double firstCourseFullBlocks = FirstCourse.FullBlockCollection.Count();
        double secondCourseFullBlocks = SecondCourse.FullBlockCollection.Count();
        double totalFullBlocks = firstCourseFullBlocks + secondCourseFullBlocks;

        //Total tooth blocks
        double firstCourseToothBlocks = FirstCourse.ToothBlockCollection.Count();
        double secondCourseToothBlocks = SecondCourse.FullBlockCollection.Count();
        double totalToothBlocks = firstCourseToothBlocks + secondCourseToothBlocks;

        //Total Blocks
        double totalBlocks = totalFullBlocks + totalToothBlocks;

        return totalBlocks;
        }

        public double TotalHorizontalJoints()
        {
            double firstCourseHorizontalJoints = FirstCourse.HorizontalJointCollection.Count();
            double secondCourseHorizontaJoints = SecondCourse.HorizontalJointCollection.Count();
            double totalHorizontalJoints = firstCourseHorizontalJoints + secondCourseHorizontaJoints;


            return totalHorizontalJoints;

        }

        public double TotalVerticalJoints()
        {
            double firstCourseVerticalJoints = FirstCourse.VerticalJointCollection.Count();
            double secondCourseVerticalJoints = SecondCourse.VerticalJointCollection.Count();

            // total number of joints

            double totalVerticalJoints = firstCourseVerticalJoints + secondCourseVerticalJoints;

            return totalVerticalJoints;

        }


        // vj = vertical joint
        // hj = horizontal joint


        public double TotalCementWeight()
        {
            // Cement weight

            double vjCementWeight = FirstCourse.VeriticalJoint.Mortar.Cement.Weight();
            double hjCementWeight = SecondCourse.HorizontalJoint.Mortar.Cement.Weight();

            double totalvjCementWeight = TotalVerticalJoints() * vjCementWeight;
            double totalhjCementWeight = TotalHorizontalJoints() * hjCementWeight;

            double totalCementWeight = totalvjCementWeight + totalhjCementWeight;

            return totalCementWeight;
        }


        public double TotalSandWeight()
        {
            // sand weight
            double vjSandWeight = FirstCourse.VeriticalJoint.Mortar.Sand.Weight();
            double hjSandWeight = SecondCourse.HorizontalJoint.Mortar.Sand.Weight();

            double totalvjSandWeight = TotalVerticalJoints() * vjSandWeight;
            double totalhjSandWeight = TotalHorizontalJoints() * hjSandWeight;

            double totalSandWeight = totalvjSandWeight + totalhjSandWeight;

           
            return totalSandWeight;
        }

        public double TotalDpcRolls()
        {
            double totalDpcStripLength = DpcStrip.SectionLength * DpcStripCollection.Count();
            double NoOfSectionsInOneRow = (int)(DpcStrip.Width / DpcStrip.SectionWidth);
            double TotalLengthofdpcPerRow = NoOfSectionsInOneRow * DpcStrip.Length;

            int NoOfRolls =Convert.ToInt32(totalDpcStripLength / TotalLengthofdpcPerRow);

            return NoOfRolls;

        }

        public double TotalHoopIronRolls()
        {
            double totalHoopIronStripLength = HoopIronStrip.StripLength * hoopIronCollection.Count();
            int NoOfRolls = Convert.ToInt32(totalHoopIronStripLength / HoopIronStrip.Length);
            
            return NoOfRolls;
        }


    }
            
}
