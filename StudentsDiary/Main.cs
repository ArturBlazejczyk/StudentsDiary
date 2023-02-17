using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private string _filePath = 
            Path.Combine(Environment.CurrentDirectory, "students.txt");

        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            SetColumnsHeaders();
          
        }

        private void RefreshDiary() 
        {
            var fileHelper = new FileHelper(_filePath);
            var students = fileHelper.DeserializeFromFile();
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
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.ShowDialog();
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
                var fileHelper = new FileHelper(_filePath);
                var students = fileHelper.DeserializeFromFile();
                students.RemoveAll(x => x.Id == Convert.ToInt32(selectedStudent.Cells[0].Value));
                fileHelper.SerializeToFile(students);
                dgvDiary.DataSource = students;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }
    }
}
