using System.Drawing;
using System.Windows.Forms;

namespace task
{
    public partial class MainForm : Form
    {
        private Pen MyPen;
        
        public MainForm()
        {
            InitializeComponent();
            MyPen = new Pen(Color.Black);
        }
        
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}