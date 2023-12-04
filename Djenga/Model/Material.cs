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

        public double Amount { get; set; }

        internal Stone(string name = "Machine Cut Stone",
                      string unit = "Pieces",
                      double rate = 50,
                      double density = 0.001)
        {
            Name = name;Unit = unit;Rate = rate;Density = density;
        }


        public void GetAmount()
        {
            Amount = Quantity * Rate;
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
        
        public double Amount { get; set; }


        // Dimensional properties

        public double Volume { get; set; }



        internal Sand(string name = "River Sand",
                      string unit = "Grams",
                      double rate = 300,
                      double density = 0.001602) //g/mm3 ( 1602kg/m3)
        {
            Name = name;Unit = unit;Rate = rate;Density = density;
        }

        public double GetWeight()
        {
            double Weight=  Volume * Density;
            return Weight;
        }

        public void GetAmount()
        {
            Amount = Quantity * Rate;
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


        // Dimensional properties
        public double Volume { get; set; }

        public double Amount { get; set; }


        internal Cement(string name = "Bamburi Cement",
                        string unit = "Bags", 
                        double rate = 605, 
                        double density = 0.00144)
        {
            Name = name; Unit = unit; Rate = rate; Density = density;
           
        }

        
        public double GetWeight()
        {
            double Weight= Volume * Density;
            return Weight;
        }

        
        public double NoOfBags(int bagSizeing)
        {
            double aproxBags = (Volume * Density) / bagSizeing;
            int EstimateBags =  (int)Math.Round(aproxBags);
            return EstimateBags;
        }

        public void GetAmount()
        {
            Amount = Quantity * Rate;
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
        public double Length { get; set; }

        public double Weight { get; set; }

        //Construtor
        internal HoopIron()
        {
            Length = 20000; // 20m (20,000mm)long
            Name = "Hoop Iron (16 Guage)";
            Unit = "Rolls";
            Rate = 3500;
            Weight = 20000;//20kg (20000g)
            Guage = 16;

        }

        //Methods
        public double Amount()
        {
            return Quantity * Rate;
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
            Width = 1000;// 1000mm (1m)
            Length = 7000; // 7000mm (7m)
        }


        public double Amount()
        {
            return Quantity * Rate;
        }

    }

}

