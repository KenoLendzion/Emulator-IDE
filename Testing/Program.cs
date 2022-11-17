using Chip_8_Emulator;

Ram ram = new Ram();
CPU cpu = new CPU();

//int n = 0;
//for( int i = 0x200; i < ram.Data.Length; i++, n++)
//{

//    Console.Write( Convert.ToString(ram.Data[i], 16) + " ");

//    if(n == 19)
//    {
//        n = 0;
//        Console.WriteLine();
//    }
//}
//ushort currentBit = (ushort)( 0xFF & ( 0b10000000 >> 1 ) );
cpu.FetchDecodeExecuteLoop();



Console.ReadLine();





//for( int k = 0; k < Display.Screen.GetLength(0); k++ )
//{
//    for( int l = 0; l < Display.Screen.GetLength(1); l++ )
//    {
//        if( Display.Screen[k, l] == true )
//        {
//            Console.Write("X");
//        }
//        else
//        {
//            Console.Write(" ");
//        }
//    }
//    Console.WriteLine();
//}