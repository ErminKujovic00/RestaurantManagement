using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace KimPOS.Components
{
    public enum categories { Food, Alcohol, ColdDrinks, HotDrinks, Desserts }

    public partial class Widget : UserControl
    {
        private categories _category;
        private double _cost;
        private DataGridView grid; // Add a field to store the reference to the grid
        private FrmMain _frmMain; 



        public event EventHandler OnSelect = null;
        public Widget(DataGridView grid, FrmMain frmMain)
        {
            InitializeComponent();
            makeButtonRound(button1);
            this.grid = grid;
            _frmMain = frmMain;
        }

        private void bunifuPictureBox1_Click(object sender, EventArgs e)
        {
            OnSelect?.Invoke(this, e);
        }

        public categories Category { get => _category; set => _category = value; }

        public string Title { get => lblTitle.Text; set => lblTitle.Text = value; }
        public double Cost { get => _cost; set  { _cost = value; lblCost.Text = _cost.ToString("C2"); } }
        public Image Icon { get => imgImage.Image; set => imgImage.Image = value; }

        private void makeButtonRound(Button button)
        {
            int diameter = Math.Min(button.Width, button.Height);
            button.Region = new Region(CreateRoundRegion(new Rectangle(0, 0, diameter, diameter), 10));
        }

        private GraphicsPath CreateRoundRegion(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Rectangle arcRect = new Rectangle(rectangle.Location, new Size(diameter, diameter));

            path.AddArc(arcRect, 180, 90);

            arcRect.X = rectangle.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            arcRect.Y = rectangle.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            arcRect.X = rectangle.Left;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();
            return path;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(Title);
            _frmMain.RemoveItem(Title);
        }
    }
}
