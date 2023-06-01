using System.Data.SqlClient;
using System;
using System.Linq;
using System.Data.Linq;
namespace WinFormsApp6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = this.textBox1.Text.Trim();
            string password = this.textBox2.Text.Trim();
            string connString = "server=DESKTOP-599E22R;database=PhanQuyen;uid=s2;pwd=1234567"; //s0 
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("PCR_FINDUSER", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter nameParam = new SqlParameter("@username", username);
            SqlParameter pwdParam = new SqlParameter("@password", password);
            SqlParameter resultParam = new SqlParameter("@result", System.Data.SqlDbType.VarChar, 7);
            resultParam.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.AddRange(new SqlParameter[] { nameParam, pwdParam, resultParam });
            connection.Open();
            cmd.ExecuteNonQuery();
            if (resultParam.Value.ToString() != "None")
            {
                cmd = new SqlCommand("PCR_FINDSTRING", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter groupId = new SqlParameter("@userGroupID", resultParam.Value);
                resultParam = new SqlParameter("@result", System.Data.SqlDbType.NVarChar, 255);
                SqlParameter formName = new SqlParameter("@form", System.Data.SqlDbType.NVarChar, 255);
                formName.Direction = System.Data.ParameterDirection.Output;
                resultParam.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.AddRange(new SqlParameter[] { groupId, resultParam, formName });
                cmd.ExecuteNonQuery();
                connection.Close();
                Type type = Type.GetType("WinFormsApp6." + formName.Value.ToString());
                object form = Activator.CreateInstance(type, new object[] { username, resultParam.Value.ToString(), groupId.Value.ToString() });
                Form newForm = (Form)form;
                newForm.Show();
                this.Dispose(false);
            }
            else
            {
                MessageBox.Show("Không tìm thấy người dùng");
            }    
        }
    }
}