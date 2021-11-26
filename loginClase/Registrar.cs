using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace loginClase
{
    public partial class Registrar : Form
    {
        SqlConnection con = new SqlConnection("Data Source = LAPTOPALEX; Initial Catalog = db_Login; Integrated Security = True");
        public Registrar()
        {
            InitializeComponent();
           // cmb_perfil.Text = "Seleccione";
        }

        private void btn_ingresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_nombre.Text) || string.IsNullOrEmpty(txt_cedula.Text) || string.IsNullOrEmpty(txt_contraseña.Text)
                || string.IsNullOrEmpty(txt_correo.Text) || string.IsNullOrEmpty(txt_direccion.Text) || string.IsNullOrEmpty(txt_nick.Text) || cmb_perfil.Text =="Seleccione")
            {
                MessageBox.Show("Campos vacios");

            }
            else if (txt_contraseña.Text.Length < 5)
            {
                MessageBox.Show("Su contraseña debe tener 5 caracteres !!");
            }
            else 
            {
                try
                {
                    con.Open();
                    string sql = "INSERT INTO tbl_usuario values(@nombre,@cedu,@nick,@pass,@dire,@correo,'A',@tusu)";
                    SqlCommand cmd = new SqlCommand(sql,con);
                    cmd.Parameters.AddWithValue("@nombre",txt_nombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@cedu", txt_cedula.Text.Trim());
                    cmd.Parameters.AddWithValue("@nick", txt_nick.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txt_contraseña.Text.Trim());
                    cmd.Parameters.AddWithValue("@dire", txt_direccion.Text.Trim());
                    cmd.Parameters.AddWithValue("@correo", txt_correo.Text.Trim());
                    cmd.Parameters.AddWithValue("@tusu",Convert.ToInt32( cmb_perfil.SelectedValue));
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Usuario Registrado !!!");
                }
                catch(Exception ex) 
                {
                    throw;
                }
            }

        }

       

        private void Registrar_Load_1(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT tusu_id, tusu_nombre FROM tbl_tipousuario WHERE tusu_estado = 'A'", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dt.Columns.Add("tusu_id", typeof(int));
            dt.Columns.Add("tusu_nombre", typeof(string));
            dt.Rows.Add(0,"Seleccione");
            sda.Fill(dt);
            con.Close();

            cmb_perfil.DataSource = dt;
            cmb_perfil.DisplayMember = "tusu_nombre";
            cmb_perfil.ValueMember = "tusu_id";
        }
    }
}
