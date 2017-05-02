using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicator
{
    public class Counter
    {
        private double maxvalue { get; set; }
        private double minvalue { get; set; }
        public double maxValue { get { return maxvalue; } set { maxvalue = value; } }
        public double minValue { get { return minvalue; } set { minvalue = value; } }
        private double value { get; set; }
        public  double Value
        {
            get { return value; }
            set
            {
                if ((value > maxValue) || (value < minValue))
                {
                    this.value = 0;
                }
                else { this.value = value; }

            }
        }
        public  Counter()
        {
            this.maxvalue = 5000;
            this.minvalue = 0;
            this.value = 0;
        }

        public  Counter(int max, int min)
        {
            this.maxValue = max;
            this.minValue = min;
        }
        
    }
    public class Electricity_meter : Counter,IEnumerable
    {
        public Electricity_meter(int max , int accuracy,double value)
        {
            this.maxValue = max;
            this.accuracy = accuracy;
            this.Value = value;
            this.minValue = 0;
        }
        //Розрядність 
        private  int Bit;
        public  int bit
        {
            get { return Bit; }
            set
            {
                if ((value == 5) || (value == 7) || (value == 8))
                {
                    Bit = value;
                }
                else Bit = 5;
            }
        }
        private  int Accuracy;
        public  int accuracy
        {
            get { return Accuracy; }
            set
            {
                Accuracy = value;
            }
        }
        public  IEnumerator GetEnumerator()
        {
            yield return this.Value;
            
        }
        public List<int> Show()
        {
            List<int> result=new List<int>();
            string Numbers = Value.ToString();
            
        var intList = Numbers.Select(digit => int.Parse(digit.ToString()));

        foreach (char i in intList)
            {
                result.Add(i);   
            }
            return result;
        }
        public static Electricity_meter operator ++(Electricity_meter a)
        {
            a.Value++;
            return a;
        }
        public static Electricity_meter operator --(Electricity_meter a)
        {
            a.Value--;
            return a;
        }
        
    }
    public class Calculated_book
    {
        private double worth;
        public double Worth { get { return worth; } set { worth = Math.Round(value, 2); } }
        public DateTime Time { get; set; }
        public double price { get; set; }
        public Calculated_book()
        {
        }
        public Calculated_book(double price)
        {
            this.price = price;
        }
        public List<Electricity_meter> Book = new List<Electricity_meter>();
        public double Calculate_price(double worth)
       {
           Worth = worth;
           double WholeElectricity=0;
           foreach (var i in Book)
           {
               WholeElectricity +=(double) i.Value/Math.Pow(10,i.accuracy); 
           }
           price = WholeElectricity * Worth;
           return price;
       }
        public double[] Total_prices = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    }
}