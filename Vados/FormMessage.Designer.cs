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
            lblTitle = new Label();
            txtMessage = new RichTextBox();
            btnConfirm = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.FlatStyle = FlatStyle.System;
            lblTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(48, 61, 99);
            lblTitle.Location = new Point(14, 12);
            lblTitle.Margin = new Padding(0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(219, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Confirmar comando";
            // 
            // txtMessage
            // 
            txtMessage.BackColor = Color.FromArgb(231, 231, 231);
            txtMessage.BorderStyle = BorderStyle.None;
            txtMessage.Font = new Font("Segoe UI", 11F);
            txtMessage.Location = new Point(14, 50);
            txtMessage.Margin = new Padding(3, 2, 3, 2);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(420, 58);
            txtMessage.TabIndex = 1;
            txtMessage.Text = "Você deseja criar um arquivo chamado \"teste\"?";
            // 
            // btnConfirm
            // 
            btnConfirm.BackColor = Color.FromArgb(82, 99, 152);
            btnConfirm.Cursor = Cursors.Hand;
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Location = new Point(79, 105);
            btnConfirm.Margin = new Padding(0, 0, 0, 0);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(82, 26);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "Sim";
            btnConfirm.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(232, 232, 232);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(82, 99, 152);
            btnCancel.FlatAppearance.BorderSize = 3;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.Black;
            btnCancel.Location = new Point(248, 105);
            btnCancel.Margin = new Padding(0, 0, 0, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(82, 26);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Não";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // FormMessage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(231, 231, 231);
            ClientSize = new Size(448, 157);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(txtMessage);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormMessage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Atenção!";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private RichTextBox txtMessage;
        private Button btnConfirm;
        private Button btnCancel;
    }
}