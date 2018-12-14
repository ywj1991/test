using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Class1 cl = new Class1();
            cl.MainRun();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }
        private  void Save()
        { 
            string sName = txtName.Text;
            string sNumber = txtNumber.Text;
            if (sName == "")
            {
                MessageBox.Show("姓名不能为空");
                return;
            }
            if (sNumber == "")
            {
                MessageBox.Show("代码不能为空");
                return;
            }
            string sSql = "insert into Emp(Name,Number) values ('" + sName + "','" + sNumber + "')";
            DbOperation Db = new DbOperation();
            Db.sqlcmd(sSql);
            MessageBox.Show("成功");

        }
        public class DbConnection
        {
            public static string sqlcon = "Data Source =DESKTOP-JL0B035\\SQL2008;Initial Catalog=222;Integrated Security=True;User ID=sa;Password=";
            public SqlConnection getcon()
            {
                SqlConnection mycon = new SqlConnection(sqlcon);
                return mycon;
            }
        }
        
        class DbOperation
        {
            DbConnection constr = new DbConnection();
            public void sqlcmd(string sqlstr)
            {
                try
                {
                    SqlConnection conn = constr.getcon();
                    conn.Open();
                    SqlCommand scmd = new SqlCommand(sqlstr, conn);
                    scmd.ExecuteNonQuery();
                    conn.Dispose();
                    conn.Close();
                }
                catch
                { }
            }
            public DataTable getTable(string sqlstr)
            {
                DataTable dt = new DataTable();
                try
                {
                    SqlConnection conn = constr.getcon();
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlstr, conn);
                    sda.Fill(dt);
                    conn.Dispose();
                    conn.Close();
                }
                catch { }
                return dt;
            }
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            string sSql = "select * from Emp";

            DbOperation Db = new DbOperation();
            dataGridView1.DataSource= Db.getTable(sSql);
        }
    }

}
