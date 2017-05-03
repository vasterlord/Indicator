using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            List<int> AllResult = CBook[CBook.Book.Count - 1].Show();
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
            for (int i = 0; i < CBook[CBook.Book.Count - 1].Accuracy; i++)
            {
                showG[i].Background = (Brush)bc.ConvertFrom("#FFAC0000");
            }
            for (int i = CBook[CBook.Book.Count - 1].Accuracy; i < showG.Count; i++)
            {
                showG[i].Background = (Brush)bc.ConvertFrom("#FF746060");
            }
            showG.Reverse();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            { 
            double CounterValue =  Convert.ToDouble(tb_value_of_new_Counter.Text);
            CBook.Book.Add(new Electricity_meter(Convert.ToInt32(l_max.Content), 0,  CounterValue, Convert.ToInt32(l_accuracy.Text), Convert.ToInt32(l_bit1.Text)));
            l_bit1.Text = CBook.Book[CBook.Book.Count - 1].Bit.ToString();
            l_accuracy.Text = CBook[CBook.Book.Count - 1].Accuracy.ToString();
            tb_value_of_new_Counter.Text = CBook[CBook.Book.Count - 1].Value.ToString();
            ShowIntoEkran(CBook);
            DesignCounter();
            CBook.Price = Convert.ToDouble(tb_price.Text);
            CBook.Date = Calendar.Text;
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X); 
            tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y); 
            }
            catch (FormatException fx)
            {
                MessageBox.Show(fx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (l_max != null)
                l_max.Content = Convert.ToInt32(slider.Value).ToString();
        }

        private void b_up_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CheckCalculatedBook())
            {
                MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CBook.Book[CBook.Book.Count - 1]--;
            ShowIntoEkran(CBook);
            CBook.Price = Convert.ToDouble(tb_price.Text);
            CBook.Date = Calendar.Text;
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
            tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
        }

        private void b_down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CheckCalculatedBook())
            {
                MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CBook[CBook.Book.Count - 1]++;
            ShowIntoEkran(CBook);
            CBook.Price = Convert.ToDouble(tb_price.Text);
            CBook.Date = Calendar.Text;
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
            tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
        }

        private void tb_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CBook.Price = Convert.ToDouble(tb_price.Text);
                Point pointResult = new Point();
                pointResult = CBook.Calculate_price();
                tb_all_price_of_electrisity.Text = pointResult.X.ToString();
                tb_all_electrisity.Text = Convert.ToString(pointResult.Y);
            }
            catch (FormatException fex)
            {
                MessageBox.Show(fex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NullReferenceException) { }
            catch (Exception) { }
        }

        private void DesignCounter()
        {
            int MakeInvalid = showL.Count - CBook[CBook.Book.Count - 1].Bit;
            showL.Reverse();
            for (int i = CBook[CBook.Book.Count - 1].Bit; i < showL.Count; i++)
            {
                showL[i].Visibility = Visibility.Collapsed;
            }
            for (int i = 0; i < CBook[CBook.Book.Count - 1].Bit; i++)
            {
                showL[i].Visibility = Visibility.Visible;
            }
            showL.Reverse();
        }
        
        private void Calendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckCalculatedBook())
            {
                return;
            }
            if (CBook.Total_prices[Calendar.SelectedIndex] == new Point(0,0))
            {
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
            }
            else
            {
                tb_all_price_of_electrisity.Text = CBook.Total_prices[Calendar.SelectedIndex].X.ToString();
                tb_all_electrisity.Text = CBook.Total_prices[Calendar.SelectedIndex].Y.ToString();
            }
        }

        private void btn_Safe_total_price_Click(object sender, RoutedEventArgs e)
        {
            if (CheckCalculatedBook())
            {
                MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CBook.Total_prices[Calendar.SelectedIndex] = new Point(Convert.ToDouble(tb_all_price_of_electrisity.Text), Convert.ToDouble(tb_all_electrisity.Text));
        }

        private void l_bit1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (l_bit1.Text == string.Empty || l_bit1.Text == "0" || l_bit1.Text == "" || l_bit1.Text == " ")
                {
                    slider.Maximum = Math.Pow(10, 8);
                }
                else
                {
                    slider.Maximum = Math.Pow(10, Convert.ToDouble(l_bit1.Text));
                }

                if (CheckCalculatedBook())
                {
                    return;
                }
                CBook.Book[CBook.Book.Count - 1].Bit = Convert.ToInt32(l_bit1.Text);
                l_bit1.Text = CBook.Book[CBook.Book.Count - 1].Bit.ToString();
                DesignCounter();
                slider.Maximum = Math.Pow(10, CBook[CBook.Book.Count - 1].Bit);
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
            }
            catch (Exception) { }
        } 

        public bool CheckCalculatedBook()
        {
            if (CBook.Book.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void b_add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (CheckCalculatedBook())
            {
                MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CBook[CBook.Book.Count - 1] += Convert.ToInt32(tb_Order.Text);
            ShowIntoEkran(CBook);
            CBook.Price = Convert.ToDouble(tb_price.Text);
            CBook.Date = Calendar.Text;
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
            tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
            }
            catch (FormatException fx)
            {
                MessageBox.Show(fx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void b_minus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (CheckCalculatedBook())
                {
                    MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                CBook[CBook.Book.Count - 1] -= Convert.ToInt32(tb_Order.Text);
                ShowIntoEkran(CBook);
                CBook.Price = Convert.ToDouble(tb_price.Text);
                CBook.Date = Calendar.Text;
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
            }
            catch (FormatException fx)
            {
                MessageBox.Show(fx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            if (CheckCalculatedBook())
            {
                MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CBook[CBook.Book.Count - 1].Value = 0;
            ShowIntoEkran(CBook);
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
            tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
            DesignCounter();
        }

        private void l_accuracy_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (CheckCalculatedBook())
                {
                    return;
                }
                CBook[CBook.Book.Count - 1].Accuracy = Convert.ToInt32(l_accuracy.Text);
                l_accuracy.Text = CBook[CBook.Book.Count - 1].Accuracy.ToString();
                showG.Reverse();
                var bc = new BrushConverter();
                for (int i = 0; i < CBook[CBook.Book.Count - 1].Accuracy; i++)
                {
                    showG[i].Background = (Brush)bc.ConvertFrom("#FFAC0000");
                }
                for (int i = CBook[CBook.Book.Count - 1].Accuracy; i < showG.Count; i++)
                {
                    showG[i].Background = (Brush)bc.ConvertFrom("#FF746060");
                }
                showG.Reverse();
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
                DesignCounter();
            }
            catch (FormatException fx)
            {
                MessageBox.Show(fx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                l_accuracy.Text = CBook[CBook.Book.Count - 1].Accuracy.ToString();
            }
            catch (NullReferenceException) { }
            catch (Exception ex) { }
        }

        private void tb_value_of_new_Counter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (CheckCalculatedBook())
                {
                    return;
                }
                CBook[CBook.Book.Count - 1].Value = Convert.ToInt32(tb_value_of_new_Counter.Text);
                l_bit1.Text = CBook.Book[CBook.Book.Count - 1].Bit.ToString();
                ShowIntoEkran(CBook);
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
                DesignCounter();
            }
            catch (FormatException fx)
            {
                MessageBox.Show(fx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NullReferenceException) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
