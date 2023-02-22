using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);

        public bool IsMaximize
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }

        public Main()
        {
            InitializeComponent();
            RefreshDiary();

            SetColumnsHeaders();

            if (IsMaximize)
                WindowState = FormWindowState.Maximized;
        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = students;
        }

        private void SetColumnsHeaders()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Jęz. polski";
            dgvDiary.Columns[8].HeaderText = "Jęz. obcy";
            dgvDiary.Columns[9].HeaderText = "Zaj. dodatkowe";
            dgvDiary.Columns[10].HeaderText = "Grupa";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Zaznacz ucznia, którego chcesz edytować");
                return;
            }
            else
            {
                var addEditStudent = new AddEditStudent(
                    Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
                addEditStudent.FormClosing += AddEditStudent_FormClosing;
                addEditStudent.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Zaznacz ucznia, którego chcesz usunąć");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {selectedStudent.Cells[1].Value.ToString().Trim()} {selectedStudent.Cells[2].Value.ToString().Trim()}?",
              "Usuwanie ucznia",
              MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                StudentDelete(selectedStudent);
                RefreshDiary();
            }
        }

        private void StudentDelete(DataGridViewRow selectedStudent)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == Convert.ToInt32(selectedStudent.Cells[0].Value));
            _fileHelper.SerializeToFile(students);
            dgvDiary.DataSource = students;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            else
                IsMaximize = false;

            Settings.Default.Save();
        }

    }


}

