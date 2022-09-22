using System;
using System.Collections.Generic;
using System.Text;

namespace JJ2AnimLib.JJ2AnimSections
{
    public struct FrameInfo //Data2
    {
        short Width;
        short Height;
        short ColdspotX;    // Relative to hotspot
        short ColdspotY;    // Relative to hotspot
        short HotspotX;
        short HotspotY;
        short GunspotX;     // Relative to hotspot
        short GunspotY;     // Relative to hotspot
        int ImageAddress;  // Address in Data3 where image starts
        int MaskAddress;   // Address in Data3 where mask starts
    }
}
