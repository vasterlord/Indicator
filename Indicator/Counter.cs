using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Indicator
{
    public abstract class Counter
    {
        public Counter()
        {
            this._minValue = 0;
            this._maxValue = 5000;
            this._value = 0;
        }

        public Counter(int max, int min, double value)
        {
            this._minValue = min;
            this._maxValue = max;
            this._value = value;
        } 

        private double _maxValue;
        private double _minValue;
        private double _value;
        public double MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (value <= 0)
                {
                    _maxValue = 5000;
                    throw new FormatException("Incorrect maximum value of electricity meter! This value setted by default: 5000");
                }
                else
                {
                    _maxValue = value;
                }
            }
        }
        public double MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                if (value < 0)
                {
                    _maxValue = 0;
                    throw new FormatException("Incorrect mainimumm value of electricity meter! This value setted by default: 0");
                }
                else
                {
                    _minValue = value;
                }
            }
        }
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                if ((value >= MaxValue) || (value < MinValue))
                {
                    this._value = 0;
                }
                else if (value < 0)
                {
                    throw new FormatException("Incorrect value of electricity meter! This value setted by default: 0");
                }
                else
                {
                    this._value = value;
                }
            }
        }
        public abstract List<int> Show();
    } 

    public class Electricity_meter : Counter, IEnumerable
    {
        public Electricity_meter() : base()
        {
            this.MinValue = 0;
            this.MaxValue = 5000;
            this.Value = 0;
        }
        public Electricity_meter(int max, int min ,double value, int accuracy, int bit ):base(max,min,value)
        {
            this._accuracy = accuracy;
            this._bit = bit;
        }
        private  int _bit;
        private int _accuracy;
        public  int Bit
        {
            get
            {
                return _bit;
            }
            set
            {
                if (value > 8)
                {
                    _bit = 8;
                    throw new FormatException("Incorrect enterd bit value!This value setted by default: 8 bit");
                }
                else
                {
                   _bit = value; 
                }
            }
        }
        public  int Accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                if (_accuracy < 0 || _accuracy >= _bit)
                {
                    _accuracy = 1;
                    throw new FormatException("Incorrect enterd accuracy value!This value setted by default: 1");
                }
                else
                {
                    _accuracy = value;
                }
            }
        }
        public  IEnumerator GetEnumerator()
        {
            yield return this.Value;
            
        }

        public override List<int> Show()
        {
            List<int> result = new List<int>();
            string Numbers = Value.ToString();

            var intList = Numbers.Select(digit => int.Parse(digit.ToString()));

            foreach (char i in intList)
            {
                result.Add(i);
            }
            return result;
        }
        public static Electricity_meter operator ++(Electricity_meter counter)
        {
            counter.Value++;
            return counter;
        }
        public static Electricity_meter operator --(Electricity_meter counter)
        {
            counter.Value--;
            return counter;
        }
    }

    public class Calculated_book
    { 
        public Calculated_book()
        {
            this._price = 10;
            this.Date = "January";
        }
        public Calculated_book(double price, string date)
        {
            this._price = price;
            Date = date;
        }
        public double _price { get; set; }
        private string _date; 
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value <= 0)
                {
                    _price = 10;
                    throw new FormatException("Incorrect enterd price value!This value setted by default: 10");
                }
                else
                {
                    _price = value;
                }
            }
        }
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (_date == null || _date == "" || _date == " " || _date == string.Empty)
                {
                    _date = "January";
                }
                else
                {
                    _date = value;
                }
            }
        }
        public double CommonSum { get; set; }
        public List<Electricity_meter> Book = new List<Electricity_meter>();
        public double Calculate_price()
        {
           double WholeElectricity = 0;
           foreach (var i in Book)
           {
               WholeElectricity +=(double) i.Value/Math.Pow(10,i.Accuracy); 
           }
           CommonSum = WholeElectricity * _price;
           return CommonSum;
       }
        public double[] Total_prices = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }
}