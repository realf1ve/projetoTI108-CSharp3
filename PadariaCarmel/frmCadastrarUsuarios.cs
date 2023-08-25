using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace PadariaCarmel
{
    public partial class frmCadastrarUsuarios : Form
    {

        //Criando variáveis para controle do menu
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);
        public frmCadastrarUsuarios()
        {
            InitializeComponent();
            desabilitarCampos();
        }
        public void desabilitarCampos()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = false;
            txtSenha.Enabled = false;
            txtContraSenha.Enabled = false;

            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = false;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            carregaFuncionarios();

        }
        public void habilitarCampos()
        {
            btnCadastrar.Enabled = true;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = true;
            btnNovo.Enabled = false;
            txtCodigo.Enabled = false;
            txtNome.Enabled = true;
            txtSenha.Enabled = true;
            txtContraSenha.Enabled = true;

            txtNome.Focus();
             }
        private void frmFuncionarios_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {


            if (txtNome.Text.Equals("") || txtSenha.Text.Equals("") || txtContraSenha.Text.Equals(""))
            {
                MessageBox.Show("Preenchimento obrigatório",
                    "Mensagem do sistema.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                txtNome.Focus();
            } 
            else
            {
                if(txtSenha.Text.Equals(txtContraSenha.Text))
                {
                    cadastrarUsuarios(Convert.ToInt32(txtCodFunc.Text));

                    MessageBox.Show("Cadastrado com sucesso!", "Mensagem do sistema.", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    desabilitarCampos();
                    btnNovo.Enabled = true;
                    limparCampos();
                }
                else
                {
                    MessageBox.Show("Usuário e senha não são compatíveis!", "Mensagem do sistema.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    txtSenha.Clear();
                    txtContraSenha.Clear();
                    txtSenha.Focus();
                }
            }
        }
        public void carregaFuncionarios()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select nome from tbFuncionarios order by nome asc;";
            comm.CommandType = CommandType.Text;

            comm.Connection = Conectar.obterConexao();
            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            lstFuncNCad.Items.Clear();

            while (DR.Read())
            {
                lstFuncNCad.Items.Add(DR.GetString(0));
            }
            Conectar.fecharConexao();
        }

        public void cadastrarUsuarios(int codFunc)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "insert into txtUsuarios(nome, senha)values(@nome, @senha);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 50).Value = txtNome.Text;
            comm.Parameters.Add("@senha", MySqlDbType.VarChar, 14).Value = txtSenha.Text;
            comm.Parameters.Add("@codFunc", MySqlDbType.Int32, 11).Value = Convert.ToInt32(codFunc);
            comm.Connection = Conectar.obterConexao();
            int res = comm.ExecuteNonQuery();
            Conectar.fecharConexao();


        }
        public void limparCampos()
        {
            txtCodigo.Enabled = false;
            txtNome.Clear();
            txtSenha.Clear();

            txtNome.Focus();
        }

        //CARREGAR USUÁRIOS

        public void carregarUsuarios(string nome)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select usu.nome, usu.senha, func.codFunc from tbFuncionarios as func inner join tbUsuarios as usu on usu.codFunc = func.codFunc where func.nome = '"+nome+"';";
            comm.CommandType = CommandType.Text;

            comm.Connection = Conectar.obterConexao();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();
            DR.Read();

            try
            { 
            txtNome.Text = DR.GetString(0);
            txtSenha.Text = DR.GetString(1);

            txtCodFunc.Text = DR.GetInt32(2).ToString();

            Conectar.fecharConexao();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Funcionário não possui usuário cadastrado!", "Mensagem do sistema.", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtNome.Clear();
                txtSenha.Clear();
                txtCodigo.Clear();
                txtNome.Focus();
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmFuncionarios abrir = new frmFuncionarios();
            abrir.Show();
            this.Hide();
        }

        private void lstFuncNCad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nome = lstFuncNCad.SelectedItem.ToString();
            carregarUsuarios(nome);
        }

    }
}
