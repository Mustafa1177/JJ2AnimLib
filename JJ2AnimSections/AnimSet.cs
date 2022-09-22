using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;

namespace JJ2AnimLib.JJ2AnimSections
{
    public class AnimSet
    {
        public ANIM_Header Header { get; set; }
        public AnimInfo Animations { get; set; }
        public FrameInfo Frames { get; set; }
        public ImageData Images { get; set; }
        public SampleData Samples { get; set; }

        private byte[] Data1 = { };
        private byte[] Data2 ={ };
        private byte[] Data3= { };
        private byte[] Data4 ={ };

        public int GetSize
        {
            get { return Header != null? Header.GetSize + Header.CData1 + Header.CData2 + Header.CData3 + Header.CData4 : 0; }
        }

        public bool Read(byte[] mem, int offset)
        {
            Header = new ANIM_Header();
            if (Header.Read(mem,offset))
            {
                offset += Header.GetSize;
                Data1 = GeneralFunctions.Unzip(mem, offset, Header.CData1);
                offset+= Header.CData1;
                Data1 = GeneralFunctions.Unzip(mem, offset, Header.CData2);
                offset += Header.CData2;
                Data1 = GeneralFunctions.Unzip(mem, offset, Header.CData3);
                offset += Header.CData3;
                Data1 = GeneralFunctions.Unzip(mem, offset, Header.CData4);
                offset += Header.CData4;

                return true;
            }
            return false;
        }
    }
}
