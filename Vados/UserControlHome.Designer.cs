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
            pnlBottom.Margin = new Padding(3, 4, 3, 4);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1920, 1080);
            pnlBottom.TabIndex = 3;
            pnlBottom.Paint += pnlBottom_Paint;
            pnlBottom.MouseMove += pnlBottom_MouseMove;
            pnlBottom.Resize += pnlBottom_Resize;
            // 
            // lblText
            // 
            lblText.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblText.AutoSize = true;
            lblText.Font = new Font("Segoe UI Semibold", 27F, FontStyle.Bold);
            lblText.Location = new Point(695, 593);
            lblText.Name = "lblText";
            lblText.Size = new Size(531, 61);
            lblText.TabIndex = 9;
            lblText.Text = "O que você deseja fazer?";
            lblText.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblDebug
            // 
            lblDebug.AutoSize = true;
            lblDebug.Location = new Point(253, 80);
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new Size(54, 20);
            lblDebug.TabIndex = 8;
            lblDebug.Text = "Debug";
            // 
            // btnTrocarPagina
            // 
            btnTrocarPagina.Anchor = AnchorStyles.Top;
            btnTrocarPagina.Location = new Point(925, 38);
            btnTrocarPagina.Margin = new Padding(3, 4, 3, 4);
            btnTrocarPagina.Name = "btnTrocarPagina";
            btnTrocarPagina.Size = new Size(106, 31);
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
            btnManual.Location = new Point(1748, 33);
            btnManual.Margin = new Padding(3, 4, 3, 4);
            btnManual.Name = "btnManual";
            btnManual.Size = new Size(57, 67);
            btnManual.SizeMode = PictureBoxSizeMode.Zoom;
            btnManual.TabIndex = 7;
            btnManual.TabStop = false;
            // 
            // btnConfigs
            // 
            btnConfigs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConfigs.Cursor = Cursors.Hand;
            btnConfigs.Image = (Image)resources.GetObject("btnConfigs.Image");
            btnConfigs.Location = new Point(1834, 33);
            btnConfigs.Margin = new Padding(3, 4, 3, 4);
            btnConfigs.Name = "btnConfigs";
            btnConfigs.Size = new Size(57, 67);
            btnConfigs.SizeMode = PictureBoxSizeMode.Zoom;
            btnConfigs.TabIndex = 6;
            btnConfigs.TabStop = false;
            // 
            // imgLogo
            // 
            imgLogo.Image = (Image)resources.GetObject("imgLogo.Image");
            imgLogo.Location = new Point(39, 16);
            imgLogo.Margin = new Padding(3, 4, 3, 4);
            imgLogo.Name = "imgLogo";
            imgLogo.Size = new Size(218, 97);
            imgLogo.SizeMode = PictureBoxSizeMode.Zoom;
            imgLogo.TabIndex = 5;
            imgLogo.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(925, 811);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(106, 20);
            textBox1.TabIndex = 2;
            textBox1.TabStop = false;
            // 
            // txtComando
            // 
            txtComando.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtComando.BackColor = Color.FromArgb(243, 243, 243);
            txtComando.BorderStyle = BorderStyle.None;
            txtComando.Cursor = Cursors.IBeam;
            txtComando.Font = new Font("Segoe UI", 20F);
            txtComando.ForeColor = Color.FromArgb(88, 99, 152);
            txtComando.ImeMode = ImeMode.NoControl;
            txtComando.Location = new Point(335, 735);
            txtComando.Margin = new Padding(3, 4, 3, 4);
            txtComando.MaximumSize = new Size(1240, 0);
            txtComando.MinimumSize = new Size(342, 4);
            txtComando.Name = "txtComando";
            txtComando.Size = new Size(1240, 45);
            txtComando.TabIndex = 2;
            txtComando.Text = "Escreva um comando...";
            txtComando.Click += txtComando_Click;
            txtComando.LostFocus += txtComando_LostFocus;
            // 
            // UserControlHome
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(231, 231, 231);
            Controls.Add(pnlBottom);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(411, 240);
            Name = "UserControlHome";
            Size = new Size(1920, 1080);
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
