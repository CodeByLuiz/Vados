namespace Vados
{
    partial class FormMessage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessage));
            lblTitle = new Label();
            txtMessage = new RichTextBox();
            btnConfirm = new RoundedButton();
            btnCancel = new RoundedButton();
            btnClose = new PictureBox();
            imgTitleIcon = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)btnClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgTitleIcon).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.FlatStyle = FlatStyle.System;
            lblTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(48, 61, 99);
            lblTitle.Location = new Point(26, 29);
            lblTitle.Margin = new Padding(0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(280, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Confirmar comando";
            // 
            // txtMessage
            // 
            txtMessage.BackColor = Color.FromArgb(231, 231, 231);
            txtMessage.BorderStyle = BorderStyle.None;
            txtMessage.Font = new Font("Segoe UI", 11F);
            txtMessage.Location = new Point(27, 75);
            txtMessage.Margin = new Padding(0);
            txtMessage.Name = "txtMessage";
            txtMessage.ReadOnly = true;
            txtMessage.ScrollBars = RichTextBoxScrollBars.None;
            txtMessage.Size = new Size(402, 29);
            txtMessage.TabIndex = 1;
            txtMessage.Text = "Você deseja criar um arquivo chamado \"teste\"?";
            txtMessage.Enter += txtMessage_Enter;
            // 
            // btnConfirm
            // 
            btnConfirm.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnConfirm.BackColor = Color.FromArgb(82, 99, 152);
            btnConfirm.Cursor = Cursors.Hand;
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Location = new Point(64, 135);
            btnConfirm.Margin = new Padding(0);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(119, 44);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "Sim";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.BackColor = Color.FromArgb(231, 231, 231);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(82, 99, 152);
            btnCancel.FlatAppearance.BorderSize = 3;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.Black;
            btnCancel.Location = new Point(277, 135);
            btnCancel.Margin = new Padding(0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(119, 44);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Não";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.Location = new Point(407, 27);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(27, 32);
            btnClose.SizeMode = PictureBoxSizeMode.Zoom;
            btnClose.TabIndex = 4;
            btnClose.TabStop = false;
            btnClose.Click += btnClose_Click;
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            // 
            // imgTitleIcon
            // 
            imgTitleIcon.Image = (Image)resources.GetObject("imgTitleIcon.Image");
            imgTitleIcon.Location = new Point(274, 33);
            imgTitleIcon.Margin = new Padding(3, 4, 3, 4);
            imgTitleIcon.Name = "imgTitleIcon";
            imgTitleIcon.Size = new Size(27, 32);
            imgTitleIcon.SizeMode = PictureBoxSizeMode.Zoom;
            imgTitleIcon.TabIndex = 5;
            imgTitleIcon.TabStop = false;
            // 
            // FormMessage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(231, 231, 231);
            ClientSize = new Size(457, 213);
            Controls.Add(imgTitleIcon);
            Controls.Add(btnClose);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(txtMessage);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormMessage";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Atenção!";
            Load += FormMessage_Load;
            Resize += FormMessage_Resize;
            ((System.ComponentModel.ISupportInitialize)btnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgTitleIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private RichTextBox txtMessage;
        private RoundedButton btnConfirm;
        private RoundedButton btnCancel;
        private PictureBox btnClose;
        private PictureBox imgTitleIcon;
    }
}