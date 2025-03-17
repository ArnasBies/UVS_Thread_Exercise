using System.Windows;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace UVS_Thread_Exercise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ConcurrentBag<Entry> generated_numbers;
        int thread_count;

        public ObservableCollection<Entry> list_entries { get; set; }
        private List<ThreadGenerator> thread_generators;

        public MainWindow()
        {
            //initialization pre-logic
            InitializeComponent();
            DataContext = this;

            generated_numbers = new ConcurrentBag<Entry>() {};
            list_entries = new ObservableCollection<Entry>() { new Entry(12412, 1)};
            thread_generators = new List<ThreadGenerator>();

            //every 300 miliseconds update the observable list of entries
            Task.Run(async () => { while (true) {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        list_entries.Clear();
                        foreach (var number in generated_numbers)
                        {
                            list_entries.Add(number);
                        }
                    });

                    await Task.Delay(300);
                }
            });
        }

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

            //creating all thread generators, not awaiting because they should execute in an infinite loop because until stop is clicked
            for(int i = 0; i < thread_count; i++)
            {
                thread_generators.Add(new ThreadGenerator(i + 1));
                thread_generators[i].Run();
            }
        }

        private void B_Stop_Click(object sender, RoutedEventArgs e)
        {
            //switch buttons
            B_Stop.Visibility = Visibility.Collapsed;
            B_Start.Visibility = Visibility.Visible;
            TB_ThreadCount.Focusable = true;

            for(int i = 0; i < thread_count; i++)
            {
                thread_generators[i].Stop();
            }

            foreach(var entry in generated_numbers)
            {
                Trace.WriteLine($"{entry.ThreadID}: {entry.ID}");
            }
        }
    }
}