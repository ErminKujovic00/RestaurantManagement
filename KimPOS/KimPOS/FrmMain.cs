using KimPOS.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data.Common;

namespace KimPOS
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            // Wire up mouse events
            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;

            // Center align the text in all cells
            /* foreach (DataGridViewColumn column in grid.Columns)
             {
                 column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             }*/

            // Create a button column for removing items
            DataGridViewButtonColumn delColumn = new DataGridViewButtonColumn();
            delColumn.Name = "Del";
            delColumn.HeaderText = "";
            delColumn.Text = "X";
            delColumn.UseColumnTextForButtonValue = true;
            grid.Columns.Add(delColumn);

            // Handle the CellClick event to remove items when the "X" button is clicked
            grid.CellClick += (sender, e) =>
            {
                if (e.ColumnIndex == grid.Columns["Del"].Index && e.RowIndex >= 0)
                {
                    var quantityCell = grid.Rows[e.RowIndex].Cells["Column2"];
                    int currentQuantity = int.Parse(quantityCell.Value.ToString());

                    /*  if (currentQuantity > 1)
                      {
                          quantityCell.Value = (currentQuantity - 1).ToString();
                      }
                      else
                      {
                          grid.Rows.RemoveAt(e.RowIndex);
                      }*/
                    grid.Rows.RemoveAt(e.RowIndex);

                    CalculateTotal();
                }
            };
        }

        private bool isDragging = false;
        private Point offset;


        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Opacity = 1;
            docker.WindowState = Bunifu.UI.WinForms.BunifuFormDock.FormWindowStates.Maximized;
        }

        public void clearAll(object sender, EventArgs e)
        {
            lblTot.Text = "£0";
            grid.Rows.Clear();        
        }



        public void AddItem(string name, double cost, categories category, string icon)
        {
            var w = new Widget(grid, this)
            {
                Title = name,
                Cost = cost,
                Category = category,
                Icon = System.Drawing.Image.FromFile("icons/" + icon),
                Tag = category

            };
            pnl.Controls.Add(w);

            w.OnSelect += (ss, ee) =>
            {
                var wdg = (Widget)ss;
                foreach (DataGridViewRow item in grid.Rows)
                {
                    if (item.Cells[0].Value.ToString()== wdg.lblTitle.Text)
                    {
                        item.Cells[1].Value = int.Parse(item.Cells[1].Value.ToString()) + 1;
                        item.Cells[2].Value = (Int32.Parse(item.Cells[1].Value.ToString()) * Double.Parse(wdg.lblCost.Text.Replace("£", ""))).ToString("C2");
                        CalculateTotal();
                        return;
                    }
                }
                grid.Rows.Add(new object[] {wdg.lblTitle.Text,1,wdg.lblCost.Text});
                CalculateTotal();
            };
        }

        public void RemoveItem(string itemName)
        {
            // Remove from the control (pnl)
            foreach (Widget widget in pnl.Controls.OfType<Widget>().ToList())
            {
                if (widget.Title == itemName)
                {
                    pnl.Controls.Remove(widget);
                    break; // Exit the loop after the first match
                }
            }

            // Remove from the DataGridView (grid)
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells[0].Value.ToString() == itemName)
                {
                    grid.Rows.Remove(row);
                    break; // Exit the loop after the first match
                }
            }

            CalculateTotal(); // Update the total after removing the item
        }

        void CalculateTotal()
        {
            double tot=0;
            foreach (DataGridViewRow item in grid.Rows)
            {
                //Console.WriteLine(item.Cells[2].Value.ToString() + "Ovo je tekst" + item.Cells[0].Value.ToString());
                tot += double.Parse(item.Cells[2].Value.ToString().Replace("£", ""));
            }

            lblTot.Text = tot.ToString("C2");
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {

            //Fectch items from db here
            //food
            AddItem("Original Burger", 7.75, categories.Food, "burger.png");
            AddItem("Ranch Burger", 7.75, categories.Food, "burger.png");
            AddItem("Pizza", 10.78, categories.Food, "pizza.png");
            AddItem("Chicken", 6.78, categories.Food, "chicken.png");
            AddItem("French Fries", 5.78, categories.Food, "fries.png");
            AddItem("Chicken Wings", 1.78, categories.Food, "wings.png");

            //alcohol
            AddItem("Beer", 10.78, categories.Alcohol, "beer.png");
            AddItem("Vodka", 11.78, categories.Alcohol, "drinks.png");
            AddItem("Whiskey", 51.78, categories.Alcohol, "drinks.png");
            AddItem("Brandy", 55.78, categories.Alcohol, "drinks.png");
            AddItem("Vermouth", 10.78, categories.Alcohol, "drinks.png");
            AddItem("Cognac", 9.78, categories.Alcohol, "drinks.png");
            AddItem("Wine", 8.78, categories.Alcohol, "drinks.png");
            AddItem("Rum", 8.78, categories.Alcohol, "drinks.png");

            //cold drinks
            AddItem("Coca-Cola", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Pepsi", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Diet Coke", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Dr. Pepper", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Mountain Dew", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Sprite", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Diet Pepsi", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Coke Zero", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Fresh Juice", 1.78, categories.ColdDrinks, "juice.png");
            AddItem("Water", 1.78, categories.ColdDrinks, "juice.png");

            //hot drinks
            AddItem("Fresh ginger tea", 1.78, categories.HotDrinks, "tea.png");
            AddItem("Fruit tea", 1.78, categories.HotDrinks, "tea.png");
            AddItem("Fresh mint tea", 1.78, categories.HotDrinks, "tea.png");
            AddItem("Coffee", 1.78, categories.HotDrinks, "tea.png");
            AddItem("Hot chocolate", 1.78, categories.HotDrinks, "tea.png");
            AddItem("Hot lemon", 1.78, categories.HotDrinks, "tea.png");
            AddItem("Green tea", 1.78, categories.HotDrinks, "tea.png");
            AddItem("Tea", 1.78, categories.HotDrinks, "tea.png");
            AddItem("Chai", 1.78, categories.HotDrinks, "tea.png");

            //desserts
            AddItem("Pasteis de Nata", 1.78, categories.Desserts, "dessert.png");
            AddItem("Tiramisu ", 1.78, categories.Desserts, "dessert.png");
            AddItem("Gulab Jamun", 1.78, categories.Desserts, "dessert.png");
            AddItem("S'mores", 1.78, categories.Desserts, "dessert.png");
            AddItem("Churros ", 1.78, categories.Desserts, "dessert.png");
            AddItem("Lamingtons ", 1.78, categories.Desserts, "dessert.png");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { }
        string path = "All Items";
        private void menu_OnItemSelected(object sender, string _path, EventArgs e)
        {
            path = _path;
            foreach (var item in pnl.Controls)
            {
                var wdg = (Widget)item;
                wdg.Visible = wdg.lblTitle.Text.ToLower().Contains(txtSearch.Text.Trim().ToLower()) 
                    &&
                    (wdg.Tag.ToString() == path.Replace(" ","") || path.Replace(" ", "") == "AllItems");
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || txtSearch.Text.Trim().Length == 0)
            {
                foreach (var item in pnl.Controls)
                {
                    var wdg = (Widget)item;
                    wdg.Visible = wdg.lblTitle.Text.ToLower().Contains(txtSearch.Text.Trim().ToLower()) 
                        &&
                      ( wdg.Tag.ToString() == path.Replace(" ", "") || path.Replace(" ", "") == "AllItems");
                        
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Create a new instance of the HelpDialogForm
            HelpDialogForm helpDialog = new HelpDialogForm();

            // Show the dialog as a modal dialog
            DialogResult result = helpDialog.ShowDialog();

            // You can perform any additional actions after the dialog is closed
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (grid.Rows.Count == 0)
            {
                MessageBox.Show("No items to create receipt.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveFileDialog.FileName = "Receipt.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        Document doc = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));

                        // Add content to the PDF document
                        doc.Open();

                        // Header
                        PdfPTable headerTable = new PdfPTable(1);
                        PdfPCell headerCell = new PdfPCell(new Phrase("Receipt\n\n"));
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        headerTable.AddCell(headerCell);
                        doc.Add(headerTable);

                        // Table with receipt items
                        PdfPTable table = new PdfPTable(3);
                        table.AddCell("Item");
                        table.AddCell("Quantity");
                        table.AddCell("Price");

                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            table.AddCell(row.Cells[0].Value.ToString());
                            table.AddCell(row.Cells[1].Value.ToString());
                            table.AddCell(row.Cells[2].Value.ToString());
                        }

                        doc.Add(table);

                        // Footer
                        PdfPTable footerTable = new PdfPTable(1);
                        PdfPCell footerCell = new PdfPCell(new Phrase($"\n\nDate: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\nCafe: Caffe management" + "\n\n Total: " + lblTot.Text));
                        footerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        footerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        footerTable.AddCell(footerCell);
                        doc.Add(footerTable);

                        doc.Close();

                        MessageBox.Show("Receipt generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblTot.Text = "£0";
                        grid.Rows.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating the receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void navigtionMenu1_OnItemSelected(object sender, string path, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Open the settings form
            AddItemForm addItemForm = new AddItemForm(this);
            addItemForm.Focus();
            addItemForm.Show();
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

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

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            // Open the settings form
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Focus();
            settingsForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Open the review form
            ReviewForm reviewForm = new ReviewForm();
            reviewForm.Focus();
            reviewForm.Show();
        }

        private void WarehouseBtn_Click(object sender, EventArgs e)
        {
            // Open the warehouse form
            WarehouseForm warehouseForm = new WarehouseForm();
            warehouseForm.Focus();
            warehouseForm.Show();
        }
    }

    public class HelpDialogForm : Form
    {
        public HelpDialogForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Helpful Advice";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Create controls for your dialog content (labels, buttons, etc.)
            Label adviceLabel = new Label();
            adviceLabel.Text = "Here's some helpful advice for you:";
            adviceLabel.Dock = DockStyle.Top;
            adviceLabel.Padding = new Padding(10);
            adviceLabel.AutoSize = true;

            TextBox adviceTextBox = new TextBox();
            adviceTextBox.Text = " To add an item click on the image or text." + "\r\n" +
                " Press the clear all button to remove sleceted items." + "\r\n" +
                " Press the category button for easier search" + "\r\n" +
                " Open the settings to make necessary changes if needed.";
            adviceTextBox.Multiline = true;
            adviceTextBox.ReadOnly = true;
            adviceTextBox.Dock = DockStyle.Fill;

            Button closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Dock = DockStyle.Bottom;
            closeButton.Click += (sender, e) => { this.DialogResult = DialogResult.OK; };

            // Add controls to the form's controls collection
            this.Controls.Add(adviceTextBox);
            this.Controls.Add(adviceLabel);
            this.Controls.Add(closeButton);

            // Set the form's size
            this.ClientSize = new System.Drawing.Size(300, 200);
        }
    }
}
