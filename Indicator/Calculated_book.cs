using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Indicator
{
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
        public List<Counter> Book = new List<Counter>();
        public Electricity_meter this[int x]
        {
            get
            {
                if (x < Book.Count)
                {
                    return ((Electricity_meter)Book[x]);
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
                WholeElectricity += ((Electricity_meter)item).CalcResult();
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
