using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnimationSystem;

namespace Test1
{
    public partial class Main : Form
    {
        bool DEBUG_LOCK = false;

        //============================================================
        //TESTING
        SerialPortManager spm = new SerialPortManager();
        //============================================================
        public Main()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //======================================
            //TODO: Połączyć w jeden konstruktor
            spm.FillAvailablePorts(SerialPortBox);
            spm.FillBaudRates(SerialBaudRateBox);
            spm.FillParity(SerialParityBox);
            spm.FillStopBits(SerialStopBitsBox);
            spm.FillHandshake(SerialHandshakeBox);
            //======================================
            Core.AddLedPreviewTable(tableLayoutPanel1, tableLayoutPanel2);
            Core.AddLedSelector(selectLedCombo);
            EffectsController.AddEffectPreview(panel321);
            EffectsController.AddEffectSelect(effectSelectCombo);
            EffectsController.AddEffectPoints(comboBox2,comboBox3);
            EffectsController.Initialize();
            AnimationEngine.Initialize();
            //======================================

            tabControl1.TabPages[1].Enabled = false;
            patternSelectCombo.SelectedIndex = 0;

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            spm.InitializeSerialPort(SerialPortBox.Text, SerialBaudRateBox.Text, SerialParityBox.Text, SerialStopBitsBox.Text,SerialHandshakeBox.Text, SerialConsole);
            tabControl1.TabPages[1].Enabled = true;
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            spm.ConnectToDevice();
        }

        private void SerialSend_Click(object sender, EventArgs e)
        {
            spm.Send(ConsoleInput.Text);
            SerialConsole.Text += ">> " + ConsoleInput.Text+"\n";
            ConsoleInput.Text = "";
        }

        private void selectLedCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DEBUG_LOCK = true;
            int index = selectLedCombo.SelectedIndex;
            ledRed.Text = Core.getRedValueFromIndex(index).ToString();
            ledGreen.Text = Core.getGreenValueFromIndex(index).ToString();
            ledBlue.Text = Core.getBlueValueFromIndex(index).ToString();
            DEBUG_LOCK = false;
        }
        private void ledColor_TextChanged(object sender, EventArgs e)
        {
            if (DEBUG_LOCK)
            {
                return;
            }
            if (ledRed.Text == "")
            {
                return;
            }
            else if (ledGreen.Text == "")
            {
                return;
            }
            else if (ledBlue.Text == "")
            {
                return;
            }
            int index = selectLedCombo.SelectedIndex;
            /*
            Tutaj trzeba dodać zabezpieczenie przed wartościami błędnymi dla byte
            */
            byte r = byte.Parse(ledRed.Text);
            byte g = byte.Parse(ledGreen.Text);
            byte b = byte.Parse(ledBlue.Text);
            Core.SetPixelColor(index, r, g, b);
            Core.ShowPreview();
        }

        private void SerialConsole_TextChanged(object sender, EventArgs e)
        {
            SerialConsole.SelectionStart = SerialConsole.Text.Length;
            SerialConsole.ScrollToCaret();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = Color.Black;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                int index = selectLedCombo.SelectedIndex;
                Color temp = MyDialog.Color;
                ledRed.Text = temp.R.ToString();
                ledGreen.Text = temp.G.ToString();
                ledBlue.Text = temp.B.ToString();
            }
        }

        private void loadPattern_Click(object sender, EventArgs e) //przenieść do SerialPortManager
        {
            int pattern = patternSelectCombo.SelectedIndex;
            SerialConsole.Text += "Downloading Pattern "+(pattern+1).ToString()+"...\n";
            spm.isDownloading = true;
            switch (pattern)
            {
                case 0:
                    spm.Send("GET_PATTERN_1");
                    break;
                case 1:
                    spm.Send("GET_PATTERN_2");
                    break;
                case 2:
                    spm.Send("GET_PATTERN_3");
                    break;
                case 3:
                    spm.Send("GET_PATTERN_4");
                    break;
                case 4:
                    spm.Send("GET_PATTERN_5");
                    break;
                case 5:
                    spm.Send("GET_PATTERN_6");
                    break;
                case 6:
                    spm.Send("GET_PATTERN_7");
                    break;
                case 7:
                    spm.Send("GET_PATTERN_8");
                    break;
                case 8:
                    spm.Send("GET_PATTERN_9");
                    break;
                case 9:
                    spm.Send("GET_PATTERN_10");
                    break;
                default:
                    SerialConsole.Text += "Unable to download pattern\n";
                    spm.isDownloading = false;
                    break;

            }
        }

        private void savePattern_Click(object sender, EventArgs e)  //przenieść do SerialPortManager
        {
            SerialConsole.Text += "Enabling Writing Mode...\n";
            Core.ConvertWS2812ToBuffer();
            int pattern = patternSelectCombo.SelectedIndex;
            spm.isUploading = true;
            spm.ClearPackets();
            switch (pattern)
            {
                case 0:
                    spm.Send("SET_PATTERN_1");
                    Thread.Sleep(100);
                    spm.SendPatternPackets();
                    break;
                case 1:
                    spm.Send("SET_PATTERN_2");
                    Thread.Sleep(100);
                    spm.SendPatternPackets(); 
                    break;
                case 2:
                    spm.Send("SET_PATTERN_3");
                    Thread.Sleep(100);
                    spm.SendPatternPackets(); 
                    break;
                case 3:
                    spm.Send("SET_PATTERN_4");
                    Thread.Sleep(100);
                    spm.SendPatternPackets(); 
                    break;
                case 4:
                    spm.Send("SET_PATTERN_5");
                    Thread.Sleep(100);
                    spm.SendPatternPackets(); 
                    break;
                case 5:
                    spm.Send("SET_PATTERN_6");
                    Thread.Sleep(100);
                    spm.SendPatternPackets(); 
                    break;
                case 6:
                    spm.Send("SET_PATTERN_7");
                    Thread.Sleep(100);
                    spm.SendPatternPackets(); 
                    break;
                case 7:
                    spm.Send("SET_PATTERN_8");
                    Thread.Sleep(100);
                    spm.SendPatternPackets(); 
                    break;
                case 8:
                    spm.Send("SET_PATTERN_9");
                    Thread.Sleep(100);
                    spm.SendPatternPackets();
                    break;
                case 9:
                    spm.Send("SET_PATTERN_10");
                    Thread.Sleep(100);
                    spm.SendPatternPackets();
                    break;
                default:
                    break;

            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int effect = effectSelectCombo.SelectedIndex;
            EffectsController.SetSelectedEffect(effect);
            EffectsController.Refresh();

        }
        private void button7_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = Color.Black;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                Color temp = MyDialog.Color;
                EffectsController.SetColor(0, temp);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = Color.Black;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                Color temp = MyDialog.Color;
                EffectsController.SetColor(1, temp);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = Color.Black;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                Color temp = MyDialog.Color;
                EffectsController.SetColor(2, temp);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = Color.Black;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                Color temp = MyDialog.Color;
                EffectsController.SetColor(3, temp);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EffectsController.ApplyEffect();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            EffectsController.UndoEffect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int pattern = patternSelectCombo.SelectedIndex;
            spm.TurnOnLeds(pattern);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            spm.TurnOffLeds();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int animation = comboBox1.SelectedIndex;
            AnimationEngine.AnimationChanged(animation);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AnimationEngine.Enable();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AnimationEngine.Disable();
        }

        private void button14_Click(object sender, EventArgs e)//AnimationEngine base color
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = Color.Black;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                Color temp = MyDialog.Color;
                AnimationEngine.SetAnimationColor(0, temp);
            }
        }

        private void button15_Click(object sender, EventArgs e)//AnimationEngine comp1
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = Color.Black;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                Color temp = MyDialog.Color;
                AnimationEngine.SetAnimationColor(1, temp);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;
            MyDialog.Color = Color.Black;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                Color temp = MyDialog.Color;
                AnimationEngine.SetAnimationColor(2, temp);
            }
        }
    }
}
