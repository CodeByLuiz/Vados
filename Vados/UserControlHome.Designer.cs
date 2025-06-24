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
            btnManual = new PictureBox();
            btnConfigs = new PictureBox();
            imgLogo = new PictureBox();
            pnlBottom = new Panel();
            txtComando = new TextBox();
            pictureBox1 = new PictureBox();
            textBox1 = new TextBox();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnManual).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnConfigs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgLogo).BeginInit();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.None;
            button1.Location = new Point(609, 34);
            button1.Name = "button1";
            button1.Size = new Size(93, 23);
            button1.TabIndex = 0;
            button1.Text = "Trocar Página";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pnlTop
            // 
            pnlTop.AutoScroll = true;
            pnlTop.Controls.Add(button1);
            pnlTop.Controls.Add(btnManual);
            pnlTop.Controls.Add(btnConfigs);
            pnlTop.Controls.Add(imgLogo);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1280, 100);
            pnlTop.TabIndex = 1;
            // 
            // btnManual
            // 
            btnManual.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnManual.Cursor = Cursors.Hand;
            btnManual.Image = (Image)resources.GetObject("btnManual.Image");
            btnManual.Location = new Point(1130, 25);
            btnManual.Name = "btnManual";
            btnManual.Size = new Size(50, 50);
            btnManual.SizeMode = PictureBoxSizeMode.Zoom;
            btnManual.TabIndex = 2;
            btnManual.TabStop = false;
            // 
            // btnConfigs
            // 
            btnConfigs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConfigs.Cursor = Cursors.Hand;
            btnConfigs.Image = (Image)resources.GetObject("btnConfigs.Image");
            btnConfigs.Location = new Point(1205, 25);
            btnConfigs.Name = "btnConfigs";
            btnConfigs.Size = new Size(50, 50);
            btnConfigs.SizeMode = PictureBoxSizeMode.Zoom;
            btnConfigs.TabIndex = 1;
            btnConfigs.TabStop = false;
            // 
            // imgLogo
            // 
            imgLogo.Image = (Image)resources.GetObject("imgLogo.Image");
            imgLogo.Location = new Point(34, 12);
            imgLogo.Name = "imgLogo";
            imgLogo.Size = new Size(191, 73);
            imgLogo.SizeMode = PictureBoxSizeMode.Zoom;
            imgLogo.TabIndex = 0;
            imgLogo.TabStop = false;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(textBox1);
            pnlBottom.Controls.Add(txtComando);
            pnlBottom.Controls.Add(pictureBox1);
            pnlBottom.Dock = DockStyle.Fill;
            pnlBottom.Location = new Point(0, 0);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1280, 720);
            pnlBottom.TabIndex = 3;
            // 
            // txtComando
            // 
            txtComando.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtComando.Cursor = Cursors.IBeam;
            txtComando.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtComando.ForeColor = Color.FromArgb(88, 99, 152);
            txtComando.Location = new Point(208, 525);
            txtComando.MinimumSize = new Size(300, 0);
            txtComando.Name = "txtComando";
            txtComando.Size = new Size(864, 57);
            txtComando.TabIndex = 1;
            txtComando.Text = "Escreva um comando...";
            txtComando.Click += txtComando_Click;
            txtComando.LostFocus += txtComando_LostFocus;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.Image = Properties.Resources.micIcon;
            pictureBox1.Location = new Point(532, 149);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(247, 214);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.None;
            textBox1.Location = new Point(609, 606);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(93, 23);
            textBox1.TabIndex = 2;
            // 
            // UserControlHome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
            MinimumSize = new Size(360, 180);
            Name = "UserControlHome";
            Size = new Size(1280, 720);
            pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnManual).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnConfigs).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgLogo).EndInit();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Panel pnlTop;
        private PictureBox btnConfigs;
        private PictureBox imgLogo;
        private PictureBox btnManual;
        private Panel pnlBottom;
        private PictureBox pictureBox1;
        private TextBox txtComando;
        private TextBox textBox1;
    }
}
