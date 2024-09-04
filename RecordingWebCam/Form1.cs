using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordingWebCam
{
    public partial class Form1 : Form
    {
        private string Path = @"C:\Users\abdie\Documents\RecordingWebCam\RecordingWebCam\";
        private bool DevicesAreFound;
        private FilterInfoCollection MyDevices;
        private VideoCaptureDevice MyWebCam;

        public Form1()
        {
            InitializeComponent();
            ChargeDevices();
        }

        public void ChargeDevices()
        {
            MyDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (MyDevices.Count > 0)
            {
                DevicesAreFound = true;
                for (int i = 0; i < MyDevices.Count; i++)
                {
                    comboBox1.Items.Add(MyDevices[i].Name.ToString());
                    comboBox1.Text = MyDevices[0].ToString();

                }
            }
            else
            {
                DevicesAreFound = false;
            }
        }

        public void Recording(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            Imagen.RotateFlip(RotateFlipType.RotateNoneFlipX);

            pictureBox1.Image = Imagen;
        }

        private void CloseWebCam()
        {
            if (MyWebCam != null && MyWebCam.IsRunning)
            {
                MyWebCam.SignalToStop();
                MyWebCam = null;
            }
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            CloseWebCam();
            int i = comboBox1.SelectedIndex;
            string NombreVideo = MyDevices[i].MonikerString;
            MyWebCam = new VideoCaptureDevice(NombreVideo);
            MyWebCam.NewFrame += new NewFrameEventHandler(Recording);
            MyWebCam.Start();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (MyWebCam != null && MyWebCam.IsRunning)
            {
                pictureBox2.Image = pictureBox1.Image;
                pictureBox2.Image.Save(Path + "ImageCapture.jpg", ImageFormat.Jpeg);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseWebCam();
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            CloseWebCam();
            pictureBox1.Image = Image.FromFile(@"C:\Users\abdie\Documents\RecordingWebCam\RecordingWebCam\Resources\Patricio.jpg");
        }
    }
}
