using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace НейроннаяСеть
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class Num
        {
            public int[,] input;
            public int[,] weight;
            public int weightedSum = 0;
            public int limit = 500;
            public float m = 0;
            public float N = 0;

            public string path = "";

            public Num(int x, int y, int[,] inputData)
            {
                weight = new int[x, y];
                input = new int[x, y];
                input = inputData;
            }

            public void Add()
            {
                weightedSum = 0;
                for (int x = 0; x < 32; x++)
                {
                    for (int y = 0; y < 32; y++)
                    {
                        weightedSum += input[x, y] * weight[x, y];
                    }
                }
            }

            public bool Guiltyness()
            {
                bool guilty = (weightedSum >= limit);
                return guilty;
            }

            public void Punishment(int[,] inputData, bool guilty)
            {
                Guiltyness();
                for (int x = 0; x < 32; x++)
                {
                    for (int y = 0; y < 32; y++)
                    {
                        if (guilty)
                        {
                            weight[x, y] -= inputData[x, y];
                        }
                        else
                        {
                            weight[x, y] += inputData[x, y];
                        }
                    }
                }
            }
        }

        public int[,] input = new int[32, 32];
        Num Perc;

        public void Distinction()
        {
            Perc.Add();
            listBox2.Items.Add("");
            Perc.Guiltyness();
            if (Perc.Guiltyness())
            {
                Perc.m++;
                listBox2.Items.Clear();
                listBox2.Items.Add("Верно. Взвешенная сумма = " + Convert.ToString(Perc.weightedSum));
                //
                //"Вероятность распознавания: " + (Perc.m / Perc.N)
            }
            else
            {
                listBox2.Items.Clear();
                listBox2.Items.Add("Неверно. Взвешенная сумма = " + Convert.ToString(Perc.weightedSum));
                //
                //"Вероятность распознавания: " + (Perc.m / Perc.N)
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            Perc = new Num(32, 32, input);

            string filePath = "";
            StreamReader sr;

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\bikhi_27b5q2u\Desktop\Предметы\4 курс\8 семестр\Интеллектуальные системы\Сеть\",
                Filter = "Текстовые файлы (*.txt)|*.txt",
                Title = "Файл весов"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
            }
            if (filePath != "")
            {
                sr = File.OpenText(@filePath);
                Perc.path = filePath;

                string line;
                string[] s1;
                int k = 0;

                while ((line = sr.ReadLine()) != null) 
                {
                    s1 = line.Split(' '); 
                    for (int i = 0; i < s1.Length; i++)
                    {
                        listBox1.Items.Add("");
                        if (k < s1.Length)
                        {
                            Perc.weight[i, k] = Convert.ToInt32(s1[i]);
                            listBox1.Items[k] += Convert.ToString(Perc.weight[i, k]) + " ";
                        }
                    }
                    k++;
                }
                sr.Close();
            }
            else MessageBox.Show("Выберите текстовый файл.", "Ошибка", MessageBoxButtons.OK);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var files = Directory.GetFiles(@"C:\Users\bikhi_27b5q2u\Desktop\Предметы\4 курс\8 семестр\Интеллектуальные системы\Цифры\7\" , "*.bmp", SearchOption.AllDirectories);

            //foreach (string filename in files)
            //{
            //    if (Regex.IsMatch(filename, @"\.bmp$"))
            //    {
            //        Perc.N ++;
            //        Image img = Image.FromFile(filename);
            //        Bitmap image = img as Bitmap;

            //        for (int x = 0; x <= 31; x++)
            //        {
            //            for (int y = 0; y <= 31; y++)
            //            {
            //                int n = (image.GetPixel(x, y).R);
            //                if (n >= 250)
            //                    n = 0;
            //                else
            //                    n = 1;
            //                input[x, y] = n;
            //            }
            //        }
            //        Perc.Guiltyness();
            //        Distinction();
            //    }
            //}

            int x = input.Length;
            int y = input.Length;
            Perc = new Num(x, y, input);

            string filePath = "";
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\bikhi_27b5q2u\Desktop\Предметы\4 курс\8 семестр\Интеллектуальные системы\Цифры\7\",
                Filter = "Bitmap-файлы (*.bmp)|*.bmp",
                Title = "Изображение с цифрой"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
            }
            if (filePath != "")
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = Image.FromFile(filePath);
                Bitmap img = pictureBox1.Image as Bitmap;

                for (x = 0; x <= 31; x++)
                {
                    for (y = 0; y <= 31; y++)
                    {
                        int n = (img.GetPixel(x, y).R);
                        if (n >= 250)
                            n = 0;
                        else
                            n = 1;
                        input[x, y] = n;
                    }
                }
                Perc.Guiltyness();
                Distinction();
            }
            else MessageBox.Show("Выберите изображение с цифрой.", "Ошибка", MessageBoxButtons.OK);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Perc.Punishment(input, Perc.Guiltyness());

            string s = "";
            string[] s1 = new string[32];
            System.IO.File.Delete(Perc.path);
            FileStream FS = new FileStream(Perc.path, FileMode.OpenOrCreate);
            StreamWriter SW = new StreamWriter(FS);

            for (int y = 0; y <= 31; y++)
            {
                for (int x = 0; x <= 31; x++)
                {
                    s += Convert.ToString(Perc.weight[x, y]);
                    s1[y] = s;
                    if (x != 31)
                        s += " ";
                }
                s += "\n";
            }
            SW.WriteLine(s);
            SW.Close();
        }
    }
}