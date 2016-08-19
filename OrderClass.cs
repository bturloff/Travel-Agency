using System;

namespace Flight_Ticket_Booking
{
    public class OrderClass
    {
        private string senderId;
        private int cardNo;
        private int receiverId;
        private int amount;
        private int unitPrice;
        private double total_charge;
        DateTime time_sent;
        DateTime time_received;
        
        //Constructor.  Sets initial values of the order
        public OrderClass(string sId, int cNo, int rId, int amt, int uPrice)
        {
            this.senderId = sId;
            this.CardNo = cNo;
            this.receiverId = rId;
            this.amount = amt;
            this.unitPrice = uPrice;
            this.time_received = DateTime.Now;
            this.time_sent = DateTime.Now;
            this.total_charge = 0.0;
        }

        public OrderClass()
        {
        }

        // Order is printed when the confirmation is sent back from the Order Processing Class
        public void printOrder()
        {
            TimeSpan time = (this.time_received - this.Time_sent);
            string interval = time.Seconds + "." + time.Milliseconds;

            Console.WriteLine(

                    "\n\n------------------------ORDER CONFIRMATION:--------------------------- \n"+
                    "        {0, -25} {1} \n"+
                    "        {2, -25} {3} \n"+ 
                    "        {4, -25} {5} \n"+
                    "        {6, -25} {7} \n"+
                    "        {8, -25} {9} \n"+
                    "        {10, -25} {11} \n"+     
                    "        {12, -25} {13:C} \n" +                                        
                    "----------------------------------------------------------------------\n\n",
                    "Travel Agent ID: ",this.senderId,
                    "Credit Card Number: ",this.CardNo,
                    "Airline ID: ",this.receiverId,
                    "Total tickets bought: ",this.amount,
                    "Ticket Price: ",this.unitPrice,
                    "Time to Order: (seconds) ",interval,
                    "Total Sale Price: ", total_charge);
        }       
        
        // Getters and Setters
        public string SenderId
        {
            get
            {
                return senderId;
            }

            set
            {
                senderId = value;
            }
        }

        public int ReceiverId
        {
            get
            {
                return receiverId;
            }

            set
            {
                receiverId = value;
            }
        }

        public int Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        public int UnitPrice
        {
            get
            {
                return unitPrice;
            }

            set
            {
                unitPrice = value;
            }
        }

        public int CardNo
        {
            get
            {
                return cardNo;
            }

            set
            {
                cardNo = value;
            }
        }

        public DateTime Time_sent
        {
            get
            {
                return time_sent;
            }

            set
            {
                time_sent = value;
            }
        }

        public DateTime Time_received
        {
            get
            {
                return time_received;
            }

            set
            {
                time_received = value;
            }
        }

        public double Total_charge
        {
            get
            {
                return total_charge;
            }

            set
            {
                total_charge = value;
            }
        }
    }
}
