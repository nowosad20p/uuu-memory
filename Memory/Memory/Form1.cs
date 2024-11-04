using Memory.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class Form1 : Form
    {
        int gameSize;
        List<Image> images = new List<Image>
        {
            Resources.mem1,
            Resources.mem11,
            Resources.mem2,
            Resources.mem3,
            Resources.mem4,
            Resources.mem5,
            Resources.mem6,
            Resources.mem7

        };
        public Form1()
        {
            InitializeComponent();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void nowaGraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 ilosc = new Form2();
          
            if (ilosc.ShowDialog(this) == DialogResult.OK)
            {
                gameSize =  int.Parse(ilosc.textBox1.Text);
                MessageBox.Show(ilosc.textBox1.Text);
            }
            startGame();
        }
        private void startGame()
        {
            Tuple<int, int> gameDimensions = FindClosestDivisors(gameSize*2);
            gamePanel.RowCount = gameDimensions.Item1;
            gamePanel.ColumnCount = gameDimensions.Item2;
            for (int i = 0; i < gameDimensions.Item1; i++)
            {
                for (int j = 0; j < gameDimensions.Item2; j++)
                {
                  var img =  new PictureBox { Image = images[new Random().Next(0, images.Count)]};
                    gamePanel.Controls.Add(img, j,i);
                }
            }
        }
        static Tuple<int, int> FindClosestDivisors(int n)
        {
            int sqrtN = (int)Math.Sqrt(n); 
            for (int i = sqrtN; i >= 1; i--) {
                if (n % i == 0) {
                    int divisor1 = i;
                    int divisor2 = n / i;
                    return Tuple.Create(divisor1, divisor2); } 
            }
            return Tuple.Create(1, n); 
        }
    }
}
