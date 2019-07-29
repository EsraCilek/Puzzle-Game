using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PuzzleGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Image picture;
        OpenFileDialog fileDialog = new OpenFileDialog();
        Bitmap[] pictures = new Bitmap[16];
        Bitmap[] realPictures = new Bitmap[16];
        Button newButton;
        Image ChangeImage;
        Image ChangeImage2;
        public struct images
        {
            public String[,] pixel;
        }
        enum Clicks { one, two, three }
        Clicks clickbtn = Clicks.one;
        int butonOne = 0, butonTwo = 0;
        Bitmap storedImage;
        int trueMove = 0, totalMove = 0, falseMove = 0;
        string filePath = @"C:\\Users\\esra_\\Desktop\\Dersler\\Ödevler\\PuzzleGame\\PuzzleGame\\EnYuksekSkor.txt";
        Button[] Buttons;

        private Bitmap CreateBitmapImage(Image picture)
        {
            Bitmap BmpImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics ImageGraphics = Graphics.FromImage(BmpImage);
            ImageGraphics.Clear(Color.White);
            ImageGraphics.DrawImage(picture, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            ImageGraphics.Flush();
            return BmpImage;
        }

        private void Split()
        {
            Image img = picture;
            int width4 = (int)((double)img.Width / 4.0);
            int height4 = (int)((double)img.Height / 4.0);
            Bitmap[,] bmps = new Bitmap[4, 4];

            int i = 0;
            while (i < 4)
            {
                int j = 0;
                while (j < 4)
                {
                    bmps[i, j] = new Bitmap(width4, height4);
                    Graphics g = Graphics.FromImage(bmps[i, j]);
                    g.DrawImage(img, new Rectangle(0, 0, width4, height4), new
                    Rectangle(j * width4, i * height4, width4, height4),
                    GraphicsUnit.Pixel);
                    g.Dispose();
                    j++;
                }
                i++;
            }
            pictures[0] = bmps[0, 0]; //button2.Image = pictures[0];
            pictures[1] = bmps[0, 1]; //button3.Image = pictures[1];
            pictures[2] = bmps[0, 2]; //button4.Image = pictures[2];
            pictures[3] = bmps[0, 3]; //button5.Image = pictures[3];
            pictures[4] = bmps[1, 0]; //button6.Image = pictures[4];
            pictures[5] = bmps[1, 1]; //button7.Image = pictures[5];
            pictures[6] = bmps[1, 2]; //button8.Image = pictures[6];
            pictures[7] = bmps[1, 3]; //button9.Image = pictures[7];
            pictures[8] = bmps[2, 0]; //button10.Image = pictures[8];
            pictures[9] = bmps[2, 1]; //button11.Image = pictures[9];
            pictures[10] = bmps[2, 2]; //button12.Image = pictures[10];
            pictures[11] = bmps[2, 3]; //button13.Image = pictures[11];
            pictures[12] = bmps[3, 0]; //button14.Image = pictures[12];
            pictures[13] = bmps[3, 1]; //button15.Image = pictures[13];
            pictures[14] = bmps[3, 2]; //button16.Image = pictures[14];
            pictures[15] = bmps[3, 3]; //button17.Image = pictures[15];
            i = 0;
            while (i < pictures.Length)
            {
                realPictures[i] = pictures[i];
                i++;
            }
        }

        private void shuffle(Bitmap[] pictures)
        {
            Random png = new Random();
            int value = 0;
            string[] arrayındex = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
            int length = arrayındex.Length;
            Bitmap[] arraypicture = new Bitmap[16];
            int counter = 0;
            while (length != 0)
            {
                int i = 0;
                while (i < 16)
                {
                    value = png.Next(0, 16);
                    int j = 0;
                    while (j < 16)
                    {
                        if (arrayındex[j].Equals(value.ToString()))
                        {
                            arrayındex[j] = "20";
                            arraypicture[counter] = pictures[value];
                            counter++;
                            break;
                        }
                        j++;
                    }
                    i++;
                }
                length--;
            }
            int k = 0;
            while (k < pictures.Length)
            {
                pictures[k] = arraypicture[k];
                k++;
            }
        }

        private void control()
        {
            int counter = 0;
            images[] realPixel = new images[16];
            images[] shufflePixel = new images[16];
            int j = 0;
            while (j < realPixel.Length)
            {
                realPixel[j].pixel = new string[realPictures[j].Width, realPictures[j].Height];
                shufflePixel[j].pixel = new string[pictures[j].Width, pictures[j].Height];
                j++;
            }
            int count = 0;

            int i = 0;
            while (i < 16)
            {
                int x = 0;
                while (x < realPictures[i].Width)
                {
                    int y = 0;
                    while (y < realPictures[i].Height)
                    {
                        realPixel[i].pixel[x, y] = realPictures[i].GetPixel(x, y).Name;//orjinal resmin parçaları 
                        shufflePixel[i].pixel[x, y] = pictures[i].GetPixel(x, y).Name;//karışmış resim parçaları
                        if (realPixel[i].pixel[x, y] == shufflePixel[i].pixel[x, y]) { count++; }//parça kontrolü 
                        y++;
                    }
                    x++;
                }
                if (count == (pictures[i].Width * pictures[i].Height))
                {
                    listBox1.Items.Add((i + 1) + ".Buton Resmi Doğru");
                    Buttons[i].Enabled = false;
                    counter++;

                }
                count = 0;
                i++;
            }
            if (counter >= 1)
            {
                BtnShuffle.Visible = false;
            }
        }

        private void skorHesapla()
        {
            double puan = 0.0;
            double skor = 0.0;
            puan = (double)(falseMove) / (double)(totalMove);
            skor = 100 - (puan * falseMove);
            MessageBox.Show("" + skor);
            writeFile(skor);
            totalMove = 0; falseMove = 0; trueMove = 0;


        }

        private void writeFile(double skor)
        {
            //string dosya_yolu = @"C:\\Users\\Doğukan Berber\\Desktop\\deneme.txt";
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            streamWriter.Close();

            StreamWriter add = File.AppendText(filePath);

            add.WriteLine(skor);

            add.Flush();
            add.Close();

        }

        private void readFile()
        {
            //string dosya_yolu = @"C:\\Users\\Doğukan Berber\\Desktop\\deneme.txt";
            string skor;
            List<double> input = new List<double>();
            double high;
            double low;

            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(file);

            skor = streamReader.ReadLine();

            while (skor != null)
            {

                input.Add(Convert.ToDouble(skor));
                skor = streamReader.ReadLine();

            }
            high = input[0];
            low = input[0];
            foreach (double number in input)
            {
                if (number > high)
                {
                    high = number;
                }
                if (number < low)
                {
                    low = number;
                }
            }

            lblSkor.Text = high.ToString("0.##");

            streamReader.Close();
            file.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            readFile();
        }

        private void SelectFile_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            fileDialog.Title = "Resim Seç ";
            fileDialog.Filter = "All Files|*.*|BMP|*.bmp|JPEG|*.jpg|PNG Resimleri|*.png|GIF|*.gif|TIF|*.tif|WMF|*.wmf";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                picture = CreateBitmapImage(Image.FromFile(fileDialog.FileName));
                pictureBox1.Image = picture;
            }
            pictureBox1.Visible = false;
            BtnShuffle.Visible = true;
            readFile();
            button0.Image = null; button0.Enabled = true;
            button1.Image = null; button1.Enabled = true;
            button2.Image = null; button2.Enabled = true;
            button3.Image = null; button3.Enabled = true;
            button4.Image = null; button4.Enabled = true;
            button5.Image = null; button5.Enabled = true;
            button6.Image = null; button6.Enabled = true;
            button7.Image = null; button7.Enabled = true;
            button8.Image = null; button8.Enabled = true;
            button9.Image = null; button9.Enabled = true;
            button10.Image = null; button10.Enabled = true;
            button11.Image = null; button11.Enabled = true;
            button12.Image = null; button12.Enabled = true;
            button13.Image = null; button13.Enabled = true;
            button14.Image = null; button14.Enabled = true;
            button15.Image = null; button15.Enabled = true;


            listBox1.Items.Clear();
        }

        private void Shuffle_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            Buttons = new Button[16];
            Split();
            shuffle(pictures);
            button0.Image = pictures[0]; Buttons[0] = button0;
            button1.Image = pictures[1]; Buttons[1] = button1;
            button2.Image = pictures[2]; Buttons[2] = button2;
            button3.Image = pictures[3]; Buttons[3] = button3;
            button4.Image = pictures[4]; Buttons[4] = button4;
            button5.Image = pictures[5]; Buttons[5] = button5;
            button6.Image = pictures[6]; Buttons[6] = button6;
            button7.Image = pictures[7]; Buttons[7] = button7;
            button8.Image = pictures[8]; Buttons[8] = button8;
            button9.Image = pictures[9]; Buttons[9] = button9;
            button10.Image = pictures[10]; Buttons[10] = button10;
            button11.Image = pictures[11]; Buttons[11] = button11;
            button12.Image = pictures[12]; Buttons[12] = button12;
            button13.Image = pictures[13]; Buttons[13] = button13;
            button14.Image = pictures[14]; Buttons[14] = button14;
            button15.Image = pictures[15]; Buttons[15] = button15;
            control();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            int count = 0;
            Button buttonone = sender as Button;
            buttonone.Image = ((Button)sender).Image;
            switch (clickbtn)
            {
                case Clicks.one:
                    newButton = new Button();
                    newButton = sender as Button;
                    ChangeImage = buttonone.Image;
                    for (int i = 0; i < pictures.Length; i++)
                    {
                        if (ChangeImage.Equals(pictures[i]))
                        {
                            butonOne = i;
                        }
                    }
                    clickbtn = Clicks.two;
                    break;
                case Clicks.two:
                    ChangeImage2 = buttonone.Image;
                    for (int i = 0; i < pictures.Length; i++)
                    {
                        if (ChangeImage2.Equals(pictures[i]))
                        {
                            butonTwo = i;
                        }
                    }
                    buttonone.Image = pictures[butonOne];
                    newButton.Image = pictures[butonTwo];
                    storedImage = pictures[butonOne];
                    pictures[butonOne] = pictures[butonTwo];
                    pictures[butonTwo] = storedImage;
                    count = listBox1.Items.Count;
                    listBox1.Items.Clear();
                    control();
                    totalMove++;
                    if (listBox1.Items.Count >= count)
                    {

                        if ((listBox1.Items.Count - count) == 0)
                        {
                            falseMove = falseMove + 2;
                        }
                        if ((listBox1.Items.Count - count) == 1)
                        {
                            trueMove = trueMove + 1;
                            falseMove = falseMove + 1;
                        }
                        if ((listBox1.Items.Count - count) == 2)
                        {
                            trueMove = trueMove + 2;
                        }
                    }

                    if (listBox1.Items.Count == 5)
                    {
                        MessageBox.Show("Puzzle Tamamlandı!");
                        MessageBox.Show("ToplamHamle:" + totalMove);
                        MessageBox.Show("yanlis Sayisi:" + falseMove);
                        MessageBox.Show("doğruSayisi" + trueMove);
                        skorHesapla();
                        button0.Enabled = false;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        button3.Enabled = false;
                        button4.Enabled = false;
                        button5.Enabled = false;
                        button6.Enabled = false;
                        button7.Enabled = false;
                        button8.Enabled = false;
                        button9.Enabled = false;
                        button10.Enabled = false;
                        button11.Enabled = false;
                        button12.Enabled = false;
                        button13.Enabled = false;
                        button14.Enabled = false;
                        button15.Enabled = false;

                    }
                    ChangeImage = null;
                    ChangeImage2 = null;
                    clickbtn = Clicks.one;
                    break;
            }
        }


    }
}



