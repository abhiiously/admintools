using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AdminTools
{
    public partial class Form1 : Form
    {
        private FlowLayoutPanel flowLayoutPanel1;

        public Form1()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            flowLayoutPanel1 = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            Controls.Add(flowLayoutPanel1);
            LoadScripts();
        }

        private void LoadScripts()
        {
            string scriptsPath = @"C:\Path\To\Scripts";  // Adjust path as needed
            flowLayoutPanel1.Controls.Clear(); // Clear existing buttons before loading new ones

            var scriptFiles = Directory.GetFiles(scriptsPath, "*.*", SearchOption.TopDirectoryOnly)
                                       .Where(s => s.EndsWith(".ps1") || s.EndsWith(".bat"));

            foreach (string script in scriptFiles)
            {
                Button scriptButton = new Button
                {
                    Text = Path.GetFileNameWithoutExtension(script),
                    Tag = script,  // Store script path for use in click event
                    Size = new Size(100, 50),
                    Margin = new Padding(10),
                    BackColor = Color.LightGray,
                    FlatStyle = FlatStyle.Flat
                };
                scriptButton.FlatAppearance.BorderSize = 0;
                scriptButton.Click += ScriptButton_Click;
                flowLayoutPanel1.Controls.Add(scriptButton);
            }
        }

        private void ScriptButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string scriptPath = clickedButton.Tag.ToString();

            try
            {
                Process.Start("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to run script: {scriptPath}\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
