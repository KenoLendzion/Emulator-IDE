using EmulatorLibrary.Interfaces;

namespace EmulatorLibrary.Helper
{
    public class Chip8Helper
    {
        public void ExecuteOneCycle(IChip8 chip8)
        {
            // For this Implementation the Execute Step is in the switch of the Decode Method 
            ushort instruction = Fetch(chip8);
            DecodeExecute(chip8, instruction);
        }

        private ushort Fetch(IChip8 chip8)
        {
            // Each instruction is 2 bytes. That is why i get 2 bytes from memory and increase program counter by 2.
            byte firstInstruction = chip8.Memory[chip8.ProgramCounter];
            chip8.ProgramCounter++;
            byte secondInstruction = chip8.Memory[chip8.ProgramCounter];
            chip8.ProgramCounter++;

            ushort instruction = (ushort)( firstInstruction << 8 );
            instruction = (ushort)( instruction | secondInstruction );

            return instruction;
        }

        private void DecodeExecute(IChip8 chip8, ushort instruction)
        {
            //I get all values I need at the beginning.That way I don't need to retriev them in the switch down below. 
            ushort firstNibble = (ushort)( instruction & 0b1111_0000_0000_0000 );
            ushort secondNibble = (ushort)( instruction & 0b0000_1111_0000_0000 );
            ushort thirdNibble = (ushort)( instruction & 0b0000_0000_1111_0000 );
            ushort forthNibble = (ushort)( instruction & 0b0000_0000_0000_1111 );
            ushort NN = (ushort)( instruction & 0b0000_0000_1111_1111 );
            ushort NNN = (ushort)( instruction & 0b0000_1111_1111_1111 );
            ushort vXRegisterAddress = (ushort)( secondNibble >> 8 );
            ushort vYRegisterAddress = (ushort)( thirdNibble >> 4 );

            switch(firstNibble, secondNibble, thirdNibble, forthNibble)
            {
                case (0x0000, 0x0000, 0x0000, 0x0000):
                    // Not implemented because this instruction on the original chip 8
                    // retruns to the cpu of the pc the chip 8 program is running on. 
                    // Remember that chip8 is only a virtual CPU
                    break;
                case (0x0000, 0x0000, 0x00E0, 0x0000):
                    Instruction0x00E0CleanScreen(chip8);
                    break;
                case (0x1000, _, _, _):
                    Instruction0x1NNNJump(chip8, NNN);
                    break;
                case (0x0000, 0x0000, 0x00E0, 0x000E):
                    Instruction0x00EEReturnFromSubroutine(chip8);
                    break;
                case (0x2000, _, _, _):
                    Instruction0x2NNNJumpAndPushPCToStack(chip8, NNN);
                    break;
                case (0x3000, _, _, _):
                    Instruction0x3XNNSkipIfEqual(chip8, vXRegisterAddress, NN);
                    break;
                case (0x4000, _, _, _):
                    Instruction0x4XNNSkipIfNotEqual(chip8, vXRegisterAddress, NN);
                    break;
                case (0x5000, _, _, _):
                    Instruction0x5XY0SkipsIfEqual(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0x6000, _, _, _):
                    Instruction0x6XNNSetRegisterVX(chip8, vXRegisterAddress, NN);
                    break;
                case (0x7000, _, _, _):
                    Instruction0x7XNNAddValueToRegisterVX(chip8, vXRegisterAddress, NN);
                    break;
                case (0x8000, _, _, 0x0000):
                    Instruction0x8XY0SetVxToVy(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0x8000, _, _, 0x0001):
                    Instruction0x8XY1SetVxToBitwiseOr(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0x8000, _, _, 0x0002):
                    Instruction0x8XY2SetVxToBitwiseAnd(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0x8000, _, _, 0x0003):
                    Instruction0x8XY3SetVxToBitwiseExclusiveOr(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0x8000, _, _, 0x0004):
                    Instruction0x8XY4AddVxAndVy(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0x8000, _, _, 0x0005):
                    Instruction0x8XY5SubtractVxMinusVy(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0x8000, _, _, 0x0006):
                    throw new NotImplementedException();
                    break;
                case (0x8000, _, _, 0x0007):
                    Instruction0x8XY7SubtractVyMinusVx(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0x8000, _, _, 0x000E):
                    throw new NotImplementedException();
                    break;
                case (0x9000, _, _, _):
                    Instruction0x9XY0SkipsIfNotEqual(chip8, vXRegisterAddress, vYRegisterAddress);
                    break;
                case (0xA000, _, _, _):
                    Instruction0xANNNSetIndexRegisterI(chip8, NNN);
                    break;
                case (0xB000, _, _, _):
                    throw new NotImplementedException();
                    break;
                case (0xC000, _, _, _):
                    Instruction0xCXNNRandomNumber(chip8, vXRegisterAddress, NN);
                    break;
                case (0xD000, _, _, _):
                    Instruction0xDXYNDraw(chip8, vXRegisterAddress, vYRegisterAddress, forthNibble);
                    break;
                case (0xE000, _, _, 0x0001):
                    throw new NotImplementedException();
                    break;
                case (0xE000, _, _, 0x000E):
                    throw new NotImplementedException();
                    break;
                case (0xF000, _, 0x0000, 0x0007):
                    throw new NotImplementedException();
                    break;
                case (0xF000, _, 0x0010, 0x0005):
                    throw new NotImplementedException();
                    break;
                case (0xF000, _, 0x0010, 0x000F):
                    throw new NotImplementedException();
                    break;
            }
        }

        private void Instruction0x00E0CleanScreen(IChip8 chip8) => chip8.Screen = new bool[chip8.Screen.GetLength(0), chip8.Screen.GetLength(1)];
        private void Instruction0x00EEReturnFromSubroutine(IChip8 chip8)
        {
            chip8.ProgramCounter = chip8.Stack.Pop();
        }
        private void Instruction0x1NNNJump(IChip8 chip8, ushort NNN) => chip8.ProgramCounter = NNN;
        private void Instruction0x2NNNJumpAndPushPCToStack(IChip8 chip8, ushort NNN)
        {
            chip8.Stack.Push(chip8.ProgramCounter);
            chip8.ProgramCounter = NNN;
        }
        private void Instruction0x3XNNSkipIfEqual(IChip8 chip8, ushort vXRegisterAddress, ushort NN)
        {
            if(chip8.VRegister[vXRegisterAddress] == NN)
            {
                chip8.ProgramCounter += 2;
            }
        }
        private void Instruction0x4XNNSkipIfNotEqual(IChip8 chip8, ushort vXRegisterAddress, ushort NN)
        {
            if( chip8.VRegister[vXRegisterAddress] != NN )
            {
                chip8.ProgramCounter += 2;
            }
        }
        private void Instruction0x5XY0SkipsIfEqual(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress)
        {
            if(chip8.VRegister[vXRegisterAddress] == chip8.VRegister[vYRegisterAddress] )
            {
                chip8.ProgramCounter += 2;
            }
        }
        private void Instruction0x6XNNSetRegisterVX(IChip8 chip8, ushort X, ushort NN) => chip8.VRegister[X] = (byte)NN;
        private void Instruction0x7XNNAddValueToRegisterVX(IChip8 chip8, ushort X, ushort NN) => chip8.VRegister[X] += (byte)NN;
        private void Instruction0x8XY0SetVxToVy(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress) => chip8.VRegister[vXRegisterAddress] = chip8.VRegister[vYRegisterAddress];
        private void Instruction0x8XY1SetVxToBitwiseOr(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress) => chip8.VRegister[vXRegisterAddress] = (byte)(chip8.VRegister[vXRegisterAddress] | chip8.VRegister[vYRegisterAddress] );
        private void Instruction0x8XY2SetVxToBitwiseAnd(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress) => chip8.VRegister[vXRegisterAddress] = (byte)(chip8.VRegister[vXRegisterAddress] & chip8.VRegister[vYRegisterAddress] );
        private void Instruction0x8XY3SetVxToBitwiseExclusiveOr(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress) => chip8.VRegister[vXRegisterAddress] = (byte)(chip8.VRegister[vXRegisterAddress] ^ chip8.VRegister[vYRegisterAddress] );
        // Overflow carry flag not handled in instruction 0x8XY4
        private void Instruction0x8XY4AddVxAndVy(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress) => chip8.VRegister[vXRegisterAddress] = (byte)(chip8.VRegister[vXRegisterAddress] + chip8.VRegister[vYRegisterAddress] );
        private void Instruction0x8XY5SubtractVxMinusVy(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress) => chip8.VRegister[vXRegisterAddress] = (byte)(chip8.VRegister[vXRegisterAddress] - chip8.VRegister[vYRegisterAddress] );
        // Overflow carry flag not handed in instruction 0x8XY7
        private void Instruction0x8XY7SubtractVyMinusVx(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress) => chip8.VRegister[vXRegisterAddress] = (byte)(chip8.VRegister[vYRegisterAddress] - chip8.VRegister[vXRegisterAddress] );
        private void Instruction0x9XY0SkipsIfNotEqual(IChip8 chip8, ushort vXRegisterAddress, ushort vYRegisterAddress)
        {
            if( chip8.VRegister[vXRegisterAddress] != chip8.VRegister[vYRegisterAddress] )
            {
                chip8.ProgramCounter += 2;
            }
        }
        private void Instruction0xANNNSetIndexRegisterI(IChip8 chip8, ushort NNN) => chip8.IRegister = NNN;
        private void Instruction0xCXNNRandomNumber(IChip8 chip8, ushort vXRegisterAddress, ushort NN)
        {
            Random rand = new Random();
            int randomNumber = rand.Next(0, NN);
            chip8.VRegister[vXRegisterAddress] = (byte)(randomNumber & NN);
        }
        private void Instruction0xDXYNDraw(IChip8 chip8, ushort X, ushort Y, ushort N)
        {
            ushort YCoordinate = (ushort)( chip8.VRegister[Y] & 31 );
            chip8.VRegister[0xF] = 0x00;

            for( int i = 0; i < N; i++ )
            {
                byte currentSprite = chip8.Memory[chip8.IRegister + i];
                ushort XCoordinate = (ushort)( chip8.VRegister[X] & 63 );
                for( int bit = 0; bit < 8; bit++ )
                {
                    ushort currentBit = (ushort)( currentSprite & (ushort)( 0b10000000 >> bit ) );

                    if( chip8.Screen.GetLength(1) == XCoordinate )
                    {
                        break;
                    }

                    if( chip8.Screen[YCoordinate, XCoordinate] == true && currentBit > 0 )
                    {
                        chip8.Screen[YCoordinate, XCoordinate] = false;
                        chip8.VRegister[0x0F] = 0x01;
                    }
                    else if( chip8.Screen[YCoordinate, XCoordinate] == false && currentBit > 0 )
                    {
                        chip8.Screen[YCoordinate, XCoordinate] = true;
                    }
                    XCoordinate++;
                }
                YCoordinate++;
                if( chip8.Screen.GetLength(0) == YCoordinate )
                {
                    break;
                }
            }
        }
        public void LoadProgramIntoMemory(IChip8 chip8, string path, int memoryLocation)
        {
            var bytes = File.ReadAllBytes(path);
            for( int i = 0; i < bytes.Length; i++ )
            {
                chip8.Memory[memoryLocation] = bytes[i];
                memoryLocation++;
            }
        }
        public void SetFonts(IChip8 chip8)
        {
            #region 0
            chip8.Memory[0x050] = 0xF0;
            chip8.Memory[0x051] = 0x90;
            chip8.Memory[0x052] = 0x90;
            chip8.Memory[0x053] = 0x90;
            chip8.Memory[0x054] = 0xF0;
            #endregion
            #region 1
            chip8.Memory[0x055] = 0x20;
            chip8.Memory[0x056] = 0x60;
            chip8.Memory[0x057] = 0x20;
            chip8.Memory[0x058] = 0x20;
            chip8.Memory[0x059] = 0x70;
            #endregion
            #region 2
            chip8.Memory[0x05A] = 0xF0;
            chip8.Memory[0x05B] = 0x10;
            chip8.Memory[0x05C] = 0xF0;
            chip8.Memory[0x05D] = 0x80;
            chip8.Memory[0x05E] = 0xF0;
            #endregion
            #region 3
            chip8.Memory[0x05F] = 0xF0;
            chip8.Memory[0x060] = 0x10;
            chip8.Memory[0x061] = 0xF0;
            chip8.Memory[0x062] = 0x10;
            chip8.Memory[0x063] = 0xF0;
            #endregion
            #region 4
            chip8.Memory[0x064] = 0x90;
            chip8.Memory[0x065] = 0x90;
            chip8.Memory[0x066] = 0xF0;
            chip8.Memory[0x067] = 0x10;
            chip8.Memory[0x068] = 0x10;
            #endregion
            #region 5
            chip8.Memory[0x069] = 0xF0;
            chip8.Memory[0x06A] = 0x80;
            chip8.Memory[0x06B] = 0xF0;
            chip8.Memory[0x06C] = 0x10;
            chip8.Memory[0x06D] = 0xF0;
            #endregion
            #region 6
            chip8.Memory[0x06E] = 0xF0;
            chip8.Memory[0x06F] = 0x80;
            chip8.Memory[0x070] = 0xF0;
            chip8.Memory[0x071] = 0x90;
            chip8.Memory[0x072] = 0xF0;
            #endregion
            #region 7
            chip8.Memory[0x073] = 0xF0;
            chip8.Memory[0x074] = 0x10;
            chip8.Memory[0x075] = 0x20;
            chip8.Memory[0x076] = 0x40;
            chip8.Memory[0x077] = 0x40;
            #endregion
            #region 8
            chip8.Memory[0x078] = 0xF0;
            chip8.Memory[0x079] = 0x90;
            chip8.Memory[0x07A] = 0xF0;
            chip8.Memory[0x07B] = 0x90;
            chip8.Memory[0x07C] = 0xF0;
            #endregion
            #region 9
            chip8.Memory[0x07D] = 0xF0;
            chip8.Memory[0x07E] = 0x90;
            chip8.Memory[0x07F] = 0xF0;
            chip8.Memory[0x080] = 0x10;
            chip8.Memory[0x081] = 0xF0;
            #endregion
            #region A
            chip8.Memory[0x082] = 0xF0;
            chip8.Memory[0x083] = 0x90;
            chip8.Memory[0x084] = 0xF0;
            chip8.Memory[0x085] = 0x90;
            chip8.Memory[0x086] = 0x90;
            #endregion
            #region B
            chip8.Memory[0x087] = 0xE0;
            chip8.Memory[0x088] = 0x90;
            chip8.Memory[0x089] = 0xE0;
            chip8.Memory[0x08A] = 0x90;
            chip8.Memory[0x08B] = 0xE0;
            #endregion
            #region C
            chip8.Memory[0x08C] = 0xF0;
            chip8.Memory[0x08D] = 0x80;
            chip8.Memory[0x08E] = 0x80;
            chip8.Memory[0x08F] = 0x80;
            chip8.Memory[0x090] = 0xF0;
            #endregion
            #region D
            chip8.Memory[0x091] = 0xE0;
            chip8.Memory[0x092] = 0x90;
            chip8.Memory[0x093] = 0x90;
            chip8.Memory[0x094] = 0x90;
            chip8.Memory[0x095] = 0xE0;
            #endregion
            #region E
            chip8.Memory[0x096] = 0xF0;
            chip8.Memory[0x097] = 0x80;
            chip8.Memory[0x098] = 0xF0;
            chip8.Memory[0x099] = 0x80;
            chip8.Memory[0x09A] = 0xF0;
            #endregion
            #region F
            chip8.Memory[0x09B] = 0xF0;
            chip8.Memory[0x09C] = 0x80;
            chip8.Memory[0x09D] = 0xF0;
            chip8.Memory[0x09E] = 0x80;
            chip8.Memory[0x09F] = 0x80;
            #endregion
        }
    }
}
