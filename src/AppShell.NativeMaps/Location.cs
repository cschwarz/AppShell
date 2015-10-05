﻿using System;

namespace AppShell.NativeMaps
{
    public class Location : IEquatable<Location>
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Location))
                return false;

            return Equals((Location)obj);
        }

        public bool Equals(Location other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            return new { Latitude, Longitude }.GetHashCode();
        }
    }
}
