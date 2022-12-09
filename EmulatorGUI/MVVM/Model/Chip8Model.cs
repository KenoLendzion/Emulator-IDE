using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using EmulatorLibrary.Interfaces;

namespace EmulatorGUI.MVVM.Model
{
    public class Chip8Model : ObservableObject, IChip8
    {
        private Stack<ushort> _stack;
        public Stack<ushort> Stack
        {
            get { return _stack; }
            set
            {
                _stack = value;
                OnPropertyChanged();
            }
        }

        private byte[] _vRegister;
        public byte[] VRegister
        {
            get { return _vRegister; }
            set
            {
                _vRegister = value;
                OnPropertyChanged();
            }
        }
        private ushort _iRegister;
        public ushort IRegister
        {
            get { return _iRegister; }
            set
            {
                _iRegister = value;
                OnPropertyChanged();
            }
        }

        private byte _delayTimer;
        public byte DelayTimer
        {
            get { return _delayTimer; }
            set
            {
                _delayTimer = value;
                OnPropertyChanged();
            }
        }

        private byte _soundTimer;
        public byte SoundTimer
        {
            get { return _soundTimer; }
            set
            {
                _soundTimer = value;
                OnPropertyChanged();
            }
        }

        private ushort _programCounter;
        public ushort ProgramCounter
        {
            get { return _programCounter; }
            set
            {
                _programCounter = value;
                OnPropertyChanged();
            }
        }

        private bool[] _keypad;

        public bool[] Keypad
        {
            get { return _keypad; }
            set { _keypad = value; OnPropertyChanged(); }
        }

        private byte[] _memory;
        public byte[] Memory 
        { 
            get { return _memory; } 
            set { _memory = value; OnPropertyChanged(); } 
        }

        private bool[,] _screen;
        public bool[,] Screen 
        { 
            get { return _screen; } 
            set { _screen = value; OnPropertyChanged(); }
        }

        public Chip8Model()
        {
            Memory = new byte[4096];
            VRegister = new byte[16];
            Stack = new Stack<ushort>();
            Screen = new bool[32, 64];

            // Program starts at address 0x200 in memory.
            ProgramCounter = 0x200;
        }
    }
}
