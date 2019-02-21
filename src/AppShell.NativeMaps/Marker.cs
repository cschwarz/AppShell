using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppShell.NativeMaps
{
    public class Marker : INotifyPropertyChanged
    {
        private Location center;
        public Location Center
        {
            get { return center; }
            set
            {
                if (center != value)
                {
                    center = value;
                    OnPropertyChanged();
                }
            }
        }

        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string icon;
        public string Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged();
                }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                if (content != value)
                {
                    content = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool draggable;
        public bool Draggable
        {
            get { return draggable; }
            set
            {
                if (draggable != value)
                {
                    draggable = value;
                    OnPropertyChanged();
                }
            }
        }

        private string layer;
        public string Layer
        {
            get { return layer; }
            set
            {
                if (layer != value)
                {
                    layer = value;
                    OnPropertyChanged();
                }
            }
        }

        private int? zIndex;
        public int? ZIndex
        {
            get { return zIndex; }
            set
            {
                if (zIndex != value)
                {
                    zIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private Label label;
        public Label Label
        {
            get { return label; }
            set
            {
                if (label != value)
                {
                    label = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
