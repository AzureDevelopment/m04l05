namespace Redis.Demo
{
    public class Order
    {
        public Order()
        {
        }

        public int SalesOrderID { get; set; }
        public double UnitPrice { get; set; }
        public string Color { get; set; }
    }
}