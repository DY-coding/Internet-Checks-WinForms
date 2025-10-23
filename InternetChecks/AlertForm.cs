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

namespace InternetChecks
{
    public partial class AlertForm : Form
    {
        public AlertForm()
        {
            InitializeComponent();
        }
        public void SetCase(string message, Color windowBackColor, Color textForeColor, Color textBackColor, Color buttonForeColor, Color buttonBackColor)
        {
            this.BackColor = windowBackColor;
            this.label1.Text = message;
            this.label1.BackColor = textBackColor;
            this.label1.ForeColor = textForeColor;
            this.button1.BackColor = buttonBackColor;
            this.button1.ForeColor = buttonForeColor;

            int x = (this.ClientSize.Width - label1.Width) / 2;
            int y = (this.ClientSize.Height - label1.Height) / 2 - 30;
            this.label1.Location = new System.Drawing.Point(x, y);

            x = (this.ClientSize.Width - button1.Width) / 2;
            y = (this.ClientSize.Height - button1.Height) / 2 + 40;
            this.button1.Location = new System.Drawing.Point(x, y);



            this.Invalidate();
        }

        private void AlertForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

            if(this.Owner is Form1 ownerForm)
            {
                ownerForm.ExternStopTimer3();
            }
        }
    }
}
