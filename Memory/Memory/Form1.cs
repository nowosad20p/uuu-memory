using Memory.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace Memory
{
    public partial class Form1 : Form
    {
        int gameSize;
        List<Image> images = new List<Image>
        {
            Resources.mem1,
            Resources.mem2,
            Resources.mem3,
            Resources.mem4,
            Resources.mem5,
            Resources.mem6,
            Resources.mem7

        };
        Image background = Resources.pilka_nozna_215cm_bialyczarny_MO9007_33;
        Tuple<int, int> card1;
        Tuple<int, int> card2;
       
        List<int[]> gameBoard;
        List<PictureBox[]> gameImages;
        int curTurn = 0;
        int[] score = new int[2] { 0, 0 };
        bool waiting = false;
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
         public static void ShuffleGameBoard(List<int[]> gameBoard, Tuple<int, int> gameDimensions)
        {
            List<int> flattened = new List<int>(); 
            for (int i = 0; i < gameDimensions.Item1; i++) { for (int j = 0; j < gameDimensions.Item2; j++)
                {
                    flattened.Add(gameBoard[i][j]);
                } }
            Random rand = new Random();
            for (int i = flattened.Count - 1; i > 0; i--)
            { int j = rand.Next(0, i + 1); 
                int temp = flattened[i]; flattened[i] = flattened[j]; flattened[j] = temp; }
        
            int index = 0;
            for (int i = 0; i < gameDimensions.Item1; i++)
            { for (int j = 0; j < gameDimensions.Item2; j++) { gameBoard[i][j] = flattened[index++]; } }

        }
            private void startGame()
        {

           

                Tuple<int, int> gameDimensions = FindClosestDivisors(gameSize*2);
            gameBoard = new List<int[]>();
            gameImages = new List<PictureBox[]>();
            int n = 0;
            int k = 0;
            for(int i=0; i<gameDimensions.Item1; i++)
            {
                gameBoard.Add(new int[gameDimensions.Item2]);
                gameImages.Add(new PictureBox[gameDimensions.Item2]);
                for(int j = 0; j < gameDimensions.Item2; j++)
                {
                    gameBoard[i][j] = n;
                    n += (k% 2);
                    k++;
                }
            }
      
            ShuffleGameBoard(gameBoard,gameDimensions);
            foreach(int[] i in gameBoard)
            {
               foreach(int j in i)
                {
                  
                }
            }
            gamePanel.RowCount = gameDimensions.Item1;
            gamePanel.ColumnCount = gameDimensions.Item2;
            for (int i = 0; i < gamePanel.ColumnCount; i++) 
            {
                gamePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / gamePanel.RowCount)); 
            }
            for (int j = 0; j < gamePanel.RowCount; j++) 
            {
                gamePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100/gamePanel.RowCount)); 
            }
            for (int i = 0; i < gameDimensions.Item1; i++)
            {
                for (int j = 0; j < gameDimensions.Item2; j++)
                {
                    int row = i;
                    int col = j;
                    var img =  new PictureBox { Image = background , Parent = gamePanel, SizeMode = PictureBoxSizeMode.StretchImage,Width = gamePanel.Width/gamePanel.ColumnCount, Height = gamePanel.Width/gamePanel.ColumnCount};
                    img.Click += (sender, e) =>
                    {
                        flip(row,col);
                    };
                    gameImages[i][j] = img;
                    gamePanel.Controls.Add(img, j,i);
              

                }
            }
        }
        void flip(int x, int y)
        {
            if(waiting) return;
            gameImages[x][y].Image = images[gameBoard[x][y]];
       
            if (card1 != null)
            {
               
                card2 = Tuple.Create(x, y);
                
                if (gameBoard[card1.Item1][card1.Item2] == gameBoard[card2.Item1][card2.Item2])
                {
                    MessageBox.Show(card1.Item1+" "+card1.Item2+" "+card2.Item1 + " " + card2.Item2+ "traf");
                    score[curTurn]++;
                    if (score[0] + score[1] == gameSize)
                    {
                        if (score[0] == score[1])
                        {
                            MessageBox.Show("Remis","Wynik",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                        else
                        {


                            string winner = score[0] > score[1] ? "1" : "2";
                            MessageBox.Show("Wygrał gracz numer " + winner, "Wynik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    label2.Text = "Wynik: " + score[0].ToString() + ":" + score[1].ToString();
                    waiting = true;
                    Task.Delay(2000).ContinueWith(_ => {
                        gameImages[x][y].Invoke((Action)(() => gameImages[x][y].Visible = false));
                        gameImages[card1.Item1][card1.Item2].Invoke((Action)(() => gameImages[card1.Item1][card1.Item2].Visible = false));
                        card1 = null;
                        card2 = null;
                        waiting = false;
                    });
                }
                else
                {


                    if (curTurn == 0)
                    {
                        curTurn = 1;
                    }
                    else
                    {
                        curTurn = 0;
                    }
                    label1.Text = "Obecnie tura gracza " + (curTurn + 1).ToString();
                    waiting = true;
                    Task.Delay(2000).ContinueWith(_ => {
                        gameImages[x][y].Invoke((Action)(() => gameImages[x][y].Image = background));
                        gameImages[card1.Item1][card1.Item2].Invoke((Action)(() => gameImages[card1.Item1][card1.Item2].Image = background));
                        card1 = null;
                        card2 = null;
                        waiting = false;
                    });
                }

                
                
            }
            else
            {
                card1 = Tuple.Create(x, y);
                
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
