using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vados
{
    public partial class UserControlHome : UserControl
    {
        public event EventHandler<LoadPageEventArgs> loadPage;

        System.Windows.Forms.Timer timer;

        //Variáveis do botão do microfone
        float circleSizeDefault = 325;
        float circleSize = 325;
        float circleSizeTarget = 325;
        float circleX = 0;
        float circleY = 0;
        bool circleHovering = false;
        bool lastCircleHovering = false;

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

        public UserControlHome()
        {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16; //~60 FPS
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void btnTrocarPagina_Click(object sender, EventArgs e)
        {
            //Ir para página de configurações
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlSettings));
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

        private void txtComando_LostFocus(object sender, EventArgs e)
        {
            //Retornar texto temporário
            if (textboxActive == true)
            {
                txtComando.Text = "Escreva um comando...";
                txtComando.ForeColor = Color.FromArgb(88, 99, 152);
                txtComando.ForeColor = Colors.blueTernary;
            }

            textboxActive = false;
        }

        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {
            int middleX = this.Width / 2;
            int middleY = this.Height / 2;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            #region BOTÃO DO MICROFONE

            circleX = middleX - circleSize / 2;
            circleY = 335 - circleSize / 2;

            //Sombra do círculo
            int shadowOffset = 15;

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(circleX, circleY + shadowOffset, circleSize, circleSize);

            PathGradientBrush pathBrush = new PathGradientBrush(path);

            pathBrush.CenterColor = Color.FromArgb(100, Color.Black);
            pathBrush.SurroundColors = new[] { Color.FromArgb(2, Color.Black) };
            e.Graphics.FillPath(pathBrush, path);


            //Contorno do círculo
            int outlineSize = 25;

            Brush brush = new SolidBrush(Colors.bluePrimary);
            RectangleF rect = new RectangleF(circleX, circleY, circleSize, circleSize);
            e.Graphics.FillEllipse(brush, rect);


            //Círculo atrás do microfone
            Color circleColor = Color.FromArgb(243, 243, 243);
            brush = new SolidBrush(circleColor);
            rect = new RectangleF(circleX + outlineSize / 2, circleY + outlineSize / 2, circleSize - outlineSize, circleSize - outlineSize);
            e.Graphics.FillEllipse(brush, rect);


            //Microfone
            int sizeDiff = 120;
            string imgPath = Path.Combine(Application.StartupPath, @"Images\Icons\micIcon.png");
            Image micIcon = Image.FromFile(imgPath);
            e.Graphics.DrawImage(micIcon, new RectangleF(circleX + sizeDiff / 2, circleY + sizeDiff / 2, circleSize - sizeDiff, circleSize - sizeDiff));

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

            imgPath = Path.Combine(Application.StartupPath, @"Images\Icons\sendIcon.png");
            Image sendIcon = Image.FromFile(imgPath);
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

            //Aumentar tamanho do botão do microfone quando passar o mouse
            PointF middle = new PointF(circleX + circleSize / 2, circleY + circleSize / 2);
            float distanceX = middle.X - mouseX;
            float distanceY = middle.Y - mouseY;
            double distance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            //Checar se o mouse está dentro do círculo
            if (distance <= circleSize / 2)
            {
                //Aumentar tamanho do círculo
                circleSizeTarget = 300;
                circleHovering = true;
                pnlBottom.Cursor = Cursors.Hand;
            }
            else
            {
                //Resetar tamanho do círculo
                circleHovering = false;
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
            {
                circleSize = circleSizeTarget;
                return;
            }

            pnlBottom.Invalidate();
        }

        private void pnlBottom_Resize(object sender, EventArgs e)
        {
            #region AJUSTAR TEXTBOX

            int middleX = this.Width / 2;

            //Ajustar tamanho para definir as variáveis corretamente
            if (setTextboxWidth == true)
            {
                txtComando.Width += txtboxWidthOffset;
            }

            //Tamanho da textbox
            double newWidth = this.Width * 0.575;
            txtComando.Width = (int)newWidth;

            //Posição da textbox
            txtComando.Location = new Point(middleX - txtComando.Width / 2, txtComando.Location.Y);

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


            //Ajustar label (o que você deseja fazer?)
            lblText.Location = new Point(middleX - lblText.Width / 2, lblText.Location.Y);

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
                string comando = txtComando.Text;

                if (textboxActive == true)
                {
                    List<string> palavras = Comandos.SepararPalavras(comando);
                    MessageBox.Show(String.Join(", ", palavras.ToArray()));
                }
            }

            #endregion

        }
    }
}
