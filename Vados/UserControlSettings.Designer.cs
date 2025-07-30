namespace Vados
{
    partial class UserControlSettings
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            btnCriar = new Button();
            btnExluir = new Button();
            cbExtensoes = new ComboBox();
            txtNome = new TextBox();
            txtNovoNome = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnRenomear = new Button();
            label4 = new Label();
            txtDestinatario = new TextBox();
            btnAdm = new Button();
            cbIdioma = new ComboBox();
            btnMudarIdioma = new Button();
            label5 = new Label();
            listateste = new ListBox();
            btnLog = new Button();
            txtSearch = new TextBox();
            btnAbrirArquivo = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(441, 15);
            button1.Name = "button1";
            button1.Size = new Size(186, 23);
            button1.TabIndex = 0;
            button1.Text = "pagina inicial";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnCriar
            // 
            btnCriar.Location = new Point(53, 199);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(75, 23);
            btnCriar.TabIndex = 1;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // btnExluir
            // 
            btnExluir.Location = new Point(277, 199);
            btnExluir.Name = "btnExluir";
            btnExluir.Size = new Size(75, 23);
            btnExluir.TabIndex = 2;
            btnExluir.Text = "Excluir";
            btnExluir.UseVisualStyleBackColor = true;
            btnExluir.Click += btnExluir_Click;
            // 
            // cbExtensoes
            // 
            cbExtensoes.FormattingEnabled = true;
            cbExtensoes.Items.AddRange(new object[] { "txt", "pasta", "img", "png", "docx", "jpg", "jpeg" });
            cbExtensoes.Location = new Point(382, 104);
            cbExtensoes.Name = "cbExtensoes";
            cbExtensoes.Size = new Size(121, 23);
            cbExtensoes.TabIndex = 3;
            // 
            // txtNome
            // 
            txtNome.Location = new Point(99, 104);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(144, 23);
            txtNome.TabIndex = 4;
            // 
            // txtNovoNome
            // 
            txtNovoNome.Location = new Point(99, 140);
            txtNovoNome.Name = "txtNovoNome";
            txtNovoNome.Size = new Size(144, 23);
            txtNovoNome.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 142);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 6;
            label1.Text = "Novo Nome:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 106);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 7;
            label2.Text = "Nome:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(315, 110);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 8;
            label3.Text = "extensão:";
            // 
            // btnRenomear
            // 
            btnRenomear.Location = new Point(167, 199);
            btnRenomear.Name = "btnRenomear";
            btnRenomear.Size = new Size(75, 23);
            btnRenomear.TabIndex = 9;
            btnRenomear.Text = "Renomear";
            btnRenomear.UseVisualStyleBackColor = true;
            btnRenomear.Click += btnRenomear_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 33);
            label4.Name = "label4";
            label4.Size = new Size(90, 15);
            label4.TabIndex = 10;
            label4.Text = "pasta \"destino\":";
            // 
            // txtDestinatario
            // 
            txtDestinatario.Location = new Point(99, 30);
            txtDestinatario.Name = "txtDestinatario";
            txtDestinatario.Size = new Size(100, 23);
            txtDestinatario.TabIndex = 11;
            // 
            // btnAdm
            // 
            btnAdm.BackColor = Color.IndianRed;
            btnAdm.Location = new Point(99, 270);
            btnAdm.Margin = new Padding(3, 2, 3, 2);
            btnAdm.Name = "btnAdm";
            btnAdm.Size = new Size(178, 26);
            btnAdm.TabIndex = 12;
            btnAdm.Text = "Adm";
            btnAdm.UseVisualStyleBackColor = false;
            btnAdm.Click += btnAdm_Click;
            // 
            // cbIdioma
            // 
            cbIdioma.FormattingEnabled = true;
            cbIdioma.Items.AddRange(new object[] { "Português do Brasil", "Inglês" });
            cbIdioma.Location = new Point(552, 201);
            cbIdioma.Name = "cbIdioma";
            cbIdioma.Size = new Size(121, 23);
            cbIdioma.TabIndex = 13;
            // 
            // btnMudarIdioma
            // 
            btnMudarIdioma.Location = new Point(537, 237);
            btnMudarIdioma.Name = "btnMudarIdioma";
            btnMudarIdioma.Size = new Size(136, 23);
            btnMudarIdioma.TabIndex = 14;
            btnMudarIdioma.Text = "mudar idioma";
            btnMudarIdioma.UseVisualStyleBackColor = true;
            btnMudarIdioma.Click += btnMudarIdioma_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(481, 207);
            label5.Name = "label5";
            label5.Size = new Size(47, 15);
            label5.TabIndex = 15;
            label5.Text = "Idioma:";
            // 
            // listateste
            // 
            listateste.FormattingEnabled = true;
            listateste.ItemHeight = 15;
            listateste.Location = new Point(679, 53);
            listateste.Name = "listateste";
            listateste.Size = new Size(415, 289);
            listateste.TabIndex = 16;
            // 
            // btnLog
            // 
            btnLog.Location = new Point(897, 27);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(186, 23);
            btnLog.TabIndex = 17;
            btnLog.Text = "log";
            btnLog.UseVisualStyleBackColor = true;
            btnLog.Click += btnLog_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(726, 28);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(144, 23);
            txtSearch.TabIndex = 18;
            txtSearch.TextChanged += textBox1_TextChanged;
            // 
            // btnAbrirArquivo
            // 
            btnAbrirArquivo.Location = new Point(113, 242);
            btnAbrirArquivo.Name = "btnAbrirArquivo";
            btnAbrirArquivo.Size = new Size(164, 23);
            btnAbrirArquivo.TabIndex = 19;
            btnAbrirArquivo.Text = "Abrir arquivo";
            btnAbrirArquivo.UseVisualStyleBackColor = true;
            btnAbrirArquivo.Click += btnAbrirArquivo_Click;
            // 
            // UserControlSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnAbrirArquivo);
            Controls.Add(txtSearch);
            Controls.Add(btnLog);
            Controls.Add(listateste);
            Controls.Add(label5);
            Controls.Add(btnMudarIdioma);
            Controls.Add(cbIdioma);
            Controls.Add(btnAdm);
            Controls.Add(txtDestinatario);
            Controls.Add(label4);
            Controls.Add(btnRenomear);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtNovoNome);
            Controls.Add(txtNome);
            Controls.Add(cbExtensoes);
            Controls.Add(btnExluir);
            Controls.Add(btnCriar);
            Controls.Add(button1);
            Name = "UserControlSettings";
            Size = new Size(1097, 530);
            Load += UserControlSettings_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button btnCriar;
        private Button btnExluir;
        private ComboBox cbExtensoes;
        private TextBox txtNome;
        private TextBox txtNovoNome;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnRenomear;
        private Label label4;
        private TextBox txtDestinatario;
        private Button btnAdm;
        private ComboBox cbIdioma;
        private Button btnMudarIdioma;
        private Label label5;
        private ListBox listateste;
        private Button btnLog;
        private TextBox txtSearch;
        private Button btnAbrirArquivo;
    }
}
