using System;
using System.Threading;

namespace Flight_Ticket_Booking
{    
    public delegate void priceCutEvent(int price, int airlineThread);

    // Simulates an airline
    class Airline 
    {        
        static public event priceCutEvent priceCut;
        static Random rng = new Random();

        private int currentPrice;
        private int previousPrice;
        private int p;              // number of price drops
        int airline_index;          // Unique airline identifier

        // Airline constructor
        public Airline(int airline_index)
        {
            previousPrice = 900;
            CurrentPrice = 900;
            this.airline_index = airline_index;
        }

        // Getters and setters for 
        public int CurrentPrice
        {
            get
            {
                Console.WriteLine("got currentPrice");
                return currentPrice;
            }

            set
            {
                currentPrice = value;
            }
        }               

        // Main thread function for Airline
        public void airlineFunction()
        {
            string orderString;
            int newPrice;
            Thread.Sleep(500);

            // loop until 20 pricecuts have been made
            while(p < 20)
            {                
                Thread.Sleep(100);
                newPrice = pricingModel();
                Console.WriteLine("Airline {0} New price is {1}", Thread.CurrentThread.Name, newPrice);
                
                // Emit pricecut event
                if (priceCut != null)
                {
                    if (newPrice < previousPrice)
                    {
                        Console.WriteLine("Ticket on sale (from Airline {0})", airline_index);
                        p++;
                        priceCut(newPrice, airline_index);
                    }                    
                }
                previousPrice = newPrice;

                // get an order from the buffer
                orderString = Main_Program.multiBuffer.getOneCell();
                
                // process order if it is a valid order for this airline in the buffer
                if(orderString != "")
                {
                    OrderClass order = Coding.decode(orderString);
                    OrderProcessing processing_instance = new  OrderProcessing(order);
                    Thread order_thread = new Thread(new ThreadStart( processing_instance.processOrder));
                    order_thread.Start();
                }                                    
            }

            // Get the remaining orders left in the buffer
            Thread.Sleep(500);
            while (!Main_Program.multiBuffer.isEmpty())
            {
                orderString = Main_Program.multiBuffer.getOneCell();
                if (orderString != "")
                {
                    OrderClass order = Coding.decode(orderString);
                    OrderProcessing processing_instance = new OrderProcessing(order);
                    Thread order_thread = new Thread(new ThreadStart(processing_instance.processOrder));
                    order_thread.Start();                    
                }
            }
            Thread.Sleep(500);
        }

        // Pricing Model
        private int pricingModel()
        {
            return rng.Next(100, 900);
        }
    }

}
