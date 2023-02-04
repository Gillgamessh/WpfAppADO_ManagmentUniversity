using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WpfAppADO_ManagmentUniversity.Abstraction;
using WpfAppADO_ManagmentUniversity.Context;

namespace WpfAppADO_ManagmentUniversity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ManegerContext mc = new ManegerContext();
        private string sqlConnectionString = @"Data Source =ManagmentUniversity ;initial catalog=MCS;user id=sa;password=1234";
        int studentId;
        
         public MainWindow()
        {
            InitializeComponent();

            mc.Groups.Add(new Group { groupId = 1, GroupName = "GR123" });
            mc.Groups.Add(new Group { groupId = 2, GroupName = "GR124" });
            mc.Groups.Add(new Group { groupId = 3, GroupName = "GR125" });
            mc.Groups.Add(new Group { groupId = 4, GroupName = "GR126" });

            ListStatusOptions();
            GetStudents();
        }

        public List<Group> gp { get; set; }
        private void ListStatusOptions()
        {
            var item = mc.Groups.ToList();
            gp = item as List<Group>;
            DataContext = gp;
        }
        

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxName.Text = "";
            TextBoxMark.Text = "";
            TextBoxContact.Text = "";
            
        }

        private void GetStudents()
        {
            List<Student> students = GetAllStudent();

            this.studentDataGrid.ItemsSource = students.OrderBy(x => x.studId);
            this.studentDataGrid.CanUserAddRows = false;
        }

        private void InsertOrUpdateStudent(object sender, RoutedEventArgs e)
        {
            if (ButtonAdd.Content.ToString() == "Update")
            {
                if (string.IsNullOrEmpty(TextBoxName.Text) || string.IsNullOrEmpty(TextBoxMark.Text) || string.IsNullOrEmpty(TextBoxContact.Text))
                {
                    MessageBox.Show("Please enter all information");
                }
                else
                {
                    MessageBox.Show(UpdateStudent(new Student() { FullName = TextBoxName.Text, Marks = Convert.ToInt32(TextBoxMark.Text), ContactInfo = TextBoxContact.Text, }).ToString() + " row(s) affected");
                    GetStudents();
                    
                    ButtonAdd.Content = "Submit";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(TextBoxName.Text) || string.IsNullOrEmpty(TextBoxMark.Text) || string.IsNullOrEmpty(TextBoxContact.Text))
                {
                    MessageBox.Show("Please enter name and marks");
                }
                else
                {
                    MessageBox.Show(InsertStudent(new Student() { FullName = TextBoxName.Text, Marks = Convert.ToInt32(TextBoxMark.Text), ContactInfo = TextBoxContact.Text, }).ToString() + " row(s) affected");

                    GetStudents();
                    
                }
            }
        }

        
        private List<Student> GetAllStudent()
        {
            List<Student> students = new List<Student>();
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                students = connection.Query<Student>("Select studId, FullName, Marks from Student").ToList();
                connection.Close();
            }
            return students;
        }
        private int InsertStudent(Student student ,int group)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Insert into Student (FullName, Marks, ContactInfo) values (@TextBoxName, @TextBoxMark @TextBoxContact)", new { TextBoxName = student.FullName, TextBoxMark = student.Marks, TextBoxContact = student.ContactInfo });
                connection.Close();
                return affectedRows;
            }
        }
        private int UpdateStudent(Student student)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Update Student set FullName = @Name, Marks = @Marks Where studId = @Id", new { studId = studentId, FullName = TextBoxName.Text, Marks = TextBoxMark.Text });
                connection.Close();
                return affectedRows;
            }
        }
        private int DeleteStudent(Student student)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Delete from Student Where studId = @Id", new { studId = studentId });
                connection.Close();
                return affectedRows;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            studentId = ((Student)studentDataGrid.SelectedItem).studId;
            TextBoxName.Text = ((Student)studentDataGrid.SelectedItem).FullName;
            TextBoxMark.Text = ((Student)studentDataGrid.SelectedItem).Marks.ToString();
            TextBoxContact.Text = ((Student)studentDataGrid.SelectedItem).ContactInfo;
            ButtonAdd.Content = "Update";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            studentId = ((Student)studentDataGrid.SelectedItem).studId;
            MessageBox.Show(DeleteStudent(new Student() { studId = studentId }).ToString() + " row(s) affected");
            GetStudents();
        }

        private void BoxGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = BoxGroup.SelectedItem as Group;
            var idGroup = item.groupId.ToString();
            InsertStudent(Student,idGroup);
        }
    }

}

