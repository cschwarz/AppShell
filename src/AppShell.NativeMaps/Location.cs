using System;

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

        public static bool operator ==(Location x, Location y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (((object)x == null) || ((object)y == null))
                return false;

            return x.Equals(y);
        }
        public static bool operator !=(Location x, Location y)
        {
            return !(x == y);
        }        
    }
}
