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
    public partial class Group : UserControl
    {
        public string connString;
        internal DataContext dc;
        internal List<int> changedCellIndex = new List<int>();
        internal Table<UserGroup> groupTable;
        public Group(string conString)
        {
            InitializeComponent();
            this.connString = conString;
        }
        
        private void Group_Load(object sender, EventArgs e)
        {
            LoadData();
            this.dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
        }
        public void LoadData()
        {
            this.dc = new DataContext(connString);
            this.groupTable = dc.GetTable<UserGroup>();
            this.dataGridView1.DataSource = groupTable;
        }
        private void DataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            changedCellIndex.Add(x);
        }

        internal void AssignValues(UserGroup g)
        {
            string groupid = this.textBox1.Text.Trim();
            string name = this.textBox2.Text.Trim();
            string form = this.textBox3.Text.Trim();
            string cs = this.textBox4.Text.Trim();
            g.GroupID = groupid;
            g.GroupName = name;
            g.UserForm = form;
            g.ConnectString = cs;
        }
        public void HandleException()
        {
            Panel panel = this.Parent as Panel;
            Group g = new Group(this.connString);
            this.Dispose();
            panel.Controls.Clear();
            panel.Controls.Add(g);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                UserGroup g = new UserGroup();
                AssignValues(g);
                this.groupTable.InsertOnSubmit(g);
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
        private void button2_Click(object sender, EventArgs e)
        {
            string title = "Cảnh báo";
            string message = "Thao tác xóa có thể ảnh hưởng đến chức năng của chương trình. Bạn có muốn tiếp tục?";
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                int cellrow = this.dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewCell cell = this.dataGridView1.Rows[cellrow].Cells[0];
                try
                {
                    var res = from g in groupTable where g.GroupID == cell.Value.ToString() select g;
                    if (res != null)
                    {
                        UserGroup g = res.FirstOrDefault();
                        this.groupTable.DeleteOnSubmit(g);
                        this.dc.SubmitChanges();
                        LoadData();
                        dataGridView1.Update();
                        this.dataGridView1.Refresh();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    HandleException();
                }
            }    
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string title = "Cảnh báo";
            string message = "Thao tác sửa có thể ảnh hưởng đến chức năng của chương trình. Bạn có muốn tiếp tục?";
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    foreach (int x in changedCellIndex)
                    {
                        DataGridViewCellCollection cells = dataGridView1.Rows[x].Cells;
                        //UserGroup user = groupTable.Where(g => g.GroupID == cells[0].Value.ToString()).FirstOrDefault();
                        //user.GroupName = cells[1].Value.ToString();
                        //user.UserForm = cells[2].Value.ToString();
                        //user.ConnectString = user.ConnectString;
                        dc.SubmitChanges();
                        LoadData();
                        dataGridView1.Update();
                        this.dataGridView1.Refresh();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message);
                    HandleException();
                }
            }    
        }
    }
}
