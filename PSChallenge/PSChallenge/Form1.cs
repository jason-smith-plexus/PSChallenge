using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.Visa;

namespace PSChallenge
{
    public partial class Form1 : Form
    {

        private MessageBasedSession mbSession;
        ResourceManager rmSession = new ResourceManager();

        public Form1()
        {
            InitializeComponent();

        }

        // Finds the resources available to use and adds to listbox
        private void Form1_Load(object sender, EventArgs e)
        {
            using (var rmSession = new ResourceManager())
            {
                var resources = rmSession.Find("(ASRL|GPIB|TCPIP|USB)?*");
                foreach (string s in resources)
                {
                    listBox.Items.Add(s);
                }
            }
        }

        // Connects to the resource and displays Active Voltages and Current Limits and Powers off the Power Supply
        private void openSession_Click(object sender, EventArgs e)
        {
            try
            {
                mbSession = (MessageBasedSession)rmSession.Open(textBox1.Text);
                MessageBox.Show("Resource Connected");

                mbSession.RawIO.Write("V1?\n");
                Act_Volt1.Text = mbSession.RawIO.ReadString().Remove(0, 3);

                mbSession.RawIO.Write("V2?\n");
                Act_Volt2.Text = mbSession.RawIO.ReadString().Remove(0, 3);

                mbSession.RawIO.Write("I1?\n");
                Act_Current1.Text = mbSession.RawIO.ReadString().Remove(0, 3);

                mbSession.RawIO.Write("I2?\n");
                Act_Current2.Text = mbSession.RawIO.ReadString().Remove(0, 3);

                mbSession.RawIO.Write("OP1 0\n");

                mbSession.RawIO.Write("OP2 0\n");
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Resource must be a message-based session");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

        }

        // Selects specific resource
        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            string selected = (string)listBox.SelectedItem;
            textBox1.Text = selected;
        }

        //Allow the button to turn on/off Output 1 and diplays the button as a color (Green = On, Red = Off)
        private void pButton1_Click(object sender, EventArgs e)
        {
            if (pButton1.BackColor == System.Drawing.Color.Green) {
                mbSession.RawIO.Write("OP1 0\n");
                pButton1.BackColor = System.Drawing.Color.Red;
            }
            else {
                mbSession.RawIO.Write("OP1 1\n");
                pButton1.BackColor = System.Drawing.Color.Green;
            }
        }

        //Allow the button to turn on/off Output 2 and diplays the button as a color (Green = On, Red = Off)
        private void pButton2_Click(object sender, EventArgs e)
        {
            if (pButton2.BackColor == System.Drawing.Color.Green)
            {
                mbSession.RawIO.Write("OP2 0\n");
                pButton2.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                mbSession.RawIO.Write("OP2 1\n");
                pButton2.BackColor = System.Drawing.Color.Green;
            }
        }

        //Sets the Voltage and Current Limit of the first output and throws error if Voltage or Current Limits are breached
        private void con_1_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(Ch_Volt1.Text) > 60 || Int32.Parse(Ch_Volt1.Text) < 0)
            {
                MessageBox.Show("Voltage Not Within Limits of 0 - 60");
                return;
            }
            if (Int32.Parse(Ch_Current1.Text) > 20 || Int32.Parse(Ch_Current1.Text) < 0)
            {
                MessageBox.Show("Current Not Within Limits of 0 - 20");
                return;
            }

            try
            {
                mbSession.RawIO.Write("V1 " + Ch_Volt1.Text + "\n");
                mbSession.RawIO.Write("V1?\n");
                Act_Volt1.Text = mbSession.RawIO.ReadString().Remove(0, 3);

                mbSession.RawIO.Write("I1 " + Ch_Current1.Text + "\n");
                mbSession.RawIO.Write("I1?\n");
                Act_Current1.Text = mbSession.RawIO.ReadString().Remove(0, 3);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

        }

        //Sets the Voltage and Current Limit of the second output and throws error if Voltage or Current Limits are breached
        private void con_2_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(Ch_Volt2.Text) > 60 || Int32.Parse(Ch_Volt2.Text) < 0)
            {
                MessageBox.Show("Voltage Not Within Limits of 0 - 60");
                return;
            }
            if (Int32.Parse(Ch_Current2.Text) > 20 || Int32.Parse(Ch_Current2.Text) < 0)
            {
                MessageBox.Show("Current Not Within Limits of 0 - 20");
                return;
            }
            try
            {
                mbSession.RawIO.Write("V2 " + Ch_Volt2.Text + "\n");
                mbSession.RawIO.Write("V2?\n");
                Act_Volt2.Text = mbSession.RawIO.ReadString().Remove(0, 3);

                mbSession.RawIO.Write("I2 " + Ch_Current2.Text + "\n");
                mbSession.RawIO.Write("I2?\n");
                Act_Current2.Text = mbSession.RawIO.ReadString().Remove(0, 3);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

    }
}
