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
            this.Text = "Admin Tools";
            this.Size = new Size(600, 400); // Adjust the size as necessary

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
            // Update these paths to match your actual directories
            string batchScriptsPath = @"C:\Users\Abhiiously\Documents\GitHub\admintools\data\scripts\batch";
            string powershellScriptsPath = @"C:\Users\Abhiiously\Documents\GitHub\admintools\data\scripts\powershell";

            MessageBox.Show($"Loading Batch scripts from: {batchScriptsPath}"); // Debugging line
            LoadScriptsFromDirectory(batchScriptsPath, "*.bat");

            MessageBox.Show($"Loading PowerShell scripts from: {powershellScriptsPath}"); // Debugging line
            LoadScriptsFromDirectory(powershellScriptsPath, "*.ps1");
        }

        private void LoadScriptsFromDirectory(string path, string pattern)
        {
            if (!Directory.Exists(path))
            {
                MessageBox.Show($"Directory not found: {path}", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var scriptFiles = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);

            if (scriptFiles.Length == 0)
            {
                MessageBox.Show($"No scripts found in: {path}", "Scripts Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                // Determine which command processor to use based on file extension
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
                MessageBox.Show($"Failed to run script: {scriptPath}\nError: {ex.Message}", "Script Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
