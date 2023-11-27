using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Djenga.Model
{

    internal class Stone
    {
        //Properties
        public string name;
        public double width;
        public double height;
        public double length;
        public double density;


        // volume
        public double Volume()
        {
            return width * length * height;
        }

        public double Weight()
        {
            return (length * width * height) * density;
        }

    }


    internal class Sand
    {
        //Properties
        public string name;
        public double density;

    }


    internal class Cement
    {
        //Properties
        public string name;
        public double density;
        public double weight; //in kg

        // volume
        public double Volume()
        {
            return weight / density;
        }
    }


    internal class HoopIron
    {
        //Properties
        public string name;
        public double density;
        public double guage;
        public double lengthOfHoopIron;


        internal HoopIron(double length)
        {
            lengthOfHoopIron = length;
        }
    }




    internal class DampProofCourse
    {
        //Properties
        public string name;
        public double width;
        public double length;
    }

}

