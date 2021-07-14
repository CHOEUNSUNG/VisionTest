using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using RootTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    public class StartWindow_ViewModel : ObservableObject
    {
        Image<Gray, byte> m_imgSrc;
        public Image<Gray, byte> p_imgSrc
        {
            get { return m_imgSrc; }
            set { SetProperty(ref m_imgSrc, value); }
        }

        BitmapImage m_bmpImg = null;
        public BitmapImage p_bmpImg
        {
            get { return m_bmpImg; }
            set { SetProperty(ref m_bmpImg, value); }
        }

        BitmapImage m_bmpImgResult = null;
        public BitmapImage p_bmpImgResult
        {
            get { return m_bmpImgResult; }
            set { SetProperty(ref m_bmpImgResult, value); }
        }

        public StartWindow_ViewModel()
        {

        }

        public RelayCommand ImageOpenCommand
        {
            get
            {
                return new RelayCommand(ImageOpen);
            }
        }

        public RelayCommand ProcessingCommand
        {
            get 
            {
                return new RelayCommand(Processing); 
            }
        }

        public void ImageOpen()
        {
            // variable
            OpenFileDialog odl = new OpenFileDialog();

            // implement
            if (odl.ShowDialog() == true)
            {
                if (p_imgSrc != null)
                    p_imgSrc.Dispose();
                p_imgSrc = new Image<Gray, byte>(odl.FileName);
                p_bmpImg = GetBitmapImageFromBitmap(p_imgSrc.Bitmap);
            }

            return;
        }

        public void Processing()
        {
            // variable

            // implement
            byte[,,] data = p_imgSrc.Data;
            Parallel.For(0, p_imgSrc.Height, (y) =>
            {
                Parallel.For(0, p_imgSrc.Width, (x) =>
                {
                    if (data[y, x, 0] > 128)
                        data[y, x, 0] = 255;
                });
            });

            Image<Gray, byte> imgResult = new Image<Gray, byte>(data);
            p_bmpImgResult = GetBitmapImageFromBitmap(imgResult.Bitmap);

            return;
        }

        #region Function
        public BitmapImage GetBitmapImageFromBitmap(Bitmap bmp)
        {
            // variable
            MemoryStream ms = new MemoryStream();
            BitmapImage bi = new BitmapImage();

            // implement
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.EndInit();

            return bi;
        }
        #endregion
    }
}
