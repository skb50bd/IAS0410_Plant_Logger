using System;

namespace IAS0410
{
    public class Unit
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public Type Type { get; set; }

        public Unit(
            string name, 
            string format, 
            Type type)
        {
            Name   = name;
            Format = format;
            Type = type;
        }


    }
}
