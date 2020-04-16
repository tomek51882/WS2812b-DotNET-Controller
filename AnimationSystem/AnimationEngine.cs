using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AnimationSystem
{
    public static class AnimationEngine
    {
        static Timer timer;
        static int frameDuration;
        static int selectedAnimation;
        static Color[] animationColors;

        static int frame;


        public static void Initialize()
        {
            timer = new Timer();
            frameDuration = 75;
            selectedAnimation = 0;
            frame = 0;
            timer.Elapsed += new ElapsedEventHandler(Tick);
            timer.Interval = frameDuration;

            Core.animationLeds = new WS2812b[150];
            for(int i=0;i<150;i++)
            {
                Core.animationLeds[i] = new WS2812b();
            }
            Core.ShowPreviewSecond();

            animationColors = new Color[4];
            for (int i = 0; i < 4; i++)
            {
                animationColors[i] = Color.White;
            }
        }
        public static void Enable()
        {
            timer.Start();
        }
        public static void Disable()
        {
            timer.Stop();
        }

        static void Tick(object source, ElapsedEventArgs e)
        {
            switch (selectedAnimation)
            {
                case 0:
                    Clear();
                    SetPixelColor(frame % 150, 255, 255, 255);
                    frame++;
                    break;
                case 1:
                    Clear(animationColors[0]);
                    Spark(frame % 150, 10, animationColors[1]);
                    frame++;
                    break;
                case 2:
                    Clear();
                    Wave(frame % 150, 15, animationColors[0]);
                    frame++;
                    break;
                case 3:
                    Clear(animationColors[0]);
                    Wave(frame % 150, 15,animationColors[1]);
                    //Wave(frame % 150, 10,Wheel((byte)(frame%255)));
                    frame++;
                    break;
            }
            
            Core.ShowPreviewSecond();
        }

        public static void AnimationChanged(int id)
        {
            selectedAnimation = id;
            Disable();
        }
        public static void SetPixelColor(int index, byte r, byte g, byte b)
        {
            if (r > 255)
            {
                r = 255;
            }
            if (g > 255)
            {
                g = 255;
            }
            if (b > 255)
            {
                b = 255;
            }
            if (r < 0)
            {
                r = 0;
            }
            if (g < 0)
            {
                g = 0;
            }
            if (b < 0)
            {
                b = 0;
            }

            Core.animationLeds[index].red = r;
            Core.animationLeds[index].green = g;
            Core.animationLeds[index].blue = b;
        }
        public static void SetPixelColor(int index, WS2812b newPixel)
        {
            Core.animationLeds[index] = newPixel;
        }
        public static void SetAnimationColor(int index, Color col)
        {
            animationColors[index] = col;
        }
        static void Clear()
        {
            for (int i = 0; i < 150; i++)
            {
                SetPixelColor(i, 0, 0, 0);
            }
            Core.ShowPreviewSecond();
        }
        static void Clear(Color col)
        {
            for (int i = 0; i < 150; i++)
            {
                SetPixelColor(i, col.R, col.G, col.B);
            }
            Core.ShowPreviewSecond();
        }
        static void Wave(int pos, int len, Color col)
        {
            //======================================================
            //TODO: Zamiast zamalowywać modyfikowany obszar, odjac 
            //od obecnego BaseColor wartości tworząc gradient 
            //BaseColor-Black
            //======================================================
            float tempRed; //= (float)col.R / len;
            float tempGreen; //= (float)col.G / len;
            float tempBlue; //= (float)col.B / len;
            WS2812b tempLed;

            //Zamalowanie modyfikowanego obszaru na czarno
            for (int i = 0; i <= len; i++) 
            {
                if (pos - i >= 0)
                {
                    SetPixelColor(pos - i, 0, 0, 0);
                }
                if (pos + i < 150)
                {
                    SetPixelColor(pos + i, 0, 0, 0);
                }
            }
            //Namalowanie na modyfikowanym obszarze gradientu Black-CompColor1
            for (int i = 0; i <= len; i++) 
            {
                if (pos - i >= 0)
                {
                    tempLed = Core.animationLeds[pos - i];
                    tempRed = (float)col.R / len * (len - i);
                    tempGreen = (float)col.G / len * (len - i);
                    tempBlue = (float)col.B / len * (len - i);

                    if (tempLed.red + tempRed > 255)
                    {
                        tempLed.red = 255;
                    }
                    else
                    {
                        tempLed.red += (byte)tempRed;
                    }
                    if (tempLed.green + tempGreen > 255)
                    {
                        tempLed.green = 255;
                    }
                    else
                    {
                        tempLed.green += (byte)tempGreen;
                    }
                    if (tempLed.blue + tempBlue > 255)
                    {
                        tempLed.blue = 255;
                    }
                    else
                    {
                        tempLed.blue += (byte)tempBlue;
                    }
                    SetPixelColor(pos - i, tempLed);
                    //SetPixelColor(pos - i, Convert.ToByte(tempRed), Convert.ToByte(tempGreen), Convert.ToByte(tempBlue));
                }
                if (pos + i < 150)
                {
                    tempLed = Core.animationLeds[pos + i];
                    tempRed = (float)col.R / len * (len - i);
                    tempGreen = (float)col.G / len * (len - i);
                    tempBlue = (float)col.B / len * (len - i);

                    if (tempLed.red + tempRed > 255)
                    {
                        tempLed.red = 255;
                    }
                    else
                    {
                        tempLed.red += (byte)tempRed;
                    }
                    if (tempLed.green + tempGreen > 255)
                    {
                        tempLed.green = 255;
                    }
                    else
                    {
                        tempLed.green += (byte)tempGreen;
                    }
                    if (tempLed.blue + tempBlue > 255)
                    {
                        tempLed.blue = 255;
                    }
                    else
                    {
                        tempLed.blue += (byte)tempBlue;
                    }
                    SetPixelColor(pos + i, tempLed);
                    //SetPixelColor(pos + i, Convert.ToByte(tempRed * (len - i)), Convert.ToByte(tempGreen * (len - i)), Convert.ToByte(tempBlue * (len - i)));
                }
            }
            //Namalowanie na modyfikowanym obszarze gradientu BaseColor-Black
            for (int i = len; i >=0; i--) 
            {
                if (pos - i >= 0)
                {
                    tempLed = Core.animationLeds[pos - i];
                    tempRed = (float)animationColors[0].R / len * i;
                    tempGreen = (float)animationColors[0].G / len * i;
                    tempBlue = (float)animationColors[0].B / len * i;

                    if (tempLed.red + tempRed > 255)
                    {
                        tempLed.red = 255;
                    }
                    else
                    {
                        tempLed.red += (byte)tempRed;
                    }
                    if (tempLed.green + tempGreen > 255)
                    {
                        tempLed.green = 255;
                    }
                    else
                    {
                        tempLed.green += (byte)tempGreen;
                    }
                    if (tempLed.blue + tempBlue > 255)
                    {
                        tempLed.blue = 255;
                    }
                    else
                    {
                        tempLed.blue += (byte)tempBlue;
                    }
                    SetPixelColor(pos - i, tempLed);
                    //SetPixelColor(pos - i, Convert.ToByte(tempRed), Convert.ToByte(tempGreen), Convert.ToByte(tempBlue));
                }
                if (pos + i < 150)
                {
                    tempLed = Core.animationLeds[pos + i];
                    tempRed = (float)animationColors[0].R / len *i;
                    tempGreen = (float)animationColors[0].G / len * i;
                    tempBlue = (float)animationColors[0].B / len * i;

                    if (tempLed.red + tempRed > 255)
                    {
                        tempLed.red = 255;
                    }
                    else
                    {
                        tempLed.red += (byte)tempRed;
                    }
                    if (tempLed.green + tempGreen > 255)
                    {
                        tempLed.green = 255;
                    }
                    else
                    {
                        tempLed.green += (byte)tempGreen;
                    }
                    if (tempLed.blue + tempBlue > 255)
                    {
                        tempLed.blue = 255;
                    }
                    else
                    {
                        tempLed.blue += (byte)tempBlue;
                    }
                    SetPixelColor(pos + i, tempLed);
                    //SetPixelColor(pos + i, Convert.ToByte(tempRed * (len - i)), Convert.ToByte(tempGreen * (len - i)), Convert.ToByte(tempBlue * (len - i)));
                }
            }
            Core.ShowPreviewSecond();
        }
        static void Spark(int pos, int len, Color col)
        {
            float tempRed; //= (float)col.R / len;
            float tempGreen; //= (float)col.G / len;
            float tempBlue; //= (float)col.B / len;
            WS2812b tempLed;
            for (int i = 0; i <= len; i++) //Zamalowanie modyfikowanego obszaru na czarno
            {
                if (pos - i >= 0)
                {
                    SetPixelColor(pos - i, 0, 0, 0);
                }
            }
            for (int i = 0; i <= len; i++) //Namalowanie na modyfikowanym obszarze gradientu Black-CompColor1
            {
                if (pos - i >= 0)
                {
                    tempLed = Core.animationLeds[pos - i];
                    tempRed = (float)col.R / len * (len - i);
                    tempGreen = (float)col.G / len * (len - i);
                    tempBlue = (float)col.B / len * (len - i);

                    if (tempLed.red + tempRed > 255)
                    {
                        tempLed.red = 255;
                    }
                    else
                    {
                        tempLed.red += (byte)tempRed;
                    }
                    if (tempLed.green + tempGreen > 255)
                    {
                        tempLed.green = 255;
                    }
                    else
                    {
                        tempLed.green += (byte)tempGreen;
                    }
                    if (tempLed.blue + tempBlue > 255)
                    {
                        tempLed.blue = 255;
                    }
                    else
                    {
                        tempLed.blue += (byte)tempBlue;
                    }
                    SetPixelColor(pos - i, tempLed);
                    //SetPixelColor(pos - i, Convert.ToByte(tempRed), Convert.ToByte(tempGreen), Convert.ToByte(tempBlue));
                }
            }
        }
        static WS2812b Wheel(byte WheelPos)
        {
            WheelPos = (byte)(255 - WheelPos);
            if (WheelPos < 85)
            {
                return new WS2812b((byte)(255 - WheelPos * 3), 0, (byte)(WheelPos * 3));
            }
            if (WheelPos < 170)
            {
                WheelPos -= 85;
                return new WS2812b(0, (byte)(WheelPos * 3), (byte)(255 - WheelPos * 3));
            }
            WheelPos -= 170;
            return new WS2812b((byte)(WheelPos * 3), (byte)(255 - WheelPos * 3), 0);
        }
    }
}
