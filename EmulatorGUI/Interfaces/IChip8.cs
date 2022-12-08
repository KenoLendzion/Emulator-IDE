using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorGUI.Interfaces
{
    internal interface IChip8
    {
        byte[] Memory { get; set; }
        public bool[,] Screen { get; set; }
        public byte[] VRegister { get; set; }
        public ushort IRegister { get; set; }
        public byte DelayTimer { get; set; }
        public byte SoundTimer { get; set; }
        public ushort ProgramCounter { get; set; }
        public bool[] Keypad { get; set; }
        public Stack<ushort> Stack { get; set; }
    }
}
