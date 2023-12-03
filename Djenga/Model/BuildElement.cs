﻿using System;
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

        internal Mortar( Cement cement, Sand sand, int ratioSand = 3, int ratioCement = 1)
        {
         Cement  = cement; Sand = sand; RatioSand = ratioSand; RatioCement = ratioCement;
        }

        public void GetVolume(double volume)
        {
            Volume = volume;
        }

        public double GetCementVolume()
        {
            return (RatioCement/ (RatioCement + RatioSand)) * Volume;
        }

        public double GetSandVolume()
        {
            return (RatioSand / (RatioCement + RatioSand)) * Volume;
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


       
        
        public Course(Block fullblock, Block toothBlock, Joint veriticalJoint, Joint horizontalJoint, double wallLength, string name)
        {
            FullBlock = fullblock;ToothBlock = toothBlock; VeriticalJoint = veriticalJoint; 
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


            // intantiate course Height property
            GetCourseHeight();

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
            Debug.WriteLine($"Total Full blocks {FullBlockCollection.Count()}");
            Debug.WriteLine($"Total Tooth blocks {ToothBlockCollection.Count()}");
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
            Debug.WriteLine("courses");
            Debug.WriteLine(courseOneCollection.Count());
            Debug.WriteLine(courseTwoCollection.Count());
        // Total full blocks
        double firstCourseFullBlocks = FirstCourse.FullBlockCollection.Count() * courseOneCollection.Count();
        double secondCourseFullBlocks = SecondCourse.FullBlockCollection.Count() * courseTwoCollection.Count();
        double totalFullBlocks = firstCourseFullBlocks + secondCourseFullBlocks;
    
        //Total tooth blocks
        double firstCourseToothBlocks = FirstCourse.ToothBlockCollection.Count() * courseOneCollection.Count();
        double secondCourseToothBlocks = SecondCourse.ToothBlockCollection.Count() * courseTwoCollection.Count();
        double totalToothBlocks = firstCourseToothBlocks + secondCourseToothBlocks;

        //Total Blocks
        double totalBlocks = totalFullBlocks + totalToothBlocks;

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
            Mortar.GetVolume(TotalHorizontalJointsVolume() + TotalVerticalJointsVolume());
        }
        
        public double TotalCementWeight()
        {
       
            setMortarVolume();
       
            double cementVolume = Mortar.GetCementVolume();
            Mortar.Cement.GetVolume(cementVolume);

            double cementWeight = Mortar.Cement.Weight;

            return cementWeight;
        }


        public double TotalSandWeight()
        {
            // sand weight
            setMortarVolume();

            double sandVolume = Mortar.GetSandVolume();
            Mortar.Sand.GetVolume(sandVolume);

            double sandWeight = Mortar.Cement.Weight;

            return sandWeight;
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
