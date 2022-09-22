using System;
using System.Collections.Generic;
using System.Text;

namespace JJ2AnimLib.JJ2AnimSections
{
    public class AnimSet
    {
        public ANIM_Header Header { get; set; }
        public AnimInfo Animations { get; set; }
        public FrameInfo Frames { get; set; }
        public ImageData Images { get; set; }
        public SampleData Samples { get; set; }

        private byte[] Data1;
        private byte[] Data2;
        private byte[] Data3;
        private byte[] Data4;

        public bool Read(byte[] mem, int offset)
        {
            Header = new ANIM_Header();
            if (Header.Read(mem,))
            {

            }
            return false;
        }
    }
}
