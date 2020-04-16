using System.Windows.Forms;
using System;
using System.Drawing;

namespace AnimationSystem
{
    public static class Core
    {
        public static int ledCount;
        public static byte[] buffer;
        public static WS2812b[] leds;
        public static WS2812b[] animationLeds;
        static TableLayoutPanel ledPreview;
        static TableLayoutPanel ledPreviewSecond;
        static ComboBox ledSelect;
        public static void UpdateBuffer(byte[] newBuffer)
        {
            buffer = newBuffer;
            ledCount = CalculateNumberOfLeds();
        }
        public static int CalculateNumberOfLeds() //poprawić, 
        {
            int temp = buffer.Length / 3;
            leds = new WS2812b[temp];
            //animationLeds = new WS2812b[temp];
            for (int i = 0; i < temp; i++)
            {
                leds[i] = new WS2812b();
                //animationLeds[i] = new WS2812b();
            }
            return temp;
        }
        public static void ConvertBufferToWS2812()
        {
            if (ledSelect != null)
            {
                ledSelect.Invoke(new System.Action(delegate () { AddItemsToLedSelect(); }));
            }
            if (leds.Length == ledCount)
            {
                for (int i = 0; i < ledCount; i++)
                {
                    leds[i].red = buffer[i * 3];
                    leds[i].green = buffer[i * 3 + 1];
                    leds[i].blue = buffer[i * 3 + 2];
                }
            }
        }
        public static void ConvertWS2812ToBuffer()
        {
            for (int i = 0; i < ledCount; i++)
            {
                buffer[3 * i] = leds[i].red;
                buffer[3 * i + 1] = leds[i].green;
                buffer[3 * i + 2] = leds[i].blue;
            }
        }
        public static void AddLedPreviewTable(TableLayoutPanel first, TableLayoutPanel second)
        {
            ledPreview = first;
            ledPreviewSecond = second;
        }
        public static void AddLedSelector(ComboBox combo)
        {
            ledSelect = combo;
        }
        public static void AddItemsToLedSelect()
        {
            if (ledCount > 0)
            {
                ledSelect.Items.Clear();
                for (int i = 0; i < Core.ledCount; i++)
                {
                    ledSelect.Items.Add("LED" + i.ToString());
                }
                ledSelect.SelectedIndex = 0;
            }
        }
        public static void ShowPreview()
        {
            int row;
            int column;
            for (int i = 0; i < ledCount; i++)
            {
                row = i % 32;
                column = i / 32;
                ledPreview.GetControlFromPosition(row, column).BackColor = Color.FromArgb(leds[i].red, leds[i].green, leds[i].blue);
                //ledPreviewSecond.GetControlFromPosition(row, column).BackColor = Color.FromArgb(leds[i].red, leds[i].green, leds[i].blue);
            }
        }
        public static void ShowPreviewSecond()
        {
            int row;
            int column;
            for (int i = 0; i < animationLeds.Length; i++)
            {
                row = i % 32;
                column = i / 32;
                ledPreviewSecond.GetControlFromPosition(row, column).BackColor = Color.FromArgb(animationLeds[i].red, animationLeds[i].green, animationLeds[i].blue);
            }
        }
        public static int getRedValueFromIndex(int index)
        {
            return leds[index].red;
        }
        public static int getGreenValueFromIndex(int index)
        {
            return leds[index].green;
        }
        public static int getBlueValueFromIndex(int index)
        {
            return leds[index].blue;
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

            leds[index].red = r;
            leds[index].green = g;
            leds[index].blue = b;
        }
        public static void SetPixelColor(int index, WS2812b newPixel)
        {
            leds[index] = newPixel;
        }

    }
}
