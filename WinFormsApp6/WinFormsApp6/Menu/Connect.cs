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
using System.Data.SqlClient;
namespace WinFormsApp6
{
    public partial class Connect : UserControl
    {
        public string connString;
        internal DataContext dc;
        internal List<int> changedCellIndex = new List<int>();
        internal Table<UserGroup> groupTable;
        public Connect(string conString)
        {
            InitializeComponent();
            this.connString = conString;
        }

        private void Connect_Load(object sender, EventArgs e)
        {   
            LoadData();
            this.dataGridView1.Columns[0].Width = 150;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void DataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            changedCellIndex.Add(x);
        }

        public void LoadData()
        {
            this.dc = new DataContext(connString);
            this.groupTable = dc.GetTable<UserGroup>();
            var res = from g in groupTable select new { g.GroupName, g.ConnectString };
            this.dataGridView1.DataSource = res;
            var result = from g in groupTable select g.GroupName;
            List<string> list = result.ToList();
            foreach (string s in list)
            {
                this.comboBox1.Items.Add(s);
            }    
        }
        public void HandleException()
        {
            Panel panel = this.Parent as Panel;
            Connect c = new Connect(this.connString);
            this.Dispose();
            panel.Controls.Clear();
            panel.Controls.Add(c);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    var result = from g in groupTable where g.GroupName == comboBox1.SelectedItem.ToString() select g.ConnectString;
                    if (result.FirstOrDefault() != null)
                    {
                        var res = from g in groupTable where g.GroupName == comboBox1.SelectedItem.ToString() select g;
                        UserGroup userGroup = res.FirstOrDefault();
                        userGroup.ConnectString = null;
                        dc.SubmitChanges();
                        LoadData();
                        dataGridView1.Update();
                        this.dataGridView1.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Nhóm người dùng không có chuỗi Connect.");
                    }
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
                if (comboBox1.SelectedItem != null)
                {
                    var result = from g in groupTable where g.GroupName == comboBox1.SelectedItem.ToString() select g.ConnectString;
                    if (result.FirstOrDefault() != null)
                    {
                        var res = from g in groupTable where g.GroupName == comboBox1.SelectedItem.ToString() select g;
                        UserGroup userGroup = res.FirstOrDefault();
                        userGroup.ConnectString = textBox1.Text;
                        dc.SubmitChanges();
                        LoadData();
                        dataGridView1.Update();
                        this.dataGridView1.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Nhóm người dùng không có chuỗi Connect.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string x = comboBox1.SelectedItem as string;
            var res = from g in groupTable where g.GroupName == x select g.ConnectString;
            List<string> list = res.ToList();
            textBox1.Text = list[0]; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    var result = from g in groupTable where g.GroupName == comboBox1.SelectedItem.ToString() select g.ConnectString;
                    if (result.FirstOrDefault() == null)
                    {
                        var res = from g in groupTable where g.GroupName == comboBox1.SelectedItem.ToString() select g;
                        UserGroup userGroup = res.FirstOrDefault();
                        userGroup.ConnectString = textBox1.Text;
                        dc.SubmitChanges();
                        LoadData();
                        dataGridView1.Update();
                        this.dataGridView1.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Nhóm người dùng đã có chuỗi Connect.");
                    }
                }
            }  
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }
    }
}
