using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Text;

namespace Test1
{
    class SerialPortManager
    {
        //System.IO.Ports.SerialPort port;
        SerialPort serialPort;
        string portName;
        int portBaudrate;
        Parity portParity;
        StopBits portStopBits;
        Handshake portHandshake;
        RichTextBox console;
        byte[] buffer;
        byte packetsComplete;
        byte packetSize;

        public bool isDownloading = false;
        public bool isUploading = false;

        public void InitializeSerialPort(string pn, string pb, string pp, string ps, string ph, RichTextBox con)
        {
            serialPort = new SerialPort();
            portName = pn;
            portBaudrate = int.Parse(pb);
            portParity = (Parity)System.Enum.Parse(typeof(Parity), pp);
            portStopBits = (StopBits)System.Enum.Parse(typeof(StopBits), ps);
            portHandshake = (Handshake)System.Enum.Parse(typeof(Handshake), ph);
            console = con;
            serialPort.PortName = portName;
            serialPort.BaudRate = portBaudrate;
            serialPort.Parity = portParity;
            serialPort.DataBits = 8;
            serialPort.StopBits = portStopBits;
            serialPort.DataReceived += DataReceived;
            //serialPort.Handshake = portHandshake;

            //===========================================
            //Specjalne ustawienia dla Arduino Micro
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            //===========================================
            packetsComplete = 0;
            packetSize = 30;

        }
        public void ConnectToDevice()
        {
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }

        }
        public void Send(string data)
        {
            serialPort.Write(data);
        }
        public void Send(byte[] data)
        {
            serialPort.Write(data,0,data.Length);
        }
        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (isDownloading)
            {
                Thread.Sleep(1000);
                int bytes = serialPort.BytesToRead;
                buffer = new byte[bytes];
                serialPort.Read(buffer, 0, bytes);

                isDownloading = false;
                AnimationSystem.Core.UpdateBuffer(buffer);
                AnimationSystem.Core.ConvertBufferToWS2812();
                AnimationSystem.Core.ShowPreview();
                AnimationSystem.EffectsController.FillEffectCombo();
                console.Invoke(new System.Action(delegate () { console.AppendText("Done\n"); }));
            }
            else
            {
                int bytes = serialPort.BytesToRead;
                buffer = new byte[bytes];
                serialPort.Read(buffer, 0, bytes);
                string data = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                if (isUploading)
                {
                    if (data == "ACK")
                    {
                        //wyślij kolejną porcję danych
                        if (packetsComplete < 15)
                        {
                            SendPatternPackets();
                        }
                    }
                    else if (data == "FIN")
                    {
                        isUploading = false;
                        packetsComplete = 0;
                        data = "";
                    }
                    else
                    {
                        console.Invoke(new System.Action(delegate () { console.AppendText(data); }));
                    }
                }
                else
                {
                    console.Invoke(new System.Action(delegate () { console.AppendText(data); }));
                }
            }
        }
        public void SendPatternPackets()
        {
            byte[] packet = new byte[packetSize];
            for (int i = 0; i < packetSize; i++)
            {
                packet[i] = AnimationSystem.Core.buffer[(packetSize*packetsComplete) +i];
            }
            Send(packet);
            packetsComplete++;
            console.Invoke(new System.Action(delegate () { console.AppendText("Packet " + packetsComplete + "/15 " + (packetsComplete*100/15)+"%\n"); }));
        }
        public void ClearPackets()
        {
            packetsComplete = 0;
        }
        public void TurnOnLeds(int pattern)
        {
            switch (pattern)
            {
                case 0:
                    Send("ON_PATTERN_1");
                    break;
                case 1:
                    Send("ON_PATTERN_2");
                    break;
                case 2:
                    Send("ON_PATTERN_3");
                    break;
                case 3:
                    Send("ON_PATTERN_4");
                    break;
                case 4:
                    Send("ON_PATTERN_5");
                    break;
                case 5:
                    Send("ON_PATTERN_6");
                    break;
                case 6:
                    Send("ON_PATTERN_7");
                    break;
                case 7:
                    Send("ON_PATTERN_8");
                    break;
                case 8:
                    Send("ON_PATTERN_9");
                    break;
                case 9:
                    Send("ON_PATTERN_10");
                    break;
                default:
                    break;

            }
        }
        public void TurnOffLeds()
        {
            Send("OFF_PATTERN");
        }
        public void FillAvailablePorts(ComboBox combo)
        {
            combo.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            //combo.SelectedIndex = 0;
        }
        public void FillBaudRates(ComboBox combo)
        {
            combo.Items.Add("1200");
            combo.Items.Add("2400");
            combo.Items.Add("4800");
            combo.Items.Add("9600");
            combo.Items.Add("14400");
            combo.Items.Add("19200");
            combo.Items.Add("38400");
            combo.Items.Add("57600");
            combo.Items.Add("115200");
            combo.SelectedIndex = combo.Items.IndexOf("9600");
        }
        public void FillParity(ComboBox combo)
        {
            foreach (var item in System.Enum.GetValues(typeof(System.IO.Ports.Parity)))
            {
                combo.Items.Add(item);
            }
            combo.SelectedIndex = 0;
        }
        public void FillStopBits(ComboBox combo)
        {
            foreach (var item in System.Enum.GetValues(typeof(System.IO.Ports.StopBits)))
            {
                combo.Items.Add(item);
            }
            combo.SelectedIndex = 0;
        }
        public void FillHandshake(ComboBox combo)
        {
            foreach (var item in System.Enum.GetValues(typeof(System.IO.Ports.Handshake)))
            {
                combo.Items.Add(item);
            }
            combo.SelectedIndex = 0;
        }
    }
}
