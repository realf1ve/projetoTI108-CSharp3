﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace PadariaCarmel
{
    public partial class frmFuncionarios : Form
    {

        //Criando variáveis para controle do menu
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmFuncionarios()
        {
            InitializeComponent();
            desabilitarCampos();
        }
        public frmFuncionarios(string nome)
        {
            InitializeComponent();
            desabilitarCampos();
            txtNome.Text = nome;
            pesquisarNome(txtNome.Text);
        }



        private void frmFuncionarios_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            pesquisarCodigo();
        }

        //desabilitar campos
        public void desabilitarCampos()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = false;
            txtEnd.Enabled = false;
            txtNumero.Enabled = false;
            txtBairro.Enabled = false;
            txtCidade.Enabled = false;
            txtEmail.Enabled = false;

            mskCEP.Enabled = false;
            mskCPF.Enabled = false;
            mskTel.Enabled = false;

            cbbEstado.Enabled = false;

            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = false;
        }
        //habilitar campos
        public void habilitarCamposBuscarNome()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = true;
            txtEnd.Enabled = true;
            txtNumero.Enabled = true;
            txtBairro.Enabled = true;
            txtCidade.Enabled = true;
            txtEmail.Enabled = true;

            mskCEP.Enabled = true;
            mskCPF.Enabled = true;
            mskTel.Enabled = true;

            cbbEstado.Enabled = true;

            btnNovo.Enabled = false;
            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = true;
            btnExcluir.Enabled = true;
            btnLimpar.Enabled = true;

            txtNome.Focus();

        }
        public void habilitarCampos()
        {
            txtCodigo.Enabled = false;
            txtNome.Enabled = true;
            txtEnd.Enabled = true;
            txtNumero.Enabled = true;
            txtBairro.Enabled = true;
            txtCidade.Enabled = true;
            txtEmail.Enabled = true;

            mskCEP.Enabled = true;
            mskCPF.Enabled = true;
            mskTel.Enabled = true;

            cbbEstado.Enabled = true;

            btnCadastrar.Enabled = true;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = true;
            btnNovo.Enabled = false;

            txtNome.Focus();
        }

        //limpar campos
        public void limparCampos()
        {
            txtCodigo.Enabled = false;
            txtNome.Clear();
            txtEnd.Clear();
            txtNumero.Clear();
            txtBairro.Clear();
            txtCidade.Clear();
            txtEmail.Clear();

            mskCEP.Clear();
            mskCPF.Clear();
            mskTel.Clear();

            cbbEstado.Text = "";

            txtNome.Focus();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtBairro.Text.Equals("") || txtCidade.Text.Equals("") || txtEmail.Text.Equals("") || txtEnd.Text.Equals("") || txtNome.Text.Equals("") || txtNumero.Text.Equals("") || mskTel.Text.Equals("(  )      -") || mskCEP.Text.Equals("     -") || mskCPF.Text.Equals("   .   .   -") || cbbEstado.Text.Equals(""))
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
                cadastrarFuncionarios();
                MessageBox.Show("Cadastrado com sucesso!!!",
                    "Mensagem do sistema.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);

                desabilitarCampos();
                btnNovo.Enabled = true;
                limparCampos();
            }
        }


        //Cadastrar Funcionários
        public void cadastrarFuncionarios()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "insert into tbFuncionarios(nome, email, telCel, cpf, endereco, numero, bairro, cidade, estado , cep)values(@nome, @email, @telCel, @cpf, @endereco, @numero, @bairro, @cidade, @estado , @cep);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = txtEmail.Text;
            comm.Parameters.Add("@telCel", MySqlDbType.VarChar, 15).Value = mskTel.Text;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 14).Value = mskCEP.Text;
            comm.Parameters.Add("@endereco", MySqlDbType.VarChar, 100).Value = txtEnd.Text;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 10).Value = txtNumero.Text;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 100).Value = txtBairro.Text;
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 100).Value = txtCidade.Text;
            comm.Parameters.Add("@estado", MySqlDbType.VarChar, 2).Value = cbbEstado.Text;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 9).Value = mskCEP.Text;

            comm.Connection = Conectar.obterConexao();
            int res = comm.ExecuteNonQuery();
            Conectar.fecharConexao();

            //Alterar funcionários
        }
        public void alterarFuncionarios(int codigo)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "update tbFuncionarios set nome = @nome, email = @email, telCel = @telCel, cpf = @cpf, endereco = @endereco, numero = @numero, bairro = @bairro, cidade = @cidade, estado = @estado , cep = @cep where codFunc="+ codigo +";";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = txtEmail.Text;
            comm.Parameters.Add("@telCel", MySqlDbType.VarChar, 15).Value = mskTel.Text;
            comm.Parameters.Add("@cpf", MySqlDbType.VarChar, 14).Value = mskCEP.Text;
            comm.Parameters.Add("@endereco", MySqlDbType.VarChar, 100).Value = txtEnd.Text;
            comm.Parameters.Add("@numero", MySqlDbType.VarChar, 10).Value = txtNumero.Text;
            comm.Parameters.Add("@bairro", MySqlDbType.VarChar, 100).Value = txtBairro.Text;
            comm.Parameters.Add("@cidade", MySqlDbType.VarChar, 100).Value = txtCidade.Text;
            comm.Parameters.Add("@estado", MySqlDbType.VarChar, 2).Value = cbbEstado.Text;
            comm.Parameters.Add("@cep", MySqlDbType.VarChar, 9).Value = mskCEP.Text;

            comm.Connection = Conectar.obterConexao();
            int res = comm.ExecuteNonQuery();
            Conectar.fecharConexao();
        }


        //Deletar Usuários
        public void deletarFuncionarios(int codigo)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "delete from tbFuncionarios where codFunc=@codFunc;";

            comm.Parameters.Clear();
            comm.Parameters.Add("@codFunc", MySqlDbType.Int32, 11).Value = codigo;

            comm.Connection = Conectar.obterConexao();

            int res = comm.ExecuteNonQuery();

            Conectar.fecharConexao();


        }



        //Pesquisar por cód
        public void pesquisarCodigo()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select codFunc+1 from tbFuncionarios order by codfunc desc;";
            comm.CommandType = CommandType.Text;
            comm.Connection = Conectar.obterConexao();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();
            DR.Read();

            txtCodigo.Text = DR.GetInt32(0).ToString();

            Conectar.fecharConexao();
        }

        public void pesquisarNome(string nome)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select * from tbFuncionarios where nome like '%" + nome + "%'";
            comm.CommandType = CommandType.Text;
            comm.Connection = Conectar.obterConexao();

            MySqlDataReader DR;
            DR = comm.ExecuteReader();
            DR.Read();

            txtCodigo.Text = DR.GetInt32(0).ToString();
            txtNome.Text = DR.GetString(1);
            txtEmail.Text = DR.GetString(2);
            mskTel.Text = DR.GetString(3);
            mskCPF.Text = DR.GetString(4);
            txtEnd.Text = DR.GetString(5);
            txtNumero.Text = DR.GetString(6);
            txtBairro.Text = DR.GetString(7);
            txtCidade.Text = DR.GetString(8);
            cbbEstado.Text = DR.GetString(9);
            mskCEP.Text = DR.GetString(10);

            Conectar.fecharConexao();

            habilitarCamposBuscarNome();
        }


        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Hide();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {

            alterarFuncionarios(Convert.ToInt32(txtCodigo.Text));

            MessageBox.Show("Alterado com sucesso!!!",
                "Mensagem do sistema.",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);

            desabilitarCampos();
            btnNovo.Enabled = true;
            limparCampos();
        }

        public void buscaCEP(string numCEP)
        {
            WSCorreios.AtendeClienteClient ws = new WSCorreios.AtendeClienteClient();
            try
            {
                WSCorreios.enderecoERP endereco = ws.consultaCEP(numCEP);

                txtEnd.Text = endereco.end;
                txtBairro.Text = endereco.bairro;
                txtCidade.Text = endereco.cidade;
                cbbEstado.Text = endereco.uf;

            }
            catch (Exception)
            {
                MessageBox.Show("Favor inserir CEP válido!!!",
                    "Mensagem do sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                mskCEP.Focus();
                mskCEP.Text = "";
            }
        }

        private void mskCEP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buscaCEP(mskCEP.Text);
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            frmPesquisarFuncionarios abrir = new frmPesquisarFuncionarios();
            abrir.Show();
            this.Hide();
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("teste");
        }

        private void txtNome_TextChanged_1(object sender, EventArgs e)
        {
            //pesquisarNome(txtNome.Text);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Deseja excluir?",
                "Mensagem do sistema", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (resp == DialogResult.Yes)
            {
                deletarFuncionarios(Convert.ToInt32(txtCodigo.Text));
                MessageBox.Show("Excluído com sucesso!",
                    "Messagem do sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                limparCampos();
            }
            else
            {
                txtNome.Focus();
            }
        }
    }

}
