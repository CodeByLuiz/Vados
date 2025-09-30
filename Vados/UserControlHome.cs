using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vados
{
    public partial class UserControlHome : UserControl
    {

        public event EventHandler<LoadPageEventArgs> loadPage;

        System.Windows.Forms.Timer timer;
        private Image micIcon;

        //Variáveis do botão do microfone
        float circleSizeMax = 325;
        float circleSizeDefault = 325;
        float circleSize = 325;
        float circleSizeTarget = 325;
        float circleX = 0;
        float circleY = 0;
        bool circleHovering = false;
        bool lastCircleHovering = false;
        float circleSizeRatio = 1;


        //Variáveis da textbox
        int txtAreaPaddingW = 18;
        int txtAreaPaddingH = 15;
        int txtAreaWidth;
        int txtAreaHeight;
        int txtAreaX;
        int txtAreaY;
        int txtAreaOutSize = 6;
        float txtIconMarginH = 8;
        float txtIconMarginW = 18;
        float txtIconSize;
        int txtboxWidthOffset;
        bool setTextboxWidth = false;
        bool textboxActive = false;



        public void PerformCommand(string command)
        {
            //Escurecer tela
            var parentForm = FindForm() as Form1;
            if (parentForm != null)
            {
                parentForm.ToggleOverlay(true);
            }

            //Extrair argumentos do comando
            var arguments = Comandos.CommandGetArguments(txtComando.Text);

            //Mostrar mensagem de confirmação
            parentForm.ShowPopupMessage(!arguments.success, parentForm, this, arguments.criteria,Comandotxt:txtComando.Text);
        }


        public void FocusCommand(bool clear = false)
        {
            txtComando.SelectionLength = 0;
            txtComando.SelectionStart = txtComando.Text.Length;
            txtComando.Focus();
            if (clear) txtComando.Text = "";
        }


        //Realizar comando quando apertar enter
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                PerformCommand(txtComando.Text);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        public UserControlHome()
        {
            InitializeComponent();
            Console.ReadLine();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16; //~60 FPS
            timer.Tick += Timer_Tick;
            timer.Start();

            micIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\micIcon.png"));
            txtComando.Select(0, 0);
        }



        private void txtComando_Click(object sender, EventArgs e)
        {
            //Apagar texto temporário
            if (textboxActive == false)
            {
                txtComando.Text = "";
                txtComando.ForeColor = Color.Black;
            }

            textboxActive = true;
        }
        public string TxtComandoEditar
        {
            
            get { return txtComando.Text; }
            set { txtComando.Text = value; }

        }
        private void txtComando_LostFocus(object sender, EventArgs e)
        {
            //Retornar texto temporário
            if (textboxActive == true && txtComando.Text == "")
            {
                txtComando.Text = "Escreva um comando...";
                txtComando.ForeColor = Color.FromArgb(88, 99, 152);
                txtComando.ForeColor = Colors.blueTernary;
                textboxActive = false;
            }
        }



        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {


            int middleX = this.Width / 2;

            int middleY = 85 + (lblText.Top - 85) / 2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            #region BOTÃO DO MICROFONE

            circleX = middleX - circleSize / 2;
            circleY = middleY - circleSize / 2;

            circleY = Math.Clamp((int)circleY, 0, lblText.Location.Y - (int)circleSize + 85);


            //Sombra do círculo
            int shadowOffset = 25;

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(circleX, circleY + shadowOffset, circleSize, circleSize);

            PathGradientBrush pathBrush = new PathGradientBrush(path);

            pathBrush.CenterColor = Color.FromArgb(100, Color.Black);
            pathBrush.SurroundColors = new[] { Color.FromArgb(2, Color.Black) };
            e.Graphics.FillPath(pathBrush, path);





            //Contorno do círculo
            float outlineSize = Math.Max(20f, Math.Min(circleSize * 0.05f, 25f));

            Brush brush = new SolidBrush(Colors.bluePrimary);
            RectangleF rect = new RectangleF(circleX, circleY, circleSize, circleSize);
            e.Graphics.FillEllipse(brush, rect);


            //Círculo atrás do microfone
            Color circleColor = Color.FromArgb(243, 243, 243);
            brush = new SolidBrush(circleColor);
            rect = new RectangleF(circleX + outlineSize / 2, circleY + outlineSize / 2, circleSize - outlineSize, circleSize - outlineSize);
            e.Graphics.FillEllipse(brush, rect);


            //Microfone
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float innerD = circleSize - outlineSize;
            float innerX = circleX + outlineSize / 2f;
            float innerY = circleY + outlineSize / 2f;

            float targetBox = innerD * 0.60f; // ocupa 60% do círculo interno
            float aspect = (float)micIcon.Width / micIcon.Height;
            float drawW, drawH;

            if (aspect >= 1f)
            {
                drawW = targetBox;
                drawH = targetBox / aspect;
            }
            else
            {
                drawH = targetBox;
                drawW = targetBox * aspect;
            }

            float drawX = innerX + (innerD - drawW) / 2f;
            float drawY = innerY + (innerD - drawH) / 2f;

            e.Graphics.DrawImage(micIcon, drawX, drawY, drawW, drawH);







            #endregion


            #region CAIXA DE TEXTO

            //Contorno
            Color outlineColor = Colors.bluePrimary;
            int outWidth = txtAreaWidth + txtAreaOutSize * 2;
            int outHeight = txtAreaHeight + txtAreaOutSize * 2;
            int outX = txtAreaX - txtAreaOutSize;
            int outY = txtAreaY - txtAreaOutSize;

            brush = new SolidBrush(outlineColor);
            rect = new RectangleF(outX, outY, outWidth, outHeight);
            GraphicsPath roundedRectPath = Global.RoundedRectangle(rect, (float)(outHeight * 0.33));
            e.Graphics.FillPath(brush, roundedRectPath);


            //Fundo
            Color backColor = txtComando.BackColor;

            brush = new SolidBrush(backColor);
            rect = new RectangleF(txtAreaX, txtAreaY, txtAreaWidth, txtAreaHeight);
            roundedRectPath = Global.RoundedRectangle(rect, (float)(txtAreaHeight * 0.33));
            e.Graphics.FillPath(brush, roundedRectPath);


            //Botão de enviar comando
            float txtIconX = txtAreaX + txtAreaWidth - txtIconMarginW - txtIconSize;
            float txtIconY = txtAreaY + txtIconMarginH;

            string sendimgPath = Path.Combine(Application.StartupPath, @"Images\Icons\sendIcon.png");
            Image sendIcon = Image.FromFile(sendimgPath);
            e.Graphics.DrawImage(sendIcon, txtIconX, txtIconY, txtIconSize, txtIconSize);

            #endregion
        }

        private void pnlBottom_MouseMove(object sender, MouseEventArgs e)
        {
            //Informações do mouse
            pnlBottom.Cursor = Cursors.Default;
            Point mousePos = this.PointToClient(Cursor.Position);
            int mouseX = mousePos.X;
            int mouseY = mousePos.Y;


            #region BOTÃO DE MICROFONE

            lastCircleHovering = circleHovering;

            //Diminuir  tamanho do botão do microfone quando passar o mouse
            PointF middle = new PointF(circleX + circleSize / 2, circleY + circleSize / 2);
            float distanceX = middle.X - mouseX;
            float distanceY = middle.Y - mouseY;
            double distance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            //Checar se o mouse está dentro do círculo
            if (distance <= circleSize / 2)
            {
                //Diminuir tamanho do círculo
                circleSizeTarget = circleSizeDefault * 0.9f;
            }
            else
            {
                //Resetar tamanho do círculo
                circleSizeTarget = circleSizeDefault;
            }

            #endregion


            #region BOTÃO DE ENVIAR COMANDO

            float txtIconX = txtAreaX + txtAreaWidth - txtIconMarginW - txtIconSize;
            float txtIconY = txtAreaY + txtIconMarginH;
            RectangleF rect = new RectangleF(txtIconX, txtIconY, txtIconSize, txtIconSize);

            lblDebug.Text = rect.Width.ToString() + ", " + rect.Height.ToString() + " - " + rect.X.ToString() + ", " + rect.Y.ToString() + " - " + mouseX.ToString() + ", " + mouseY.ToString();

            //Checar se o mouse está em dentro do botão
            if (Global.InsideRectangle(mousePos, rect) == true)
            {
                //Trocar imagem do mouse
                pnlBottom.Cursor = Cursors.Hand;
            }

            #endregion
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {

            //Ajustar tamanho do botão do microfone
            circleSize += (circleSizeTarget - circleSize) / 3;

            if (Math.Abs(circleSizeTarget - circleSize) < 1)
                circleSize = circleSizeTarget;



            pnlBottom.Invalidate();
        }

        private void pnlBottom_Resize(object sender, EventArgs e)
        {
            #region AJUSTAR LABEL

            int labelX = this.Width / 2 - lblText.Width / 2;
            int labelY = txtComando.Top - lblText.Height - 40;

            lblText.Location = new Point(labelX, labelY);

            #endregion


            #region AJUSTAR BOTÃO DO MICROFONE

            //Corrigir tamanho
            int minY = 85;
            int maxY = lblText.Top;
            float newSize = (maxY - minY) * 0.8f;
            circleSizeDefault = Math.Min(newSize, circleSizeMax);
            circleSize = circleSizeDefault;
            circleSizeTarget = circleSizeDefault;

            //Reposicionar círculo no centro
            int middleX = this.Width / 2;
            int middleY = minY + (lblText.Top - minY) / 2;

            circleX = middleX - circleSize / 2;
            circleY = middleY - circleSize / 2;

            #endregion


            #region AJUSTAR TEXTBOX

            //Ajustar tamanho para definir as variáveis corretamente
            if (setTextboxWidth == true)
            {
                txtComando.Width += txtboxWidthOffset;
            }

            //Tamanho da textbox
            double newWidth = this.Width * 0.575;
            txtComando.Width = (int)newWidth;

            //Posição da textbox
            int txtY = labelY + txtComando.Height + lblText.Height;
            txtY = Math.Clamp(txtComando.Location.Y, 0, this.Height - 10);
            txtComando.Location = new Point(middleX - txtComando.Width / 2, txtY);


            //Variáveis da área atrás da textbox
            float txtOldAreaHeight = txtComando.Height + txtAreaPaddingH * 2;
            txtIconSize = txtOldAreaHeight - 2 * txtIconMarginH;
            txtboxWidthOffset = (int)(txtIconSize + txtIconMarginW * 3 - txtAreaPaddingW);
            txtAreaWidth = txtComando.Width + txtAreaPaddingW * 2;
            txtAreaHeight = txtComando.Height + txtAreaPaddingH * 2;
            txtAreaX = txtComando.Location.X - txtAreaPaddingW;
            txtAreaY = txtComando.Location.Y - txtAreaPaddingH;

            //Diminuir tamanho da textbox para não passar por cima do botão de enviar
            txtComando.Width -= txtboxWidthOffset;
            setTextboxWidth = true;

            #endregion


            pnlBottom.Invalidate();
        }

        private void pnlBottom_Click(object sender, EventArgs e)
        {
            //Informações do mouse
            pnlBottom.Cursor = Cursors.Default;
            Point mousePos = this.PointToClient(Cursor.Position);
            int mouseX = mousePos.X;
            int mouseY = mousePos.Y;


            #region BOTÃO DE ENVIAR COMANDO

            float sendButtonX = txtAreaX + txtAreaWidth - txtIconMarginW - txtIconSize;
            float sendButtonY = txtAreaY + txtIconMarginH;
            RectangleF rect = new RectangleF(sendButtonX, sendButtonY, txtIconSize, txtIconSize);

            //Checar se o mouse está em dentro do botão
            if (Global.InsideRectangle(mousePos, rect) == true)
            {
                PerformCommand(txtComando.Text);
            }

            #endregion

        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlManual));

            
        }

        private void btnConfigs_Click(object sender, EventArgs e)
        {
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlSettings));

           
        }

        private void txtComando_TextChanged(object sender, EventArgs e)
        {

        }

        private void UserControlHome_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("enter");

            if (e.KeyCode == Keys.Enter)
            {
                PerformCommand(txtComando.Text);
            }
        }
        private bool historyOpen = false;

        public bool HistoryOpen
        {
            get { return historyOpen; }
            set { historyOpen = value; } 
        }


        private void btnHistorico_Click(object sender, EventArgs e)
        {
            var parentForm = FindForm() as Form1;
            if (!historyOpen)
            {
                //var parentForm = FindForm() as Form1;
                parentForm.ShowHistoryTab(parentForm, this);
                
            }
            else
            {
                parentForm.CloseHistoryTab();
               
            }

        }

        
    }
}
