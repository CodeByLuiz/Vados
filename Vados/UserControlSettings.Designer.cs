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
            cbExtensoes.Items.AddRange(new object[] { "txt", "pasta", "img", "png", "docx" });
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
            // 
            // btnAdm
            // 
            btnAdm.BackColor = Color.IndianRed;
            btnAdm.Location = new Point(297, 375);
            btnAdm.Name = "btnAdm";
            btnAdm.Size = new Size(203, 35);
            btnAdm.TabIndex = 12;
            btnAdm.Text = "Adm";
            btnAdm.UseVisualStyleBackColor = false;
            btnAdm.Click += btnAdm_Click;
            // 
            // UserControlSettings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
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
            Size = new Size(731, 480);
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
    }
}
