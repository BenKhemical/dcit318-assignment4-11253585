using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalAppointmentApp
{
    public class ManageAppointmentsForm : Form
    {
        private DataGridView dataGridViewAppointments;
        private Button btnUpdate, btnDelete;
        private DateTimePicker dateTimePicker;

        public ManageAppointmentsForm()
        {
            Text = "Manage Appointments";
            Width = 800;
            Height = 500;

            dataGridViewAppointments = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Label lblDate = new Label { Text = "New Date:", Left = 20, Top = 320, Width = 100 };
            dateTimePicker = new DateTimePicker { Left = 120, Top = 315, Width = 200 };

            btnUpdate = new Button { Text = "Update Date", Left = 340, Top = 315, Width = 120 };
            btnDelete = new Button { Text = "Delete Appointment", Left = 480, Top = 315, Width = 160 };

            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;

            Controls.Add(dataGridViewAppointments);
            Controls.Add(lblDate);
            Controls.Add(dateTimePicker);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);

            Load += ManageAppointmentsForm_Load;
        }

        private void ManageAppointmentsForm_Load(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            A.AppointmentID, 
                            D.FullName AS DoctorName, 
                            P.FullName AS PatientName, 
                            A.AppointmentDate, 
                            A.Notes
                        FROM Appointments A
                        JOIN Doctors D ON A.DoctorID = D.DoctorID
                        JOIN Patients P ON A.PatientID = P.PatientID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridViewAppointments.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to update.");
                return;
            }

            int appointmentId = Convert.ToInt32(dataGridViewAppointments.SelectedRows[0].Cells["AppointmentID"].Value);
            DateTime newDate = dateTimePicker.Value;

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Appointments SET AppointmentDate = @Date WHERE AppointmentID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Date", newDate);
                    cmd.Parameters.AddWithValue("@ID", appointmentId);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                        MessageBox.Show("Appointment updated successfully!");
                    else
                        MessageBox.Show("Update failed.");

                    LoadAppointments(); // refresh
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating appointment: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to delete.");
                return;
            }

            int appointmentId = Convert.ToInt32(dataGridViewAppointments.SelectedRows[0].Cells["AppointmentID"].Value);

            var confirm = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.No)
                return;

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Appointments WHERE AppointmentID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", appointmentId);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                        MessageBox.Show("Appointment deleted.");
                    else
                        MessageBox.Show("Delete failed.");

                    LoadAppointments(); // refresh
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting appointment: " + ex.Message);
            }
        }
    }
}
