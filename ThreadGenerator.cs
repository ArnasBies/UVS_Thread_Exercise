//ADO.NET sqlite provider
using Microsoft.Data.Sqlite;

namespace UVS_Thread_Exercise
{
    public class ThreadGenerator
    {
        public int ThreadID { get; set; }

        private bool shouldExecute;
        private readonly string ConnectionString = $"Data Source={Environment.CurrentDirectory}/GeneratedNumbers.sqlite";

        public ThreadGenerator(int ThreadID) {
            this.ThreadID = ThreadID;
            shouldExecute = true;

            //initializing db, creating it and the Entries table if it didn't exist before
            using(SqliteConnection db = new SqliteConnection(ConnectionString)) {
                db.Open();

                SqliteCommand command = new SqliteCommand("CREATE TABLE IF NOT EXISTS Entries (ID INTEGER PRIMARY KEY AUTOINCREMENT, ThreadID INTEGER NOT NULL, Time TEXT NOT NULL, Data TEXT NOT NULL);", db);
                command.ExecuteNonQuery();
            }
        }

        //this task is responsible for generating numbers
        private async Task generation()
        {

            Random random = new Random();
            SqliteCommand command;
            Entry generated;

            using (SqliteConnection db = new SqliteConnection(ConnectionString))
            {
                db.Open();
                

                while (shouldExecute)
                {
                    await Task.Delay(random.Next(500, 2000));

                    generated = new Entry(random.NextInt64(10000, 9999999999), ThreadID);

                    //adding extra if in order to instantly stop task after stop button was pressed instead of getting leftover generations
                    if (shouldExecute)
                    {
                        MainWindow.generated_numbers.Add(generated);

                        //not adding parameters to command because no sql injection possible as all data is safe
                        command = new($"INSERT INTO Entries (ThreadID, Time, Data) VALUES ({generated.ThreadID}, '{generated.GenerationTime.ToString("HH:mm:ss")}', '{generated.Data}');", db);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        //public tasks for turning the generator on and off
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
