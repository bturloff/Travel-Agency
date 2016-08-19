using System;

namespace Flight_Ticket_Booking
{
    public delegate void order_confirmation_event(OrderClass order);
    class OrderProcessing
    {
        OrderClass order;
        public OrderProcessing(OrderClass order)
        {
            this.order = order;
        }

        public void processOrder()
        {
            bool valid_card = false;
            double total_charge;

            int card_number = order.CardNo;
            valid_card = isValidCardNo(card_number);
            total_charge = calcTotal();
            order.Total_charge = total_charge;
            
            // send confirmation of order to travel agent
            if (valid_card)
            {
                int travel_agent_id = Convert.ToInt32(order.SenderId);
                TravelAgency destination_agency = Main_Program.agencyArray[travel_agent_id];

                // Ensures that only one processing thread can send in a conformed order
                // at one time
                destination_agency.order_event.WaitOne();
                destination_agency.order_confirmation = order;
            }
        }

        // Determines that there is a valid credit card number being used for the order
        private bool isValidCardNo(int number)
        {
            bool isValid = false;
            if (number >= 1000 && number <= 9999)
                isValid = true;
            return isValid;
        }

        // gets the total charge for the order
        private double calcTotal()
        {
            int num_to_order = order.Amount;
            int price = order.UnitPrice;
            return num_to_order * price * 1.095;


        }
    }
}
