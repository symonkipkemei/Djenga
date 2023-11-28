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
            double totalLengthHoopIron = noOfHoopIron * hoopiron.LengthOfHoopIron;

            return totalLengthHoopIron;
        }
     
        public double AddStones()
        {
            double firstCourseStonesFull = firstCourse.StoneFullCollection.Count();
            double secondCourseStonesFull = secondCourse.StoneFullCollection.Count();
            double firstCourseStonesTooth = firstCourse.StoneToothCollection.Count();
            double secondCourseStonesTooth = secondCourse.StoneFullCollection.Count();

            double totalStoneFull = firstCourseStonesFull + secondCourseStonesFull;
            double totalStoneTooth = firstCourseStonesTooth + secondCourseStonesTooth;
            double totalStones = totalStoneFull + totalStoneTooth;

            return totalStones;
        }


        public double AddMortar()
        {
            double firstCourseHorizontalMortar = firstCourse.MortarHorizontalCollection.Count();
            double secondCourseHorizontalMortar = secondCourse.MortarHorizontalCollection.Count();

            double firstCourseVerticalMortar = firstCourse.MortarVerticalCollection.Count();
            double secondCourseVerticalMortar = secondCourse.MortarVerticalCollection.Count();

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
