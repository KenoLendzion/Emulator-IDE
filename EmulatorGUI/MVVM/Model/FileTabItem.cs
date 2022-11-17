using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorGUI.MVVM.Model
{
    internal class FileTabItem
    {
        public string FileName { get; set; }
        public string FullFilePath { get; set; }
        public string Content { get; set; }
    }
}
