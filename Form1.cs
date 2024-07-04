using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace AdminTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            Button clickMeButton = new Button
            {
                Text = "Click Me",
                Location = new System.Drawing.Point(100, 100),
                Size = new System.Drawing.Size(100, 50)
            };

            clickMeButton.Click += ClickMeButton_Click;
            Controls.Add(clickMeButton);
        }

        private void ClickMeButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello from WinForms!");
        }
    }
}
