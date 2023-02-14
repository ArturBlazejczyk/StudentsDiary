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

namespace StudentsDiary
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            var pathToDataBase = $@"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\..\DataBase.txt";

            //if (!File.Exists(pathToDataBase))
            //File.Create(pathToDataBase);

            //File.WriteAllText(pathToDataBase, "Zostań programisto");
            File.AppendAllText(pathToDataBase, "Akademia\n");

            var text  = File.ReadAllText(pathToDataBase);
            MessageBox.Show(text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
    }
}
