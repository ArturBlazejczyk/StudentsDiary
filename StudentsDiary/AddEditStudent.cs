﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private string _filePath =
            Path.Combine(Environment.CurrentDirectory, "students.txt");
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();

            _studentId = id;

            if(id != 0)
            {
                var fileHelper = new FileHelper(_filePath);
                Text = "Edytowanie danych ucznia";

                var students = fileHelper.DeserializeFromFile();
                var student = students.FirstOrDefault(x => x.Id == id);

                if(student == null) 
                    throw new Exception("Brak ucznia o podanym ID");

                tbId.Text = student.Id.ToString();
                tbFirstName.Text = student.FirstName;
                tbLastName.Text = student.LastName;
                tbMath.Text =   student.Math;
                tbTechnology.Text = student.Technology;
                tbPhysics.Text = student.Physics;
                tbPolishLang.Text = student.PolishLang;
                tbForeignLang.Text = student.ForeignLang;
                rtbComments.Text = student.Comments;
            }

            tbFirstName.Select();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var fileHelper = new FileHelper(_filePath);
            var students = fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
            {
                var studentWithHighestId = students
                    .OrderByDescending(x => x.Id).FirstOrDefault();

                _studentId = studentWithHighestId == null ?
                    1 : studentWithHighestId.Id + 1;
            }

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
            };

            students.Add(student);

            fileHelper.SerializeToFile(students);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
