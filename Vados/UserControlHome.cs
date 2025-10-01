using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

namespace Vados
{
    public partial class UserControlHome : UserControl
    {
        public event EventHandler<LoadPageEventArgs> loadPage;

        System.Windows.Forms.Timer timer;

        //Botões de pausar e parar comando de voz
        PictureBox btnPause;
        PictureBox btnStop;
        Image pauseIcon;
        Image playIcon;

        bool hasTranscribedAudio = true;
        System.Windows.Forms.Timer audioTimer;
        int audioSeconds = 0;

        //Variáveis do botão do microfone
        Image inactiveMicIcon;
        Image activeMicIcon;
        Image loadingMicIcon;
        Image pausedMicIcon;
        Image micIcon;
        float circleSizeDefaultMax = 325;
        float circleSizeCurrent = 325;
        float circleSize = 325;
        float circleSizeTarget = 325;
        float circleSizeDefault = 325;
        float circleSizeListeningMax = 325 * 0.85f;
        float circleSizeListening = 325 * 0.85f;
        float circleX = 0;
        float circleY = 0;
        float circleTargetX = -1;
        float circleTargetY = -1;
        float circleDefaultX = 0;
        float circleDefaultY = 0;
        float circleListeningX = 0;
        float circleListeningY = 0;
        Color circleColor = Colors.grayPrimary;
        bool circleHovering = false;


        //Variáveis da onda de áudio
        int barHeight = 100;
        int barWidth = 4;
        int barMargin = 2;
        int waveAreaMargin = 30;
        float waveAreaLeft;
        float waveAreaRight;
        float waveAreaTop;
        float waveAreaWidth;
        float waveAreaHeight;
        int maxBars;


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
            Console.ReadLine();

            //Timer para pintar tela a 60 FPS
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16;
            timer.Tick += Timer_Tick;
            timer.Start();

            //Timer do comando falado
            audioTimer = new System.Windows.Forms.Timer();
            audioTimer.Interval = 1000;
            audioTimer.Tick += audioTimer_Tick;

            //Variáveis do botão de microfone
            inactiveMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\inactiveMicIcon.png"));
            activeMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\activeMicIcon.png"));
            pausedMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\pausedMicIcon.png"));
            loadingMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"GIFs\loading.gif"));
            micIcon = inactiveMicIcon;
            ImageAnimator.Animate(loadingMicIcon, Timer_Tick);

            txtComando.Select(0, 0);


            //Inicializar botões de pausar e parar comando de voz
            btnPause = new PictureBox();
            btnStop = new PictureBox();
            pauseIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\pauseIcon.png"));
            playIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\playIcon.png"));

            btnPause.Click += btnPause_Click;
            btnPause.Visible = false;
            btnPause.Enabled = false;
            btnPause.SizeMode = PictureBoxSizeMode.Zoom;
            btnPause.Image = pauseIcon;
            btnPause.Cursor = Cursors.Hand;
            pnlBottom.Controls.Add(btnPause);

            btnStop.Click += btnStop_Click;
            btnStop.Visible = false;
            btnStop.Enabled = false;
            btnStop.SizeMode = PictureBoxSizeMode.Zoom;
            btnStop.Image = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\stopRecordingIcon.png"));
            btnStop.Cursor = Cursors.Hand;
            pnlBottom.Controls.Add(btnStop);


            //Otimizar pintura
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
        }


        public void PerformCommand(string command)
        {
            //Escurecer tela
            var parentForm = FindForm() as Form1;
            if (parentForm != null)
            {
                parentForm.ToggleOverlay(true);
            }

            //Extrair argumentos do comando
            string commandText = txtComando.Text.Replace(",", "");
            commandText = commandText.Replace("!", "");
            commandText = commandText.Replace("?", "");
            MessageBox.Show(commandText);
            var arguments = Comandos.CommandGetArguments(commandText);

            //Mostrar mensagem de confirmação
            parentForm.ShowPopupMessage(!arguments.success, parentForm, this, arguments.criteria);
        }

        public void FocusCommand(bool clear = false)
        {
            txtComando.SelectionLength = 0;
            txtComando.SelectionStart = txtComando.Text.Length;
            txtComando.Focus();
            if (clear) txtComando.Text = "";
        }


        //Começa a escutar o comando de voz
        public void StartListening()
        {
            //Box.Show("start");
            //Definir evento que acontece quando houver silêncio
            if (Global.VoiceRecognizer.HasSubscribers == false)
            {
                Global.VoiceRecognizer.OnSilence += OnSilence;
            }

            hasTranscribedAudio = false;
            Global.VoiceRecognizer.Start();

            micIcon = activeMicIcon;
            circleColor = Colors.bluePrimary;

            //Definir posição e tamanho corretos quando gravando áudio
            CorrectMicButton(true);

            //Ativar botões de pausar e parar
            btnStop.Enabled = true;
            btnStop.Visible = true;
            btnPause.Enabled = true;
            btnPause.Visible = true;

            //Começar timer
            audioTimer.Enabled = true;
            audioTimer.Start();

            TextBoxReset("Escutando...");
        }
        
        //Para de escutar o comando de voz
        public async Task StopListening()
        {
            micIcon = loadingMicIcon;
            btnStop.Enabled = false;
            btnPause.Enabled = false;

            audioTimer.Stop();  //Parar timer


            //Transcrever audio
            TextBoxReset("Transcrevendo...");
            string result = await Global.VoiceRecognizer.Stop();
            result = Comandos.CleanText(result);

            TextBoxWrite(result);
            hasTranscribedAudio = true;

            //Resetar botão do microfone
            micIcon = inactiveMicIcon;
            circleColor = Colors.grayPrimary;


            //Desativar os botões de pausar e parar
            btnStop.Visible = false;
            btnPause.Visible = false;

            //Resetar timer
            audioSeconds = 0;

            //Retornar o botão do microfone para a posição e tamanho padrões
            CorrectMicButton(true);
        }

        //Pausar comando de voz
        public async void PauseListening()
        {
            btnPause.Image = playIcon;
            micIcon = pausedMicIcon;

            //Pausar timer
            audioTimer.Enabled = false;

            ////Mostrar texto parcial
            //TextBoxReset("Transcrevendo...");
            //string result = await Global.VoiceRecognizer.Stop();
            //result = Comandos.CleanText(result);
            //TextBoxWrite(result);

            Global.VoiceRecognizer.Pause();
            hasTranscribedAudio = true;
        }


        //Continuar comando de voz
        public async void ResumeListening()
        {
            hasTranscribedAudio = false;
            Global.VoiceRecognizer.Resume();

            //Continuar timer
            audioTimer.Enabled = true;

            micIcon = activeMicIcon;
            btnPause.Image = pauseIcon;
        }


        public void TextBoxReset(string text, bool canClick = true)
        {
            //MessageBox.Show("textbox");
            txtComando.Text = text;
            txtComando.ForeColor = Colors.blueSecondary;
            textboxActive = false;
            //textboxCanClick = canClick;
        }

        public void TextBoxWrite(string text)
        {
            txtComando.Text = text;
            txtComando.ForeColor = Color.Black;
            textboxActive = true;
            //textboxCanClick = true;
        }


        //Corrige as variáveis do botão de microfone
        public void CorrectMicButton(bool setOnlyTargets = false)
        {
            //Corrigir tamanho
            int minY = 85;
            int maxY = lblText.Top;
            float newSize = (maxY - minY) * 0.8f;
            float newListeningSize = newSize * (circleSizeListeningMax / circleSizeDefaultMax);
            circleSizeDefault = Math.Min(newSize, circleSizeDefaultMax);
            circleSizeListening = Math.Min(newListeningSize, circleSizeListeningMax);

            circleSizeCurrent = circleSizeDefault;
            circleSizeTarget = circleSizeDefault;

            //Posição do círculo no centro
            circleDefaultX = this.Width / 2;
            circleDefaultY = minY + (lblText.Top - minY) / 2;
            circleTargetX = circleDefaultX;
            circleTargetY = circleDefaultY;

            if (setOnlyTargets == false)
            {
                circleX = circleDefaultX;
                circleY = circleDefaultY;
                circleSize = circleSizeDefault;
            }


            //Variáveis quando se está gravando áudio
            circleListeningX = txtAreaX - txtAreaOutSize + circleSize / 2;
            circleListeningY = circleDefaultY;

            if (Global.VoiceRecognizer.isRunning)
            {
                circleSizeCurrent = circleSizeListening;
                circleSizeTarget = circleSizeListening;
                circleTargetX = circleListeningX;
                circleTargetY = circleListeningY;

                if (setOnlyTargets == false)
                {
                    circleX = circleListeningX;
                    circleY = circleListeningY;
                    circleSize = circleSizeListening;
                }
            }

            //Corrigir y
            circleTargetY = Math.Clamp((int)circleY, 0, lblText.Location.Y - (int)circleSize + 85);
            circleY = circleTargetY;
        }

        //Corrige as variáveis da onda de áudio
        public void CorrectAudioWave()
        {
            waveAreaRight = (txtAreaX + txtAreaWidth + txtAreaOutSize);
            waveAreaWidth = waveAreaRight - (circleX + circleSizeListening / 2 + waveAreaMargin);
            waveAreaHeight = circleSizeListening;

            float barSpace = barWidth + barMargin;
            maxBars = (int)(waveAreaWidth / barSpace);
            float barsLeftoverWidth = waveAreaWidth - maxBars * barSpace;

            waveAreaLeft = circleX + circleSizeListening / 2 + waveAreaMargin + barsLeftoverWidth;
            waveAreaTop = circleY - circleSizeListening / 2;
        }

        public void CorrectVoiceButtons()
        {
            //Definir posições dos botões
            float fixedLeft = circleX + circleSizeListening / 2 + waveAreaMargin;
            float buttonLeftMargin = 20;
            int buttonSize = 54;
            int buttonDistance = 30 + buttonSize;
            int buttonTopMargin = 8;
            float buttonTop = waveAreaTop + waveAreaHeight / 2 + barHeight / 2 + buttonTopMargin;

            //Botão de parar
            btnPause.Width = buttonSize;
            btnPause.Height = buttonSize;
            btnPause.Left = (int)(fixedLeft + buttonLeftMargin);
            btnPause.Top = (int)buttonTop;

            //Botão de parar
            btnStop.Width = buttonSize;
            btnStop.Height = buttonSize;
            btnStop.Left = (int)(fixedLeft + buttonLeftMargin + buttonDistance);
            btnStop.Top = (int)buttonTop;
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


        
        //Eventos da textbox
        private void txtComando_Click(object sender, EventArgs e)
        {
            //Apagar texto temporário
            if (textboxActive == false)
            {
                TextBoxWrite("");
            }
        }

        private void txtComando_LostFocus(object sender, EventArgs e)
        {
            //Retornar texto temporário
            if (textboxActive == true && txtComando.Text == "")
            {
                TextBoxReset("Escreva um comando...");
            }
        }


        //Eventos do painel (onde tudo está e é desenhado)
        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            #region BOTÃO DO MICROFONE

            //------Círculo do microfone-------
            float circleLeft = circleX - circleSize / 2;
            float circleTop = circleY - circleSize / 2;


            //Sombra do círculo
            int shadowOffset = 25;

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(circleLeft, circleTop + shadowOffset, circleSize, circleSize);

            PathGradientBrush pathBrush = new PathGradientBrush(path);

            pathBrush.CenterColor = Color.FromArgb(100, Color.Black);
            pathBrush.SurroundColors = new[] { Color.FromArgb(2, Color.Black) };
            e.Graphics.FillPath(pathBrush, path);


            //Contorno do círculo
            float outlineSize = Math.Max(25f, Math.Min(circleSize * 0.05f, 25f));

            Brush brush = new SolidBrush(Colors.bluePrimary);
            RectangleF rect = new RectangleF(circleLeft, circleTop, circleSize, circleSize);
            e.Graphics.FillEllipse(brush, rect);


            //Círculo atrás do microfone
            brush = new SolidBrush(circleColor);
            rect = new RectangleF(circleLeft + outlineSize / 2, circleTop + outlineSize / 2, circleSize - outlineSize, circleSize - outlineSize);
            e.Graphics.FillEllipse(brush, rect);


            //-----Ícone do microfone-----
            ImageAnimator.UpdateFrames(micIcon);    //Animar imagem do botão de microfone (se for gif)

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float innerD = circleSize - outlineSize;
            float innerX = circleLeft + outlineSize / 2f;
            float innerY = circleTop + outlineSize / 2f;

            float targetBox = innerD * 0.60f; // ocupa 60% do círculo interno
            float ratio = (float)micIcon.Width / micIcon.Height;
            float drawW, drawH;
            
            //Definir tamanhos correto
            if (ratio >= 1f)
            {
                drawW = targetBox;
                drawH = targetBox / ratio;
            }
            else
            {
                drawH = targetBox;
                drawW = targetBox * ratio;
            }

            float drawX = innerX + (innerD - drawW) / 2f;
            float drawY = innerY + (innerD - drawH) / 2f;

            //Desenhar ícone
            e.Graphics.DrawImage(micIcon, drawX, drawY, drawW, drawH);

            #endregion


            #region ONDA DE ÁUDIO

            CorrectAudioWave();
            CorrectVoiceButtons();

            //Desenhar cada barra de áudio
            if (Global.VoiceRecognizer.isRunning)
            {
                var bars = Global.VoiceRecognizer.audioWaveBars;
                maxBars = Math.Min(bars.Count, maxBars);
                int start = bars.Count - maxBars;

                for (int i = start; i < bars.Count; i++)
                {
                    double height = bars[i];
                    height = barHeight - (height / -100 * barHeight);
                    //height = barHeight;
                    int distance = barWidth + barMargin;
                    float barX = waveAreaLeft + distance * i;
                    barX -= distance * start;
                    float barY = waveAreaTop + waveAreaHeight / 2 - (float)height / 2;

                    Brush barBrush = new SolidBrush(Colors.bluePrimary);
                    RectangleF bar = new RectangleF(barX, barY, barWidth, (float)height);
                    e.Graphics.FillRectangle(barBrush, bar);
                }
            }

            #endregion


            #region TEMPO DE ÁUDIO

            if (Global.VoiceRecognizer.isRunning)
            {
                //Variáveis
                TimeSpan time = TimeSpan.FromSeconds(audioSeconds);
                string timerText = time.ToString(@"mm\:ss");
                System.Drawing.Font timerFont = new System.Drawing.Font("Segoe UI", 15f);
                Size timerSize = TextRenderer.MeasureText("Teste", timerFont);
                int timerHeight = timerSize.Height;
                int timerLeft = btnStop.Right + 25;
                int timerTop = btnStop.Top + btnStop.Height / 2 - timerHeight / 2;

                //Desenhar texto
                Brush textBrush = new SolidBrush(Color.Black);
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(timerText, timerFont, textBrush, timerLeft, timerTop);
            }

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

        private void pnlBottom_Click(object sender, EventArgs e)
        {
            //Informações do mouse
            pnlBottom.Cursor = Cursors.Default;
            Point mousePos = this.PointToClient(Cursor.Position);
            int mouseX = mousePos.X;
            int mouseY = mousePos.Y;


            #region BOTÃO DE MICROFONE

            //Iniciar comando de voz
            if (circleHovering)
            {
                if (!Global.VoiceRecognizer.isRunning)
                {
                    StartListening();
                }
            }

            #endregion


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

        private void pnlBottom_Resize(object sender, EventArgs e)
        {
            #region AJUSTAR LABEL

            int labelX = this.Width / 2 - lblText.Width / 2;
            int labelY = txtComando.Top - lblText.Height - 40;

            lblText.Location = new Point(labelX, labelY);

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
            txtComando.Location = new Point(Width / 2 - txtComando.Width / 2, txtY);


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


            #region AJUSTAR BOTÃO DO MICROFONE

            if (Global.VoiceRecognizer != null)
            { 
                CorrectMicButton();
                CorrectAudioWave();
            }

            #endregion


            pnlBottom.Invalidate();
        }

        private void pnlBottom_MouseMove(object sender, MouseEventArgs e)
        {
            //Informações do mouse
            pnlBottom.Cursor = Cursors.Default;
            Point mousePos = this.PointToClient(Cursor.Position);
            int mouseX = mousePos.X;
            int mouseY = mousePos.Y;


            #region BOTÃO DE MICROFONE

            circleHovering = false;

            //Diminuir  tamanho do botão do microfone quando passar o mouse
            float distanceX = circleX - mouseX;
            float distanceY = circleY - mouseY;
            double distance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            //Resetar tamanho do círculo
            if (Global.VoiceRecognizer.isRunning == false)
                circleSizeTarget = circleSizeCurrent;

            //Checar se o mouse está dentro do círculo
            if (distance <= circleSize / 2)
            {
                circleHovering = true;

                //Diminuir tamanho do círculo
                if (Global.VoiceRecognizer.isRunning == false)
                    circleSizeTarget = circleSizeCurrent * 0.9f;
            }

            #endregion


            #region BOTÃO DE ENVIAR COMANDO

            float txtIconX = txtAreaX + txtAreaWidth - txtIconMarginW - txtIconSize;
            float txtIconY = txtAreaY + txtIconMarginH;
            RectangleF rect = new RectangleF(txtIconX, txtIconY, txtIconSize, txtIconSize);

            //lblDebug.Text = rect.Width.ToString() + ", " + rect.Height.ToString() + " - " + rect.X.ToString() + ", " + rect.Y.ToString() + " - " + mouseX.ToString() + ", " + mouseY.ToString();

            //Checar se o mouse está em dentro do botão
            if (Global.InsideRectangle(mousePos, rect) == true)
            {
                //Trocar imagem do mouse
                pnlBottom.Cursor = Cursors.Hand;
            }

            #endregion
        }

        
        //Evento que acontece todo frame
        private async void Timer_Tick(object? sender, EventArgs e)
        {
            int circleSizeChangeSpeed = 3;

            //Vibrar botão do microfone enquanto estiver escutando a voz
            if (Global.VoiceRecognizer != null && Global.VoiceRecognizer.isRunning)
            {
                double decibels;
                if (double.TryParse(Global.decibeis, out decibels))
                {
                    circleSizeChangeSpeed = 2;
                    float maxSize = 1.2f * circleSizeListening;
                    float normalizedDecibels = 1f - ((float)decibels / -100f);
                    circleSizeTarget = circleSizeListening + (maxSize - circleSizeListening) * normalizedDecibels;
                }
            }

            //Ajustar tamanho do botão do microfone
            circleSize += (circleSizeTarget - circleSize) / circleSizeChangeSpeed;

            if (Math.Abs(circleSizeTarget - circleSize) < 1)
                circleSize = circleSizeTarget;


            //Ajustar posição do microfone
            circleX += (circleTargetX - circleX) / 4;
            circleY += (circleTargetY - circleY) / 4;

            //Redesenhar tela
            pnlBottom.Invalidate();
        }

        
        //Botões da interface
        private void btnManual_Click(object sender, EventArgs e)
        {
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlManual));
        }

        private void btnConfigs_Click(object sender, EventArgs e)
        {
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlSettings));
        }

        private void btnHistorico_Click(object sender, EventArgs e)
        {

        }


        //Timer do comando falado
        private void audioTimer_Tick(object? sender, EventArgs e)
        {
            audioSeconds += 1;
        }


        //Botões do comando falado
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopListening();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (Global.VoiceRecognizer.isPaused)
            {
                //Despausar
                ResumeListening();
            }
            else
            {
                //Pausar
                PauseListening();
            }
        }


        private async void OnSilence(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => StopListening()));
        }
    }
}
