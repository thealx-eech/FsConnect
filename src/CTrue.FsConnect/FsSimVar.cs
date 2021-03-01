using System.Collections.Generic;

namespace CTrue.FsConnect
{
    public enum FsSimVar
    {
        Title,
        Category,
        PlaneLatitude,
        PlaneLongitude,
        PlaneAltitudeAboveGround,
        StaticCGtoGround,
        PlaneAltitude,
        PlaneHeading,
        PlanePitch,
        PlaneBank,
        GpsGroundSpeed,
        AirspeedTrue,
        AirspeedIndicated,
        Verticalspeed,
        VelocityWorldX,
        VelocityWorldY,
        VelocityWorldZ,
        VelocityBodyX,
        VelocityBodyY,
        VelocityBodyZ,
        RotationVelocityBodyX,
        RotationVelocityBodyY,
        RotationVelocityBodyZ,
        SimOnGround,
        BrakeParkingPosition,
        AbsoluteTime,
        TailhookPosition,
        LaunchbarPosition,
        WaterRudderHandlePosition,
        OnAnyRunway,
        IsSlewActive,

        // WEATHER
        AmbientDensity,
        AmbientTemperature,
        AmbientPressure,
        AmbientWindVelocity,
        AmbientWindDirection,
        AmbientPrecipState,
        AmbientBarometerPressure,
        AmbientSeaLevelPressure,
        AmbientAirTemperature,
        AmbientInCloud,
        AmbientVisibility,
        AmbientStandardAtmTemperature,
        LocalDayOfYear,
        TimeOfDay,
        LocalTime,

        // LIGHTS
        LIGHTPANEL,
        LIGHTSTROBE,
        LIGHTLANDING,
        LIGHTTAXI,
        LIGHTBEACON,
        LIGHTNAV,
        LIGHTLOGO,
        LIGHTWING,
        LIGHTRECOGNITION,
        LIGHTCABIN,
        LIGHTGLARESHIELD,
        LIGHTPEDESTRAL,
        LIGHTPOTENTIOMETER,
        FSSIMVAR_LAST,
        ATCFLIGHTNUMBER,
        TURBTHRUST1,
        TURBTHRUST2,
        TURBTHRUST3,
        TURBTHRUST4,
        ENGTORQUE1,
        ENGTORQUE2,
        ENGTORQUE3,
        ENGTORQUE4,
    }

    public static class FsSimVarFactory
    {
        private static Dictionary<FsSimVar, string> _enumToCodeDictionary = new Dictionary<FsSimVar, string>();

        static FsSimVarFactory()
        {
            _enumToCodeDictionary.Add(FsSimVar.Title, "TITLE");
            _enumToCodeDictionary.Add(FsSimVar.Category, "CATEGORY");
            _enumToCodeDictionary.Add(FsSimVar.PlaneLatitude, "PLANE LATITUDE");
            _enumToCodeDictionary.Add(FsSimVar.PlaneLongitude, "PLANE LONGITUDE");
            _enumToCodeDictionary.Add(FsSimVar.PlaneAltitudeAboveGround, "PLANE ALT ABOVE GROUND");
            _enumToCodeDictionary.Add(FsSimVar.StaticCGtoGround, "STATIC CG TO GROUND"); 
            _enumToCodeDictionary.Add(FsSimVar.PlaneAltitude, "PLANE ALTITUDE");
            _enumToCodeDictionary.Add(FsSimVar.PlaneHeading, "PLANE HEADING DEGREES TRUE");
            _enumToCodeDictionary.Add(FsSimVar.PlanePitch, "PLANE PITCH DEGREES");
            _enumToCodeDictionary.Add(FsSimVar.PlaneBank, "PLANE BANK DEGREES");
            _enumToCodeDictionary.Add(FsSimVar.GpsGroundSpeed, "GPS GROUND SPEED");
            _enumToCodeDictionary.Add(FsSimVar.AirspeedTrue, "AIRSPEED TRUE");
            _enumToCodeDictionary.Add(FsSimVar.AirspeedIndicated, "AIRSPEED INDICATED");
            _enumToCodeDictionary.Add(FsSimVar.Verticalspeed, "VERTICAL SPEED");
            _enumToCodeDictionary.Add(FsSimVar.VelocityWorldX, "VELOCITY WORLD X");
            _enumToCodeDictionary.Add(FsSimVar.VelocityWorldY, "VELOCITY WORLD Y");
            _enumToCodeDictionary.Add(FsSimVar.VelocityWorldZ, "VELOCITY WORLD Z");
            _enumToCodeDictionary.Add(FsSimVar.VelocityBodyX, "VELOCITY BODY X");
            _enumToCodeDictionary.Add(FsSimVar.VelocityBodyY, "VELOCITY BODY Y");
            _enumToCodeDictionary.Add(FsSimVar.VelocityBodyZ, "VELOCITY BODY Z");
            _enumToCodeDictionary.Add(FsSimVar.RotationVelocityBodyX, "ROTATION VELOCITY BODY X");
            _enumToCodeDictionary.Add(FsSimVar.RotationVelocityBodyY, "ROTATION VELOCITY BODY Y");
            _enumToCodeDictionary.Add(FsSimVar.RotationVelocityBodyZ, "ROTATION VELOCITY BODY Z");
            _enumToCodeDictionary.Add(FsSimVar.SimOnGround, "SIM ON GROUND");
            _enumToCodeDictionary.Add(FsSimVar.BrakeParkingPosition, "BRAKE PARKING POSITION");
            _enumToCodeDictionary.Add(FsSimVar.AbsoluteTime, "ABSOLUTE TIME");
            _enumToCodeDictionary.Add(FsSimVar.TailhookPosition, "TAILHOOK POSITION");
            _enumToCodeDictionary.Add(FsSimVar.LaunchbarPosition, "LAUNCHBAR POSITION");
            _enumToCodeDictionary.Add(FsSimVar.WaterRudderHandlePosition, "WATER RUDDER HANDLE POSITION");
            _enumToCodeDictionary.Add(FsSimVar.OnAnyRunway, "ON ANY RUNWAY");
            _enumToCodeDictionary.Add(FsSimVar.IsSlewActive, "IS SLEW ACTIVE");

            // AMBIENT
            _enumToCodeDictionary.Add(FsSimVar.AmbientDensity, "AMBIENT DENSITY");
            _enumToCodeDictionary.Add(FsSimVar.AmbientTemperature, "AMBIENT TEMPERATURE");
            _enumToCodeDictionary.Add(FsSimVar.AmbientPressure, "AMBIENT PRESSURE");
            _enumToCodeDictionary.Add(FsSimVar.AmbientWindVelocity, "AMBIENT WIND VELOCITY");
            _enumToCodeDictionary.Add(FsSimVar.AmbientWindDirection, "AMBIENT WIND DIRECTION");
            _enumToCodeDictionary.Add(FsSimVar.AmbientPrecipState, "AMBIENT PRECIP STATE");
            _enumToCodeDictionary.Add(FsSimVar.AmbientBarometerPressure, "BAROMETER PRESSURE");
            _enumToCodeDictionary.Add(FsSimVar.AmbientSeaLevelPressure, "SEA LEVEL PRESSURE");
            _enumToCodeDictionary.Add(FsSimVar.AmbientAirTemperature, "TOTAL AIR TEMPERATURE");
            _enumToCodeDictionary.Add(FsSimVar.AmbientInCloud, "AMBIENT IN CLOUD");
            _enumToCodeDictionary.Add(FsSimVar.AmbientVisibility, "AMBIENT VISIBILITY");
            _enumToCodeDictionary.Add(FsSimVar.AmbientStandardAtmTemperature, "STANDARD ATM TEMPERATURE");
            _enumToCodeDictionary.Add(FsSimVar.LocalDayOfYear, "LOCAL DAY OF YEAR");
            _enumToCodeDictionary.Add(FsSimVar.TimeOfDay, "TIME OF DAY");
            _enumToCodeDictionary.Add(FsSimVar.LocalTime, "LOCAL TIME"); 

            // LIGHTS
            _enumToCodeDictionary.Add(FsSimVar.LIGHTPANEL, "LIGHT PANEL");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTSTROBE, "LIGHT STROBE");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTLANDING, "LIGHT LANDING");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTTAXI, "LIGHT TAXI");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTBEACON, "LIGHT BEACON");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTNAV, "LIGHT NAV");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTLOGO, "LIGHT LOGO");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTWING, "LIGHT WING");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTRECOGNITION, "LIGHT RECOGNITION");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTCABIN, "LIGHT CABIN");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTGLARESHIELD, "LIGHT GLARESHIELD");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTPEDESTRAL, "LIGHT PEDESTRAL");
           _enumToCodeDictionary.Add(FsSimVar.LIGHTPOTENTIOMETER, "LIGHT POTENTIOMETER");
            // AI
            _enumToCodeDictionary.Add(FsSimVar.ATCFLIGHTNUMBER, "ATC FLIGHT NUMBER");

            // ENGINE
            _enumToCodeDictionary.Add(FsSimVar.TURBTHRUST1, "TURB ENG JET THRUST:1");
            _enumToCodeDictionary.Add(FsSimVar.TURBTHRUST2, "TURB ENG JET THRUST:2");
            _enumToCodeDictionary.Add(FsSimVar.TURBTHRUST3, "TURB ENG JET THRUST:3");
            _enumToCodeDictionary.Add(FsSimVar.TURBTHRUST4, "TURB ENG JET THRUST:4");
            _enumToCodeDictionary.Add(FsSimVar.ENGTORQUE1, "ENG TORQUE:1");
            _enumToCodeDictionary.Add(FsSimVar.ENGTORQUE2, "ENG TORQUE:2");
            _enumToCodeDictionary.Add(FsSimVar.ENGTORQUE3, "ENG TORQUE:3");
            _enumToCodeDictionary.Add(FsSimVar.ENGTORQUE4, "ENG TORQUE:4");



            //(A: TOW RELEASE HANDLE, percent)
            //ROTATION VELOCITY BODY, degree per second
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simvar"></param>
        /// <returns></returns>
        public static string GetSimVarCode(FsSimVar simVar)
        {
            return _enumToCodeDictionary[simVar];
        }
    }
}