using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EmulatorGUI.MVVM.Model
{
    public class CPU : ObservableObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public Display Display { get; set; } 
        public Ram Memory { get; set; } 

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
                Debug.WriteLine("PropertyChanged");
            }
        }
        public CPU()
        {
            Memory = new Ram();
            VRegister = new byte[16];
            Name = "Chip-8"; //TODO Remove name everywhere
            Stack = new Stack<ushort>();
            Display = new Display();

            // Program starts at address 0x200 in memory.
            ProgramCounter = 0x200;
        }

        public void FetchDecodeExecuteLoop()
        {
            // For this Implementation the Execute Step is in the switch of the Decode Method 
            ushort instruction = Fetch();
            Decode(instruction);
        }

        private ushort Fetch()
        {
            // Each instruction is 2 bytes. That is why i get 2 bytes from memory and increase program counter by 2.
            byte firstInstruction = Memory.Data[ProgramCounter];
            ProgramCounter++;
            byte secondInstruction = Memory.Data[ProgramCounter];
            ProgramCounter++;

            ushort instruction = (ushort)( firstInstruction << 8 );
            instruction = (ushort)( instruction | secondInstruction );

            return instruction;
        }

        private void Decode(ushort instruction)
        {
            // I get all values I need at the beginning. That way I don't need to retriev them in the switch down below. 
            ushort firstNibble = (ushort)(instruction  & 0b1111000000000000);
            ushort secondNibble = (ushort)(instruction & 0b0000111100000000);
            ushort thirdNibble = (ushort)(instruction  & 0b0000000011110000);
            ushort forthNibble = (ushort)(instruction  & 0b0000000000001111);
            ushort NN = (ushort)( instruction & 0b0000000011111111 );
            ushort NNN = (ushort)( instruction & 0b0000111111111111 );
            ushort vXRegisterAddress = (ushort)( secondNibble >> 8 );
            ushort vYRegisterAddress = (ushort)( thirdNibble >> 4 );

            switch( firstNibble )
            {
                case 0x0000:
                    switch( secondNibble )
                    {
                        case 0x0000:

                            switch( thirdNibble )
                            {
                                case 0x00E0:
                                    switch( forthNibble )
                                    {
                                        case 0x0000:
                                            Display.ClearScreen();
                                            break;
                                        case 0x000E:
                                            ushort stackAddress = Stack.Pop(); //TODO Why is this not used?
                                            break;
                                        default:
                                            throw new NotImplementedException();
                                    }
                                    break;
                                
                                default:
                                    throw new NotImplementedException();
                            }
                            break;
                        default: // Opcode 0NNN Not implemented see Langhoffs Articel on why.
                            throw new NotImplementedException();
                            break;
                    }
                    break;
                case 0x1000:
                    ProgramCounter = NNN;
                    break;
                case 0x2000:
                    Stack.Push(ProgramCounter);
                    ProgramCounter = NNN;
                    break;
                case 0x3000:
                    throw new NotImplementedException();
                    break;
                case 0x4000:
                    throw new NotImplementedException();
                    break;
                case 0x5000:
                    throw new NotImplementedException();
                    break;
                case 0x6000:
                    VRegister[vXRegisterAddress] = (byte)NN;
                    break;
                case 0x7000:
                    VRegister[vXRegisterAddress] += (byte)NN; 
                    break;
                case 0x8000:
                    throw new NotImplementedException();
                    break;
                case 0x9000:
                    throw new NotImplementedException();
                    break;
                case 0xA000:
                    IRegister = NNN;
                    break;
                case 0xB000:
                    throw new NotImplementedException();
                    break;
                case 0xC000:
                    throw new NotImplementedException();
                    break;
                case 0xD000:
                    ushort YCoordinate = (ushort)( VRegister[vYRegisterAddress] & 31 );
                    VRegister[0xF] = 0x00;

                    for( int i = 0; i < forthNibble; i++ )
                    {
                        byte currentSprite = Memory.Data[IRegister + i];
                        ushort XCoordinate = (ushort)( VRegister[vXRegisterAddress] & 63 );
                        for( int bit = 0; bit < 8; bit++ )
                        {
                            ushort currentBit = (ushort)( currentSprite & (ushort)( 0b10000000 >> bit ) );

                            if( Display.Screen.GetLength(1) == XCoordinate )
                            {
                                break;
                            }

                            if(Display.Screen[YCoordinate, XCoordinate] == true && currentBit > 0 )
                            {
                                Display.Screen[YCoordinate, XCoordinate] = false;
                                VRegister[0x0F] = 0x01;
                            }
                            else if( Display.Screen[YCoordinate, XCoordinate] == false && currentBit > 0)
                            {
                                Display.Screen[YCoordinate, XCoordinate] = true;
                            }
                            XCoordinate++;
                        }
                        YCoordinate++;
                        if( Display.Screen.GetLength(0) == YCoordinate )
                        {
                            break;
                        }
                    }
                    break;
                case 0xE000:
                    throw new NotImplementedException();
                    break;
                case 0xF000:
                    throw new NotImplementedException();
                    break;
                default:
                    break;
            }
        }
    }
}
