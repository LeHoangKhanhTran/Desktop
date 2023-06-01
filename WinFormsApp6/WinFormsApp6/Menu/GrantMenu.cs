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
    public partial class GrantMenu : UserControl
    {
        public string connString;
        internal DataContext dc;
        Table<UserMenu> userMenuTable;
        Table<UserGroup> groupTable;
        public GrantMenu(string conString)
        {
            InitializeComponent();
            this.connString = conString;
        }

        private void GrantMenu_Load(object sender, EventArgs e)
        {
            this.dc = new DataContext(connString);
            groupTable = dc.GetTable<UserGroup>();
            comboBox1.DataSource = groupTable;
            comboBox1.DisplayMember = "GroupName";
            comboBox1.ValueMember = "GroupID";
            comboBox2.DataSource = groupTable;
            comboBox2.DisplayMember = "GroupName";
            comboBox2.ValueMember = "GroupID";
            userMenuTable = dc.GetTable<UserMenu>();
            var res = from g in userMenuTable group g by g.MenuName into gmn select gmn;
            //List<string> list = res.ToList();
            foreach (var s in res)
            {
                comboBox3.Items.Add(s.Key); 
            }      
            comboBox3.SelectedIndex = 0;
        }

        public void HandleException()
        {
            Panel panel = this.Parent as Panel;
            GrantMenu gr = new GrantMenu(this.connString);
            this.Dispose();
            panel.Controls.Clear();
            panel.Controls.Add(gr);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string x = this.comboBox1.SelectedValue.ToString();
                var res = from um in userMenuTable join g in groupTable on um.GroupID equals g.GroupID where g.GroupID == x select new { g.GroupName, um.MenuName };
                this.dataGridView1.DataSource = res;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string groupID = this.comboBox2.SelectedValue.ToString();
                string menu = this.comboBox3.SelectedItem.ToString();
                var res = from um in userMenuTable where um.GroupID == groupID && um.MenuName == menu select um;
                if (res.FirstOrDefault() == null)
                {
                    SqlConnection con = new SqlConnection(this.connString);
                    SqlCommand cmd = new SqlCommand("INSERT INTO UserMenu VALUES (@groupid, @menuname)", con);
                    SqlParameter id = new SqlParameter("@groupid", groupID);
                    SqlParameter name = new SqlParameter("@menuname", menu);
                    cmd.Parameters.AddRange(new SqlParameter[] { id, name });
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    MessageBox.Show(menu + " đã có trong menu của " + comboBox2.Text);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string groupID = this.comboBox2.SelectedValue.ToString();
                string menu = this.comboBox3.SelectedItem.ToString();
                var res = from um in userMenuTable where um.GroupID == groupID && um.MenuName == menu select um;
                if (res.FirstOrDefault() != null)
                {
                    SqlConnection con = new SqlConnection(this.connString);
                    SqlCommand cmd = new SqlCommand("DELETE FROM UserMenu WHERE GroupID = @groupid AND MenuName = @menuname", con);
                    SqlParameter id = new SqlParameter("@groupid", groupID);
                    SqlParameter name = new SqlParameter("@menuname", menu);
                    cmd.Parameters.AddRange(new SqlParameter[] { id, name });
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    MessageBox.Show(menu + " chưa có trong menu của " + comboBox2.Text);
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
