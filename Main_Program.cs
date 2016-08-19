using System;
using System.Threading;

namespace Flight_Ticket_Booking
{
    // Main program that creates airline and travel agent threads
    class Main_Program
    {
        const int NUMBER_OF_AIRLINES = 3;
        const int NUMBER_OF_AGENTS = 5;

        static public MultiCellBuffer multiBuffer;
        static public TravelAgency[] agencyArray;
        static public Thread[] agencyThreads;

        static void Main(string[] args)
        {
            multiBuffer = new MultiCellBuffer();
            
            // Creates and starts the Airline threads
            Airline[] airlineArray = new Airline[NUMBER_OF_AIRLINES];
            Thread[] airlineThreads = new Thread[NUMBER_OF_AIRLINES];
            for (int i = 0; i < NUMBER_OF_AIRLINES; i++)
            {
                airlineArray[i] = new Airline(i);
                airlineThreads[i] = new Thread(new ThreadStart(airlineArray[i].airlineFunction));
                airlineThreads[i].Name = (i).ToString();
                airlineThreads[i].Start();
                Thread.Sleep(050);
            }

            Console.WriteLine("Airlines Initialized");

            // Creates and starts agency threads
            agencyArray = new TravelAgency[NUMBER_OF_AGENTS];
            agencyThreads = new Thread[NUMBER_OF_AGENTS];
            for (int i = 0; i < NUMBER_OF_AGENTS; i++)
            {
                agencyArray[i] = new TravelAgency();
                Airline.priceCut += new priceCutEvent(agencyArray[i].ticketOnSale);
                agencyThreads[i] = new Thread(new ThreadStart(agencyArray[i].agencyFunction));
                agencyThreads[i].Name = (i).ToString();
                agencyThreads[i].Start();   
            }

            Console.WriteLine("Agencies Initialized");

            // Wait for airlines threads to complete then alert travel agency threads to quit
            for (int i = 0; i < airlineThreads.Length; i++)
            {
                airlineThreads[i].Join();
            }
            TravelAgency.airlines_done = true;
            
            Console.WriteLine("...................................main thread complete");
        }
        

    }
}


