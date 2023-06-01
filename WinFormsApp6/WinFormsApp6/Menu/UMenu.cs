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
    public partial class UMenu : UserControl
    {
        public string connString;
        internal DataContext dc;
        internal List<int> changedCellIndex = new List<int>();
        internal Table<Menu> menuTable;
        public UMenu(string conString)
        {
            InitializeComponent();
            this.connString = conString;
        }


        private void Menu_Load(object sender, EventArgs e)
        {
            LoadData();
            var menus = from m in menuTable select m.MenuName;
            List<string> list = menus.ToList();
            foreach (string menuName in list)
            {
                this.comboBox1.Items.Add(menuName);
            }
            this.comboBox1.SelectedIndex = 0;
            this.dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
        }

        private void DataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            changedCellIndex.Add(x);
        }

        public void LoadData()
        {
            this.dc = new DataContext(connString);
            this.menuTable = dc.GetTable<Menu>();
            this.dataGridView1.DataSource = menuTable;
        }
        internal void AssignValues(Menu m)
        {
            string name = this.textBox2.Text.Trim();
            string description = this.textBox3.Text.Trim();
            m.MenuName = name;
            m.MenuDescription = description;
            
        }

        public void HandleException()
        {
            Panel panel = this.Parent as Panel;
            UMenu um = new UMenu(this.connString);
            this.Dispose();
            panel.Controls.Clear();
            panel.Controls.Add(um);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Menu m = new Menu();    
                AssignValues(m);
                this.menuTable.InsertOnSubmit(m);
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
                    var res = from m in menuTable where m.MenuName == cell.Value.ToString() select m;
                    if (res != null)
                    {
                        Menu m = res.FirstOrDefault();
                        this.menuTable.DeleteOnSubmit(m);
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
                        Menu menu = menuTable.Where(m => m.MenuName == cells[0].Value.ToString()).FirstOrDefault();
                        menu.MenuDescription = cells[1].Value.ToString();
                        dc.SubmitChanges();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message);
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

        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Table<UserGroup> userGroup = dc.GetTable<UserGroup>();
                Table<UserMenu> userMenu = dc.GetTable<UserMenu>();
                string x = this.comboBox1.SelectedItem.ToString();
                var res = from u in userGroup join um in userMenu on u.GroupID equals um.GroupID where um.MenuName == x select new { u.GroupID, u.GroupName };
                dataGridView1.DataSource = res;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandleException();
            }
        }
    }
}
