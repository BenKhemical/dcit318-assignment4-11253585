using System.Windows.Forms;

namespace PharmacyInventoryApp
{
    partial class MainForm
    {
        private TextBox txtName, txtCategory, txtPrice, txtQuantity, txtSearch;
        private Button btnAdd, btnSearch, btnUpdateStock, btnRecordSale, btnViewAll;
        private DataGridView dgvMedicines;

        private void InitializeComponent()
        {
            this.Text = "Pharmacy Inventory & Sales System";
            this.Width = 850;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblName = new Label() { Text = "Medicine Name:", Left = 20, Top = 20, Width = 110 };
            txtName = new TextBox() { Left = 140, Top = 18, Width = 200 };

            var lblCategory = new Label() { Text = "Category:", Left = 20, Top = 50, Width = 110 };
            txtCategory = new TextBox() { Left = 140, Top = 48, Width = 200 };

            var lblPrice = new Label() { Text = "Price:", Left = 20, Top = 80, Width = 110 };
            txtPrice = new TextBox() { Left = 140, Top = 78, Width = 200 };

            var lblQuantity = new Label() { Text = "Quantity:", Left = 20, Top = 110, Width = 110 };
            txtQuantity = new TextBox() { Left = 140, Top = 108, Width = 200 };

            btnAdd = new Button() { Text = "Add Medicine", Left = 370, Top = 15, Width = 120 };
            btnSearch = new Button() { Text = "Search", Left = 370, Top = 55, Width = 120 };
            btnUpdateStock = new Button() { Text = "Update Stock", Left = 510, Top = 15, Width = 120 };
            btnRecordSale = new Button() { Text = "Record Sale", Left = 510, Top = 55, Width = 120 };
            btnViewAll = new Button() { Text = "View All", Left = 650, Top = 15, Width = 120 };

            var lblSearch = new Label() { Text = "Search (Name/Category):", Left = 20, Top = 150, Width = 150 };
            txtSearch = new TextBox() { Left = 180, Top = 148, Width = 300 };

            dgvMedicines = new DataGridView()
            {
                Left = 20,
                Top = 180,
                Width = 750,
                Height = 360,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            this.Controls.AddRange(new Control[]
            {
                lblName, txtName, lblCategory, txtCategory,
                lblPrice, txtPrice, lblQuantity, txtQuantity,
                btnAdd, btnSearch, btnUpdateStock, btnRecordSale, btnViewAll,
                lblSearch, txtSearch, dgvMedicines
            });
        }
    }
}
