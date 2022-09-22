using System;
using System.Collections.Generic;
using System.Text;
using static System.BitConverter;

namespace JJ2AnimLib.JJ2AnimSections
{
    public class ALIB_Header
    {
       public char[] Magic = new char[4];     // Magic number
        public int Unknown1; // Little endian, unknown purpose
        public int HeaderSize;            // Equals 464 bytes for v1.23 Anims.j2a
        public short Version;     // Probably means v2.0
        public short Unknown2;    // Unknown purpose
        public int FileSize;              // Equals 8182764 bytes for v1.23 anims.j2a
        public int CRC32;                 // Note: CRC buffer starts after the end of header
        public int SetCount;              // Number of sets in the Anims.j2a (109 in v1.23)
        public int[] SetAddress = new int[0];  // Each set's starting address within the file

        public ALIB_Header()
        {
        }

        public bool Read(byte[] mem, int offset = 0)
        {
            Magic = new char[4];
            Array.Copy(mem, offset, Magic, 0, Magic.Length);
            if (new string(Magic) == "ALIB")
            {
                Unknown1 = ToInt32(mem, offset + 4);
                HeaderSize = ToInt32(mem, offset + 8);
                Version = ToInt16(mem, offset + 12);
                Unknown2 = ToInt16(mem, offset + 14);
                FileSize = ToInt32(mem, offset + 16);
                CRC32 = ToInt32(mem, offset + 20);
                SetCount = ToInt32(mem, offset + 24);
                SetAddress = new int[SetCount];
                offset += 28;
                for (int i = 0; i < SetCount; i++)
                {
                    SetAddress[i] = ToInt32(mem, offset);
                    offset += 4;
                }
                return true;
            }
            return false;
        }

        public int GetSize {
            get { return 28 + SetCount * 4;}
        }
    }
}
