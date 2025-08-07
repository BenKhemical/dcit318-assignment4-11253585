using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalAppointmentApp
{
    public class AppointmentForm : Form
    {
        private ComboBox comboBoxDoctor;
        private ComboBox comboBoxPatient;
        private DateTimePicker dateTimePicker;
        private TextBox textBoxNotes;
        private Button buttonBook;

        public AppointmentForm()
        {
            Text = "Book Appointment";
            Width = 400;
            Height = 350;

            Label lblDoctor = new Label { Text = "Select Doctor:", Left = 20, Top = 30, Width = 100 };
            comboBoxDoctor = new ComboBox { Left = 140, Top = 25, Width = 200 };

            Label lblPatient = new Label { Text = "Select Patient:", Left = 20, Top = 70, Width = 100 };
            comboBoxPatient = new ComboBox { Left = 140, Top = 65, Width = 200 };

            Label lblDate = new Label { Text = "Appointment Date:", Left = 20, Top = 110, Width = 120 };
            dateTimePicker = new DateTimePicker { Left = 140, Top = 105, Width = 200 };

            Label lblNotes = new Label { Text = "Notes:", Left = 20, Top = 150, Width = 100 };
            textBoxNotes = new TextBox { Left = 140, Top = 145, Width = 200, Height = 60, Multiline = true };

            buttonBook = new Button { Text = "Book Appointment", Left = 100, Top = 220, Width = 180 };
            buttonBook.Click += ButtonBook_Click;

            Controls.Add(lblDoctor);
            Controls.Add(comboBoxDoctor);
            Controls.Add(lblPatient);
            Controls.Add(comboBoxPatient);
            Controls.Add(lblDate);
            Controls.Add(dateTimePicker);
            Controls.Add(lblNotes);
            Controls.Add(textBoxNotes);
            Controls.Add(buttonBook);

            Load += AppointmentForm_Load;
        }

        private void AppointmentForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Load Doctors
                    SqlCommand cmdDoctors = new SqlCommand("SELECT DoctorID, FullName FROM Doctors WHERE Availability = 1", conn);
                    SqlDataAdapter daDoctors = new SqlDataAdapter(cmdDoctors);
                    DataTable dtDoctors = new DataTable();
                    daDoctors.Fill(dtDoctors);
                    comboBoxDoctor.DataSource = dtDoctors;
                    comboBoxDoctor.DisplayMember = "FullName";
                    comboBoxDoctor.ValueMember = "DoctorID";

                    // Load Patients
                    SqlCommand cmdPatients = new SqlCommand("SELECT PatientID, FullName FROM Patients", conn);
                    SqlDataAdapter daPatients = new SqlDataAdapter(cmdPatients);
                    DataTable dtPatients = new DataTable();
                    daPatients.Fill(dtPatients);
                    comboBoxPatient.DataSource = dtPatients;
                    comboBoxPatient.DisplayMember = "FullName";
                    comboBoxPatient.ValueMember = "PatientID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading doctors or patients: " + ex.Message);
            }
        }

        private void ButtonBook_Click(object sender, EventArgs e)
        {
            if (comboBoxDoctor.SelectedValue == null || comboBoxPatient.SelectedValue == null)
            {
                MessageBox.Show("Please select both doctor and patient.");
                return;
            }

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Step 1: Check if doctor is already booked at that time
                    string checkQuery = @"SELECT COUNT(*) 
                                          FROM Appointments 
                                          WHERE DoctorID = @DoctorID AND AppointmentDate = @Date";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@DoctorID", comboBoxDoctor.SelectedValue);
                    checkCmd.Parameters.AddWithValue("@Date", dateTimePicker.Value);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("This doctor is already booked for the selected date/time. Please choose another.");
                        return;
                    }

                    // Step 2: Book the appointment
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) VALUES (@DoctorID, @PatientID, @Date, @Notes)", conn
                    );
                    cmd.Parameters.AddWithValue("@DoctorID", comboBoxDoctor.SelectedValue);
                    cmd.Parameters.AddWithValue("@PatientID", comboBoxPatient.SelectedValue);
                    cmd.Parameters.AddWithValue("@Date", dateTimePicker.Value);
                    cmd.Parameters.AddWithValue("@Notes", textBoxNotes.Text);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        MessageBox.Show("Appointment booked successfully!");
                    else
                        MessageBox.Show("Failed to book appointment.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error booking appointment: " + ex.Message);
            }
        }
    }
}
