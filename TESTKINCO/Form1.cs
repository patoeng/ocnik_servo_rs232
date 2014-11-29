using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TESTKINCO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public byte[] Buffers;
        private string _rxString="";
        private void button1_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            serialPort1.Open();
            Buffers = new byte[] {0x01,0x40,0x20,0x20,0x09,0x40, 0x05, 0x0A, 0xFF,00};
            int fc = Buffers.Aggregate(0, (current, i) => current + i);
            var z = -fc & 0xff;
           
            Buffers[9] = Convert.ToByte(z);
            serialPort1.Write(Buffers, 0, 10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();
        }
        
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            var ii = (SerialPort) sender;
            ii.Read(Buffers, 0, 10);
            //_rxString = serialPort1.ReadExisting(); 
            Invoke(new EventHandler(DisplayText));
        }

       
        private void DisplayText(object sender, EventArgs e)
        {
          
              //  textBox1.AppendText(BitConverter.ToString(Encoding.UTF8.GetBytes(_rxString)) + "\r\n");

            textBox1.AppendText(BitConverter.ToString(Buffers) + "\r\n");
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
                serialPort1.Open();
            Buffers = new byte[] { 0x01, 0x2F, 0xF0, 0x01, 0x01, 0x01, 0x00, 00, 00, 00 };
            int fc = 0;
            foreach (byte i in Buffers)
            {
                fc += i;
            }
            var z = -fc & 0xff;
            //  z = z.Remove(0, z.Length - 2);
            //  MessageBox.Show(z.ToString());
            Buffers[9] = Convert.ToByte(z);
            serialPort1.Write(Buffers, 0, 10);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1.ReceivedBytesThreshold = 10;
            serialPort1.PortName = "COM11";
            serialPort1.BaudRate = 19200; //default 38400
            serialPort1.StopBits = StopBits.One;
            serialPort1.Parity = Parity.None;
        }
    }
}
