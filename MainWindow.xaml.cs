using System.Windows;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace UVS_Thread_Exercise
{
    public partial class MainWindow : Window
    {
        //using concurrent bag because I needed a thread-safe collection
        public static ConcurrentBag<Entry> generated_numbers;
        public ObservableCollection<Entry> list_entries { get; set; }

        private int thread_count;
        private List<ThreadGenerator> thread_generators;
        private bool checking;

        public MainWindow()
        {
            //initialization logic
            InitializeComponent();
            DataContext = this;

            generated_numbers = new ConcurrentBag<Entry>() {};
            list_entries = new ObservableCollection<Entry>() {};
            thread_generators = new List<ThreadGenerator>();
            checking = false;

            //db is created in ThreadGenerator.cs and is created in executable directory
            TB_DbPath.Text = $@"{Environment.CurrentDirectory}\GeneratedNumbers.sqlite";
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

            //every 300 miliseconds update the observable list of entries up to 20 elements while checking is turned on
            checking = true;
            Task.Run(async () => { while (checking) {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        list_entries.Clear();
                        foreach (var number in generated_numbers)
                        {
                            if (list_entries.Count < 20)
                                list_entries.Add(number);
                            else
                                break;
                        }
                    });

                    await Task.Delay(300);
                }
            });

            //creating all thread number generators, not awaiting because they should execute in an infinite loop until stop is clicked
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

            //turning off checker task
            checking = false;

            //turning off all of the thread number generators
            for(int i = 0; i < thread_count; i++)
            {
                thread_generators[i].Stop();
            }
        }
    }
}