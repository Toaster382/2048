using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp13
{
    public partial class Form1 : Form
    {
        const int filedSize = 4;
        const int maxRaitingSize = 5;
        int currentRaitingSize = 0;

        Button[,] field;
        Random rand;

        string[,] results;
        string name;
        public Form1()
        {
            InitializeComponent();
            field = new Button[filedSize, filedSize];
            rand = new Random();
            results = new string[maxRaitingSize, 2];

            ReadResults();

            field[0, 0] = button1;
            field[0, 1] = button2;
            field[0, 2] = button3;
            field[0, 3] = button4;

            field[1, 0] = button5;
            field[1, 1] = button6;
            field[1, 2] = button7;
            field[1, 3] = button8;

            field[2, 0] = button9;
            field[2, 1] = button10;
            field[2, 2] = button11;
            field[2, 3] = button12;

            field[3, 0] = button13;
            field[3, 1] = button14;
            field[3, 2] = button15;
            field[3, 3] = button16;
        }

        private void SortResults()
        {
            string tempScore;
            string tempName;

            for (int i = 0; i < currentRaitingSize; i++)
            {
                for (int j = i + 1; j < currentRaitingSize; j++)
                {
                    if (int.Parse(results[i, 0]) < int.Parse(results[j, 0]))
                    {
                        tempScore = results[i, 0];
                        tempName = results[i, 1];
                        results[i, 0] = results[j, 0];
                        results[i, 1] = results[j, 1];
                        results[j, 0] = tempScore;
                        results[j, 1] = tempName;
                    }
                }
            }
        }

        private void ResetField()
        {
            foreach (Button btn in field)
            {
                btn.Text = "";
            }
            fieldBox.Enabled = false;
            nameBox.Enabled = true;
            scoreLabel.Text = "0";
            nameTextBox.Text = "";
        }


        private void WriteResults()
        {
            using (StreamWriter sw = new StreamWriter(@"results.txt"))
            {
                for (int i = 0; i < currentRaitingSize; i++)
                {
                    sw.WriteLine(results[i, 0] + " " + results[i, 1]);
                }
            }
        }

        private void ReadResults()
        {
            try
            {
                string result;
                FileStream file = new FileStream(@"results.txt", FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(file);
                for (int i = 0; !sr.EndOfStream; i++)
                {
                    currentRaitingSize++;
                    result = sr.ReadLine();

                    results[i, 0] = result.Split(' ')[0];
                    results[i, 1] = result.Split(' ')[1];

                }
                sr.Close();
                file.Close();
            }
            catch
            {
                MessageBox.Show("Press f");
            }
            SortResults();
        }

        private void GameOver()
        {
            int score = int.Parse(scoreLabel.Text);
            string resultsMsg = "";
            if (currentRaitingSize >= maxRaitingSize)
            {
                if (score > int.Parse(results[maxRaitingSize - 1, 0]))
                {
                    results[maxRaitingSize - 1, 0] = score.ToString();
                    results[maxRaitingSize - 1, 1] = name;
                }
            }
            else
            {
                results[currentRaitingSize, 0] = score.ToString();
                results[currentRaitingSize, 1] = name;
                currentRaitingSize++;
            }

            SortResults();
            for (int i = 0; i < results.Length / 2; i++)
            {
                resultsMsg += results[i, 0] + " " + results[i, 1] + "\n";
            }
            MessageBox.Show(resultsMsg);
            WriteResults();
            ResetField();
        }

        private void GenerateNumber()
        {
            int emptyFields = 0;
            int i, j;

            foreach(Button b in field)
            {
                if (b.Text.Equals(""))
                {
                    emptyFields++;
                }
            }

            if(emptyFields > 0)
            {
                do
                {
                    i = rand.Next(4);
                    j = rand.Next(4);
                } while (!field[i, j].Text.Equals("") == false);

                int percent = rand.Next(11);
                if(percent < 9)
                {
                    field[i, j].Text = "2";
                    scoreLabel.Text = (int.Parse(scoreLabel.Text) + 2).ToString();
                }
                else
                {
                    field[i, j].Text = "4";
                    scoreLabel.Text = (int.Parse(scoreLabel.Text) + 4).ToString();
                }
            }
            else
            {
                GameOver();
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!nameTextBox.Text.Equals(""))
            {
                name = nameTextBox.Text;
                fieldBox.Enabled = true;
                nameBox.Enabled = false;

                GenerateNumber();
                GenerateNumber();
            }
            else
            {
                MessageBox.Show("Введите имя !");
            }
        }

        private void MoveRight()
        {
            for (int b = 0; b < filedSize; b++)
            {
                for (int i = 0; i < filedSize; i++)
                {
                    for (int j = 0; j < filedSize - 1; j++)
                    {
                        if (field[i, j + 1].Text.Equals(""))
                        {
                            field[i, j + 1].Text = field[i, j].Text;
                            field[i, j].Text = "";
                        }
                        else if (field[i, j].Text.Equals(field[i, j + 1].Text))
                        {
                            field[i, j + 1].Text = (int.Parse(field[i, j].Text) * 2).ToString();
                            field[i, j].Text = "";
                        }
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    MoveRight();
                    break;

                case Keys.A:
                    MoveRight();
                    break;

                case Keys.W:
                    MoveRight();
                    break;

                case Keys.S:
                    MoveRight();
                    break;
            }
            GenerateNumber();
        }
    }
}
