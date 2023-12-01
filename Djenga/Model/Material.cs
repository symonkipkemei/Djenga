using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Djenga.Model
{

    internal class Stone
    {
        // Descriptive Properties
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity {  get; set; }
        public double Rate {  get; set; }

        public double Density { get; set; }


        internal Stone()
        {
            Unit = "Pieces";
            Name = "Machine Cut Stone";
            Rate = 50;
        }


        public double Amount()
        {
            return Quantity * Rate;
        }

    }


    internal class Sand
    {
        // Descriptive Properties
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public double Density { get; set; }
        public double Volume { get; set; }


        internal Sand(double volume)
        {
            Name = "River Sand";
            Unit = "Volume";
            Rate = 300;
            Density = 1602; //kg/m3
            Volume = volume;
        }
        public double Weight()
        {
            return Volume * Density;
        }

        public double Amount()
        {
            return Quantity * Rate;
        }

    }


    internal class Cement
    {
        // Descriptive Properties
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public double Density { get; set; }
        public double Volume { get; set; }


        internal Cement(double volume)
        {
            Name = "Bamburi Cement";
            Unit = "Bags";
            Rate = 605;
            Density = 1440; //kg/m3
            Volume = volume;
        }

        
        public double Weight()
        {
            return Volume * Density;
        }

        public double NoOfBags(int bagSizeinKg)
        {
            double aproxBags = (Volume * Density) / bagSizeinKg;
            int EstimateBags =  (int)Math.Round(aproxBags);
            return EstimateBags;
        }

        public double Amount()
        {
            return Quantity * Rate;
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
        // Descriptive Properties

        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }


        // Dimensional properties
        public double Width { get; set; }
        public double Length { get; set; }


        internal DampProofCourse()
        {
            Name = "Damp Proof Course (DPC)";
            Unit = "Rolls";
            Rate = 2200;
            Width = 1000;
            Length = 7000;
        }


        public double Amount()
        {
            return Quantity * Rate;
        }

    }

}

