using System;

namespace Auto
{
    public class Autopark
    {
        public byte ID { get; set; }

        public string Name { get; set; }
        public int CarInspection { get; set; }
        public static byte TotalInspection { get; set; }

        public Autopark(byte ID, string Name, int CarInspection)
        {
            this.ID = ID;
            this.Name = Name;
            this.CarInspection = CarInspection;

        }
        public static bool Checks(Autopark el)
        {
            if (el.CarInspection == 0)
                return false;
            ++TotalInspection;
            return true;
        }


    }
}