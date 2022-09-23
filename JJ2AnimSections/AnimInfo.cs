using System;
using System.Collections.Generic;
using System.Text;

namespace JJ2AnimLib.JJ2AnimSections
{
    public struct AnimInfo //Data1
    {
        public const int SIZE = 8;
        public short FrameCount;   // Number of frames for this particular animation
        public short FPS;          // Most likely frames per second
        public int Reserved;      // Used internally by Jazz2.exe

        public int FramesStartIndex { get; set; }
        public AnimInfo(short frameCount, short fps, int reserved, int startFrame = -1)
        {
            FrameCount = frameCount;
            FPS = fps;
            Reserved = 0;
            FramesStartIndex = startFrame;
        }
    }
}
