using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private Student _student;
        private List<Group> _groups;
        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;

            _groups = GroupsHelper.GetGroups("Brak");
            InitGroupsCombobox();

            GetStudentData();
            tbFirstName.Select();
        }

        private void InitGroupsCombobox()
        {
            cmbGroup.DataSource = _groups;
            cmbGroup.DisplayMember = "Name";
            cmbGroup.ValueMember = "Id";
        }

        public void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak ucznia o podanym ID");

                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPhysics.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            rtbComments.Text = _student.Comments;
            cbAdditionalClasses.Checked = _student.AdditionalClasses;
            cmbGroup.SelectedItem = _groups.FirstOrDefault(x => x.Id == _student.GroupId);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AsignId(students);

            AddNewUserToList(students);
            _fileHelper.SerializeToFile(students);
            Close();
        }

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Math = tbMath.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                Comments = rtbComments.Text,
                AdditionalClasses = cbAdditionalClasses.Checked,
                GroupId = (cmbGroup.SelectedItem as Group).Id,
            };
            students.Add(student);
        }

        private void AsignId(List<Student> students)
        {
            var studentWithHighestId = students
                    .OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ?
                1 : studentWithHighestId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
