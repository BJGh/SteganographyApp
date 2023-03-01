using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyApp
{
    public partial class Form1 : Form
    {
        Bitmap image1 = null;
        Bitmap image2 = null;
        KochZhao koch;
        public Form1()
        {
            InitializeComponent();
             Form form = this;
           koch= new KochZhao(this);


        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 47 && e.KeyChar <= 58)
            {
                textBox2.Text += e.KeyChar;
                e.Handled = true;
            }
            else if (e.KeyChar == 8 && textBox2.Text.Length > 0)
            {
                textBox2.Text = textBox2.Text.Substring(0, textBox2.Text.Length - 1);
                if (textBox2.Text.Length > 0)
                {
                    int m = Convert.ToInt32(textBox2.Text);
                    koch.mKey = m;
                }
                else
                {
                    koch.mKey = 0;
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
            update();
            textBox2.Select(textBox2.Text.Length, textBox2.Text.Length);
        }


        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }

        public void maxDataSize()
        {
            koch.maxMessSize();
            if (koch.hideInf == null)
            {
                koch.flag = false;
                return;
            }
            if (koch.maxSize > 0 && koch.hideInf.Length > 0 && koch.hideInf.Length <= koch.maxSize)
            {
                koch.flag = true;
            }
            else koch.flag = false;
        }
        private void getKey()
        {
            if (this.textBox2.Text.Length != 0)
            {
                koch.mKey = Convert.ToInt32(textBox2.Text);
            }
        }
        private void update()
        {
            if (progressBar1.Value > 99)
            {
                progressBar1.Value = 0;
            }
            if (koch.sizeSegment != 0 && koch.image1 != null)
            {
                int count = (int)(koch.image1.Height / koch.sizeSegment) * (int)(koch.image1.Width / koch.sizeSegment);
                label9.Text = "Block amount: " + count;
            }
            else
            {
                label9.Text = "Block amount: ";
            }
            if (koch.hideInf != null)
            {
                label8.Text = "Message size: " + koch.hideInf.Length + " bytes";
            }
            else
            {
                label8.Text = "Messagee size: ";
            }
            maxDataSize();
            if (koch.sizeSegment != 0 && koch.image1 != null)
            {
                label10.Text = "Max value of embedded info: " + koch.maxSize + " bytes";
            }
            else
            {
                label10.Text = "Max value of embedded info: ";
            }
            if (koch.hideInf != null && koch.sizeSegment != 0 && koch.image1 != null && koch.flag)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
            getKey();

            trackBar1.Value = koch.P;

            if (koch.hideInf == null)
            {
                textBox1.Text = "";
            }
            if (koch.image1 == null)
            {
                label7.Text = "Container size: ";
                label2.Text = "Original image ";

            }
            if (image2 == null)
            {
                label1.Text = "After embedding ";

            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog load = new OpenFileDialog();
            load.Multiselect = false;
            DialogResult result = load.ShowDialog();
            String filename = load.FileName;
            load.Dispose();
            if (result == DialogResult.OK)
            {
                try
                {
                    // Console.WriteLine(filename);
                    textBox1.Text = filename;
                    koch.datafile = filename;
                    koch.loadMessage(filename);
                    update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open file\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void sendMess(String mess)
        {

        }
        public void pict2(Bitmap image)
        {
            this.image2 = image;
            this.picOutput.Size = this.panel2.Size;
            picOutput.Image = resizeImage(image2, this.picOutput.Size);
            int imageX = picOutput.Image.Width;
            int imageY = picOutput.Image.Height;
            int panelX = panel2.Width;
            int panelY = panel2.Height;
            int dX = (panelX - imageX) / 2;
            int dY = (panelY - imageY) / 2;
            picOutput.Left = dX;
            picOutput.Top = dY;
            picOutput.Size = new Size(imageX, imageY);
            picOutput.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(koch.hideInf.Length);
            Thread backgroundThread = new Thread(new ThreadStart(DoWork1));
            backgroundThread.Start();
          
        }
        private void DoWork1()
        {
            koch.inlining();
           // pict2(image2);

        }
       
        public void progress(double val)
        {
            if (val > 100)
            {
                progressBar1.Value = 100;
            }
            else
            {
                progressBar1.Value = (int)val;
            }
        }
        private void picInput_Click_1(object sender, EventArgs e)
        {
            picInput.Invalidate();
            OpenFileDialog load = new OpenFileDialog();
            load.Multiselect = false;
            load.Filter = "Image Files(*.bmp;*.png)|*.bmp;*.png|All files (*.*)|*.*";
            if (load.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    String filename = load.FileName;
                    String ex = filename.Substring(filename.LastIndexOf(".") + 1);
                    if (String.Compare(ex, "bmp") != 0 && String.Compare(ex, "png") != 0 && String.Compare(ex, "Bmp") != 0 && String.Compare(ex, "Png") != 0)
                    {
                        throw new Exception("Unsupported format");
                    }
                    image1 =  new Bitmap(load.FileName);
                    picInput.Image = resizeImage(image1, this.picInput.Size);
                    Bitmap b = (Bitmap)picInput.Image;
                    int imageX = picInput.Image.Width;
                    int imageY = picInput.Image.Height;
                    int panelX = panel1.Width;
                    int panelY = panel1.Height;
                    int dX = (panelX - imageX) / 2;
                    int dY = (panelY - imageY) / 2;
                    picInput.Left = dX;
                    picInput.Top = dY;
                    picInput.Size = new Size(imageX, imageY);

                    Stream inputStream = File.OpenRead(filename);
                    long size = inputStream.Length;
                    inputStream.Close();

                    label7.Text = "Container size: " + image1.Width + "x" + image1.Height + " (" + size + " bytes)";
                    koch.infilename = filename;
                    koch.image1= (Bitmap)image1;
                    //label2.Text = "Исходное изображение: " + filename;
                    update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error open file\n" + ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    picOutput.Image = null;
                    picOutput.Size = panel2.Size;
                    picOutput.Top = 0;
                    picOutput.Left = 0;
                }

            }
        }
    }
}