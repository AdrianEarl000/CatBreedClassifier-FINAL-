using System;
using System.Drawing;
using System.Windows.Forms;

namespace CatBreedClassifier
{
    public partial class Form1 : Form
    {
        private ModelLoader modelLoader;

        public Form1()
        {
            InitializeComponent();
            modelLoader = new ModelLoader(@"C:\Users\admin\source\repos\CatBreedClassifier\CatBreedClassifier\MLModel1.zip");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd.FileName != null)
            {
                // Load the original image from file
                Image originalImage = Image.FromFile(ofd.FileName);

                // Resize the image to 224x224 pixels
                Image resizedImage = ResizeImage(originalImage, 524, 524);

                // Set the resized image to the PictureBox
                pictureBox1.Image = resizedImage;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                // Convert the image to bytes
                ImageConverter converter = new ImageConverter();
                byte[] imageBytes = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                // Prepare input data for prediction
                ModelInput input = new ModelInput
                {
                    ImageSource = imageBytes
                };

                // Make prediction
                var prediction = modelLoader.Predict(input);

                // Display the predicted breed
                MessageBox.Show("Selected file is an image of a " + prediction.Prediction);
            }
            else
            {
                MessageBox.Show("Please select an image first.");
            }
        }
        private Image ResizeImage(Image image, int width, int height)
        {
            // Create a new Bitmap with the desired dimensions
            Bitmap resizedImage = new Bitmap(width, height);

            // Create a Graphics object from the Bitmap
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                // Set the interpolation mode to high quality bicubic
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Draw the original image onto the new Bitmap with the desired dimensions
                graphics.DrawImage(image, 0, 0, width, height);
            }

            // Return the resized image
            return resizedImage;
        }
    }
}