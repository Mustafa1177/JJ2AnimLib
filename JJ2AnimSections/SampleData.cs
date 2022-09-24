using System;
using System.Collections.Generic;
using System.Text;

namespace JJ2AnimLib.JJ2AnimSections
{
    public class SampleData
    {
        public int TotalSize; //SampleSize + 4
        public byte[] Buffer;

        public int SampleSize { get { return Buffer.Length; } }
        public int SampleID { get; set; }

        public SampleData(int totSize = 0, int smpID = -1)
        {
            TotalSize = totSize;
            Buffer = new byte[totSize - 4];
            SampleID = smpID;
        }
    }
}
