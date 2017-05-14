using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Indicator
{

    public class Electricity_meter : Counter
    {
        public Electricity_meter() : base()
        {
            this.MinValue = 0;
            this.MaxValue = 1000000000;
            this.Value = 0;
            this.Bit = 8;
            this.Accuracy = 3;
        }
        public Electricity_meter( int min, int max, int value, int bit, int accuracy) : base(min, max, value)
        {
            this.Bit = bit;
            this.Accuracy = accuracy;
        }
        private int _bit;
        private int _accuracy;
        public int Bit
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
                if (value < 0 || value > _bit)
                {
                    _accuracy = 1;
                    MessageBox.Show("Accuracy can't be higher then bits. Accuracy setted by default to 1", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _accuracy = value;
                }
            }
        }

        public override double CalcResult()
        {
            return (double)this.Value / Math.Pow(10, this.Accuracy);
        }

        public override List<int> Show()
        {
            List <int> result = new List<int>();
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
}
