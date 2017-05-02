using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Indicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Calculated_book CBook = new Calculated_book();
        public static List<Label> showL;
        public static List<Grid> showG;
        public MainWindow()
        {
            InitializeComponent();
            showL = new List<Label>() { n1, n2, n3, n4, n5, n6, n7, n8 };
            showG = new List<Grid>() { g1, g2, g3, g4, g5, g6, g7, g8 };
        }

        public static void ShowIntoEkran(Calculated_book CBook)
        {
            List<int> AllResult = CBook.Book[CBook.Book.Count - 1].Show();
            int count = 0;
            showL.Reverse();
            AllResult.Reverse();
            foreach (var i in AllResult)
            {
                showL[count].Content = i.ToString();
                count++;
            }
            do
            {
                showL[count].Content = 0;
                count++;
            } while (count < showL.Count);

            showL.Reverse();
            showG.Reverse();
            var bc = new BrushConverter();
            for (int i = 0; i < CBook.Book[CBook.Book.Count - 1].accuracy; i++)
            {
                showG[i].Background = (Brush)bc.ConvertFrom("#FFAC0000");
            }
            for (int i = CBook.Book[CBook.Book.Count - 1].accuracy; i < showG.Count; i++)
            {
                showG[i].Background = (Brush)bc.ConvertFrom("#FF746060");
            }
            showG.Reverse();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double CounterValue =  Convert.ToDouble(tb_value_of_new_Counter.Text);
          
            CBook.Book.Add(new Electricity_meter(Convert.ToInt32(l_max.Content), Convert.ToInt32(l_accuracy.Text), CounterValue));

            ShowIntoEkran(CBook);

            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price(Convert.ToDouble(tb_price.Text)));
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (l_max != null)
                l_max.Content = Convert.ToInt32(slider.Value).ToString();
        }

        private void b_up_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CBook.Book[CBook.Book.Count - 1]--;
            ShowIntoEkran(CBook);
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price(Convert.ToDouble(tb_price.Text)));
        }

        private void b_down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CBook.Book[CBook.Book.Count - 1]++;
            ShowIntoEkran(CBook);
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price(Convert.ToDouble(tb_price.Text)));
        }

        private void tb_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price(Convert.ToDouble(tb_price.Text)));
        }

        private void Calendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_all_price_of_electrisity.Text = CBook.Total_prices[Calendar.SelectedIndex].ToString();
        }

        private void btn_Safe_total_price_Click(object sender, RoutedEventArgs e)
        {
            CBook.Total_prices[Calendar.SelectedIndex] = Math.Round(( Convert.ToDouble(tb_all_price_of_electrisity.Text)),2);
        }

        private void l_bit1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
                int bit = Convert.ToInt32(l_bit1.Text);
            
                int MakeInvalid = showL.Count - bit;
                showL.Reverse();
                for (int i = bit; i < showL.Count; i++)
                {
                    showL[i].Visibility = Visibility.Collapsed;
                }
                for (int i = 0; i < bit; i++)
                {
                    showL[i].Visibility = Visibility.Visible;
                }
                showL.Reverse();
                slider.Maximum = Math.Pow(10, bit);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int accuracy = Convert.ToInt32(l_accuracy.Text); 
            showG.Reverse();
            var bc = new BrushConverter();

            for (int i = 0; i < accuracy; i++)
            {
                //
                showG[i].Background = (Brush)bc.ConvertFrom("#FFAC0000");
            }
            for (int i = accuracy; i < showG.Count; i++)
            {
                showG[i].Background = (Brush)bc.ConvertFrom("#FF746060");
            }
            showG.Reverse();
        }
    }
}
