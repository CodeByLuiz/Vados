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
            button1 = new Button();
            pnlTop = new Panel();
            lblDebug = new Label();
            btnManual = new PictureBox();
            btnConfigs = new PictureBox();
            imgLogo = new PictureBox();
            pnlBottom = new OptmizedPanel();
            textBox1 = new TextBox();
            txtComando = new TextBox();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnManual).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnConfigs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgLogo).BeginInit();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.None;
            button1.Location = new Point(605, 38);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(106, 31);
            button1.TabIndex = 0;
            button1.Text = "Trocar Página";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pnlTop
            // 
            pnlTop.AutoScroll = true;
            pnlTop.Controls.Add(lblDebug);
            pnlTop.Controls.Add(button1);
            pnlTop.Controls.Add(btnManual);
            pnlTop.Controls.Add(btnConfigs);
            pnlTop.Controls.Add(imgLogo);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1280, 118);
            pnlTop.TabIndex = 1;
            // 
            // lblDebug
            // 
            lblDebug.AutoSize = true;
            lblDebug.Location = new Point(263, 80);
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new Size(54, 20);
            lblDebug.TabIndex = 3;
            lblDebug.Text = "Debug";
            // 
            // btnManual
            // 
            btnManual.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnManual.Cursor = Cursors.Hand;
            btnManual.Image = (Image)resources.GetObject("btnManual.Image");
            btnManual.Location = new Point(1108, 33);
            btnManual.Margin = new Padding(3, 4, 3, 4);
            btnManual.Name = "btnManual";
            btnManual.Size = new Size(57, 67);
            btnManual.SizeMode = PictureBoxSizeMode.Zoom;
            btnManual.TabIndex = 2;
            btnManual.TabStop = false;
            // 
            // btnConfigs
            // 
            btnConfigs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConfigs.Cursor = Cursors.Hand;
            btnConfigs.Image = (Image)resources.GetObject("btnConfigs.Image");
            btnConfigs.Location = new Point(1194, 33);
            btnConfigs.Margin = new Padding(3, 4, 3, 4);
            btnConfigs.Name = "btnConfigs";
            btnConfigs.Size = new Size(57, 67);
            btnConfigs.SizeMode = PictureBoxSizeMode.Zoom;
            btnConfigs.TabIndex = 1;
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
            imgLogo.TabIndex = 0;
            imgLogo.TabStop = false;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(textBox1);
            pnlBottom.Controls.Add(txtComando);
            pnlBottom.Dock = DockStyle.Fill;
            pnlBottom.Location = new Point(0, 0);
            pnlBottom.Margin = new Padding(3, 4, 3, 4);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1280, 720);
            pnlBottom.TabIndex = 3;
            pnlBottom.Paint += pnlBottom_Paint;
            pnlBottom.MouseMove += pnlBottom_MouseMove;
            pnlBottom.Resize += pnlBottom_Resize;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.None;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(605, 649);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(106, 20);
            textBox1.TabIndex = 2;
            // 
            // txtComando
            // 
            txtComando.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtComando.BackColor = Color.FromArgb(243, 243, 243);
            txtComando.BorderStyle = BorderStyle.None;
            txtComando.Cursor = Cursors.IBeam;
            txtComando.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtComando.ForeColor = Color.FromArgb(88, 99, 152);
            txtComando.ImeMode = ImeMode.NoControl;
            txtComando.Location = new Point(238, 573);
            txtComando.Margin = new Padding(3, 4, 3, 4);
            txtComando.MaximumSize = new Size(1240, 0);
            txtComando.MinimumSize = new Size(342, 4);
            txtComando.Name = "txtComando";
            txtComando.Size = new Size(804, 44);
            txtComando.TabIndex = 1;
            txtComando.Text = "Escreva um comando...";
            txtComando.Click += txtComando_Click;
            txtComando.LostFocus += txtComando_LostFocus;
            // 
            // UserControlHome
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(231, 231, 231);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(411, 240);
            Name = "UserControlHome";
            Size = new Size(1280, 720);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)btnManual).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnConfigs).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgLogo).EndInit();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Panel pnlTop;
        private PictureBox btnConfigs;
        private PictureBox imgLogo;
        private PictureBox btnManual;
        private TextBox txtComando;
        private TextBox textBox1;
        private Label lblDebug;
        private OptmizedPanel pnlBottom;
    }
}
