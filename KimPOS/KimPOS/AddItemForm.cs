using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using KimPOS.Components;

namespace KimPOS
{
    public partial class AddItemForm : Form
    {

        private FrmMain mainFormInstance;

        public AddItemForm(FrmMain mainForm)
        {
            InitializeComponent();
            mainFormInstance = mainForm;
            this.FormBorderStyle = FormBorderStyle.None;
            // Wire up mouse events
            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;
            // Attach the KeyPress event handler to the textbox
            textBox2.KeyPress += textBox2_KeyPress;
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

            public void AddNewItem(string name, double price, categories category, string imageName)
        {
            mainFormInstance.AddItem(name, price, category, imageName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                // Process the selected file (e.g., upload to a server, etc.)
                MessageBox.Show("File uploaded: " + selectedFilePath);
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                button3.Enabled = true;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image != null && textBox1.Text != "" && textBox2.Text != "")
            {
                string currentDirectory = Directory.GetCurrentDirectory(); // Get the current directory
                Console.WriteLine(currentDirectory);
                string iconsFolderPath = Path.Combine(currentDirectory, "icons"); // Construct the relative path
                Directory.CreateDirectory(iconsFolderPath); // Create the directory if it doesn't exist
                // string iconsFolderPath = "./icons";
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string imagePath = Path.Combine(iconsFolderPath, fileName);

                byte[] imageData;
                using(MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                    imageData = ms.ToArray();
                }

                File.WriteAllBytes(imagePath, imageData);
                MessageBox.Show("Image uploaded and saved to icons folder");

                string itemName = textBox1.Text;
                double itemPrice = Convert.ToDouble(textBox2.Text);
                categories itemCategory = (categories)Enum.Parse(typeof(categories), categoryComboBox.SelectedValue.ToString());
                AddNewItem(itemName, itemPrice, itemCategory, fileName);

            }
            else
            {
                if(textBox1.Text == "")
                {
                    MessageBox.Show("No item name");
                }

                if (textBox2.Text == "")
                {
                    MessageBox.Show("No item price");
                }

                if (pictureBox1.Image == null)
                {
                    MessageBox.Show("No image selected");

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            pictureBox1.Image = null;
        }

        private void SettingsForm_Load_1(object sender, EventArgs e)
        {
            // Populate ComboBox with enum values
            categoryComboBox.DataSource = Enum.GetValues(typeof(categories));
        }

        private void btnZatvori_Click_1(object sender, EventArgs e)
        {
            this.Close();
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit or a control character (like Backspace or Delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the pressed key is not a digit or a control character, suppress it
                e.Handled = true;
            }
        }
    }
}
