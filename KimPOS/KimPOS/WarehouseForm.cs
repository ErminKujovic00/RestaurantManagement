using KimPOS.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimPOS
{
    public partial class WarehouseForm : Form
    {
        public WarehouseForm()
        {
            InitializeComponent();
            // Set the form to open in full screen
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            // Wire up mouse events
            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;
        }

        private bool isDragging = false;
        private Point offset;

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                offset = e.Location;
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - offset.X;
                newLocation.Y += e.Y - offset.Y;
                this.Location = newLocation;
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void btnZatvori_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMax_Click_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btnMin_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void AddItem(string name, double quantity, categories category, string icon)
        {
            var w = new warehouseWidget()
            {
                Title = name,
                Quantity = quantity,
                Category = category,
                Icon = System.Drawing.Image.FromFile("icons/" + icon),
                Tag = category

            };
            pnl.Controls.Add(w);
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {

            //Fectch items from db here
            //food
            AddItem("Original Burger", 5, categories.Food, "burger.png");
            AddItem("Ranch Burger", 7, categories.Food, "burger.png");
            AddItem("Pizza", 10, categories.Food, "pizza.png");
            AddItem("Chicken", 6, categories.Food, "chicken.png");
            AddItem("French Fries", 5, categories.Food, "fries.png");
            AddItem("Chicken Wings", 1, categories.Food, "wings.png");

            //alcohol
            AddItem("Beer", 100, categories.Alcohol, "beer.png");
            AddItem("Vodka", 11, categories.Alcohol, "drinks.png");
            AddItem("Whiskey", 51, categories.Alcohol, "drinks.png");
            AddItem("Brandy", 55, categories.Alcohol, "drinks.png");
            AddItem("Vermouth", 10, categories.Alcohol, "drinks.png");
            AddItem("Cognac", 9, categories.Alcohol, "drinks.png");
            AddItem("Wine", 8, categories.Alcohol, "drinks.png");
            AddItem("Rum", 8, categories.Alcohol, "drinks.png");

            //cold drinks
            AddItem("Coca-Cola", 8, categories.ColdDrinks, "juice.png");
            AddItem("Pepsi", 7, categories.ColdDrinks, "juice.png");
            AddItem("Diet Coke", 15, categories.ColdDrinks, "juice.png");
            AddItem("Dr. Pepper", 17, categories.ColdDrinks, "juice.png");
            AddItem("Mountain Dew", 20, categories.ColdDrinks, "juice.png");
            AddItem("Sprite", 18, categories.ColdDrinks, "juice.png");
            AddItem("Diet Pepsi", 0, categories.ColdDrinks, "juice.png");
            AddItem("Coke Zero", 33, categories.ColdDrinks, "juice.png");
            AddItem("Fresh Juice", 9, categories.ColdDrinks, "juice.png");
            AddItem("Water", 4, categories.ColdDrinks, "juice.png");

            //hot drinks
            AddItem("Fresh ginger tea", 1, categories.HotDrinks, "tea.png");
            AddItem("Fruit tea", 1, categories.HotDrinks, "tea.png");
            AddItem("Fresh mint tea", 5, categories.HotDrinks, "tea.png");
            AddItem("Coffee", 8, categories.HotDrinks, "tea.png");
            AddItem("Hot chocolate", 9, categories.HotDrinks, "tea.png");
            AddItem("Hot lemon", 0, categories.HotDrinks, "tea.png");
            AddItem("Green tea", 0, categories.HotDrinks, "tea.png");
            AddItem("Tea", 3, categories.HotDrinks, "tea.png");
            AddItem("Chai", 2, categories.HotDrinks, "tea.png");

            //desserts
            AddItem("Pasteis de Nata", 12, categories.Desserts, "dessert.png");
            AddItem("Tiramisu ", 25, categories.Desserts, "dessert.png");
            AddItem("Gulab Jamun", 24, categories.Desserts, "dessert.png");
            AddItem("S'mores", 15, categories.Desserts, "dessert.png");
            AddItem("Churros ", 12, categories.Desserts, "dessert.png");
            AddItem("Lamingtons ", 2, categories.Desserts, "dessert.png");
        }
    }
}
