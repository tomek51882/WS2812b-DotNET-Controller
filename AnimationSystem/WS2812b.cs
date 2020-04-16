using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationSystem
{
    public class WS2812b
    {
        public byte red;
        public byte green;
        public byte blue;

        public WS2812b()
        {
            red = 0;
            green = 0;
            blue = 0;
        }
        public WS2812b(byte r, byte g, byte b)
        {
            red = r;
            green = g;
            blue = b;
        }
    }
}
