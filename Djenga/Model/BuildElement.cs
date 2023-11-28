using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB.Electrical;

namespace Djenga.Model
{

    internal class Mortar
    {
        public double thickness;
        public double length;
        public double width;
        public int ratioSand;
        public int ratioCement;

        public double Volume()
        {
            return thickness * width * length;
        }
    }


    internal class Course
    {

        // Material Properties/Objects
        public Stone StoneFull { get; set; }
        public Stone StoneTooth { get; set; }
        public Mortar VerticalMortar { get; set; }
        public Mortar HorizontalMortar { get; set; }


        //Descriptive Properties
        public double WallLength { get; set; }
        public double CourseHeight { get; set; }
        public string Name { get; set; }
      


        // Collection properties
        public ObservableCollection<Stone> StoneFullCollection { get; set; }
        public ObservableCollection<Stone> StoneToothCollection { get; set; }
        public ObservableCollection<Mortar> MortarVerticalCollection { get; set; }
        public ObservableCollection<Mortar> MortarHorizontalCollection { get; set; }


       
        
        public Course(double wallWidth, double wallLength, double wallHeight, double mortarThickness, double masonryHeight, double masonryLength, double masonryWidth)
        {
            // Intanitiate Material objects
            VerticalMortar = new Mortar
            {
                thickness = mortarThickness,
                width = wallWidth,
                length = wallHeight,
                ratioSand = 3,
                ratioCement = 1
            };

            HorizontalMortar = new Mortar
            {
                thickness = mortarThickness,
                width = wallWidth,
                length = wallLength,
                ratioSand = 3,
                ratioCement = 1
            };

            StoneFull = new Stone
            {
                Name = "full",
                Width = masonryWidth,
                Length = masonryLength,
                Height = masonryHeight
            };

            StoneTooth = new Stone
            {
                Name = "Tooth",
                Width = masonryWidth,
                Length = masonryLength / 2,
                Height = masonryHeight
            };

            // Intanitiate Dimensional properties
            WallLength = wallLength;
            CourseHeight = StoneFull.Height + HorizontalMortar.thickness;

            // instantiate collection properties to enable it store values
            MortarVerticalCollection = new ObservableCollection<Mortar>();
            MortarHorizontalCollection = new ObservableCollection<Mortar>();
            StoneFullCollection = new ObservableCollection<Stone>();
            StoneToothCollection = new ObservableCollection<Stone>();
        }

      
        public void AddCourseElements(bool firstCourse)
        {
            MortarVerticalCollection.Add(HorizontalMortar);
            
            // start with corner stone
            if (firstCourse)
            {
                // add full stone first
                StoneFullCollection.Add(StoneFull);
                WallLength -= StoneFull.Length;
            }

            else
            {
                // add tooth stone
                StoneToothCollection.Add(StoneTooth);
                WallLength -= StoneTooth.Length;
            }

            // cross checking the length of the wall
            double count = 0.0;
            while (count < WallLength)
            {

                //insert block and mortar
                StoneFullCollection.Add(StoneFull);
                MortarVerticalCollection.Add(VerticalMortar);


                // adjust new dimensions of built masonry
                count += StoneFull.Length;
                count += VerticalMortar.thickness;

                // est the remaining length
                double rem = WallLength - count;

                if (rem < StoneFull.Length && rem > StoneTooth.Length)
                {
                    //use a bigger block
                    StoneFullCollection.Add(StoneFull);
                    count += rem;
                }

                else if (rem < StoneTooth.Length)
                {
                    //use  a tooth block
                    StoneToothCollection.Add(StoneFull);
                    count += rem;
                }
                // if this length is more than the size of the blocks, keep building.
            }
        }

    }


    internal class AbstractWall
    {
        public ObservableCollection<Course> courseOneCollection { get; set; }
        public ObservableCollection<Course> courseTwoCollection { get; set; }
        public ObservableCollection<HoopIron> hoopIronCollection { get; set; }


        public Course FirstCourse { get; set; }  
        public Course SecondCourse { get; set; }
        public HoopIron HoopIronPiece { get; set; }
       

        internal AbstractWall(Course firstCourse, Course secondCourse, HoopIron hoopiron)
        {

            // store objects into abstract wall properties
            FirstCourse = firstCourse;
            SecondCourse = secondCourse;
            HoopIronPiece = hoopiron;

            // Intantiate collections

            courseOneCollection = new ObservableCollection<Course>();
            courseTwoCollection = new ObservableCollection<Course>();
            hoopIronCollection = new ObservableCollection<HoopIron>();


        }

        public void AddCourses(double heightOfWall)
        {
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
                    hoopIronCollection.Add(HoopIronPiece);
                    count += rem;
                }
                   
                else 
                {
                    courseTwoCollection.Add(SecondCourse);
                    hoopIronCollection.Add(HoopIronPiece);

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
        }
    }
