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
            btnMover = new Button();
            btnDupe = new Button();
            btnAbrirArquivo = new Button();
            txtNomeAplicativo = new TextBox();
            label6 = new Label();
            label7 = new Label();
            button2 = new Button();
            btnStartRecTest = new Button();
            btnStopRecTest = new Button();
            btnPauseTest = new Button();
            txtTranscriçãoTest = new TextBox();
            cbMicrofones = new ComboBox();
            label8 = new Label();
            lblTest = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(504, 20);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(213, 31);
            button1.TabIndex = 0;
            button1.Text = "pagina inicial";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnCriar
            // 
            btnCriar.Location = new Point(61, 265);
            btnCriar.Margin = new Padding(3, 4, 3, 4);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(86, 31);
            btnCriar.TabIndex = 1;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // btnExluir
            // 
            btnExluir.Location = new Point(317, 265);
            btnExluir.Margin = new Padding(3, 4, 3, 4);
            btnExluir.Name = "btnExluir";
            btnExluir.Size = new Size(86, 31);
            btnExluir.TabIndex = 2;
            btnExluir.Text = "Excluir";
            btnExluir.UseVisualStyleBackColor = true;
            btnExluir.Click += btnExluir_Click;
            // 
            // cbExtensoes
            // 
            cbExtensoes.FormattingEnabled = true;
            cbExtensoes.Items.AddRange(new object[] { "txt", "pasta", "img", "png", "docx", "jpg", "jpeg" });
            cbExtensoes.Location = new Point(437, 139);
            cbExtensoes.Margin = new Padding(3, 4, 3, 4);
            cbExtensoes.Name = "cbExtensoes";
            cbExtensoes.Size = new Size(138, 28);
            cbExtensoes.TabIndex = 3;
            // 
            // txtNome
            // 
            txtNome.Location = new Point(113, 139);
            txtNome.Margin = new Padding(3, 4, 3, 4);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(164, 27);
            txtNome.TabIndex = 4;
            // 
            // txtNovoNome
            // 
            txtNovoNome.Location = new Point(113, 187);
            txtNovoNome.Margin = new Padding(3, 4, 3, 4);
            txtNovoNome.Name = "txtNovoNome";
            txtNovoNome.Size = new Size(164, 27);
            txtNovoNome.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 189);
            label1.Name = "label1";
            label1.Size = new Size(93, 20);
            label1.TabIndex = 6;
            label1.Text = "Novo Nome:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 141);
            label2.Name = "label2";
            label2.Size = new Size(53, 20);
            label2.TabIndex = 7;
            label2.Text = "Nome:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(360, 147);
            label3.Name = "label3";
            label3.Size = new Size(71, 20);
            label3.TabIndex = 8;
            label3.Text = "extensão:";
            // 
            // btnRenomear
            // 
            btnRenomear.Location = new Point(191, 265);
            btnRenomear.Margin = new Padding(3, 4, 3, 4);
            btnRenomear.Name = "btnRenomear";
            btnRenomear.Size = new Size(86, 31);
            btnRenomear.TabIndex = 9;
            btnRenomear.Text = "Renomear";
            btnRenomear.UseVisualStyleBackColor = true;
            btnRenomear.Click += btnRenomear_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 44);
            label4.Name = "label4";
            label4.Size = new Size(113, 20);
            label4.TabIndex = 10;
            label4.Text = "pasta \"destino\":";
            // 
            // txtDestinatario
            // 
            txtDestinatario.Location = new Point(113, 40);
            txtDestinatario.Margin = new Padding(3, 4, 3, 4);
            txtDestinatario.Name = "txtDestinatario";
            txtDestinatario.Size = new Size(114, 27);
            txtDestinatario.TabIndex = 11;
            txtDestinatario.TextChanged += txtDestinatario_TextChanged;
            // 
            // btnAdm
            // 
            btnAdm.BackColor = Color.IndianRed;
            btnAdm.Location = new Point(113, 360);
            btnAdm.Name = "btnAdm";
            btnAdm.Size = new Size(203, 35);
            btnAdm.TabIndex = 12;
            btnAdm.Text = "Adm";
            btnAdm.UseVisualStyleBackColor = false;
            btnAdm.Click += btnAdm_Click;
            // 
            // cbIdioma
            // 
            cbIdioma.FormattingEnabled = true;
            cbIdioma.Items.AddRange(new object[] { "Português do Brasil", "Inglês" });
            cbIdioma.Location = new Point(631, 268);
            cbIdioma.Margin = new Padding(3, 4, 3, 4);
            cbIdioma.Name = "cbIdioma";
            cbIdioma.Size = new Size(138, 28);
            cbIdioma.TabIndex = 13;
            // 
            // btnMudarIdioma
            // 
            btnMudarIdioma.Location = new Point(614, 316);
            btnMudarIdioma.Margin = new Padding(3, 4, 3, 4);
            btnMudarIdioma.Name = "btnMudarIdioma";
            btnMudarIdioma.Size = new Size(155, 31);
            btnMudarIdioma.TabIndex = 14;
            btnMudarIdioma.Text = "mudar idioma";
            btnMudarIdioma.UseVisualStyleBackColor = true;
            btnMudarIdioma.Click += btnMudarIdioma_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(550, 276);
            label5.Name = "label5";
            label5.Size = new Size(59, 20);
            label5.TabIndex = 15;
            label5.Text = "Idioma:";
            // 
            // listateste
            // 
            listateste.FormattingEnabled = true;
            listateste.Location = new Point(776, 71);
            listateste.Margin = new Padding(3, 4, 3, 4);
            listateste.Name = "listateste";
            listateste.Size = new Size(474, 384);
            listateste.TabIndex = 16;
            listateste.SelectedIndexChanged += listateste_SelectedIndexChanged;
            // 
            // btnLog
            // 
            btnLog.Location = new Point(1025, 36);
            btnLog.Margin = new Padding(3, 4, 3, 4);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(213, 31);
            btnLog.TabIndex = 17;
            btnLog.Text = "log";
            btnLog.UseVisualStyleBackColor = true;
            btnLog.Click += btnLog_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(830, 37);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(164, 27);
            txtSearch.TabIndex = 18;
            txtSearch.TextChanged += textBox1_TextChanged;
            // 
            // btnMover
            // 
            btnMover.Location = new Point(61, 323);
            btnMover.Margin = new Padding(3, 4, 3, 4);
            btnMover.Name = "btnMover";
            btnMover.Size = new Size(86, 31);
            btnMover.TabIndex = 19;
            btnMover.Text = "Mover";
            btnMover.UseVisualStyleBackColor = true;
            btnMover.Click += btnMover_Click;
            // 
            // btnDupe
            // 
            btnDupe.Location = new Point(191, 323);
            btnDupe.Margin = new Padding(3, 4, 3, 4);
            btnDupe.Name = "btnDupe";
            btnDupe.Size = new Size(86, 31);
            btnDupe.TabIndex = 20;
            btnDupe.Text = "Duplicar";
            btnDupe.UseVisualStyleBackColor = true;
            btnDupe.Click += btnDupe_Click;
            // 
            // btnAbrirArquivo
            // 
            btnAbrirArquivo.Location = new Point(113, 401);
            btnAbrirArquivo.Margin = new Padding(3, 4, 3, 4);
            btnAbrirArquivo.Name = "btnAbrirArquivo";
            btnAbrirArquivo.Size = new Size(187, 31);
            btnAbrirArquivo.TabIndex = 19;
            btnAbrirArquivo.Text = "Abrir arquivo";
            btnAbrirArquivo.UseVisualStyleBackColor = true;
            btnAbrirArquivo.Click += btnAbrirArquivo_Click;
            // 
            // txtNomeAplicativo
            // 
            txtNomeAplicativo.Location = new Point(191, 500);
            txtNomeAplicativo.Margin = new Padding(3, 4, 3, 4);
            txtNomeAplicativo.Name = "txtNomeAplicativo";
            txtNomeAplicativo.Size = new Size(164, 27);
            txtNomeAplicativo.TabIndex = 20;
            txtNomeAplicativo.Text = "...";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(97, 504);
            label6.Name = "label6";
            label6.Size = new Size(53, 20);
            label6.TabIndex = 21;
            label6.Text = "Nome:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(235, 476);
            label7.Name = "label7";
            label7.Size = new Size(74, 20);
            label7.TabIndex = 22;
            label7.Text = "aplicativo";
            // 
            // button2
            // 
            button2.Location = new Point(191, 539);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(155, 43);
            button2.TabIndex = 23;
            button2.Text = "executar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnStartRecTest
            // 
            btnStartRecTest.Location = new Point(627, 661);
            btnStartRecTest.Margin = new Padding(3, 4, 3, 4);
            btnStartRecTest.Name = "btnStartRecTest";
            btnStartRecTest.Size = new Size(86, 31);
            btnStartRecTest.TabIndex = 24;
            btnStartRecTest.Text = "iniciar";
            btnStartRecTest.UseVisualStyleBackColor = true;
            btnStartRecTest.Click += btnStartRecTest_Click;
            // 
            // btnStopRecTest
            // 
            btnStopRecTest.Location = new Point(736, 661);
            btnStopRecTest.Margin = new Padding(3, 4, 3, 4);
            btnStopRecTest.Name = "btnStopRecTest";
            btnStopRecTest.Size = new Size(86, 31);
            btnStopRecTest.TabIndex = 25;
            btnStopRecTest.Text = "Parar";
            btnStopRecTest.UseVisualStyleBackColor = true;
            btnStopRecTest.Click += btnStopRecTest_Click;
            // 
            // btnPauseTest
            // 
            btnPauseTest.Location = new Point(841, 661);
            btnPauseTest.Margin = new Padding(3, 4, 3, 4);
            btnPauseTest.Name = "btnPauseTest";
            btnPauseTest.Size = new Size(86, 31);
            btnPauseTest.TabIndex = 26;
            btnPauseTest.Text = "Pausar / Despausar";
            btnPauseTest.UseVisualStyleBackColor = true;
            btnPauseTest.Click += btnPauseTest_Click;
            // 
            // txtTranscriçãoTest
            // 
            txtTranscriçãoTest.Location = new Point(550, 741);
            txtTranscriçãoTest.Margin = new Padding(3, 4, 3, 4);
            txtTranscriçãoTest.Multiline = true;
            txtTranscriçãoTest.Name = "txtTranscriçãoTest";
            txtTranscriçãoTest.Size = new Size(546, 179);
            txtTranscriçãoTest.TabIndex = 27;
            // 
            // cbMicrofones
            // 
            cbMicrofones.FormattingEnabled = true;
            cbMicrofones.Location = new Point(1103, 741);
            cbMicrofones.Margin = new Padding(3, 4, 3, 4);
            cbMicrofones.Name = "cbMicrofones";
            cbMicrofones.Size = new Size(138, 28);
            cbMicrofones.TabIndex = 28;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(1103, 705);
            label8.Name = "label8";
            label8.Size = new Size(165, 20);
            label8.TabIndex = 29;
            label8.Text = "Dispositivos de Entrada";
            // 
            // lblTest
            // 
            lblTest.AutoSize = true;
            lblTest.Location = new Point(685, 500);
            lblTest.Name = "lblTest";
            lblTest.Size = new Size(43, 20);
            lblTest.TabIndex = 30;
            lblTest.Text = "Teste";
            // 
            // UserControlSettings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(lblTest);
            Controls.Add(label8);
            Controls.Add(cbMicrofones);
            Controls.Add(txtTranscriçãoTest);
            Controls.Add(btnPauseTest);
            Controls.Add(btnStopRecTest);
            Controls.Add(btnStartRecTest);
            Controls.Add(button2);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(txtNomeAplicativo);
            Controls.Add(btnDupe);
            Controls.Add(btnMover);
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
            Margin = new Padding(3, 4, 3, 4);
            Name = "UserControlSettings";
            Size = new Size(1254, 1029);
            Load += UserControlSettings_Load;
            Paint += UserControlSettings_Paint;
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
        private Button btnMover;
        private Button btnDupe;
        private Button btnAbrirArquivo;
        private TextBox txtNomeAplicativo;
        private Label label6;
        private Label label7;
        private Button button2;
        private Button btnStartRecTest;
        private Button btnStopRecTest;
        private Button btnPauseTest;
        private TextBox txtTranscriçãoTest;
        private ComboBox cbMicrofones;
        private Label label8;
        private Label lblTest;
    }
}
