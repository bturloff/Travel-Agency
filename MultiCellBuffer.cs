using System;
using System.Threading;

namespace Flight_Ticket_Booking
{
    class MultiCellBuffer
    {
        static public int BUFFER_LENGTH = 3;

        static public Semaphore sema;
        static ReaderWriterLock rwlock = new ReaderWriterLock();       
        public int numInBuffer = 0;
        private string[] buffer;
        
        // Returns true is there are no orders in buffer
        public bool isEmpty()
        {
            if (numInBuffer <= 0)
                return true;
            else
                return false;
        }        

        // Constructor.  Sets each cell to null.
        // Sets semaphore to 3.
        public MultiCellBuffer()
        {
            sema = new Semaphore(0, 3);
            sema.Release(3);
            buffer =  new string[BUFFER_LENGTH];
            for ( int x = 0; x < buffer.Length; x++)
            {
                buffer[x] = null;
            }               
        }

        // Called from travel agency
        public void setOneCell(string order)
        {
            sema.WaitOne();         
            rwlock.AcquireWriterLock(300);

            // cycle through each cell until an empty cell is reached and fill
            // it with the order
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == null)
                {

                    buffer[i] = order;
                    numInBuffer++;
                    break;
                }
            }
            rwlock.ReleaseWriterLock();
        }

        // Called from Airline
        public string getOneCell()
        {
            string cellToDeliver = "";
            rwlock.AcquireReaderLock(300);

            if (numInBuffer > 0)
            {
                OrderClass order_from_buffer;

                // Cycle through each cell, checking if the Order Airline ID matches the 
                // callers Airline ID
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] != null)
                    {
                        order_from_buffer = Coding.decode(buffer[i]);
                        if (order_from_buffer.ReceiverId == Convert.ToInt32(Thread.CurrentThread.Name))
                        {                            
                            cellToDeliver = buffer[i];
                            buffer[i] = null;
                            numInBuffer--;
                            sema.Release();
                            break;
                        }
                    }
                }
            }
            rwlock.ReleaseLock();
            return cellToDeliver;    
        }
    }
}
