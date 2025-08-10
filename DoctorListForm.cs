using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalAppointmentApp
{
    public class DoctorListForm : Form
    {
        private DataGridView dataGridViewDoctors;

        public DoctorListForm()
        {
            Text = "Available Doctors";
            Width = 600;
            Height = 400;

            dataGridViewDoctors = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Controls.Add(dataGridViewDoctors);
            Load += DoctorListForm_Load;
        }

        private void DoctorListForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT DoctorID, FullName, Specialty, CASE WHEN Availability = 1 THEN 'Available' ELSE 'Unavailable' END AS Status FROM Doctors";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(reader);
                    dataGridViewDoctors.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading doctors: " + ex.Message);
            }
        }
    }
}
