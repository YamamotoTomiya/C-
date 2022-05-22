using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;


namespace ArduinoMonitor
{
    public partial class Form1 : Form
    {
        private ManualResetEvent IsNotReading = new ManualResetEvent(true);
        String[] separator_newline = new string[] { "\r\n" };
        Series[] serial_rcv_msg;
        public Form1()
        {
            InitializeComponent();
            InitializeSerialComponent();
        }
        private void InitializeSerialComponent()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                comboBox_COMselect.Items.Add(portName);
            }
            if (comboBox_COMselect.Items.Count > 0)
            {
                comboBox_COMselect.SelectedIndex = 0;
                comboBox_COMselect.Enabled = true;
                comboBox_baudrate.Enabled = true;
                comboBox_newline.Enabled = true;
                button_serial_conn.Enabled = true;

            }
        }
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
       
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button_serial_conn_Click(object sender, EventArgs e)
        {
            if (button_serial_conn.Enabled==true)
            {
                serialPort_Arduino.PortName = comboBox_COMselect.SelectedItem.ToString();
                serialPort_Arduino.BaudRate = int.Parse(comboBox_baudrate.SelectedItem.ToString());
                switch (comboBox_newline.SelectedItem.ToString())
                {
                    case "GR+LF":
                        serialPort_Arduino.NewLine = "\r\n";
                        break;

                    case "CR":
                        serialPort_Arduino.NewLine = "\r";
                        break;

                    case "LF":
                        serialPort_Arduino.NewLine = "\n";
                        break;

                    default:
                        return;

                }
                serialPort_Arduino.Parity = System.IO.Ports.Parity.None;
                serialPort_Arduino.DataBits = 8;
                serialPort_Arduino.StopBits = System.IO.Ports.StopBits.One;
                serialPort_Arduino.Handshake = System.IO.Ports.Handshake.RequestToSend;
                serialPort_Arduino.Encoding = Encoding.UTF8;
                serialPort_Arduino.DtrEnable = true;

                System.Threading.Thread.Sleep(500);
                serialPort_Arduino.Open();
                serialPort_Arduino.DiscardInBuffer();
                button_serial_conn.Enabled = false;
                button_serial_disconn.Enabled = true;
                button_send_msg.Enabled = true;

                label_serial_status.Text = "connected";

            }
        }

        private void serialPort_Arduino_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort_Arduino.IsOpen == false)
            {
                return;
            }
            try
            {
                this.IsNotReading.Reset();
                string data = serialPort_Arduino.ReadLine();
                if (!string.IsNullOrEmpty(data))
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        textBox_rcv_msg.AppendText(data + "\r\n");
                    }));
                    String[] splitted = data.Split(separator_newline, StringSplitOptions.RemoveEmptyEntries);
                    String[] digits = splitted[0].Split(',');
                    int numeric_count = 0;
                    for(int j=0;j<digits.Length;j++)
                    {
                        if(double.TryParse(digits[j],out double tmp_digits))
                        {
                            numeric_count++;
                        }
                    }
                    B
                }
                this.IsNotReading.Set();
                Thread.Sleep(50);
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private void label8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_serial_disconn_Click(object sender, EventArgs e)
        {
            if (serialPort_Arduino.IsOpen == true)
            {
                this.IsNotReading.WaitOne();
                serialPort_Arduino.Close();
                button_serial_conn.Enabled = true;
                button_serial_disconn.Enabled = false;
                button_send_msg.Enabled = false;
                label_serial_status.Text = "disconnected";

            }
        }

        private void button_send_msg_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox_send_msg.Text))
            {
                Serial_send_msg(textBox_send_msg.Text);
                textBox_send_msg.Text = null;
                textBox_rcv_msg.Focus();
            }
        }
        private void Serial_send_msg(string msg)
        {
            try
            {
                serialPort_Arduino.WriteLine(msg);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "送信エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox_send_msg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Serial_send_msg(textBox_rcv_msg.Text);
                textBox_send_msg.Text = null;
                textBox_rcv_msg.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = "新しいファイル.txt";
            sfd.InitialDirectory = @"C:\";
            sfd.Filter = "テキスト　ファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            sfd.Title = "保存先のファイルを選択してください";
            sfd.RestoreDirectory = true;

            sfd.ShowDialog();

            StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.ASCII);
            sw.Write(textBox_rcv_msg.Text);
            sw.Close();
            MessageBox.Show("Saved.");
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void textBox_send_msg_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (serialPort_Arduino.IsOpen)
            {
                this.IsNotReading.WaitOne();
                serialPort_Arduino.Close();
            }
        }
    }
}
