using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmulatorGUI.MVVM.Model;
using System.Collections.Generic;
using System.Windows.Media;

namespace EmulatorGUI.MVVM.ViewModel
{
    internal class Chip8ViewModel : ObservableObject
    {
        public RelayCommand RunOneCycle { get; set; }

        private int _rectangleSize = 4;
        private List<Rect> _rectangles;
        public List<Rect> Rectangles
        {
            get { return _rectangles; }
            set 
            { 
                _rectangles = value; 
                OnPropertyChanged();
            }
        }
        private CPU _cpu;
        public CPU Cpu
        {
            get { return _cpu; }
            set 
            { 
                _cpu = value;
                OnPropertyChanged();
            }
        }

        public Chip8ViewModel()
        {
            Cpu = new CPU();
            RunOneCycle = new RelayCommand(RunOneCycleMethod);
            SetUpRectangles();
        }

        public void UpdateRectangles()
        {
            List<bool> BoolArrayForRectangles = new List<bool>(); 

            for( int i = 0; i < Cpu.Display.Screen.GetLength(0); i++ )
            {
                for( int j = 0; j < Cpu.Display.Screen.GetLength(1); j++ )
                {
                    BoolArrayForRectangles.Add(Cpu.Display.Screen[i, j]);
                }
            }

            for( int i = 0; i < BoolArrayForRectangles.Count; i++ )
            {
                if( BoolArrayForRectangles[i] == true )
                {
                    Rectangles[i].Fill = Brushes.Cyan;
                }
                else
                {
                    Rectangles[i].Fill = Brushes.White;
                }
            }
        }

        public void SetUpRectangles()
        {
            Rectangles = new List<Rect>();
            Rectangles.Clear();
            for( int i = 0; i < 32; i++ )
            {
                for( int j = 0; j < 64; j++ )
                {
                    Rect rectangle = new Rect
                    {
                        Height = _rectangleSize,
                        Width = _rectangleSize,
                        X = j * _rectangleSize,
                        Y = i * _rectangleSize,
                        Fill = Cpu.Display.Screen[i,j] == true ? Brushes.Cyan : Brushes.White,
                    };
                    Rectangles.Add(rectangle);
                }
            }
        }

        public void RunOneCycleMethod()
        {
            Cpu.FetchDecodeExecuteLoop();
            UpdateRectangles();
        }
    }
}
