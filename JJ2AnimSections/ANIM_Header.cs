using System;
using System.Collections.Generic;
using System.Text;
using static System.BitConverter;

namespace JJ2AnimLib.JJ2AnimSections
{
    public class ANIM_Header //44-byte
    {
      public  char[] Magic = new char[4];         // Magic number
        public byte AnimationCount;   // Number of animations in set
        public byte SampleCount;      // Number of sound samples in set
        public short FrameCount;               // Total number of frames in set
        public int PriorSampleCount;             // Total number of sound sample across all sets preceding this one
        public int CData1;                    // Compressed size of Data1
        public int UData1;                    // Uncompressed size of Data1
        public int CData2;                    // Compressed size of Data2
        public int UData2;                    // Uncompressed size of Data2
        public int CData3;                    // Compressed size of Data3
        public int UData3;                    // Uncompressed size of Data3
        public int CData4;                    // Compressed size of Data4
        public int UData4;                    // Uncompressed size of Data4


        public int GetSize
        {
            get { return 44; }
        }

        public bool Read(byte[] mem, int offset)
        {
            Magic = new char[4];
            Array.Copy(mem, offset, Magic, 0, Magic.Length);
            if (new string(Magic) == "ANIM")
            {
               // using (var reader = new System.IO.BinaryReader(new System.IO.MemoryStream(mem))){ }
                AnimationCount = mem[offset + 4];
                SampleCount = mem[offset + 5];
                FrameCount = ToInt16(mem, offset + 6);
                PriorSampleCount = ToInt32(mem, offset + 8);
                CData1 = ToInt32(mem, offset + 12);
                UData1 = ToInt32(mem, offset + 16);
                CData1 = ToInt32(mem, offset + 20); 
                UData2 = ToInt32(mem, offset + 24);
                CData2 = ToInt32(mem, offset + 28);
                UData3 = ToInt32(mem, offset + 32);
                CData3 = ToInt32(mem, offset + 36);
                UData4 = ToInt32(mem, offset + 40);
                CData4 = ToInt32(mem, offset + 44);
                return true;
            }
            return false;
        }
    }


}
