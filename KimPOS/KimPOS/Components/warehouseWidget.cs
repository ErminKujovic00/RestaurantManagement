using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimPOS.Components
{
    public partial class warehouseWidget : UserControl
    {
        private categories _category;
        private double _quantity;
        public warehouseWidget()
        {
            InitializeComponent();
        }

        public categories Category { get => _category; set => _category = value; }

        public string Title { get => lblTitle.Text; set => lblTitle.Text = value; }
        public double Quantity { get => _quantity; set { _quantity = value; lblQuantity.Text = _quantity.ToString(); } }
        public Image Icon { get => imgImage.Image; set => imgImage.Image = value; }
    }
}
