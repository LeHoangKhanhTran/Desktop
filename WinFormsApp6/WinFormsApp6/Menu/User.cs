using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;

namespace WinFormsApp6
{
    public partial class User : UserControl
    {
        public string connString;
        internal DataContext dc;
        internal List<int> changedCellIndex = new List<int>();
        internal Table<Users> userTable;
        public User(string conString)
        {
            InitializeComponent();
            this.connString = conString;
        }

        private void User_Load(object sender, EventArgs e)
        {
            LoadData();
            this.dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
        }

        private void DataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            changedCellIndex.Add(x);
        }

        public void HandleException()
        {
            Panel panel = this.Parent as Panel;
            User u = new User(this.connString);
            this.Dispose();
            panel.Controls.Clear();
            panel.Controls.Add(u);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Users u = new Users();
                AssignValues(u);
                this.userTable.InsertOnSubmit(u);
                this.dc.SubmitChanges();
                LoadData();
                dataGridView1.Update();
                this.dataGridView1.Refresh();
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox)
                    {
                        (c as TextBox).Text = String.Empty;
                    }    
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }

        public void LoadData()
        {
            this.dc = new DataContext(connString);
            this.userTable = dc.GetTable<Users>();
            this.dataGridView1.DataSource = userTable;
        }

        internal void AssignValues(Users u)
        {
            int id = int.Parse(this.textBox1.Text.Trim());
            string name = this.textBox2.Text.Trim();
            string password = this.textBox3.Text.Trim();
            string groupID = this.textBox4.Text.Trim();
            u.UserId = id;
            u.UserName = name;
            u.Password = password;
            u.UserGroupID = groupID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int cellrow = this.dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewCell cell = this.dataGridView1.Rows[cellrow].Cells[0];
            try
            {
                var res = from us in userTable where us.UserId == int.Parse(cell.Value.ToString()) select us;
                if (res != null)
                {
                    Users us = res.FirstOrDefault();
                    this.userTable.DeleteOnSubmit(us);
                    this.dc.SubmitChanges();
                    LoadData();
                    dataGridView1.Update();
                    this.dataGridView1.Refresh();
                    this.Parent.Refresh();
                }
                
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (int x in changedCellIndex)
                {
                    DataGridViewCellCollection cells = dataGridView1.Rows[x].Cells;
                    Users user = userTable.Where(us => us.UserId == int.Parse(cells[0].Value.ToString())).FirstOrDefault();
                    user.UserName = cells[1].Value.ToString();
                    user.Password = cells[2].Value.ToString();
                    user.UserGroupID = cells[3].Value.ToString();
                    dc.SubmitChanges();
                    LoadData();
                    dataGridView1.Update();
                    this.dataGridView1.Refresh();
                    this.Parent.Refresh();
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message);
                HandleException();
            }
            finally
            {
                LoadData();
                dataGridView1.Update();
                this.dataGridView1.Refresh();
            }
        }
    }
}
