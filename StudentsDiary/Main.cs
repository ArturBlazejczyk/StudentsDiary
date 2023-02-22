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
        private List<Group> _groups;

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

            _groups = GroupsHelper.GetGroups("Wszyscy");
            InitGroupsCombobox();

            SetColumnsHeaders();

            if (IsMaximize)
                WindowState = FormWindowState.Maximized;
        }

        private void InitGroupsCombobox()
        {
            cmbFilter.DataSource = _groups;
            cmbFilter.DisplayMember = "Name";
            cmbFilter.ValueMember = "Id";
        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = students;
        }

        private void SetColumnsHeaders()
        {
            dgvDiary.Columns[nameof(Student.Id)].HeaderText = "Numer";
            dgvDiary.Columns[nameof(Student.FirstName)].HeaderText = "Imię";
            dgvDiary.Columns[nameof(Student.LastName)].HeaderText = "Nazwisko";
            dgvDiary.Columns[nameof(Student.Comments)].HeaderText = "Uwagi";
            dgvDiary.Columns[nameof(Student.Math)].HeaderText = "Matematyka";
            dgvDiary.Columns[nameof(Student.Technology)].HeaderText = "Technologia";
            dgvDiary.Columns[nameof(Student.Physics)].HeaderText = "Fizyka";
            dgvDiary.Columns[nameof(Student.PolishLang)].HeaderText = "Jęz. polski";
            dgvDiary.Columns[nameof(Student.ForeignLang)].HeaderText = "Jęz. obcy";
            dgvDiary.Columns[nameof(Student.AdditionalClasses)].HeaderText = "Zaj. dodatkowe";
            dgvDiary.Columns[nameof(Student.GroupId)].HeaderText = "Grupa";
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

