using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorGUI.MVVM.Model
{
    public class Display
    {
        public bool[,] Screen { get; set; }

        public Display()
        {
            Screen = new bool[32,64];
            ClearScreen();
        }

        public void ClearScreen()
        {
            for (int i = 0; i < Screen.GetLength(0); i++ )
            {
                for( int j = 0; j < Screen.GetLength(1); j++ )
                {
                    Screen[i,j] = false;
                }
            }
        }
    }
}
