using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADB_Tools
{
    public partial class Wireless_prompt : Form
    {
        private Main parent = null;

        public Wireless_prompt(Main parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "adb.exe";
            if(textBox2.Text != null || textBox2.Text != "")
                p.StartInfo.Arguments = "connect " + textBox1.Text;
            else
                p.StartInfo.Arguments = "connect " + textBox1.Text + ":" + textBox2.Text;
            p.Start();
            p.WaitForExit();
            parent.triggerDeviceUpdate();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
