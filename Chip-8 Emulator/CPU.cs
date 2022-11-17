using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip_8_Emulator
{
    public class CPU
    {
        // TODO Implement Keyboard
        public Display Display { get; set; }
        public Ram Memory { get; set; }
        public Stack<ushort> Stack{ get; set; }
        public byte[] VRegister { get; set; }
        public ushort IRegister { get; set; }
        public byte DelayTimer { get; set; } // TODO Implement DelayTimer
        public byte SoundTimer { get; set; } // TODO Implement SoundTimer
        public ushort ProgramCounter { get; set; }

        public CPU()
        {
            Memory = new Ram();
            VRegister = new byte[16];

            // Program starts at address 0x200 in memory
            ProgramCounter = 0x200;
            Display = new Display();
        }

        public void FetchDecodeExecuteLoop()
        {
            while(true)
            {
                ushort instruction = Fetch();

                // For this Implementation the Execute Step is in the switch of the Decode Method 
                Decode(instruction);
            }
        }

        private ushort Fetch()
        {
            // Each instruction is 2 bytes that why i get 2 bytes from memory
            byte firstInstruction = Memory.Data[ProgramCounter++];
            byte secondInstruction = Memory.Data[ProgramCounter++];

            ushort instruction = (ushort)( firstInstruction << 8 );
            instruction = (ushort)( instruction | secondInstruction );

            return instruction;
        }

        private void Decode(ushort instruction)
        {
            // TODO Switch here
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
                                            ushort stackAddress = Stack.Pop();
                                            break;
                                        default:
                                            throw new NotImplementedException();
                                    }
                                    break;
                                
                                default:
                                    throw new NotImplementedException();
                            }
                            break;
                        default: // Opcode 0NNN Don't implement see Langhoffs Articel on why
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
