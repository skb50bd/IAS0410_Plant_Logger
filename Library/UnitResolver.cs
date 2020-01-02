using System;
using System.Collections.Generic;

namespace IAS04110
{
    public class UnitResolver
    {
        private readonly Dictionary<string, Unit> _units =
            new Dictionary<string, Unit> {
                {"temperature", new Unit("°C", "0.0", typeof(double)) },
                {"pressure", new Unit("atm", "0.0", typeof(double)) },
                {"concentration", new Unit("%", "", typeof(int))},
                {"level", new Unit("%", "", typeof(int)) },
                {"viscosity", new Unit("cSt", "0.00", typeof(double)) },
                {"turbidity", new Unit("NTU", "", typeof(int)) },
                {"conductivity", new Unit("S/m", "0.00", typeof(double)) },
                {"flow", new Unit("m³/s", "0.000", typeof(double)) },
                {"quantity", new Unit("kg", "", typeof(int)) },
                {"volume", new Unit("L", "", typeof(int)) },
                {"ph", new Unit("", "0.0", typeof(double)) }
            };

        public Type GetType(string name) => 
            GetUnit(name)?.Type;

        public string GetUnitName(string name) => 
            GetUnit(name)?.Name;

        public string GetFormat(string name) =>
            GetUnit(name)?.Format;

        public Unit GetUnit(string name)
        {
            foreach (var key in _units.Keys)
            {
                if (name.ToLower()
                        .Contains(key))
                    return _units[key];
            }

            return null;
        }
    }
}
