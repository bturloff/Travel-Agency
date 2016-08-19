

namespace Flight_Ticket_Booking
{
    // Encodes and Decodes Orders
    class Coding
    {
        // Convert order to a string
        static public string encode(OrderClass order)
        {
            string encodedOrder = "";

            encodedOrder += "       Ordered from Agency Thread: " + order.SenderId + "\n" +
                            "       Received by Airline Thread: " + order.ReceiverId + "\n" +
                            "       Credit Card Number: " + order.CardNo + "\n" +
                            "       Number of Tickets Ordered: " + order.Amount + "\n" +
                            "       Ticket Price: " + order.UnitPrice + "\n" +
                            "       Time sent: " + order.Time_sent + "\n";

            return encodedOrder;
        }

        // Convert string into an OrderClass Object
        static public OrderClass decode(string orderString)
        {
            OrderClass decodedOrder = new OrderClass();
            string[] lines = orderString.Split('\n');
            
            decodedOrder.SenderId = lines[0].Substring(lines[0].IndexOf(":") + 2);
            decodedOrder.ReceiverId = System.Convert.ToInt32(lines[1].Substring(lines[1].IndexOf(":") + 2));
            decodedOrder.CardNo = System.Convert.ToInt32( lines[2].Substring(lines[2].IndexOf(":") + 2));
            decodedOrder.Amount = System.Convert.ToInt32(lines[3].Substring(lines[3].IndexOf(":") + 2));
            decodedOrder.UnitPrice = System.Convert.ToInt32(lines[4].Substring(lines[4].IndexOf(":") + 2));
            decodedOrder.Time_sent = System.Convert.ToDateTime(lines[5].Substring(lines[5].IndexOf(":") + 2));

            return decodedOrder;
        }

    }
}
