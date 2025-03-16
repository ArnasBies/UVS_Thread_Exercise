using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVS_Thread_Exercise
{
    public class Entry
    {
        long ID;
        int thread_ID;
        DateTime generation_time;
        
        public Entry(long ID, int thread_ID)
        {
            this.ID = ID;
            this.thread_ID = thread_ID;
            generation_time = DateTime.Now;
        }
    }
}
