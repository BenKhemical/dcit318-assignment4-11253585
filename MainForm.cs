using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PharmacyInventoryApp
{
    public partial class MainForm : Form
    {
        private readonly string connectionString;

        public MainForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;

            // Wire events after InitializeComponent
            btnAdd.Click += BtnAdd_Click;
            btnSearch.Click += BtnSearch_Click;
            btnUpdateStock.Click += BtnUpdateStock_Click;
            btnRecordSale.Click += BtnRecordSale_Click;
            btnViewAll.Click += BtnViewAll_Click;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(out decimal price, out int quantity)) return;

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("AddMedicine", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Category", txtCategory.Text.Trim());
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Medicine added successfully.");
                ClearInputFields();
                LoadAllMedicines();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding medicine: {ex.Message}");
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Enter a search term.");
                return;
            }

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("SearchMedicine", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

            try
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvMedicines.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching medicines: {ex.Message}");
            }
        }

        private void BtnUpdateStock_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuantity.Text.Trim(), out int newQuantity))
            {
                MessageBox.Show("Enter a valid quantity.");
                return;
            }

            if (dgvMedicines.CurrentRow == null)
            {
                MessageBox.Show("Select a medicine from the list first.");
                return;
            }

            int medicineId = Convert.ToInt32(dgvMedicines.CurrentRow.Cells["MedicineID"].Value);

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("UpdateStock", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@MedicineID", medicineId);
            cmd.Parameters.AddWithValue("@Quantity", newQuantity);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Stock updated successfully.");
                LoadAllMedicines();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock: {ex.Message}");
            }
        }

        private void BtnRecordSale_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantitySold))
            {
                MessageBox.Show("Enter a valid quantity.");
                return;
            }

            if (dgvMedicines.CurrentRow == null)
            {
                MessageBox.Show("Select a medicine from the list first.");
                return;
            }

            int medicineId = Convert.ToInt32(dgvMedicines.CurrentRow.Cells["MedicineID"].Value);

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("RecordSale", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@MedicineID", medicineId);
            cmd.Parameters.AddWithValue("@QuantitySold", quantitySold);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Sale recorded successfully.");
                LoadAllMedicines();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error recording sale: {ex.Message}");
            }
        }

        private void BtnViewAll_Click(object sender, EventArgs e)
        {
            LoadAllMedicines();
        }

        private void LoadAllMedicines()
        {
            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("GetAllMedicines", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvMedicines.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading medicines: {ex.Message}");
            }
        }

        private bool ValidateInput(out decimal price, out int quantity)
        {
            price = 0;
            quantity = 0;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter medicine name.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCategory.Text))
            {
                MessageBox.Show("Enter category.");
                return false;
            }
            if (!decimal.TryParse(txtPrice.Text.Trim(), out price) || price < 0)
            {
                MessageBox.Show("Enter valid price.");
                return false;
            }
            if (!int.TryParse(txtQuantity.Text.Trim(), out quantity) || quantity < 0)
            {
                MessageBox.Show("Enter valid quantity.");
                return false;
            }
            return true;
        }

        private void ClearInputFields()
        {
            txtName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }
    }
}
