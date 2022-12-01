using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorGUI.MVVM.Model
{
    public class Ram
    {
        public byte[] Data { get; set; }

        public Ram()
        {
            Data = new byte[4096];
            SetFonts();
            LoadProgramToMemory(@"C:\Users\kenol\source\Chip-8\IBM Logo.ch8", 0x200);
        }

        public void LoadProgramToMemory(string path, int memoryLocation)
        {
            var bytes = File.ReadAllBytes(path);
            for( int i = 0; i < bytes.Length; i++ )
            {
                Data[memoryLocation] = bytes[i];
                memoryLocation++;
            }
        }

        public void SetFonts()
        {
            #region 0
            Data[0x050] = 0xF0;
            Data[0x051] = 0x90;
            Data[0x052] = 0x90;
            Data[0x053] = 0x90;
            Data[0x054] = 0xF0;
            #endregion
            #region 1
            Data[0x055] = 0x20;
            Data[0x056] = 0x60;
            Data[0x057] = 0x20;
            Data[0x058] = 0x20;
            Data[0x059] = 0x70;
            #endregion
            #region 2
            Data[0x05A] = 0xF0;
            Data[0x05B] = 0x10;
            Data[0x05C] = 0xF0;
            Data[0x05D] = 0x80;
            Data[0x05E] = 0xF0;
            #endregion
            #region 3
            Data[0x05F] = 0xF0;
            Data[0x060] = 0x10;
            Data[0x061] = 0xF0;
            Data[0x062] = 0x10;
            Data[0x063] = 0xF0;
            #endregion
            #region 4
            Data[0x064] = 0x90;
            Data[0x065] = 0x90;
            Data[0x066] = 0xF0;
            Data[0x067] = 0x10;
            Data[0x068] = 0x10;
            #endregion
            #region 5
            Data[0x069] = 0xF0;
            Data[0x06A] = 0x80;
            Data[0x06B] = 0xF0;
            Data[0x06C] = 0x10;
            Data[0x06D] = 0xF0;
            #endregion
            #region 6
            Data[0x06E] = 0xF0;
            Data[0x06F] = 0x80;
            Data[0x070] = 0xF0;
            Data[0x071] = 0x90;
            Data[0x072] = 0xF0;
            #endregion
            #region 7
            Data[0x073] = 0xF0;
            Data[0x074] = 0x10;
            Data[0x075] = 0x20;
            Data[0x076] = 0x40;
            Data[0x077] = 0x40;
            #endregion
            #region 8
            Data[0x078] = 0xF0;
            Data[0x079] = 0x90;
            Data[0x07A] = 0xF0;
            Data[0x07B] = 0x90;
            Data[0x07C] = 0xF0;
            #endregion
            #region 9
            Data[0x07D] = 0xF0;
            Data[0x07E] = 0x90;
            Data[0x07F] = 0xF0;
            Data[0x080] = 0x10;
            Data[0x081] = 0xF0;
            #endregion
            #region A
            Data[0x082] = 0xF0;
            Data[0x083] = 0x90;
            Data[0x084] = 0xF0;
            Data[0x085] = 0x90;
            Data[0x086] = 0x90;
            #endregion
            #region B
            Data[0x087] = 0xE0;
            Data[0x088] = 0x90;
            Data[0x089] = 0xE0;
            Data[0x08A] = 0x90;
            Data[0x08B] = 0xE0;
            #endregion
            #region C
            Data[0x08C] = 0xF0;
            Data[0x08D] = 0x80;
            Data[0x08E] = 0x80;
            Data[0x08F] = 0x80;
            Data[0x090] = 0xF0;
            #endregion
            #region D
            Data[0x091] = 0xE0;
            Data[0x092] = 0x90;
            Data[0x093] = 0x90;
            Data[0x094] = 0x90;
            Data[0x095] = 0xE0;
            #endregion
            #region E
            Data[0x096] = 0xF0;
            Data[0x097] = 0x80;
            Data[0x098] = 0xF0;
            Data[0x099] = 0x80;
            Data[0x09A] = 0xF0;
            #endregion
            #region F
            Data[0x09B] = 0xF0;
            Data[0x09C] = 0x80;
            Data[0x09D] = 0xF0;
            Data[0x09E] = 0x80;
            Data[0x09F] = 0x80;
            #endregion
        }
    }
}
