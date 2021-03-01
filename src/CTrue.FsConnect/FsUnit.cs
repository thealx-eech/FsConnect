﻿using System.Collections.Generic;

namespace CTrue.FsConnect
{
    public enum FsUnit
    {
        None,
        Degrees,
        Radians,
        RadianPerSecond,
        Feet,
        Meter,
        MeterPerSecond,
        Boolean,
        Second,
        Percent,
        FootPounds,
        SlugsPerCubiFeet,
        Celsius,
        inHg,
        Mask,
        Millibars,
        Rankine,
    }

    public static class UnitFactory
    {
        private static Dictionary<FsUnit, string> _enumToCodeDictionary = new Dictionary<FsUnit, string>();

        static UnitFactory()
        {
            _enumToCodeDictionary.Add(FsUnit.None, null);
            _enumToCodeDictionary.Add(FsUnit.Degrees, "degrees");
            _enumToCodeDictionary.Add(FsUnit.Radians, "radians");
            _enumToCodeDictionary.Add(FsUnit.RadianPerSecond, "radian per second");
            _enumToCodeDictionary.Add(FsUnit.Feet, "feet");
            _enumToCodeDictionary.Add(FsUnit.Meter, "meter");
            _enumToCodeDictionary.Add(FsUnit.MeterPerSecond, "meter per second");
            _enumToCodeDictionary.Add(FsUnit.Boolean, "boolean");
            _enumToCodeDictionary.Add(FsUnit.Second, "second");
            _enumToCodeDictionary.Add(FsUnit.Percent, "percent");
            _enumToCodeDictionary.Add(FsUnit.FootPounds, "foot pounds");

            // WEATHER
            _enumToCodeDictionary.Add(FsUnit.SlugsPerCubiFeet, "Slugs per cubic feet");
            _enumToCodeDictionary.Add(FsUnit.Celsius, "Celsius");
            _enumToCodeDictionary.Add(FsUnit.inHg, "inHg");
            _enumToCodeDictionary.Add(FsUnit.Mask, "Mask");
            _enumToCodeDictionary.Add(FsUnit.Millibars, "Millibars");
            _enumToCodeDictionary.Add(FsUnit.Rankine, "Rankine"); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string GetUnitCode(FsUnit unit)
        {
            return _enumToCodeDictionary[unit];
        }
    }
}