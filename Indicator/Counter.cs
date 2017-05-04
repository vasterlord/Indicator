using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            this.MinValue = 0;
            this.MaxValue = 5000;
            this.Value = 0;
        }

        public Counter(int max, int min, double value)
        {
            this.MinValue = min;
            this.MaxValue = max;
            this.Value = value;
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
                    MessageBox.Show("Bits can't be less then accuracy. Bits setted by default to 8", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    _minValue = 0;
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
                    MessageBox.Show("The value must be in  the correct  interval!", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                }
                else if (value < 0)
                {
                    value = 0;
                    MessageBox.Show("The value must be non-negative and integer value!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    this._value = value;
                }
            }
        }
        public virtual List<int> Show()
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

        public static Counter operator ++(Counter counter)
        {
            counter.Value++;
            return counter;
        }
        public static Counter operator --(Counter counter)
        {
            counter.Value--;
            return counter;
        }
        public static Counter operator +(Counter counter, int asc)
        {
            counter.Value = counter.Value + asc;
            return counter;
        }
        public static Counter operator -(Counter counter, int desc)
        {
            counter.Value = counter.Value - desc;
            return counter;
        }
    } 

    public class Electricity_meter : Counter
    {
        public Electricity_meter() : base()
        {
            this.MinValue = 0;
            this.MaxValue = 5000;
            this.Value = 0;
            this.Bit = 8;
            this.Accuracy = 3;
        }
        public Electricity_meter(int max, int min ,double value, int accuracy, int bit ):base(max,min,value)
        {
            this.Bit = bit;
            this.Accuracy = accuracy;
        }
        private int _bit;
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
                    MessageBox.Show("Maximum value of bit must be less or equal 8. Bits setted by default to 8", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (value < _accuracy)
                {
                    _bit = 8;
                    MessageBox.Show("Bits can't be less then accuracy. Bits setted by default to 8", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                   _bit = value; 
                }
            }
        }
        public int Accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                if (value < 0 || value >= _bit)
                {
                    _accuracy = 1;
                    MessageBox.Show("Accuracy can't be higher then bits. Accurace setted by default to 1", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _accuracy = value;
                }
            }
        }

        public double CalcResult()
        {
            return (double)this.Value / Math.Pow(10, this.Accuracy);
        }

        public override List<int> Show()
        {
            return base.Show();
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
        public static Electricity_meter operator +(Electricity_meter counter, int asc)
        {
            counter.Value = counter.Value + asc;
            return counter;
        }
        public static Electricity_meter operator -(Electricity_meter counter, int desc)
        {
            counter.Value = counter.Value - desc;
            return counter;
        }
    }

    public class Calculated_book
    { 
        public Calculated_book()
        {
            this._price = 10;
            this.Date = "January";
            Book = new List<Counter>(); 
        }
        public Calculated_book(double price, string date)
        {
            this._price = price;
            Date = date; 
        }
        private double _price;
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
                    MessageBox.Show("Price can't be less or equal 0. Price setted by default to 10", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
        public List<Counter> Book { get; set; }
        public Electricity_meter this[int x]
        {
            get
            {
                if (x < Book.Count)
                {
                    return Book[x] as Electricity_meter;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Book[x] = value;
            }     
        }
        public Point Calculate_price()
        { 
           double WholeElectricity = 0;
           foreach (Counter item in Book)
           {
                WholeElectricity += (item as Electricity_meter).CalcResult();
           }
           double CommonSum = WholeElectricity * _price;
           Point result = new Point(CommonSum, WholeElectricity);
           return result;
        }
        public Point[] Total_prices = new Point[12] { new Point(0,0), new Point(0, 0), new Point(0, 0),
            new Point(0,0), new Point(0,0), new Point(0,0), new Point(0,0), new Point(0,0), new Point(0,0),
            new Point(0,0), new Point(0,0), new Point(0,0) };
    }
}