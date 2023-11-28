using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Djenga.Model
{

    internal class Stone
    {
        // Descriptive Properties
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity {  get; set; }
        public double Rate {  get; set; }


        // Dimensional properties
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public double Density { get; set; }



        internal Stone()
        {
            Unit = "Pieces";
            Name = "Machine Cut Stone";
            Rate = 50;
        }


        // Methods
        public double Volume()
        {
            return Width * Length * Height;
        }

        public double Weight()
        {
            return (Length * Width * Height) * Density;
        }

        public double Amount()
        {
            return Quantity * Rate;
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
        public string name ;
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
        // Descriptive Properties

        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity {  get; set; }
        public double Rate { get; set; }


        // Dimensional Properties
        public double Density { get; set; }
        public double Guage { get; set; }
        public double LengthOfHoopIron { get; set; }


        //Construtor
        internal HoopIron(double length)
        {
            LengthOfHoopIron = length;
            Name = "Hoop Iron (25 Guage)";
        }

        //Methods
        public double Amount()
        {
            return Quantity * Rate;
        }

        public double GetNoOfRolls(double totalHoopIronLength)
        {
            return totalHoopIronLength / 65.6168;
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

