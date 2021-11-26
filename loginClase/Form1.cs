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
    public partial class Form1 : Form
    {
        //cadena de conexion
        SqlConnection con = new SqlConnection("Data Source = LAPTOPALEX; Initial Catalog = db_Login; Integrated Security = True");

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_ingresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_nombre.Text) || string.IsNullOrEmpty(txt_contraseña.Text))   {
                MessageBox.Show("Campos vacios !!");
            }else
            {
                loguear(txt_nombre.Text,txt_contraseña.Text);
            }
        }

        private void loguear(string nombre, string pass) 
        {
            try
            {
                //Verificar datos si existen o no usuario
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT usu_nombre, tusu_id FROM tbl_usuario WHERE usu_nomlogin = @usuario",con);
                cmd.Parameters.AddWithValue("usuario", nombre);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Close();

                //Comparando usuario de la base de datos y la contraseña encriptada
                if (dt.Rows.Count ==1) 
                {
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("SELECT usu_nombre, tusu_id FROM tbl_usuario WHERE usu_nomlogin = @usuario and (CONVERT(varchar(max),DECRYPTBYPASSPHRASE ('password',usu_password))) = @passw" , con);
                    cmd1.Parameters.AddWithValue("usuario", nombre);
                    cmd1.Parameters.AddWithValue("passw", pass);
                    SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    sda1.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count == 1) 
                    {
                        this.Hide();
                        if(dt1.Rows[0][1].ToString() == "1")
                        {
                            new Admin().ShowDialog();
                        }
                        else if (dt1.Rows[0][1].ToString() == "2")
                        {
                            new Usuario().ShowDialog();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Datos incorrectos..!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Usuario no existente");
                }
            }
            catch(Exception ex) 
            { 
            
            }
        }

        private void lnkl_registrarse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new Registrar().ShowDialog();
        }
    }
}
