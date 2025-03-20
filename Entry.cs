namespace UVS_Thread_Exercise
{
    public class Entry
    {
        public long Data { get; private set; }
        public int ThreadID { get; private set; }
        public DateTime GenerationTime { get; private set; }   
        
        public Entry(long Data, int thread_ID)
        {
            this.Data = Data;
            this.ThreadID = thread_ID;
            GenerationTime = DateTime.Now;
        }
    }
}
