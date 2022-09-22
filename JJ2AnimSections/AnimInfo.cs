using System;
using System.Collections.Generic;
using System.Text;

namespace JJ2AnimLib.JJ2AnimSections
{
    public struct AnimInfo //Data1
    {
        short FrameCount;   // Number of frames for this particular animation
        short FPS;          // Most likely frames per second
        int Reserved;      // Used internally by Jazz2.exe
        public AnimInfo(short frameCount, short fps)
        {
            FrameCount = frameCount;
            FPS = fps;
            Reserved = 0;
        }
    }
}
