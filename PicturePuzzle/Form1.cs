using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicturePuzzle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenImageButton_Click(object sender, EventArgs e)
        {
            if (openImage.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string imagePath = openImage.FileName;
            Bitmap image = new Bitmap(imagePath);
            pictureBox1.Image = image;

            startButton.Visible = true;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Bitmap image = (Bitmap)pictureBox1.Image;
            PuzzleForm puzzleForm = new PuzzleForm(image, this);

            puzzleForm.Show();
            this.Hide();
        }
    }
}
