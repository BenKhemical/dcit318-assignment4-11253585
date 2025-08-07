using System;
using System.Windows.Forms;

namespace MedicalAppointmentApp
{
    public class MainForm : Form
    {
        private Button btnDoctors, btnBook, btnManage;

        public MainForm()
        {
            Text = "Medical Appointment System";
            Width = 400;
            Height = 300;

            btnDoctors = new Button() { Text = "View Doctors", Top = 30, Left = 100, Width = 200 };
            btnBook = new Button() { Text = "Book Appointment", Top = 80, Left = 100, Width = 200 };
            btnManage = new Button() { Text = "Manage Appointments", Top = 130, Left = 100, Width = 200 };

            Controls.Add(btnDoctors);
            Controls.Add(btnBook);
            Controls.Add(btnManage);

            btnDoctors.Click += BtnDoctors_Click;
            btnBook.Click += BtnBook_Click;
            btnManage.Click += BtnManage_Click;
        }

        private void BtnDoctors_Click(object sender, EventArgs e)
        {
            DoctorListForm form = new DoctorListForm();
            form.ShowDialog();
        }

        private void BtnBook_Click(object sender, EventArgs e)
        {
            AppointmentForm form = new AppointmentForm();
            form.ShowDialog();
        }

       private void BtnManage_Click(object sender, EventArgs e)
{
    ManageAppointmentsForm form = new ManageAppointmentsForm();
    form.ShowDialog();
}

    }
}
