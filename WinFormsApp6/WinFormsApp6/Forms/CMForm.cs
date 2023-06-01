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
    public partial class CMForm : Form
    {
        public string name;
        public string conString;
        public string groupID;
        public CMForm(string name, string conString, string id)
        {
            InitializeComponent();
            this.name = name;
            this.conString = conString;
            this.groupID = id;
            DataContext dataContext = new DataContext(conString);
            Table<UserMenu> userMenus = dataContext.GetTable<UserMenu>();
            var result = from um in userMenus where um.GroupID == this.groupID select um.MenuName;
            List<string> list = result.ToList();
            int y = 83;
            foreach (string item in list)
            {
                MyButton button = new MyButton(this.conString, item);
                button.Location = new Point(14, y);
                button.Text = item;
                this.Controls.Add(button);
                y += 100;
            }
        }
        private void CMForm_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Xin chào, " + name + "\n";
            this.label2.Text = "Nhấn một nút để tiếp tục";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }   
    }
}
