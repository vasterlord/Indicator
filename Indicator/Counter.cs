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
    public class Counter
    {
        public Counter()
        {
            this.MinValue = 0;
            this.MaxValue = 1000000000;
            this.Value = 0;
        }

        public Counter( int min, int max, int value)
        {
            this.MinValue = min;
            this.MaxValue = max;
            this.Value = value;
        } 

        private int _maxValue;
        private int _minValue;
        private int _value;
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (value <= 0)
                {
                    _maxValue = 1000000000;
                    MessageBox.Show("Maximum value can't be less or equal 0", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _maxValue = value;
                }
            }
        }
        public int MinValue
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
                    MessageBox.Show("Minimum value can't be less then 0", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _minValue = value;
                }
            }
        }
        public int Value
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
                    this.MinValue = 0;
                    MessageBox.Show("The value must be in  the correct  interval!", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                }
                else if (value < 0)
                {
                    value = 0;
                    this.MinValue = 0;
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
}