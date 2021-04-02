using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;
using microSGE.APIConsumer;

namespace microSGE
{
    public partial class Main : Form
    {
        public string UltimoAluno;
        public string UltimoFuncionario;
        public bool CriandoNovoItem = false;
        public List<string> ListaDeFunc = new List<string>();
        public List<string> ListaDeAlunos = new List<string>();

        public Main()
        {
            InitializeComponent();
            checkDependencies();
            Instituicao_UpdateValues();
            UpdateAlunosList();
            UpdateFuncList();
        }

        private void RevertBtn_Click(object sender, EventArgs e)
        {
            Instituicao_UpdateValues();
        }

        private void EditarBtn_Click(object sender, EventArgs e)
        {
            EscolaPanel.Enabled = true;
            EditarBtn.Enabled = false;
            ChangeLogoBtn.Enabled = true;
            RevertBtn.Enabled = true;
            LimparInfoBtn.Enabled = true;
            GravarBtn.Enabled = true;
        }

        private void GravarBtn_Click(object sender, EventArgs e)
        {
            EscolaPanel.Enabled = false;
            EditarBtn.Enabled = true;
            ChangeLogoBtn.Enabled = false;
            RevertBtn.Enabled = false;
            LimparInfoBtn.Enabled = false;
            GravarBtn.Enabled = false;
            Instituicao_ChangeValues();
        }

        private void ChangeLogoBtn_Click_1(object sender, EventArgs e)
        {
            ChangeLogo();
        }

        private void LimparInfoBtn_Click(object sender, EventArgs e)
        {
            NomeTxt.Text = "";
            CepTxt.Text = "";
            DescText.Text = "";
            CNPJText.Text = "";
            BairroText.Text = "";
            CidadeText.Text = "";
            UFText.Text = "";
            TelText.Text = "";
            PrefText.Text = "";
            ZonaTxt.Text = "";
        }

        //BOTÕES PARA ALUNOS \/

        private void NovoAluno_Click(object sender, EventArgs e)
        {
            AlterarImagemAluno.Enabled = true;
            FindAluno.Enabled = false;
            AlunosPainel.Enabled = true;
            NomePesquisaAluno.Enabled = false;
            NovoAluno.Enabled = false;
            GravarAluno.Enabled = true;
            SairAluno.Enabled = true;
            UltimoAluno = "";
            CriandoNovoItem = true;
            LimparInfoAluno();
            UpdateAlunosList();
        }

        private void GravarAluno_Click(object sender, EventArgs e)
        {
            if (NomeAluno.Text != "")
            {
                if (AlunoGravar())
                {
                    AlunoImagem.Image = null;
                    AlterarImagemAluno.Enabled = false;
                    FindAluno.Enabled = true;
                    AlunosPainel.Enabled = false;
                    NomePesquisaAluno.Enabled = true;
                    NovoAluno.Enabled = true;
                    GravarAluno.Enabled = false;
                    SairAluno.Enabled = false;
                    CriandoNovoItem = false;
                    UpdateAlunosList();
                    LimparInfoAluno();
                }
            }
            else
            {
                MessageBox.Show("Existem campos obrigatórios a serem preenchidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SairAluno_Click(object sender, EventArgs e)
        {
            UpdateAlunosList();
            LimparInfoAluno();
            AlunoImagem.Image = null;
            AlterarImagemAluno.Enabled = false;
            FindAluno.Enabled = true;
            AlunosPainel.Enabled = false;
            NomePesquisaAluno.Enabled = true;
            NovoAluno.Enabled = true;
            GravarAluno.Enabled = false;
            SairAluno.Enabled = false;
            CriandoNovoItem = false;
        }

        private void FindAluno_Click(object sender, EventArgs e)
        {
            UpdateAlunosList();
            string path = Application.StartupPath + @"\Data\Alunos\" + NomePesquisaAluno.Text.Replace(" ", "_");
            if (Directory.Exists(path) && NomePesquisaAluno.Text != "")
            {
                AlunoObter(NomePesquisaAluno.Text.Replace(" ", "_"));
                AlterarImagemAluno.Enabled = true;
                AlunosPainel.Enabled = true;
                NomePesquisaAluno.Enabled = false;
                NovoAluno.Enabled = false;
                FindAluno.Enabled = false;
                GravarAluno.Enabled = true;
                SairAluno.Enabled = true;
            }
        }

        private void PastaAluno_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + @"\Data\Alunos\" + NomeAluno.Text.Replace(" ", "_") + @"\PastaAluno";
                System.Diagnostics.Process.Start(path);
            }
            catch
            {

            }
        }
        private void ExcluirAluno_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Você está apagando as informações de um aluno junto a sua pasta particular que pode conter informações importantes. Deseja prosseguir?", "Confirmar apagamento de aluno", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (NomeAluno.Text != "")
                {
                    Directory.Delete(Application.StartupPath + @"\Data\Alunos\" + NomeAluno.Text.Replace(" ", "_"), true);
                    LimparInfoAluno();
                    AlterarImagemAluno.Enabled = false;
                    FindAluno.Enabled = true;
                    AlunosPainel.Enabled = false;
                    NomePesquisaAluno.Enabled = true;
                    NovoAluno.Enabled = true;
                    GravarAluno.Enabled = false;
                    SairAluno.Enabled = false;
                    CriandoNovoItem = false;
                    AlunoImagem.Image = null;
                }
            }
        }

        private void AlterarImagemAluno_Click(object sender, EventArgs e)
        {
            OpenFileDialog alFoto = new OpenFileDialog();
            alFoto.Title = "Selecione uma imagem";
            alFoto.ShowDialog();
            if (alFoto.CheckFileExists)
            {
                AlunoImagem.Image = Image.FromFile(alFoto.FileName);
            }
        }

        //BOTÕES PARA FUNCIONÁRIOS \/

        private void NovoFunc_Click(object sender, EventArgs e)
        {
            UpdateFuncList();
            AlterarFotoFunc.Enabled = true;
            CriandoNovoItem = true;
            NovoFunc.Enabled = false;
            GravarFunc.Enabled = true;
            SairFunc.Enabled = true;
            FuncPanel.Enabled = true;
            PesquisaFuncTxt.Enabled = false;
            FuncPesquisa.Enabled = false;
        }

        private void GravarFunc_Click(object sender, EventArgs e)
        {
            if (NomeFunc.Text != "")
            {
                if (FuncGravar())
                {
                    UpdateFuncList();
                    LimparInfoFunc();
                    FuncImagem.Image = null;
                    AlterarFotoFunc.Enabled = false;
                    CriandoNovoItem = false;
                    NovoFunc.Enabled = true;
                    GravarFunc.Enabled = false;
                    SairFunc.Enabled = false;
                    FuncPanel.Enabled = false;
                    PesquisaFuncTxt.Enabled = true;
                    FuncPesquisa.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Existem campos obrigatórios a serem preenchidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SairFunc_Click(object sender, EventArgs e)
        {
            UpdateFuncList();
            LimparInfoFunc();
            FuncImagem.Image = null;
            AlterarFotoFunc.Enabled = false;
            CriandoNovoItem = false;
            NovoFunc.Enabled = true;
            GravarFunc.Enabled = false;
            SairFunc.Enabled = false;
            FuncPanel.Enabled = false;
            PesquisaFuncTxt.Enabled = true;
            FuncPesquisa.Enabled = true;
        }
        private void FuncPesquisa_Click(object sender, EventArgs e)
        {
            UpdateFuncList();
            string path = Application.StartupPath + @"\Data\Funcionarios\" + PesquisaFuncTxt.Text.Replace(" ", "_");
            if (Directory.Exists(path) && PesquisaFuncTxt.Text != "")
            {
                FuncObter(PesquisaFuncTxt.Text.Replace(" ", "_"));
                AlterarFotoFunc.Enabled = true;
                FuncPanel.Enabled = true;
                PesquisaFuncTxt.Enabled = false;
                NovoFunc.Enabled = false;
                FuncPesquisa.Enabled = false;
                GravarFunc.Enabled = true;
                SairFunc.Enabled = true;
            }
        }

        private void FuncPasta_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + @"\Data\Funcionarios\" + NomeFunc.Text.Replace(" ", "_") + @"\PastaFunc";
                System.Diagnostics.Process.Start(path);
            }
            catch
            {

            }
        }

        private void ExcluirFunc_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Você está apagando as informações de um funcionário junto a sua pasta particular que pode conter informações importantes. Deseja prosseguir?", "Confirmar apagamento de aluno", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (NomeFunc.Text != "")
                {
                    Directory.Delete(Application.StartupPath + @"\Data\Funcionarios\" + NomeFunc.Text.Replace(" ", "_"), true);
                    LimparInfoFunc();
                    FuncImagem.Image = null;
                    AlterarFotoFunc.Enabled = false;
                    FuncPesquisa.Enabled = true;
                    FuncPanel.Enabled = false;
                    PesquisaFuncTxt.Enabled = true;
                    NovoFunc.Enabled = true;
                    GravarFunc.Enabled = false;
                    SairFunc.Enabled = false;
                    CriandoNovoItem = false;
                    FuncImagem.Image = null;
                }
            }
        }
        private void AlterarFotoFunc_Click(object sender, EventArgs e)
        {
            OpenFileDialog funcFoto = new OpenFileDialog();
            funcFoto.Title = "Selecione uma imagem";
            funcFoto.ShowDialog();
            if (funcFoto.CheckFileExists)
            {
                FuncImagem.Image = Image.FromFile(funcFoto.FileName);
            }
        }

        //FUNÇÕES DA TELA PRINCIPAL

        void checkDependencies()
        {
            if (!File.Exists(Application.StartupPath + @"\Data\master.msge") ||
                !File.Exists(Application.StartupPath + @"\Data\logo.png") ||
                !Directory.Exists(Application.StartupPath + @"\Data\Funcionarios") ||
                !Directory.Exists(Application.StartupPath + @"\Data\Alunos"))
            {
                MessageBox.Show("Um arquivo vital para o funcionamento do programa não foi encontrado" +
                    " ou está obstruído.", "Erro Fatal",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }


        }
        void UpdateAlunosList()
        {
            /* LISTA DE ALUNOS */
            string alunosPath = Application.StartupPath + @"\Data\Alunos\";
            string[] alunosRaw = Directory.GetDirectories(alunosPath);

            ListaDeAlunos = new List<string>();

            foreach (string i in alunosRaw)
            {
                ListaDeAlunos.Add(i.Replace(alunosPath, ""));
            }

            NomePesquisaAluno.Items.Clear();
            NomePesquisaAluno.Items.AddRange(ListaDeAlunos.ToArray());

        }

        void UpdateFuncList()
        {
            /* LISTA DE FUNCIONÁRIOS */
            string funcPath = Application.StartupPath + @"\Data\Funcionarios\";
            string[] funcRaw = Directory.GetDirectories(funcPath);

            ListaDeFunc = new List<string>();

            foreach (string i in funcRaw)
            {
                ListaDeFunc.Add(i.Replace(funcPath, ""));
            }

            PesquisaFuncTxt.Items.Clear();
            PesquisaFuncTxt.Items.AddRange(ListaDeFunc.ToArray());

        }

        //CHECAR VALORES NOS ARQUIVOS DE DADOS

        void Instituicao_UpdateValues()
        {
            try
            {
                string path = Application.StartupPath + @"\Data\master.msge";
                foreach(string c in File.ReadAllLines(path))
                {
                        if(c.Contains("%nome_ins%="))
                        {
                            NomeTxt.Text = c.Replace("%nome_ins%=", "");
                        }

                        else if (c.Contains("%cep%="))
                        {
                            CepTxt.Text = c.Replace("%cep%=", "");
                        }


                        else if (c.Contains("%zona%="))
                        {
                            ZonaTxt.Text = c.Replace("%zona%=", "");
                        }

                        else if (c.Contains("%prefix%="))
                        {
                            PrefText.Text = c.Replace("%prefix%=", "");
                        }

                        else if (c.Contains("%tel%="))
                        {
                            TelText.Text = c.Replace("%tel%=", "");
                        }

                        else if (c.Contains("%uf%="))
                        {
                            UFText.Text = c.Replace("%uf%=", "");
                        }

                        else if (c.Contains("%mun%="))
                        {
                            CidadeText.Text = c.Replace("%mun%=", "");
                        }

                        else if (c.Contains("%bairro%="))
                        {
                            BairroText.Text = c.Replace("%bairro%=", "");
                        }

                        else if (c.Contains("%cnpj%="))
                        {
                            CNPJText.Text = c.Replace("%cnpj%=", "");
                        }

                        else if (c.Contains("%desc%="))
                        {
                            DescText.Text = c.Replace("%desc%=", "");
                        }

                        else if (c.Contains("%logo%="))
                        {
                            Logo.ImageLocation = Application.StartupPath + @"\Data\" + c.Replace("%logo%=", "");
                            Logo.SizeMode = PictureBoxSizeMode.StretchImage;
                            FuncImagem.SizeMode = PictureBoxSizeMode.StretchImage;
                            AlunoImagem.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    }
            }
            catch
            {

            }
        }

        void AlunoObter(string aluno)
        {
            string path = Application.StartupPath + @"\Data\Alunos\" + aluno;
            if (File.Exists(path + @"\info.aluno"))
            {
                foreach(string c in File.ReadAllLines(path + @"\info.aluno"))
                {
                    if (c.Contains("%nome_aluno%="))
                    {
                        NomeAluno.Text = c.Replace("%nome_aluno%=", "");
                    }

                    else if (c.Contains("%rg_aluno%="))
                    {
                        RGAluno.Text = c.Replace("%rg_aluno%=", "");
                    }

                    else if (c.Contains("%end_aluno%="))
                    {
                        EndAluno.Text = c.Replace("%end_aluno%=", "");
                    }

                    else if (c.Contains("%UF_aluno%="))
                    {
                        UFAluno.Text = c.Replace("%UF_aluno%=", "");
                    }

                    else if (c.Contains("%zona_aluno%="))
                    {
                        ZonaAluno.Text = c.Replace("%zona_aluno%=", "");
                    }

                    else if (c.Contains("%info_aluno%="))
                    {
                        infoAluno.Text = c.Replace("%info_aluno%=", "");
                    }

                    else if (c.Contains("%foto%="))
                    {
                        AlunoImagem.ImageLocation = path + @"\" + c.Replace("%foto%=", "");
                    }
                }
            }
        }

        void FuncObter(string func)
        {
            string path = Application.StartupPath + @"\Data\Funcionarios\" + func;
            if (File.Exists(path + @"\info.func"))
            {
                foreach (string c in File.ReadAllLines(path + @"\info.func"))
                {
                    if (c.Contains("%nome_func%="))
                    {
                        NomeFunc.Text = c.Replace("%nome_func%=", "");
                    }

                    else if (c.Contains("%rg_func%="))
                    {
                        RGFunc.Text = c.Replace("%rg_func%=", "");
                    }

                    else if (c.Contains("%end_func%="))
                    {
                        EndFunc.Text = c.Replace("%end_func%=", "");
                    }

                    else if (c.Contains("%UF_func%="))
                    {
                        UFFunc.Text = c.Replace("%UF_func%=", "");
                    }

                    else if (c.Contains("%zona_func%="))
                    {
                        ZonaFunc.Text = c.Replace("%zona_func%=", "");
                    }

                    else if (c.Contains("%info_func%="))
                    {
                        DescFunc.Text = c.Replace("%info_func%=", "");
                    }

                    else if (c.Contains("%foto%="))
                    {
                       FuncImagem.ImageLocation = path + @"\" + c.Replace("%foto%=", "");
                    }
                }
            }
        }

        //ATUALIZAR VALORES NOS ARQUIVOS DE DADOS

        void Instituicao_ChangeValues()
        {
            try
            {
                string content = "";
                string path = Application.StartupPath + @"\Data\master.msge";

                content += "[MICROSGE_REGISTRO]\n";
                content += "%nome_ins%=" + NomeTxt.Text + "\n";
                content += "%cep%=" + CepTxt.Text + "\n";
                content += "%zona%=" + ZonaTxt.Text + "\n";
                content += "%prefix%=" + PrefText.Text + "\n";
                content += "%tel%=" + TelText.Text + "\n";
                content += "%uf%=" + UFText.Text + "\n";
                content += "%mun%=" + CidadeText.Text + "\n";
                content += "%bairro%=" + BairroText.Text + "\n";
                content += "%cnpj%=" + CNPJText.Text + "\n";
                content += "%desc%=" + DescText.Text + "\n";
                content += "%logo%=logo.png";

                Logo.Image.Save(Application.StartupPath + @"\Data\logo.png", ImageFormat.Png);

                File.WriteAllText(path, content);
            }
            catch
            {
                
            }
        }

        bool FuncGravar()
        {
            string content = "";
            string path = Application.StartupPath + @"\Data\Funcionarios\" + NomeFunc.Text.Replace(" ", "_");

            content += "%nome_func%=" + NomeFunc.Text + "\n";
            content += "%rg_func%=" + RGFunc.Text + "\n";
            content += "%end_func%=" + EndFunc.Text + "\n";
            content += "%UF_func%=" + UFFunc.Text + "\n";
            content += "%zona_func%=" + ZonaFunc.Text + "\n";
            content += "%info_func%=" + DescFunc.Text + "\n";
            content += "%foto%=foto.png" + "\n";

            UltimoFuncionario = NomeFunc.Text.Replace(" ", "_");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Directory.CreateDirectory(path + @"\PastaFunc");
            }
            else
            {
                if (CriandoNovoItem == true)
                {
                    MessageBox.Show("Já existe um funcionário com o mesmo nome ou a operação é inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            try
            {
                try
                {
                    FuncImagem.Image.Save(path + @"\foto.png", ImageFormat.Png);
                }
                catch
                {

                }
                File.WriteAllText(path + @"\info.func", content);
                return true;
            }
            catch
            {
                MessageBox.Show("Ocorreu um erro desconhecido, apague o funcionário e o crie novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        bool AlunoGravar()
        {
            string content = "";
            string path = Application.StartupPath + @"\Data\Alunos\" + NomeAluno.Text.Replace(" ", "_");

            content += "%nome_aluno%=" + NomeAluno.Text + "\n";
            content += "%rg_aluno%=" + RGAluno.Text + "\n";
            content += "%end_aluno%=" + EndAluno.Text + "\n";
            content += "%UF_aluno%=" + UFAluno.Text + "\n";
            content += "%zona_aluno%=" + ZonaAluno.Text + "\n";
            content += "%info_aluno%=" + infoAluno.Text + "\n";
            content += "%foto%=foto.png" + "\n";

            UltimoAluno = NomeAluno.Text.Replace(" ", "_");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Directory.CreateDirectory(path + @"\PastaAluno");
            }
            else
            {
                if (CriandoNovoItem == true)
                {
                    MessageBox.Show("Já existe um aluno com o mesmo nome ou a operação é inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            try
            {
                try
                {
                    AlunoImagem.Image.Save(path + @"\foto.png", ImageFormat.Png);
                }
                catch
                {

                }
                File.WriteAllText(path + @"\info.aluno", content);
                return true;
            }
            catch
            {
                MessageBox.Show("Ocorreu um erro desconhecido, apague o aluno e o crie novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        //FUNÇÕES AUXILIARES

        void LimparInfoAluno()
        {
            NomeAluno.Text = "";
            UFAluno.Text = "";
            RGAluno.Text = "";
            EndAluno.Text = "";
            infoAluno.Text = "";
            ZonaAluno.Text = "";
        }

        void LimparInfoFunc()
        {
            NomeFunc.Text = "";
            UFFunc.Text = "";
            RGFunc.Text = "";
            EndFunc.Text = "";
            DescFunc.Text = "";
            ZonaFunc.Text = "";
        }

        //ALTERAR LOGO DA INSTITUIÇÃO

        void ChangeLogo()
        {
            OpenFileDialog logoTela = new OpenFileDialog();
            logoTela.Title = "Selecione uma imagem";
            logoTela.ShowDialog();
                if (logoTela.CheckFileExists)
                {
                    Logo.Image = Image.FromFile(logoTela.FileName);
                }
            
        }

        //FUNÇÕES PARA OBTENÇÃO DE API

        private void CepTxt_TextChanged(object sender, EventArgs e)
        {
            if (CepTxt.TextLength >= 8)
            {
                try
                {
                    string[] infos = ViaCEP.ViaCEP_Obter(CepTxt.Text);
                    BairroText.Text = infos[0];
                    CidadeText.Text = infos[1];
                    UFText.Text = infos[2];
                }
                catch
                {

                }
            }
        }
    }
}
