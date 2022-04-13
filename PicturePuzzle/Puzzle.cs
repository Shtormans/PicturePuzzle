using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicturePuzzle
{
    class Puzzle
    {
        public readonly Size GridSize = new Size(3, 3);
        private readonly Size _pictureSize;
        private readonly Cell[,] _field;
        private Point _emptyCellCoords;

        public Puzzle()
        {
            _field = new Cell[GridSize.Height, GridSize.Height];
            _pictureSize = new Size(GridSize.Width * Cell.Size.Width, GridSize.Height * Cell.Size.Height);
        }

        public void SetImage(Bitmap image, Form form)
        {
            image = new Bitmap(image, _pictureSize);
            _emptyCellCoords = new Point(GridSize.Width - 1, 0);

            for (int y = 0; y < GridSize.Height; y++)
            {
                for (int x = 0; x < GridSize.Width; x++)
                {

                    Point point = new Point(x, y);
                    Cell cell = new Cell(point, this);

                    Point rectanglePoint = new Point(x * Cell.Size.Width, y * Cell.Size.Height);

                    Rectangle rectangle = new Rectangle(rectanglePoint, Cell.Size);
                    Bitmap cellImage = image.Clone(rectangle, image.PixelFormat);
                    cell.Display(form);

                    _field[y, x] = cell;

                    if (new Point(x, y) != _emptyCellCoords)
                    {
                        cell.SetImage(cellImage);
                    }
                }
            }
        }

        public void SetRandomPosition()
        {
            Random random = new Random();
            int moves = random.Next(10, 20);

            for (int i = 0; i < moves; i++)
            {
                int maxIndex = GridSize.Width * GridSize.Height;

                int index1 = random.Next(0, maxIndex);
                int index2 = random.Next(0, maxIndex);

                Point location1 = IndexToPoint(index1);
                Point location2 = IndexToPoint(index2);

                SwapValues(location1, location2);
            }

            FindEmptyCell();
        }

        private Point IndexToPoint(int index)
        {
            int y = index / GridSize.Width;
            int x = index % GridSize.Width;

            return new Point(x, y);
        }

        private void FindEmptyCell()
        {
            for (int y = 0; y < GridSize.Height; y++)
            {
                for (int x = 0; x < GridSize.Width; x++)
                {
                    Cell cell = _field[y, x];

                    if (cell.isEmpty)
                    {
                        _emptyCellCoords = new Point(x, y);
                        return;
                    }
                }
            }
        }

        private void SwapValues(Point location1, Point location2)
        {
            var temp = _field[location1.Y, location1.X];

            _field[location1.Y, location1.X] = _field[location2.Y, location2.X];
            _field[location2.Y, location2.X] = temp;

            _field[location1.Y, location1.X].ChangePosition(new Point(location1.X, location1.Y));
            _field[location2.Y, location2.X].ChangePosition(new Point(location2.X, location2.Y));
        }

        public bool TryMoveCells(Point location)
        {
            if (Math.Abs(_emptyCellCoords.X - location.X) == 1
                ^ Math.Abs(_emptyCellCoords.Y - location.Y) == 1)
            {
                SwapValues(location, _emptyCellCoords);
                _emptyCellCoords = location;
            }

            return false;
        }
    }

    class Cell
    {
        private Button _body;
        private Point _gridLocation;
        public static readonly Size Size = new Size(200, 200);
        private Puzzle _puzzle;

        public bool isEmpty
        {
            get { return _body.BackgroundImage == null; }
        }

        public Cell(Point gridLocation, Puzzle puzzle)
        {
            _body = new Button();
            _puzzle = puzzle;

            _gridLocation = gridLocation;
            _body.Location = new Point(gridLocation.X * Cell.Size.Width, gridLocation.Y * Cell.Size.Height); ;
            _body.Size = Size;
            _body.FlatStyle = FlatStyle.Flat;
            _body.Click += Cell_OnClick;
        }

        public void SetImage(Bitmap image)
        {
            _body.BackgroundImage = image;
        }

        public void Display(Form form)
        {
            form.Controls.Add(_body);
        }

        public void ChangePosition(Point gridCoords)
        {
            _gridLocation = gridCoords;
            _body.Location = new Point(gridCoords.X * Size.Width, gridCoords.Y * Size.Height);
        }

        public void Cell_OnClick(object sender, EventArgs e)
        {
            _puzzle.TryMoveCells(_gridLocation);
        }
    }
}
