using System;
using System.Threading;

namespace Flight_Ticket_Booking
{
    // Listens for pricecut events, orders tickets, and prints order confirmations
    class TravelAgency
    {
        // Order_confirmation is instantiated when Order Processsing
        // validates the order
        public OrderClass order_confirmation = null;    
                                         
        //    order_event is used in case multiple OrderProcessing threads
        //    are trying to confirm an order for the same Agency.           
        public AutoResetEvent order_event = new AutoResetEvent(true);

        static Random rand = new Random();

        // order_ready is set to true when the pricecut handler is executed
        // so the agency thread can start filling the order
        bool order_ready = false;

        // airliines_done is set to true when the airlines threads are completed in 
        // Main()
        static public bool airlines_done = false;

        int from_airline_index;
        int incoming_new_price;
        private OrderClass newOrder;
        int total_tickets_purchased = 0;


        // Travel agency thread method
        public void agencyFunction()
        {
            // Keep looping until the ariline threads are done
            while (!airlines_done || order_ready == true)
            {
                order_event.Set();   // Makes sure only one order processing thread can send a confirmation during one loop

                // Creates, encodes, and sends an order
                if (order_ready)
                {
                    // create the order
                    string encodedOrder;                    
                    int qty = numToOrder();
                    int cardnumber = rand.Next(1000, 9999);
                    newOrder = new OrderClass(Thread.CurrentThread.Name, cardnumber, from_airline_index, qty, incoming_new_price);
                    newOrder.Time_sent = DateTime.Now;

                    // Encode the order
                    encodedOrder = Coding.encode(newOrder);
                    
                    // Send the order to the buffer
                    Main_Program.multiBuffer.setOneCell(encodedOrder);

                    newOrder = null;                    
                    order_ready = false;
                }

                // Check to see if there is an order confirmation from OrderProcessing
                if (order_confirmation != null)
                {
                    order_confirmation.Time_received = DateTime.Now;
                    order_confirmation.printOrder();
                    updatePurchaseTotal(order_confirmation.Amount);
                    order_confirmation = null;
                }
            }
        }

        // price cut event handler.  sets fields to prepare for a new order placement.
        // order_ready flag is set to to so the agency thread knows when to create an order
        public void ticketOnSale(int salePrice, int airline_index)
        {
            from_airline_index = airline_index;
            incoming_new_price = salePrice;
            order_ready = true;
        }

        // determines the number of tickets to order
        private int numToOrder()
        {
            int qty_to_order;

            if (incoming_new_price < 200)
                qty_to_order = 100 * 300 / (total_tickets_purchased + 300);
            else if (incoming_new_price < 400)
            {
                qty_to_order = 50 * 300 / (total_tickets_purchased + 300);
            }
            else if (incoming_new_price < 600)
            {
                qty_to_order = 25 * 300 / (total_tickets_purchased + 300);
            }
            else if (incoming_new_price < 800)
            {
                qty_to_order = 15 * 300 / (total_tickets_purchased + 300);
            }
            else
                qty_to_order = 5 * 300 / (total_tickets_purchased + 300);

            return qty_to_order;
        }

        private void updatePurchaseTotal(int amount)
        {
            total_tickets_purchased += amount;
        }

    }
}
