using System;
using System.Collections.Generic;
using System.Text;

namespace Naqliyat.Enums
{
    public enum TruckTypes : byte
    {
        Unknown = 0,

        // Tractor Units
        SemiTruck,
        DayCab,
        SleeperCab,

        // Dry Trailers
        DryVan,
        BoxTruck,
        CurtainSide,

        // Temperature Controlled
        Reefer,           // Standard refrigerated
        RefrigeratedChilled,   // 0°C to 10°C
        RefrigeratedFrozen,    // -10°C to -20°C
        RefrigeratedDeepFrozen, // -20°C and below
        Insulated,        // Temp-protected but not powered

        // Flat / Open
        Flatbed,
        StepDeck,
        DoubleDrop,
        Lowboy,
        Conestoga,
        Hotshot,

        // Specialized
        Tanker,
        PneumaticTanker,
        CarCarrier,
        LoggingTruck,
        DumpTruck,
        GarbageTruck,
        CementMixer,

        // Heavy / Oversize
        HeavyHaul,
        OversizeLoad,

        // Small Vehicles
        SprinterVan,
        CargoVan,
        PickUp
    }
}
