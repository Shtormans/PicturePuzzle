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
    public partial class PuzzleForm : Form
    {
        private Form _form1;

        public PuzzleForm(Bitmap image, Form form)
        {
            InitializeComponent();

            _form1 = form;
            CreateField(image);
        }

        private void CreateField(Bitmap image)
        {
            Puzzle puzzle = new Puzzle();
            puzzle.SetImage(image, this);
            puzzle.SetRandomPosition();

            this.Size = new Size(puzzle.GridSize.Width * Cell.Size.Width + 16, puzzle.GridSize.Height * Cell.Size.Height + 39);
        }

        private void PuzzleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _form1.Close();
        }
    }
}
