namespace SmartParkingApp
{
    public class Tariff
    {
        public Tariff(int minutes, decimal rate)
        {
            Minutes = minutes;
            Rate = rate;
        }

        public int Minutes { get; set; }
        public decimal Rate { get; set; }
    }
}
