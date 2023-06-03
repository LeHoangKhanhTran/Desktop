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
    public partial class Grant : UserControl
    {
        public string connString;
        internal DataContext dc;
        Table<UserGroup> groupTable;
        public Grant(string conString)
        {
            InitializeComponent();
            this.connString = conString;
        }

        private void Grant_Load(object sender, EventArgs e)
        {
            LoadData();
            var res = from g in groupTable select g.GroupName;
            List<string> list = res.ToList();
            foreach (string s in list)
            {
                this.comboBox1.Items.Add(s);
                this.comboBox2.Items.Add(s);    
            }
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
        }
        public void LoadData()
        {
            this.dc = new DataContext(connString);
            this.groupTable = dc.GetTable<UserGroup>();
            SqlConnection con = new SqlConnection(this.connString);
            SqlCommand cmd = new SqlCommand("SELECT name FROM sys.tables", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                this.comboBox3.Items.Add(dr.GetString(0));
            }    
            con.Close();
        }

        public void HandleException()
        {
            Panel panel = this.Parent as Panel;
            Grant gr = new Grant(this.connString);
            this.Dispose();
            panel.Controls.Clear();
            panel.Controls.Add(gr);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string x = this.comboBox1.SelectedItem as string;
                SqlConnection con = new SqlConnection(this.connString);
                SqlCommand cmd = new SqlCommand("SELECT pri.name As UserGroup, pri.type_desc AS [User Type], permit.permission_name AS [Permission], object_name(permit.major_id) AS [Object Name] FROM sys.database_principals pri LEFT JOIN sys.database_permissions permit ON permit.grantee_principal_id = pri.principal_id  WHERE name = '" + x + "'", con);
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "permission");
                this.dataGridView1.DataSource = ds;
                this.dataGridView1.DataMember = "permission";
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }

        public List<string> GetPermission()
        {
            List<string> list = new List<string>();
            Dictionary<CheckBox, string> map = new Dictionary<CheckBox, string>();
            map.Add(checkBox1, "INSERT");
            map.Add(checkBox2, "DELETE");
            map.Add(checkBox3, "UPDATE");
            map.Add(checkBox4, "SELECT");
            foreach (Control c in this.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox chk = (CheckBox)c;
                    if (chk.Checked)
                    {
                        list.Add(map[chk]);
                    }    
                }    
            }    
            return list;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("server=DESKTOP-599E22R;database=PhanQuyen;Integrated Security=true"); //Thay đổi tên server
                List<string> list = GetPermission();
                string table = this.comboBox3.SelectedItem.ToString();
                string group = this.comboBox2.SelectedItem.ToString();
                con.Open();
                foreach (string s in list)
                {
                    SqlCommand sqlCommand = new SqlCommand("GRANT " + s + " ON " + table + " TO " + group, con);
                    sqlCommand.ExecuteNonQuery();
                }
                con.Close();
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
                SqlConnection con = new SqlConnection("server=DESKTOP-599E22R;database=PhanQuyen;Integrated Security=true"); //Thay đổi tên server
                List<string> list = GetPermission();
                string table = this.comboBox3.SelectedItem.ToString();
                string group = this.comboBox2.SelectedItem.ToString();
                con.Open();
                foreach (string s in list)
                {
                    SqlCommand sqlCommand = new SqlCommand("REVOKE " + s + " ON " + table + " FROM " + group, con);
                    sqlCommand.ExecuteNonQuery();
                }
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }
    }
}
