using System;
using System.Collections.Generic;
using System.Text;

namespace JJ2AnimLib.JJ2AnimSections
{
    public class FrameInfo //Data2
    {
        public const int SIZE = 24;
       public short Width;
        public short Height;
        public short ColdspotX;    // Relative to hotspot
        public short ColdspotY;    // Relative to hotspot
        public short HotspotX;
        public short HotspotY;
        public short GunspotX;     // Relative to hotspot
        public short GunspotY;     // Relative to hotspot
        public int ImageAddress;  // Address in Data3 where image starts
        public int MaskAddress;   // Address in Data3 where mask starts

        public byte[,] Img8Bit;
        public byte[,] Mask;
        public FrameInfo(short w, short h, short coldspotX, short coldspotY, short hotspotX, short hotspotY, short gunspotX, short gunspotY, int imgAddress, int maskAddress)
        {
            Width = w;
            Height = h;
            ColdspotX = coldspotX;
            ColdspotY = coldspotY;
            HotspotX = hotspotX;
            HotspotY = hotspotY;
            GunspotX = gunspotX;
            GunspotY = gunspotY;
            ImageAddress = imgAddress;
            MaskAddress = maskAddress;
        }
    }
}
