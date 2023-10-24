using System;
using System.Collections.Generic;
using System.Text;

namespace SmartParkingApp
{
    public interface ITariffLoad
    {
        List<Tariff> LoadTariff();
    }
        
}
