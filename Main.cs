using System;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace ADB_Tools
{
    public partial class Main : Form
    {
        string push_file, push_file_name, url_remote, permission = "644";
        static bool isDeviceSelected = false;
        static string device_serial = null;

        public Main()
        {
            InitializeComponent();
            if (Utils.is_ADBRunning())
            {
                richTextBox1.Text += "ADB is running...!\n";
                triggerDeviceUpdate();
            }
            else
            {
                richTextBox1.Text += "ADB not running...!\n";
                richTextBox1.Text += "Starting ADB server...!\n";
                Utils.startADB();
                triggerDeviceUpdate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Utils.is_ADBRunning())
            {
                richTextBox1.Text += "Starting ADB...!\n";
                Utils.startADB();
            }
            else
            {
                richTextBox1.Text += "ADB already running...\nKilling...!\n";
                if (!Utils.killADB())
                {
                    richTextBox1.Text += "Error Killing adb server...!";
                }
                richTextBox1.Text += "Starting new server...!\n";
                Utils.startADB();
            }
            triggerDeviceUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Utils.is_ADBRunning())
            {
                richTextBox1.Text += "Killing ADB server...!\n";
                if (!Utils.killADB())
                    richTextBox1.Text += "Error killing ADB, is server running?\n";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("ADB Tools\t\t\nVersion 0.1\t\t\nDeveloper: Harsh Panchal <panchal.harsh18@gmail.com>\t\t", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            var form_abt = new About();
            form_abt.Show();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            adb_push_select.Title = "Choose file to push";
            adb_push_select.ShowDialog();
            push_file = adb_push_select.FileName;
            push_file_name = adb_push_select.SafeFileName;
            file_url.Text = push_file;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                clear_remote_path();
                groupBox2.Visible = true;
                url_remote = "/system" + "/" + push_file_name;
                update_status_push_dialog();
            }
            else
            {
                groupBox2.Visible = false;
            }
        }

        private void clear_remote_path()
        {
            url_remote = "";
            update_status_push_dialog();
        }

        private void update_status_push_dialog()
        {
            richTextBox2.Text = url_remote;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                clear_remote_path();
                url_remote = "/data" + "/" + push_file_name;
                update_status_push_dialog();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                clear_remote_path();
                url_remote = "/preload" + "/" + push_file_name;
                update_status_push_dialog();
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                clear_remote_path();
                url_remote = "/" + push_file_name;
                update_status_push_dialog();
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                clear_remote_path();
                url_remote = "/system/app" + "/" + push_file_name;
                update_status_push_dialog();
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                clear_remote_path();
                url_remote = "/system/lib" + "/" + push_file_name;
                update_status_push_dialog();
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
            {
                clear_remote_path();
                url_remote = "/system/bin" + "/" + push_file_name;
                update_status_push_dialog();
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                clear_remote_path();
                url_remote = "/system/xbin" + "/" + push_file_name;
                update_status_push_dialog();
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
            {
                clear_remote_path();
                url_remote = "/system/framework" + "/" + push_file_name;
                update_status_push_dialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            List<string> op = Utils.listDevices();
            foreach (string s in op)
            {
                comboBox1.Items.Add(s);
            }
        }

        public void triggerDeviceUpdate()
        {

            comboBox1.Items.Clear();
            List<string> op = Utils.listDevices();
            foreach (string s in op)
            {
                comboBox1.Items.Add(s);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text += "Selected device:\t" + comboBox1.Text +"\n";
            isDeviceSelected = true;
            device_serial = comboBox1.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var wireless_conn = new Wireless_prompt(this);
            wireless_conn.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string op_buf = null;

            if (push_file == null || push_file == "")
            {
                MessageBox.Show("Select file to push...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                richText_file.Text += "File to push not selected...\n";
                return;
            }
            url_remote = richTextBox2.Text;
            if(url_remote == null || url_remote == "")
            {
                MessageBox.Show("Remote path not selected...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                richText_file.Text += "Remote location not selected...\n";
                return;
            }
            if (!Utils.is_ADBRunning())
            {
                MessageBox.Show("ADB not running, start ADB server first & then selecte Device...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(!isDeviceSelected)
            {
                MessageBox.Show("Device not selected, selecte your device first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            richText_file.Text += "Pushing " + push_file + " to " + url_remote + "\n";
            Process pusher = new Process();
            pusher.StartInfo.UseShellExecute = false;
            pusher.StartInfo.CreateNoWindow = true;
            pusher.StartInfo.RedirectStandardOutput = true;
            pusher.StartInfo.RedirectStandardError = true;
            pusher.StartInfo.FileName = "adb.exe";
            pusher.StartInfo.Arguments = "-s " + device_serial + " " + "push " + push_file + " " + url_remote;
            pusher.Start();
            pusher.WaitForExit();
            op_buf = pusher.StandardOutput.ReadToEnd();
            richText_file.Text += op_buf + "\n";
            op_buf = pusher.StandardError.ReadToEnd();
            richText_file.Text += op_buf + "\n";
            if (pusher.ExitCode != 0)
            {
                richText_file.Text += "Error occured while pushing file...!" + "\n";
                return;
            }
            if (url_remote.StartsWith("/sdcard/"))
            {
                richText_file.Text += "Changing file permissions on sdcard is not possible...!\nSkipping...\n";
                MessageBox.Show("Changing file permissions on sdcard is not possible...!\n", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                richText_file.Text += "Changing permission to " + permission + "\n";
                pusher.StartInfo.FileName = "adb.exe";
                pusher.StartInfo.Arguments = "shell chmod " + permission + " " + url_remote;
                pusher.Start();
                pusher.WaitForExit();
                op_buf = pusher.StandardOutput.ReadToEnd();
                richText_file.Text += op_buf + "\n";
            }
            richText_file.Text += "Operation completed...!" + "\n";
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton14.Checked)
                permission = "644";
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton13.Checked)
                permission = "755";
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton12.Checked)
                permission = "777";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.killADB();
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentMessageWindow().Text = "";
        }

        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog1.FileName = "log.txt";
            saveFileDialog1.Filter = "Text Files|*.txt|All Files|*.";
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string path = saveFileDialog1.FileName;
                getCurrentMessageWindow().SaveFile(path, RichTextBoxStreamType.PlainText);
            }
        }

        private RichTextBox getCurrentMessageWindow()
        {
            if (tabControl1.SelectedIndex == 0)
                return richTextBox1;
            else if (tabControl1.SelectedIndex == 1)
                return richText_file;
            else
                return new RichTextBox();   //dummy textbox
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
