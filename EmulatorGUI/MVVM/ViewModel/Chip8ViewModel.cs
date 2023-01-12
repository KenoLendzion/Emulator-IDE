using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmulatorGUI.MVVM.Model;
using EmulatorLibrary.Helper;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;

namespace EmulatorGUI.MVVM.ViewModel
{
    internal class Chip8ViewModel : ObservableObject
    {
        public RelayCommand RunOneCycle { get; set; }
        public RelayCommand RunChip8 { get; set; }
        private Thread chip8RunThread { get; set; }
        private Chip8Helper chip8Helper { get; set; } 
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
        private Chip8Model _chip8Model;
        public Chip8Model Chip8Model
        {
            get { return _chip8Model; }
            set 
            { 
                _chip8Model = value;
                OnPropertyChanged();
            }
        }

        public Chip8ViewModel()
        {
            Chip8Model = new Chip8Model();
            RunOneCycle = new RelayCommand(RunOneCycleMethod);
            RunChip8 = new RelayCommand(StartChip8Thread);
            chip8RunThread = new Thread();
            SetUpRectangles();

            Chip8Helper chip8Helper = new Chip8Helper();
            chip8Helper.LoadProgramIntoMemory(Chip8Model, @"C:\Users\kenol\source\Chip-8\IBM Logo.ch8", 0x200);
        }

        public void UpdateRectangles()
        {
            List<bool> BoolArrayForRectangles = new List<bool>(); 

            for( int i = 0; i < Chip8Model.Screen.GetLength(0); i++ )
            {
                for( int j = 0; j < Chip8Model.Screen.GetLength(1); j++ )
                {
                    BoolArrayForRectangles.Add(Chip8Model.Screen[i, j]);
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
                        Fill = Chip8Model.Screen[i,j] == true ? Brushes.Cyan : Brushes.White,
                    };
                    Rectangles.Add(rectangle);
                }
            }
        }

        public void RunOneCycleMethod()
        {
            chip8Helper.ExecuteOneCycle(Chip8Model);
            UpdateRectangles();
        }

        public void StartChip8Thread()
        {
            chip8RunThread.Start();
        }

        public void RunChip8Method()
        {

        }
    }
}
