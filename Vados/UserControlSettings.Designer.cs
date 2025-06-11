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
            btnCriar.Location = new Point(53, 140);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(75, 23);
            btnCriar.TabIndex = 1;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // btnExluir
            // 
            btnExluir.Location = new Point(278, 140);
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
            cbExtensoes.Items.AddRange(new object[] { "txt", "pasta", "img", "png", "docx" });
            cbExtensoes.Location = new Point(232, 95);
            cbExtensoes.Name = "cbExtensoes";
            cbExtensoes.Size = new Size(121, 23);
            cbExtensoes.TabIndex = 3;
            // 
            // txtNome
            // 
            txtNome.Location = new Point(53, 95);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(144, 23);
            txtNome.TabIndex = 4;
            // 
            // UserControlSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtNome);
            Controls.Add(cbExtensoes);
            Controls.Add(btnExluir);
            Controls.Add(btnCriar);
            Controls.Add(button1);
            Name = "UserControlSettings";
            Size = new Size(640, 360);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button btnCriar;
        private Button btnExluir;
        private ComboBox cbExtensoes;
        private TextBox txtNome;
    }
}
