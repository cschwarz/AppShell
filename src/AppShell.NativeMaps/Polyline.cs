using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppShell.NativeMaps
{
    public class Polyline : Marker
    {
        private List<Location> points;
        public List<Location> Points
        {
            get { return points; }
            set
            {
                if (points != value)
                {
                    points = value;
                    OnPropertyChanged();
                }
            }
        }

        private string strokeColor;
        public string StrokeColor
        {
            get { return strokeColor; }
            set
            {
                if (strokeColor != value)
                {
                    strokeColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private float? strokeWidth;
        public float? StrokeWidth
        {
            get { return strokeWidth; }
            set
            {
                if (strokeWidth != value)
                {
                    strokeWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        public Polyline()
        {
            Points = new List<Location>();            
        }
    }
}
