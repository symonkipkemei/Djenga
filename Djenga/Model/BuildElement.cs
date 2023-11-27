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

    
        private Stone stoneFull;
        private Stone stoneTooth;
        private double lengthOfWall;

        public string name;
        public double courseHeight;
        public ObservableCollection<Stone> stoneFullCollection;
        public ObservableCollection<Stone> stoneToothCollection;
        public ObservableCollection<Mortar> mortarVerticalCollection;
        public ObservableCollection<Mortar> mortarHorizontalCollection;


        public Mortar VerticalMortar { get; set; } 
        public Mortar HorizontalMortar { get; set; }

        
        public Course(double wallWidth, double wallLength, double wallHeight, double mortarThickness, double masonryHeight, double masonryLength, double masonryWidth)
        {
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

            stoneFull = new Stone
            {
                name = "full",
                width = masonryWidth,
                length = masonryLength,
                height = masonryHeight
            };

            stoneTooth = new Stone
            {
                name = "Tooth",
                width = masonryWidth,
                length = masonryLength / 2,
                height = masonryHeight
            };

            lengthOfWall = wallLength;
        }

      
        public void AddCourseElements(bool firstCourse)
        {
            mortarVerticalCollection.Add(HorizontalMortar);
            courseHeight = stoneFull.height + HorizontalMortar.thickness;

            // Ensure the collections are empty first


            // start with corner stone
            if (firstCourse)
            {
                // add full stone first
                stoneFullCollection.Add(stoneFull);
                lengthOfWall -= stoneFull.length;
            }

            else
            {
                // add tooth stone
                stoneToothCollection.Add(stoneTooth);
                lengthOfWall -= stoneTooth.length;
            }

            // cross checking the length of the wall
            double count = 0.0;
            while (count < lengthOfWall)
            {

                //insert block and mortar
                stoneFullCollection.Add(stoneFull);
                mortarVerticalCollection.Add(VerticalMortar);


                // adjust new dimensions of built masonry
                count += stoneFull.length;
                count += VerticalMortar.thickness;

                // est the remaining length
                double rem = lengthOfWall - count;

                if (rem < stoneFull.length && rem > stoneTooth.length)
                {
                    //use a bigger block
                    stoneFullCollection.Add(stoneFull);
                    count += rem;
                }

                else if (rem < stoneTooth.length)
                {
                    //use  a tooth block
                    stoneToothCollection.Add(stoneFull);
                    count += rem;
                }
                // if this length is more than the size of the blocks, keep building.
            }
        }

    }


    internal class AbstractWall
    {
        public ObservableCollection<Course> courseOneCollection;
        public ObservableCollection<Course> courseTwoCollection;
        public ObservableCollection<HoopIron> hoopIronCollection;

    
        public Course FirstCourse { get; set; }  
        public Course SecondCourse { get; set; }
        public HoopIron HoopIronPiece { get; set; }
       

        internal AbstractWall(Course firstCourse, Course secondCourse, HoopIron hoopiron)
        {
            FirstCourse = firstCourse;
            SecondCourse = secondCourse;
            HoopIronPiece = hoopiron;

        }

        public void AddCourses(double heightOfWall)
        {
            double count = 0.0;
            while (count < heightOfWall)
            {
                //Insert the first course
                courseOneCollection.Add(FirstCourse);
                count += FirstCourse.courseHeight;

                double rem = heightOfWall - count;

                if (rem < FirstCourse.courseHeight)
                {
                    courseTwoCollection.Add(SecondCourse);
                    hoopIronCollection.Add(HoopIronPiece);
                    count += rem;
                }
                   
                else 
                {
                    courseTwoCollection.Add(SecondCourse);
                    hoopIronCollection.Add(HoopIronPiece);

                    count += FirstCourse.courseHeight;

                    if (rem < SecondCourse.courseHeight)
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
