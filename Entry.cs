namespace UVS_Thread_Exercise
{
    public class Entry
    {
        public long ID { get; private set; }
        public int ThreadID { get; private set; }
        public DateTime GenerationTime { get; private set; }   
        
        public Entry(long ID, int thread_ID)
        {
            this.ID = ID;
            this.ThreadID = thread_ID;
            GenerationTime = DateTime.Now;
        }
    }
}
