using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Djenga.Model
{
    public class Calculator
    {

        internal Course firstCourse;
        internal Course secondCourse;
        internal HoopIron hoopiron;
        internal AbstractWall _abstractWall;

        internal Calculator(AbstractWall abstractWall)
        {
            _abstractWall = abstractWall;
            firstCourse = abstractWall.FirstCourse;
            secondCourse = abstractWall.SecondCourse;
            hoopiron = abstractWall.HoopIronPiece;
    
        }

        public double AddHoopIron()
        {
            double noOfHoopIron = _abstractWall.hoopIronCollection.Count();
            double totalLengthHoopIron = noOfHoopIron * hoopiron.lengthOfHoopIron;

            return totalLengthHoopIron;
        }
     
        public double AddStones()
        {
            double firstCourseStonesFull = firstCourse.stoneFullCollection.Count();
            double secondCourseStonesFull = secondCourse.stoneFullCollection.Count();
            double firstCourseStonesTooth = firstCourse.stoneToothCollection.Count();
            double secondCourseStonesTooth = secondCourse.stoneFullCollection.Count();

            double totalStoneFull = firstCourseStonesFull + secondCourseStonesFull;
            double totalStoneTooth = firstCourseStonesTooth + secondCourseStonesTooth;
            double totalStones = totalStoneFull + totalStoneTooth;

            return totalStones;
        }


        public double AddMortar()
        {
            double firstCourseHorizontalMortar = firstCourse.mortarHorizontalCollection.Count();
            double secondCourseHorizontalMortar = secondCourse.mortarHorizontalCollection.Count();

            double firstCourseVerticalMortar = firstCourse.mortarVerticalCollection.Count();
            double secondCourseVerticalMortar = secondCourse.mortarVerticalCollection.Count();

            double verticalMortarVolume = firstCourse.VerticalMortar.Volume();
            double horizontalMortarVolume = secondCourse.HorizontalMortar.Volume();

            double totalVerticalMortar = firstCourseVerticalMortar + secondCourseVerticalMortar;
            double totalHorizontalMortar = firstCourseHorizontalMortar + secondCourseHorizontalMortar;
            double totalVerticalMortarVolume = totalVerticalMortar * verticalMortarVolume;
            double totalHorizontalMortarVolume = totalHorizontalMortar * horizontalMortarVolume;

            double totalMortarVolume = totalVerticalMortarVolume + totalHorizontalMortarVolume;

            return totalMortarVolume;
        }


    }
}
