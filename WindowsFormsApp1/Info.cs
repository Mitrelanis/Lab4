using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Info : Form
    {
        public TextBox Folder;
        public TextBox Date;
        public Info()
        {
            InitializeComponent();
            InitializeComponents();
        }

        public void InitializeComponents()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            int width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.3);
            int height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.2);
            this.Size = new System.Drawing.Size(width, height);

            Folder = textBox1;
            Date = textBox2;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
