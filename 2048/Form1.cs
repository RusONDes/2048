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

namespace _2048
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        const int fieldSize = 4;
        const int raitingTableSlots = 5;
         int usedRaitingTadle = 0;
        Button[,] field;

        string[,] results;
        string playerName;  
        private string resultsMsg;

        public Form1()
        {
            InitializeComponent();
            field = new Button[fieldSize, fieldSize];
            results = new string[raitingTableSlots, 2];
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
        private void GenerateNum()
        {
            
            int emptyFilelds = 0;
            int x = 0 , y = 0;
            foreach (Button b in field)
            {
                if (b.Text.Equals(""))
                {
                    emptyFilelds++;
                }
            }
            if (emptyFilelds > 0)
            {
                do
                {
                    x = rnd.Next(4);
                    y = rnd.Next(4);

                } while (!field[x, y].Text.Equals(""));
                int percent = rnd.Next(11);
                if (percent < 9)
                {
                    field[x, y].Text = "2";
                    label1.Text = (int.Parse(label1.Text) + 2).ToString();
                }
                else
                {
                    field[x, y].Text = "4";
                    label1.Text = (int.Parse(label1.Text) + 4).ToString();
                }
            }
            else
            {
                GameOver();
            }
        }
        private void GameOver()
        {
            int score = int.Parse(label1.Text);
            string resultMsg = "";
            if(usedRaitingTadle >= raitingTableSlots)
            {
                if (score > int.Parse(results[raitingTableSlots -1 , 0]))
                {
                    results[raitingTableSlots -1, 0]=score.ToString();
                    results[raitingTableSlots - 1, 1] = playerName;

                }
                else
                {
                    results[usedRaitingTadle , 0] = score.ToString();
                    results[usedRaitingTadle - 1, 1] = playerName;
                    usedRaitingTadle++;
                }
                Sort();
                for (int i = 0; i < results.Length/2; i++)
                {
                    resultsMsg += results [i, 0] + " " + results[i, 1] + "n";
                }
                MessageBox.Show(resultMsg);
                WriteResults();
                reset();
            }
        }
        private void reset()
        {
            foreach(Button b in field)
            {
                b.Text = "";
            }
            groupBox1.Enabled = false;
            groupBox2.Enabled = true;
            label1.Text = "";
            textBox1.Text = "";
        }
        private void ReadResults()
        {
            try
            {
                string res;
                FileStream file = new FileStream(@"result.txt", FileMode.OpenOrCreate);
                StreamReader reader = new StreamReader(file);
                for (int i = 0;!reader.EndOfStream ; i++)
                {
                    usedRaitingTadle++;
                    res = reader.ReadLine();
                    results[i, 0] += res.Split(' ')[0];
                    results[i, 0] = res.Split(' ')[1];
                }
                reader.Close();
                file.Close();
                Sort();
            }
            catch 
            {
                MessageBox.Show("БОБЕР ПОМЕР");
            }
        }
        private void Sort()

        {
            string tempOne;
            string tempTwo;
            for (int i = 0; i < raitingTableSlots; i++)
            {
                for (int j = i + 1; j < raitingTableSlots; j++)
                {
                    if (int.Parse(results[i, 0]) < int.Parse(results[j, 0]))
                    {
                        tempOne = results[j, 0];
                        tempTwo = results[j, 1];
                        results[i, 0] = results[j, 0];
                        results[i, 0] = results[j, 1];
                        results[j, 0] = tempOne;
                        results[j, 1] = tempTwo;
                    }
                }
            }
        }
        private void WriteResults()
        {
            using (StreamWriter writen = new StreamWriter(@"results.txt"))
            {
                for (int i = 0; i < usedRaitingTadle ; i++)
                {
                    writen.WriteLine(results[i,0]+ " " + results);
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) == false)
            {
                button17.Enabled = true;
            }
            else
            {
                button17.Enabled = false;
            }
        }
        private void button17_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                playerName= textBox1.Text;
                button17.Enabled = false;
                groupBox1.Enabled = true;
                groupBox2.Enabled = false;
                GenerateNum();
                GenerateNum();
            }
            else
            {
                MessageBox.Show("Ввидите имя ");
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Right:
                    MoveRight();
                    GenerateNum();
                    break;

                case Keys.Left:
                    MoveLeft();
                    GenerateNum();
                    break;
                case Keys.Up:
                    MoveUp();
                    GenerateNum();
                    break;
                case Keys.Down:
                    MoveDovn();
                    GenerateNum();
                    break;
            }
            
            
        }
        private void MoveRight()
        {
            for (int v = 0; v < fieldSize; v++)
            {
                for (int i = 0; i < fieldSize; i++)
                {
                    for (int j = 0; j < fieldSize - 1; j++)
                    {
                        if (field[i, j + 1].Text.Equals(""))
                        {
                            field[i, j + 1].Text = field[i, j].Text;
                            field[i, j].Text = "";
                        }
                        else if (field[i, j ].Text.Equals(field[i, j + 1 ].Text))
                        {
                            field[i,j + 1 ].Text = (int.Parse(field[i,j].Text)*2 ).ToString();
                            field[i,j ].Text = "";
                        }
                    }
                }
            }
        }
        private void MoveLeft()
        {
            for (int P = 0; P < fieldSize; P++)
            {
                for (int i = 0; i < fieldSize; i++)
                {
                    for (int j = fieldSize  - 1; j > 0 ;  j-- )
                    {
                        if (field[i, j - 1].Text.Equals(""))
                        {
                            field[i, j - 1].Text = field[i, j].Text;
                            field[i, j].Text = "";
                        }
                        else if (field[i, j].Text.Equals(field[i, j - 1 ].Text))
                        {
                            field[i, j - 1].Text = (int.Parse(field[i, j].Text) * 2).ToString();
                            field[i, j].Text = "";
                        }
                    }
                }
            }
        }
        private void MoveDovn()
        {
            for (int P = 0; P < fieldSize; P++)
            {
                for (int i = 0; i < fieldSize - 1; i++)
                {
                    for (int j = 0; j < fieldSize ; j++)
                    {
                        if (field[i + 1, j ].Text.Equals(""))
                        {
                            field[i + 1,j ].Text = field[i, j].Text;
                            field[i, j].Text = "";
                        }
                        else if (field[i, j].Text.Equals(field[i + 1, j ].Text))
                        {
                            field[i + 1, j ].Text = (int.Parse(field[i, j].Text) * 2).ToString();
                            field[i, j].Text = "";
                        }
                    }
                }
            }
        }
        private void MoveUp()
        {
            for (int P = 0; P < fieldSize; P++)
            {
                for (int i =  fieldSize - 1;i >0; i--)
                {
                    for (int j = 0; j < fieldSize ; j++)
                    {
                        if (field[i - 1, j].Text.Equals(""))
                        {
                            field[i - 1, j].Text = field[i, j].Text;
                            field[i, j].Text = "";
                        }
                        else if (field[i, j].Text.Equals(field[i - 1, j].Text))
                        {
                            field[i - 1, j].Text = (int.Parse(field[i, j].Text) * 2).ToString();
                            field[i, j].Text = "";
                        }
                    }
                }
            }
        }
    }
}
