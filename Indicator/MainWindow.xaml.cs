using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<Label> showL;
        public List<Grid> showG;
        public int idCounter;
        public MainWindow()
        {
            InitializeComponent();
            idCounter = 0;
            showL = new List<Label>() { n1, n2, n3, n4, n5, n6, n7, n8 };
            showG = new List<Grid>() { g1, g2, g3, g4, g5, g6, g7, g8 };
        }

        public  void ShowIntoEkran(Calculated_book CBook)
        {
            List<int> AllResult = CBook[idCounter].Show();
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
            for (int i = 0; i < CBook[idCounter].Accuracy; i++)
            {
                showG[i].Background = (Brush)bc.ConvertFrom("#FFAC0000");
            }
            for (int i = CBook[idCounter].Accuracy; i < showG.Count; i++)
            {
                showG[i].Background = (Brush)bc.ConvertFrom("#FF746060");
            }
            showG.Reverse();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                double CounterValue = Convert.ToDouble(tb_value_of_new_Counter.Text);
                CBook.Book.Add(new Electricity_meter(Convert.ToInt32(l_max.Content), 0, CounterValue, Convert.ToInt32(l_accuracy.Text), Convert.ToInt32(l_bit1.Text)));
                l_bit1.Text = CBook[idCounter].Bit.ToString();
                l_accuracy.Text = CBook[idCounter].Accuracy.ToString();
                tb_value_of_new_Counter.Text = CBook[idCounter].Value.ToString();
                ShowIntoEkran(CBook);
                DesignCounter();
                CBook.Price = Convert.ToDouble(tb_price.Text);
                CBook.Date = Calendar.Text;
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
                dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
                dataGrid.Items.Refresh();
            }
            catch (Exception) { }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (l_max != null)
                l_max.Content = Convert.ToInt32(slider.Value).ToString();
                CBook.Book[idCounter].MaxValue = Convert.ToInt32(l_max.Content);
                dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
                dataGrid.Items.Refresh();
            }
            catch (Exception) { }
        }

        private void b_up_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CheckCalculatedBook())
            {
                MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CBook[idCounter]--;
            ShowIntoEkran(CBook);
            CBook.Price = Convert.ToDouble(tb_price.Text);
            CBook.Date = Calendar.Text;
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
            tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
            dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
            dataGrid.Items.Refresh();
        }

        private void b_down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CheckCalculatedBook())
            {
                MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CBook[idCounter]++;
            ShowIntoEkran(CBook);
            CBook.Price = Convert.ToDouble(tb_price.Text);
            CBook.Date = Calendar.Text;
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
            tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
            dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
            dataGrid.Items.Refresh();
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
            catch (Exception) { }
        }

        private void DesignCounter()
        {
            int MakeInvalid = showL.Count - CBook[idCounter].Bit;
            showL.Reverse();
            for (int i = CBook[idCounter].Bit; i < showL.Count; i++)
            {
                showL[i].Visibility = Visibility.Collapsed;
            }
            for (int i = 0; i < CBook[idCounter].Bit; i++)
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
                CBook[idCounter].Bit = Convert.ToInt32(l_bit1.Text);
                l_bit1.Text = CBook[idCounter].Bit.ToString();
                DesignCounter();
                slider.Maximum = Math.Pow(10, CBook[idCounter].Bit);
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
                dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
                dataGrid.Items.Refresh();
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
                CBook[idCounter] += Convert.ToInt32(tb_Order.Text);
                ShowIntoEkran(CBook);
                CBook.Price = Convert.ToDouble(tb_price.Text);
                CBook.Date = Calendar.Text;
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y); 
                dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
                dataGrid.Items.Refresh();
            }
            catch (FormatException fx)
            {
                MessageBox.Show(fx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tb_Order.Text = "10";
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
                CBook[idCounter] -= Convert.ToInt32(tb_Order.Text);
                ShowIntoEkran(CBook);
                CBook.Price = Convert.ToDouble(tb_price.Text);
                CBook.Date = Calendar.Text;
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
                dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
                dataGrid.Items.Refresh();
            }
            catch (FormatException fx)
            {
                MessageBox.Show(fx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tb_Order.Text = "10";
            }
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            if (CheckCalculatedBook())
            {
                MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CBook[idCounter].Value = 0;
            ShowIntoEkran(CBook);
            tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
            tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
            tb_value_of_new_Counter.Text = "0";
            DesignCounter();
            dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
            dataGrid.Items.Refresh();
        }

        private void l_accuracy_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (CheckCalculatedBook())
                {
                    return;
                }
                CBook[idCounter].Accuracy = Convert.ToInt32(l_accuracy.Text);
                l_accuracy.Text = CBook[idCounter].Accuracy.ToString();
                showG.Reverse();
                var bc = new BrushConverter();
                for (int i = 0; i < CBook[idCounter].Accuracy; i++)
                {
                    showG[i].Background = (Brush)bc.ConvertFrom("#FFAC0000");
                }
                for (int i = CBook[idCounter].Accuracy; i < showG.Count; i++)
                {
                    showG[i].Background = (Brush)bc.ConvertFrom("#FF746060");
                }
                showG.Reverse();
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
                DesignCounter();
                dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
                dataGrid.Items.Refresh();
            }
            catch (Exception) { }
        }

        private void tb_value_of_new_Counter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (CheckCalculatedBook())
                {
                    return;
                }
                CBook[idCounter].Value = Convert.ToInt32(tb_value_of_new_Counter.Text);
                l_bit1.Text = CBook[idCounter].Bit.ToString();
                ShowIntoEkran(CBook);
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
                DesignCounter();
                dataGrid.ItemsSource = CBook.Book.Cast<Electricity_meter>();
                dataGrid.Items.Refresh();
            }
            catch (Exception) { }
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Electricity_meter tempCounter = (Electricity_meter)dataGrid.SelectedItem;
                idCounter = CBook.Book.IndexOf(tempCounter);
                l_bit1.Text = CBook[idCounter].Bit.ToString();
                l_accuracy.Text = CBook[idCounter].Accuracy.ToString();
                tb_value_of_new_Counter.Text = CBook[idCounter].Value.ToString();
                showG.Reverse();
                var bc = new BrushConverter();
                for (int i = 0; i < CBook[idCounter].Accuracy; i++)
                {
                    showG[i].Background = (Brush)bc.ConvertFrom("#FFAC0000");
                }
                for (int i = CBook[idCounter].Accuracy; i < showG.Count; i++)
                {
                    showG[i].Background = (Brush)bc.ConvertFrom("#FF746060");
                }
                showG.Reverse();
                tb_all_price_of_electrisity.Text = Convert.ToString(CBook.Calculate_price().X);
                tb_all_electrisity.Text = Convert.ToString(CBook.Calculate_price().Y);
                ShowIntoEkran(CBook);
                DesignCounter();
            }
            catch (Exception) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckCalculatedBook())
                {
                    MessageBox.Show("We don't have electricity mater! Please, add at least one more!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                CBook.Book.Remove(CBook[idCounter]);
                dataGrid.ItemsSource = CBook.Book;
                dataGrid.Items.Refresh();
                dataGrid.SelectedIndex = 0;
            }
            catch (Exception) { }
        }
    }
}
