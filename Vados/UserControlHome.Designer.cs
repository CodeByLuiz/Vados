namespace Vados
{
    partial class UserControlHome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlHome));
            pnlBottom = new OptmizedPanel();
            lblText = new Label();
            lblDebug = new Label();
            btnTrocarPagina = new Button();
            btnManual = new PictureBox();
            btnConfigs = new PictureBox();
            imgLogo = new PictureBox();
            textBox1 = new TextBox();
            txtComando = new TextBox();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnManual).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnConfigs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgLogo).BeginInit();
            SuspendLayout();
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(lblText);
            pnlBottom.Controls.Add(lblDebug);
            pnlBottom.Controls.Add(btnTrocarPagina);
            pnlBottom.Controls.Add(btnManual);
            pnlBottom.Controls.Add(btnConfigs);
            pnlBottom.Controls.Add(imgLogo);
            pnlBottom.Controls.Add(textBox1);
            pnlBottom.Controls.Add(txtComando);
            pnlBottom.Dock = DockStyle.Fill;
            pnlBottom.Location = new Point(0, 0);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1680, 810);
            pnlBottom.TabIndex = 3;
            pnlBottom.Click += pnlBottom_Click;
            pnlBottom.Paint += pnlBottom_Paint;
            pnlBottom.MouseMove += pnlBottom_MouseMove;
            pnlBottom.Resize += pnlBottom_Resize;
            // 
            // lblText
            // 
            lblText.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblText.AutoSize = true;
            lblText.Font = new Font("Segoe UI Semibold", 27F, FontStyle.Bold);
            lblText.Location = new Point(608, 445);
            lblText.Name = "lblText";
            lblText.Size = new Size(425, 48);
            lblText.TabIndex = 9;
            lblText.Text = "O que você deseja fazer?";
            lblText.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblDebug
            // 
            lblDebug.AutoSize = true;
            lblDebug.Location = new Point(231, 60);
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new Size(42, 15);
            lblDebug.TabIndex = 8;
            lblDebug.Text = "Debug";
            // 
            // btnTrocarPagina
            // 
            btnTrocarPagina.Anchor = AnchorStyles.Top;
            btnTrocarPagina.Location = new Point(809, 28);
            btnTrocarPagina.Name = "btnTrocarPagina";
            btnTrocarPagina.Size = new Size(93, 23);
            btnTrocarPagina.TabIndex = 1;
            btnTrocarPagina.Text = "Trocar Página";
            btnTrocarPagina.UseVisualStyleBackColor = true;
            btnTrocarPagina.Click += btnTrocarPagina_Click;
            // 
            // btnManual
            // 
            btnManual.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnManual.Cursor = Cursors.Hand;
            btnManual.Image = (Image)resources.GetObject("btnManual.Image");
            btnManual.Location = new Point(1530, 25);
            btnManual.Name = "btnManual";
            btnManual.Size = new Size(50, 50);
            btnManual.SizeMode = PictureBoxSizeMode.Zoom;
            btnManual.TabIndex = 7;
            btnManual.TabStop = false;
            // 
            // btnConfigs
            // 
            btnConfigs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConfigs.Cursor = Cursors.Hand;
            btnConfigs.Image = (Image)resources.GetObject("btnConfigs.Image");
            btnConfigs.Location = new Point(1605, 25);
            btnConfigs.Name = "btnConfigs";
            btnConfigs.Size = new Size(50, 50);
            btnConfigs.SizeMode = PictureBoxSizeMode.Zoom;
            btnConfigs.TabIndex = 6;
            btnConfigs.TabStop = false;
            // 
            // imgLogo
            // 
            imgLogo.Image = (Image)resources.GetObject("imgLogo.Image");
            imgLogo.Location = new Point(34, 12);
            imgLogo.Name = "imgLogo";
            imgLogo.Size = new Size(191, 73);
            imgLogo.SizeMode = PictureBoxSizeMode.Zoom;
            imgLogo.TabIndex = 5;
            imgLogo.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(809, 608);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(93, 16);
            textBox1.TabIndex = 2;
            textBox1.TabStop = false;
            // 
            // txtComando
            // 
            txtComando.Anchor = AnchorStyles.None;
            txtComando.BackColor = Color.FromArgb(243, 243, 243);
            txtComando.BorderStyle = BorderStyle.None;
            txtComando.Cursor = Cursors.IBeam;
            txtComando.Font = new Font("Segoe UI", 20F);
            txtComando.ForeColor = Color.FromArgb(88, 99, 152);
            txtComando.ImeMode = ImeMode.NoControl;
            txtComando.Location = new Point(293, 551);
            txtComando.MaximumSize = new Size(1085, 0);
            txtComando.MinimumSize = new Size(299, 4);
            txtComando.Name = "txtComando";
            txtComando.Size = new Size(1085, 36);
            txtComando.TabIndex = 2;
            txtComando.Text = "Escreva um comando...";
            txtComando.Click += txtComando_Click;
            txtComando.LostFocus += txtComando_LostFocus;
            // 
            // UserControlHome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(231, 231, 231);
            Controls.Add(pnlBottom);
            MinimumSize = new Size(360, 180);
            Name = "UserControlHome";
            Size = new Size(1680, 810);
            KeyDown += UserControlHome_KeyDown;
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)btnManual).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnConfigs).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TextBox txtComando;
        private TextBox textBox1;
        private OptmizedPanel pnlBottom;
        private Label lblDebug;
        private Button btnTrocarPagina;
        private PictureBox btnManual;
        private PictureBox btnConfigs;
        private PictureBox imgLogo;
        private Label lblText;
    }
}
