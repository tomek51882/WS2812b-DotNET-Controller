using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AnimationSystem
{
    public static class EffectsController
    {
        static Panel effectPreview;
        static ComboBox effectSelect;
        static int selectedEffect;
        static Color[] colors;
        static ComboBox effectStart;
        static ComboBox effectEnd;
        static WS2812b[] undoBuffer;
        public static void Initialize()
        {
            effectPreview.Paint += new PaintEventHandler(ShowEffect);
            colors = new Color[4];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.White;
            }
        }
        public static void AddEffectPreview(Panel pan)
        {
            effectPreview = pan;
        }
        public static void AddEffectSelect(ComboBox sel)
        {
            effectSelect = sel;
        }
        public static void AddEffectPoints(ComboBox start, ComboBox end)
        {
            effectStart = start;
            effectEnd = end;
        }
        public static void SetSelectedEffect(int selected)
        {
            selectedEffect = selected;
        }
        public static void Refresh()
        {
            effectPreview.Refresh();
        }
        public static void SetColor(int colorIndex, Color col)
        {
            colors[colorIndex] = col;
            Refresh();
        }
        public static void FillEffectCombo()
        {
            effectSelect.Invoke(new System.Action(delegate () { FillEffectComboInvoke(); }));
        }
        public static void FillEffectComboInvoke()
        {
            int count = Core.ledCount;
            effectStart.Items.Clear();
            effectEnd.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                effectStart.Items.Add("LED" + i.ToString());
                effectEnd.Items.Add("LED" + i.ToString());
            }
            effectStart.SelectedIndex = 0;
            effectEnd.SelectedIndex = 0;
        }
        public static void ApplyEffect()
        {
            //int effect = effectSelect.SelectedIndex; //selectedEffect
            int start = effectStart.SelectedIndex;
            int end = effectEnd.SelectedIndex;
            if (start > end)
            {
                string message = "Start Point cannot be greater than End Point!";
                string caption = "Incorrect Start Point";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
                return;
            }
            undoBuffer = Core.leds;
            switch (selectedEffect)
            {
                case 0: //fill
                    for (int i = start; i < end; i++)
                    {
                        Core.SetPixelColor(i, colors[0].R, colors[0].G, colors[0].B);
                    }
                    Core.ShowPreview();
                    break;
                case 1:
                    FadeIn(start, end, colors[0], false);
                    break;
                case 2:
                    FadeOut(start, end, colors[0], false);
                    break;
                case 3:
                    FadeIn(start, end, colors[0], true);
                    break;
                case 4:
                    FadeOut(start, end, colors[0], true);
                    break;
                case 5:
                    Gradient2C(start, end);
                    break;
                case 6:
                    Gradient3C(start,end);
                    break;
                case 7:
                    Gradient4C(start, end);
                    break;
            }

        }
        static void FadeIn(int start, int end, Color col, bool isAdditive)
        {
            int steps = end - start;
            int counter = 0;
            float tempRed;
            float tempGreen;
            float tempBlue;
            WS2812b tempLed;
            if (!isAdditive)
            {
                for (int i = start; i <= end; i++)
                {
                    tempRed = (float)col.R / steps * counter;
                    tempGreen = (float)col.G / steps * counter;
                    tempBlue = (float)col.B / steps * counter;
                    Core.SetPixelColor(i, (byte)(tempRed), (byte)(tempGreen), (byte)(tempBlue));
                    counter++;
                }
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    tempLed = Core.leds[i];
                    tempRed = (float)col.R / steps * counter;
                    tempGreen = (float)col.G / steps * counter;
                    tempBlue = (float)col.B / steps * counter;

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

                    Core.SetPixelColor(i, tempLed);
                    counter++;
                }
            }
            Core.ShowPreview();
        }
        static void FadeOut(int start, int end, Color col, bool isAdditive)
        {
            int steps = end - start;
            int counter = 0;
            float tempRed;
            float tempGreen;
            float tempBlue;
            WS2812b tempLed;
            if (!isAdditive)
            {
                for (int i = end; i >= start; i--)
                {
                    tempRed = (float)col.R / steps * counter;
                    tempGreen = (float)col.G / steps * counter;
                    tempBlue = (float)col.B / steps * counter;
                    Core.SetPixelColor(i, (byte)(tempRed), (byte)(tempGreen), (byte)(tempBlue));
                    counter++;
                }
            }
            else
            {
                for (int i = end; i >= start; i--)
                {
                    tempLed = Core.leds[i];
                    tempRed = (float)col.R / steps * counter;
                    tempGreen = (float)col.G / steps * counter;
                    tempBlue = (float)col.B / steps * counter;

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
                    Core.SetPixelColor(i, tempLed);
                    counter++;
                }
            }
            Core.ShowPreview();
        }
        static void Gradient2C(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Core.SetPixelColor(i, 0, 0, 0);
            }
            FadeOut(start, end, colors[0], true);
            FadeIn(start, end, colors[1], true);
        }
        static void Gradient3C(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Core.SetPixelColor(i, 0, 0, 0);
            }
            int part = end / 2;
            FadeOut(start, part, colors[0], true);
            FadeIn(start, part, colors[1], true);
            FadeOut(part, end, colors[1], true);
            FadeIn(part, end, colors[2], true);
        }
        static void Gradient4C(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Core.SetPixelColor(i, 0, 0, 0);
            }
            int part = end / 3;
            FadeOut(start, part, colors[0], true);
            FadeIn(start, part, colors[1], true);
            FadeOut(part, part*2, colors[1], true);
            FadeIn(part, part*2, colors[2], true);
            FadeOut(part * 2, end, colors[2], true);
            FadeIn(part * 2, end, colors[3], true);
        }
        public static void UndoEffect()
        {
            Core.leds = undoBuffer;
            Core.ShowPreview();
        }

        public static void ShowEffect(object sender, PaintEventArgs e)
        {
            LinearGradientBrush linearGradientBrush;
            ColorBlend cblend;
            switch (selectedEffect)
            {
                case 0: //fill
                    linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, colors[0], colors[0], 0f);
                    e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);
                    break;
                case 1: //fade in
                    linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, Color.Black, colors[0], 0f);
                    cblend = new ColorBlend(2);
                    cblend.Colors = new Color[2] { Color.Black, colors[0] };
                    cblend.Positions = new float[2] { 0f, 1f };
                    linearGradientBrush.InterpolationColors = cblend;
                    e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);
                    break;
                case 2: //fade out
                    linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, colors[0], Color.Black, 0f);
                    cblend = new ColorBlend(2);
                    cblend.Colors = new Color[2] { colors[0], Color.Black };
                    cblend.Positions = new float[2] { 0f, 1f };
                    linearGradientBrush.InterpolationColors = cblend;
                    e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);
                    break;
                case 3: //fade in (transparent)
                    linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, Color.Black, colors[0], 0f);
                    cblend = new ColorBlend(2);
                    cblend.Colors = new Color[2] { Color.Black, colors[0] };
                    cblend.Positions = new float[2] { 0f, 1f };
                    linearGradientBrush.InterpolationColors = cblend;
                    e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);
                    break;
                case 4: //fade out (transparent)
                    linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, colors[0], Color.Black, 0f);
                    cblend = new ColorBlend(2);
                    cblend.Colors = new Color[2] { colors[0], Color.Black };
                    cblend.Positions = new float[2] { 0f, 1f };
                    linearGradientBrush.InterpolationColors = cblend;
                    e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);
                    break;
                case 5: //gradient 2c
                    linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, colors[0], colors[1], 0f);
                    cblend = new ColorBlend(2);
                    cblend.Colors = new Color[2] { colors[0], colors[1] };
                    cblend.Positions = new float[2] { 0f, 1f };
                    linearGradientBrush.InterpolationColors = cblend;
                    e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);
                    break;
                case 6: //gradient 3c
                    linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, colors[0], colors[1], 0f);
                    cblend = new ColorBlend(3);
                    cblend.Colors = new Color[3] { colors[0], colors[1], colors[2] };
                    cblend.Positions = new float[3] { 0f,0.5f, 1f };
                    linearGradientBrush.InterpolationColors = cblend;
                    e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);
                    break;
                case 7: //gradient 4c
                    linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, colors[0], colors[1], 0f);
                    cblend = new ColorBlend(3);
                    cblend.Colors = new Color[4] { colors[0], colors[1], colors[2],colors[3] };
                    cblend.Positions = new float[4] { 0f,0.33f, 0.66f, 1f };
                    linearGradientBrush.InterpolationColors = cblend;
                    e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);
                    break;
                default:

                    break;
            }
            //LinearGradientBrush linearGradientBrush = new LinearGradientBrush(effectPreview.ClientRectangle, Color.Red, Color.Yellow, 0f);
            //ColorBlend cblend = new ColorBlend(3);
            //cblend.Colors = new Color[3] { Color.Red, Color.Yellow, Color.Green };
            //cblend.Positions = new float[3] { 0f, 0.5f, 1f };

            //linearGradientBrush.InterpolationColors = cblend;

            //e.Graphics.FillRectangle(linearGradientBrush, effectPreview.ClientRectangle);

            //effectPreview.Refresh();
        }
    }
}
