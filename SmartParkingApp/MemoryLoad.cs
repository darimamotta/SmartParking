using System;
using System.Collections.Generic;
using System.Text;

namespace SmartParkingApp
{
    public class MemoryLoad : ITariffLoad
    {
        
        public List<Tariff> LoadTariff()
        {
            List<Tariff> tariffs = new List<Tariff>();
            Tariff tariff1 = new Tariff(15, 0);
            Tariff tariff2 = new Tariff(60, 50);
            Tariff tariff3 = new Tariff(120, 100);
            Tariff tariff4 = new Tariff(180, 140);
            Tariff tariff5 = new Tariff(240, 180);
            tariffs.Add(tariff1);
            tariffs.Add(tariff2);
            tariffs.Add(tariff3);
            tariffs.Add(tariff4);
            tariffs.Add(tariff5);
            return tariffs;
        }
       
    }
}
