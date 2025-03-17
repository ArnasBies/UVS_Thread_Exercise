using System;
using System.Diagnostics;

namespace UVS_Thread_Exercise
{
    public class ThreadGenerator
    {
        public int ThreadID { get; set; }
        bool shouldExecute;

        public ThreadGenerator(int ThreadID) {
            this.ThreadID = ThreadID;
            shouldExecute = true;
            Trace.WriteLine($"Created thread {ThreadID}");
        }

        private async Task generation()
        {
            Random random = new Random();

            while (shouldExecute)
            {
                MainWindow.generated_numbers.Add(new Entry(random.NextInt64(10000, 9999999999), ThreadID));
                Trace.WriteLine($"Added a number in thread {ThreadID}");
                await Task.Delay(random.Next(500, 2000));
            }

            Trace.WriteLine($"Finished executing thread {ThreadID}");
        }

        public async Task Run()
        {
            shouldExecute = true;
            await generation();
        }

        public void Stop()
        {
            shouldExecute = false;
        }
    }
}
