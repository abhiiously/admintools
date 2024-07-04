using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

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
            string batchScriptsPath = @"C:\Users\Abhiiously\Documents\GitHub\admintools\data\scripts\batch";  // Adjust path for batch scripts
            string powershellScriptsPath = @"C:\Users\Abhiiously\Documents\GitHub\admintools\data\scripts\powershell";  // Adjust path for PowerShell scripts

            // Load Batch scripts
            LoadScriptsFromDirectory(batchScriptsPath, "*.bat");

            // Load PowerShell scripts
            LoadScriptsFromDirectory(powershellScriptsPath, "*.ps1");
        }

        private void LoadScriptsFromDirectory(string path, string pattern)
        {
            var scriptFiles = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);

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
                // Adjust the process start info based on script extension
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = scriptPath.EndsWith(".ps1") ? "powershell.exe" : "cmd.exe",
                    Arguments = scriptPath.EndsWith(".ps1") ? $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"" : $"/c \"{scriptPath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process process = Process.Start(psi);
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to run script: {scriptPath}\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
