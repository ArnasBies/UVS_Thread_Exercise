using System.Windows;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using Microsoft.Data.Sqlite;

namespace UVS_Thread_Exercise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random;
        int thread_count;
        ConcurrentBag<Entry> generated_numbers;

        public MainWindow()
        {
            //initialization pre-logic
            random = new();
            generated_numbers = new ConcurrentBag<Entry>() {new Entry(generate_number(), 1)};
            InitializeComponent();
        }

        //returns a 5-10 digit random number
        public long generate_number() => random.NextInt64(10000, 9999999999);

        //event handlers
        private void B_Start_Click(object sender, RoutedEventArgs e)
        {
            //switch buttons
            B_Stop.Visibility = Visibility.Visible;
            B_Start.Visibility = Visibility.Collapsed;
            TB_ThreadCount.Focusable = false;

            //check validity of thread count
            if (!int.TryParse(TB_ThreadCount.Text, out thread_count) || thread_count < 2)
            {
                TB_ThreadCount.Text = "2";
                thread_count = 2;
            } else if (thread_count > 15)
            {
                thread_count = 15;
                TB_ThreadCount.Text = "15";
            }
        }

        private void B_Stop_Click(object sender, RoutedEventArgs e)
        {
            //switch buttons
            B_Stop.Visibility = Visibility.Collapsed;
            B_Start.Visibility = Visibility.Visible;
            TB_ThreadCount.Focusable = true;
        }
    }
}