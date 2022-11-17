using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace EmulatorGUI.MVVM.Model
{
    public class Rect : ObservableObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        private SolidColorBrush _fill;

        public SolidColorBrush Fill
        {
            get { return _fill; }
            set 
            { 
                _fill = value;
                OnPropertyChanged();
            }
        }

        public Rect()
        {
            Fill = Brushes.White;
        }
    }
}
