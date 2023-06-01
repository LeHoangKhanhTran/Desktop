using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;

namespace WinFormsApp6
{
    internal class MyButton : Button
    {
        public string connString;
        public string userControl;
        public MyButton(string connString, string uc)
        {
            this.connString = connString;
            this.userControl = uc;
            this.Size = new Size(198, 55);
            this.Click += User_Click;
            this.UseVisualStyleBackColor = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        }

        private void User_Click(object? sender, EventArgs e)
        {
            
            object uc = Activator.CreateInstance(Type.GetType("WinFormsApp6." + this.userControl), new object[] { this.connString });
            UserControl userControl = uc as UserControl; 
            Panel panel = this.Parent.Controls.OfType<Panel>().FirstOrDefault();
            panel.Controls.Clear();
            panel.Controls.Add(userControl);
            this.Parent.Refresh();    
        }
    }
    
   

}   
