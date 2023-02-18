using StudentsDiary.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

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
            
            SetColumnsHeader();

            if (IsMaximize)
                WindowState = FormWindowState.Maximized;
        }
   
        public void ShowFilterValues()
        {
         
            var students = _fileHelper.DeserializeFromFile();

            var valuesFromFilter = new List<string>();
            valuesFromFilter.Add("wszyscy");
            foreach (var item in students)
            {
                valuesFromFilter.Add(item.IdGroup.ToString());
            }

            valuesFromFilter.OrderBy(x => x.ToString());
            var groups = valuesFromFilter.Distinct().ToList();
            cmbSortByStudentGroup.DataSource = groups;
          
        }
        public void RefreshDiary()        {
            
            var students = _fileHelper.DeserializeFromFile();    
            ShowFilterValues(); 
            dgvDiary.DataSource = students;
        }
        private void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "zajęcia dodatkowe";
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
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytować");
                return;
            }
            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego chcesz usunąć");
                return;
            }
            var selectedStudent = dgvDiary.SelectedRows[0];
            var confirmDelete =
                MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()}",
                "Usuwanie ucznia",
                MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }
        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
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

        private void Main_Load(object sender, EventArgs e)
        {          
            RefreshDiary();
        }                
        public void ShowWantedGroup()
        {
            var students = _fileHelper.DeserializeFromFile();
            var SelectedGroup = cmbSortByStudentGroup.SelectedItem;
           
            if (cmbSortByStudentGroup.SelectedItem == "wszyscy")
             dgvDiary.DataSource = students;
            else
            {                       
                orderBy(SelectedGroup.ToString());
            }
        }
        private void cmbSortByStudentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowWantedGroup();           
        }
        public void orderBy(string idGroup)
        {
            var students = _fileHelper.DeserializeFromFile();     
            var tt = students.FindAll(x => x.IdGroup == idGroup);
            dgvDiary.DataSource = tt;         
        }
        
    }
}
