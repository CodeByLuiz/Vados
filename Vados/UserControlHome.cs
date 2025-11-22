using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
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

        //Imagens dos botões da interface
        Image btnHistoryImage;
        Image btnHistoryImageHover;
        Image btnHistoryShadow;
        Image btnManualImage;
        Image btnManualImageHover;
        Image btnManualShadow;
        Image btnConfigsImage;
        Image btnConfigsImageHover;
        Image btnConfigsShadow;
        Image btnSendImage;
        Image btnSendImageHover;
        Image btnSendShadow;
        Image btnPauseImage;
        Image btnPauseImageHover;
        Image btnStopImage;
        Image btnStopImageHover;
        Image btnPlayImage;
        Image btnPlayImageHover;

        //Botões de pausar e parar comando de voz
        PictureBox btnPause;
        PictureBox btnStop;

        bool isTranscribingAudio = false;
        System.Windows.Forms.Timer audioTimer;
        int audioSeconds = 0;

        //Variáveis do botão do microfone
        Image inactiveMicIcon;
        Image activeMicIcon;
        Image loadingWhiteMicIcon;
        Image loadingBlueMicIcon;
        Image pausedMicIcon;
        Image micIcon;
        Image micShadow;
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
        bool textboxCanClick = true;
        bool btnSendHovering = false;
        Image btnSendCurrentImage;

        //Informação do progresso do comando
        System.Windows.Forms.Timer infoTimer;
        int infoTimerMaxMs = 2500;
        int infoTimerCurrentMs = 2500;
        int infoTextAlpha = 0;
        Color infoTextColor;
        Font infoTextFont;
        string infoText = "Info";


        public UserControlHome()
        {
            InitializeComponent();

            #region TIMERS

            //Timer para pintar tela a 60 FPS
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16;
            timer.Tick += Timer_Tick;
            timer.Start();

            //Timer do comando de voz
            audioTimer = new System.Windows.Forms.Timer();
            audioTimer.Interval = 1000;
            audioTimer.Tick += audioTimer_Tick;

            //Timer para desaparecer a informação do comando
            infoTimer = new System.Windows.Forms.Timer();
            infoTimer.Interval = 16;    //Todo frame
            infoTimer.Tick += infoTimer_Tick;

            #endregion


            #region IMAGENS

            float brightnessChange = 0.1f;

            //Botões da interface
            btnConfigsImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\configIcon.png"));
            btnConfigsImageHover = Global.ImageChangeBrightness(btnConfigsImage, brightnessChange);
            btnConfigsShadow = Global.ImageCreateShadow(btnConfigsImage, Color.Black, 0.2f, 15);
            btnManualImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\manualIcon.png"));
            btnManualImageHover = Global.ImageChangeBrightness(btnManualImage, brightnessChange);
            btnManualShadow = Global.ImageCreateShadow(btnManualImage, Color.Black, 0.2f, 15);
            btnHistoryImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\historyIcon.png"));
            btnHistoryImageHover = Global.ImageChangeBrightness(btnHistoryImage, brightnessChange);
            btnHistoryShadow = Global.ImageCreateShadow(btnHistoryImage, Color.Black, 0.2f, 15);

            //Botões do comando de voz
            btnPauseImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\pauseIcon.png"));
            btnPauseImageHover = Global.ImageChangeBrightness(btnPauseImage, 0.25f);
            btnPlayImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\playIcon.png"));
            btnPlayImageHover = Global.ImageChangeBrightness(btnPlayImage, 0.25f);
            btnStopImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\stopRecordingIcon.png"));
            btnStopImageHover = Global.ImageChangeBrightness(btnStopImage, 0.25f);

            //Botões da textbox
            btnSendImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\sendIcon.png"));
            btnSendImageHover = Global.ImageChangeBrightness(btnSendImage, brightnessChange);
            btnSendShadow = Global.ImageCreateShadow(btnSendImage, Color.Black, 0.3f, 15);
            btnSendCurrentImage = btnSendImage;

            //Botão do microfone
            inactiveMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\inactiveMicIcon.png"));
            activeMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\activeMicIcon.png"));
            pausedMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\pausedMicIcon.png"));
            loadingWhiteMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"GIFs\loadingWhite.gif"));
            loadingBlueMicIcon = Image.FromFile(Path.Combine(Application.StartupPath, @"GIFs\loadingBlue.gif"));
            micShadow = Global.ImageCreateShadow(inactiveMicIcon, Color.Black, 0.35f, 12);
            ImageAnimator.Animate(loadingWhiteMicIcon, Timer_Tick);
            ImageAnimator.Animate(loadingBlueMicIcon, Timer_Tick);
            micIcon = inactiveMicIcon;

            #endregion


            #region FONTES

            txtComando.Font = new Font("Segoe UI", 20);
            lblText.Font = new Font(Fonts.DarkerSemiBold, 34);
            lblButtonName.Font = new Font(Fonts.DarkerMedium, 13);
            infoTextFont = new Font(Fonts.DarkerMedium, 17);

            #endregion


            //Inicializar botões de pausar e parar comando de voz
            btnPause = new PictureBox();
            btnStop = new PictureBox();
            btnPauseImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\pauseIcon.png"));
            btnPlayImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\playIcon.png"));

            btnPause.Click += btnPause_Click;
            btnPause.MouseEnter += btnPause_MouseEnter;
            btnPause.MouseLeave += btnPause_MouseLeave;
            btnPause.Visible = false;
            btnPause.Enabled = false;
            btnPause.SizeMode = PictureBoxSizeMode.Zoom;
            btnPause.Image = btnPauseImage;
            btnPause.Cursor = Cursors.Hand;
            pnlBottom.Controls.Add(btnPause);

            btnStop.Click += btnStop_Click;
            btnStop.MouseEnter += btnStop_MouseEnter;
            btnStop.MouseLeave += btnStop_MouseLeave;
            btnStop.Visible = false;
            btnStop.Enabled = false;
            btnStop.SizeMode = PictureBoxSizeMode.Zoom;
            btnStop.Image = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\stopRecordingIcon.png"));
            btnStop.Cursor = Cursors.Hand;
            pnlBottom.Controls.Add(btnStop);


            txtComando.Select(0, 0);

            //Otimizar pintura
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
        }


        public void PerformCommand(string command, bool isVoiceCommand)
        {
            var parentForm = FindForm() as Form1;

            //Comando vazio
            if (string.IsNullOrEmpty(command))
            {
                #region---------ERRO: comando de voz não identificado---------

                if (isVoiceCommand)
                {
                    var rtb = new RichTextBox();
                    Global.AppendPlainText(rtb, "Áudio não identificado. ");
                    Global.AppendFormattedText(rtb, "(Inaudível / Ruído / Música)", Color.Gray, rtb.Font);
                    parentForm.ShowPopupMessage(true, parentForm, this, null, rtb.Rtf);
                }

                #endregion----------------------------------------

                return;
            }

            //Realizer testes
            if (command == "teste")
            {
                Comandos.TestCases();
                return;
            }

            //Extrair argumentos do comando
            string commandText = command.Replace(",", "");
            //commandText = commandText.Replace(".", "");
            commandText = commandText.Replace("!", "");
            commandText = commandText.Replace("?", "");
            var arguments = Comandos.CommandGetArguments(commandText);

            //Mostrar mensagem de confirmação
            parentForm.ToggleOverlay(true);
            parentForm.ShowPopupMessage(!arguments.success, parentForm, this, arguments.criteria, commandText: txtComando.Text);
        }

        public void FocusCommand(bool clear = false)
        {
            if (clear) txtComando.Text = "";
            txtComando.Focus();
            txtComando.Select(0, 0);
        }


        //Começa a escutar o comando de voz
        public void StartListening()
        {
            //Definir evento que acontece quando houver silêncio
            if (Global.VoiceRecognizer.HasSubscribers == false)
            {
                Global.VoiceRecognizer.OnSilence += OnSilence;
            }

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

            TextBoxReset("Escutando...", false);
        }

        //Para de escutar o comando de voz
        public async Task StopListening()
        {
            SetMicLoadingIcon(true, "loadingWhite");
            btnStop.Enabled = false;
            btnPause.Enabled = false;

            audioTimer.Stop();  //Parar timer

            //Transcrever áudio
            if (Global.VoiceRecognizer.isInitialized)
            {
                TextBoxReset("Transcrevendo...", false);
                isTranscribingAudio = true;

                string result = await Global.VoiceRecognizer.Stop();
                result = result.Replace("\"", "");
                result = Comandos.CleanText(result);
                result = Comandos.CorrectCommonErrors(result, Comandos.commonErrorSynonyms);

                //Realizar comando
                isTranscribingAudio = false;
                TextBoxWrite(result);
                PerformCommand(result, true);
            }

            #region---------ERRO: modelo de reconhecimento de voz não inicializado---------

            else
            {
                TextBoxReset("Escreva um comando...");

                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Modelo de reconhecimento de voz não inicializado.");

                //Mostrar mensagem de erro
                var parentForm = FindForm() as Form1;
                parentForm.ToggleOverlay(true);
                parentForm.ShowPopupMessage(true, parentForm, this, null, rtb.Rtf);
            }

            #endregion----------------------------------------


            //Resetar botão do microfone
            SetMicLoadingIcon(false);
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
            btnPause.Image = btnPlayImage;
            micIcon = pausedMicIcon;
            TextBoxReset("Comando de voz pausado...", false);

            //Pausar timer
            audioTimer.Enabled = false;
            Global.VoiceRecognizer.Pause();
        }


        //Continuar comando de voz
        public async void ResumeListening()
        {
            Global.VoiceRecognizer.Resume();
            TextBoxReset("Escutando...", false);

            //Continuar timer
            audioTimer.Enabled = true;

            micIcon = activeMicIcon;
            btnPause.Image = btnPauseImage;
        }


        //Ativa o carregamento no botão do microfone
        public void SetMicLoadingIcon(bool loading, string gif = "")
        {
            Image loadingGif = loadingWhiteMicIcon;
            if (gif == "loadingBlue") loadingGif = loadingBlueMicIcon;

            micIcon = inactiveMicIcon;
            if (loading)
                micIcon = loadingGif;
        }


        //Desativa a textbox
        public void TextBoxReset(string text, bool canClick = true)
        {
            txtComando.Text = text;
            txtComando.ForeColor = Colors.blueSecondary;
            textboxActive = false;
            textboxCanClick = canClick;

            //Desfocar textbox
            if (!canClick)
            {
                txtComando.Select(0, 0);
                txtComando.Cursor = Cursors.Default;
                ActiveControl = imgLogo;
            }
        }

        //Ativa a textbox
        public void TextBoxWrite(string text)
        {
            txtComando.Text = text;
            txtComando.ForeColor = Color.Black;
            txtComando.Select(0, 0);
            textboxActive = true;
            textboxCanClick = true;
            txtComando.Cursor = Cursors.IBeam;
        }
        public string TxtComandoEditar
        {

            get { return txtComando.Text; }
            set
            {
                txtComando.Text = value;
                txtComando.ForeColor = Color.Black;
                FocusCommand();
            }

        }


        //Define o texto da label de informação do comando
        public void UpdateCommandInfoLabel(string text, FontStyle fontStyle, Color color, bool startTimer)
        {
            infoText = text;
            infoTextColor = color;
            infoTextFont = new Font(infoTextFont, fontStyle);

            infoTextAlpha = 255;
            if (startTimer)
            {
                infoTimer.Start();
                infoTimerCurrentMs = infoTimerMaxMs;
            }
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
            if (keyData == Keys.Enter && ActiveControl == txtComando && txtComando.Text != "")
            {
                //string command = Comandos.GetClosestMatch(txtComando.Text, Global.VoiceRecognizer.hints);
                PerformCommand(txtComando.Text, false);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }



        //Eventos do painel (onde tudo está e é desenhado)
        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            #region BOTÃO DO MICROFONE

            //------Círculo do microfone-------
            float circleLeft = circleX - circleSize / 2;
            float circleTop = circleY - circleSize / 2;


            //Sombra do círculo
            int shadowOffset = 15;

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(circleLeft, circleTop + shadowOffset, circleSize, circleSize);
            Global.DrawShadow(e, path, Color.Black, new PointF(0.65f, 0.65f));


            //Contorno do círculo
            float outlineSize = 30f;

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

            //Desenhar sombra
            if (micIcon != loadingWhiteMicIcon && micIcon != loadingBlueMicIcon)
            {
                shadowOffset = 4;
                e.Graphics.DrawImage(micShadow, drawX, drawY + shadowOffset, drawW, drawH);
            }

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
                    barBrush.Dispose();
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
                textBrush.Dispose();
            }

            #endregion


            #region CAIXA DE TEXTO

            //-----Contorno-----
            Color outlineColor = Colors.bluePrimary;
            int outWidth = txtAreaWidth + txtAreaOutSize * 2;
            int outHeight = txtAreaHeight + txtAreaOutSize * 2;
            int outX = txtAreaX - txtAreaOutSize;
            int outY = txtAreaY - txtAreaOutSize;

            rect = new RectangleF(outX, outY, outWidth, outHeight);
            GraphicsPath roundedRectPath = Global.RoundedRectangle(rect, (float)(outHeight * 0.33));

            //Desenhar sombra
            shadowOffset = 8;
            RectangleF shadowRect = new RectangleF(outX, outY + shadowOffset, outWidth, outHeight);
            GraphicsPath shadowPath = Global.RoundedRectangle(shadowRect, (float)(outHeight * 0.33));
            Global.DrawShadow(e, shadowPath, Color.Black, new PointF(1f, 0.1f));

            //Desenhar contorno
            brush = new SolidBrush(outlineColor);
            e.Graphics.FillPath(brush, roundedRectPath);


            //-----Fundo-----
            Color backColor = txtComando.BackColor;

            brush = new SolidBrush(backColor);
            rect = new RectangleF(txtAreaX, txtAreaY, txtAreaWidth, txtAreaHeight);
            roundedRectPath = Global.RoundedRectangle(rect, (float)(txtAreaHeight * 0.33));
            e.Graphics.FillPath(brush, roundedRectPath);


            //-----Botão de enviar comando-----
            float txtIconX = txtAreaX + txtAreaWidth - txtIconMarginW - txtIconSize;
            float txtIconY = txtAreaY + txtIconMarginH;

            //Desenhar sombra
            shadowOffset = 3;
            e.Graphics.DrawImage(btnSendShadow, txtIconX, txtIconY + shadowOffset, txtIconSize, txtIconSize);

            //Desenhar ícone
            e.Graphics.DrawImage(btnSendCurrentImage, txtIconX, txtIconY, txtIconSize, txtIconSize);

            #endregion


            #region INFORMAÇÃO DO COMANDO

            int topMargin = 35;
            SizeF textSize = e.Graphics.MeasureString(infoText, infoTextFont);
            int infoTextX = Width / 2 - (int)textSize.Width / 2;
            int infoTextY = txtComando.Bottom + topMargin;
            Point infoTextPos = new Point(infoTextX, infoTextY);
            Color textColor = Color.FromArgb(infoTextAlpha, infoTextColor);

            brush = new SolidBrush(textColor);
            e.Graphics.DrawString(infoText, infoTextFont, brush, infoTextPos);

            #endregion


            path.Dispose();
            brush.Dispose();

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

            //Checar se o mouse está em dentro do botão
            if (btnSendHovering && txtComando.ForeColor == Color.Black)
            {
                string command = Comandos.GetClosestMatch(txtComando.Text, Global.VoiceRecognizer.hints);
                PerformCommand(command, false);
            }

            #endregion
        }

        private void pnlBottom_Resize(object sender, EventArgs e)
        {
            #region AJUSTAR LABELS (O que você deseja fazer?)

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
                {
                    circleSizeTarget = circleSizeCurrent * 0.9f;
                    pnlBottom.Cursor = Cursors.Hand;
                }
            }

            #endregion


            #region BOTÃO DE ENVIAR COMANDO

            btnSendHovering = false;
            btnSendCurrentImage = btnSendImage;

            float txtIconX = txtAreaX + txtAreaWidth - txtIconMarginW - txtIconSize;
            float txtIconY = txtAreaY + txtIconMarginH;
            RectangleF rect = new RectangleF(txtIconX, txtIconY, txtIconSize, txtIconSize);

            //Checar se o mouse está em dentro do botão
            if (Global.InsideRectangle(mousePos, rect) == true)
            {
                btnSendHovering = true;
                btnSendCurrentImage = btnSendImageHover;
                pnlBottom.Cursor = Cursors.Hand;
            }

            #endregion
        }


        //Eventos da textbox
        private void txtComando_Click(object sender, EventArgs e)
        {
            if (textboxCanClick == false)
            {
                ActiveControl = imgLogo;
                return;
            }

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

        private void txtComando_Enter(object sender, EventArgs e)
        {
            //Desfoca a textbox se ela não puder ser clicada
            if (textboxCanClick == false)
            {
                ActiveControl = imgLogo;
            }
        }

        private void txtComando_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Remove o texto temporário assim que o usuário escrever
            if (txtComando.ForeColor != Color.Black)
            {
                TextBoxWrite("");
            }
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
                    if (isTranscribingAudio) normalizedDecibels = 0;    //Retomar tamanho ao terminar fala

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
            //pnlBottom.Invalidate(new Rectangle((int)circleX, (int)circleY, (int)circleSize+ 20, (int)circleSize + 20));
            pnlBottom.Invalidate();
        }


        //Timer do comando de voz
        private void audioTimer_Tick(object? sender, EventArgs e)
        {
            audioSeconds += 1;
        }

        //Timer da informação do comando
        private void infoTimer_Tick(object? sender, EventArgs e)
        {
            infoTimerCurrentMs -= infoTimer.Interval;   //Diminuir tempo
            infoTimerCurrentMs = Math.Max(infoTimerCurrentMs, 0);

            //Definir opacidade do texto
            float fadeOutMs = 300;
            float alpha = infoTextAlpha;

            if (infoTimerCurrentMs <= fadeOutMs)
            {
                alpha = (float)infoTimerCurrentMs / fadeOutMs * 255f;
            }

            infoTextAlpha = (int)Math.Ceiling(alpha);

            //Parar timer quando alpha for 0
            if (infoTimerCurrentMs == 0)
            {
                infoTimer.Stop();
            }
        }


        //Acontece quando há silêncio por determinado tempo no comando de voz
        private async void OnSilence(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => StopListening()));
        }


        //Botão do manual
        private void btnManual_Paint(object sender, PaintEventArgs e)
        {
            int yy = (btnManual.Height - btnManual.Width) / 2;

            //Sombra
            e.Graphics.DrawImage(btnManualShadow, new Rectangle(0, yy + 3, btnManual.Width, btnManual.Width));

            //Imagem normal
            e.Graphics.DrawImage(btnManual.Image, new Rectangle(0, yy, btnManual.Width, btnManual.Width));
        }

        private void btnManual_Click(object sender, EventArgs e) => loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlManual));
        private void btnManual_MouseEnter(object sender, EventArgs e)
        {
            btnManual.Image = btnManualImageHover;

            //Nome do botão
            lblButtonName.Text = "Manual";

            Global.LabelFitWidth(lblButtonName);
            int btnX = btnManual.Location.X + btnManual.Width / 2 - lblButtonName.Width / 2;
            int btnY = btnManual.Location.Y + btnManual.Height;
            lblButtonName.Location = new Point(btnX, btnY);

            lblButtonName.Visible = true;
            lblButtonName.Enabled = true;
        }
        private void btnManual_MouseLeave(object sender, EventArgs e)
        {
            btnManual.Image = btnManualImage;
            lblButtonName.Visible = false;
            lblButtonName.Enabled = false;
        }


        //Botão das configurações
        private void btnConfigs_Paint(object sender, PaintEventArgs e)
        {
            int yy = (btnConfigs.Height - btnConfigs.Width) / 2;

            //Sombra
            e.Graphics.DrawImage(btnConfigsShadow, new Rectangle(0, yy + 3, btnConfigs.Width, btnConfigs.Width));

            //Imagem normal
            e.Graphics.DrawImage(btnConfigs.Image, new Rectangle(0, yy, btnConfigs.Width, btnConfigs.Width));
        }

        private void btnConfigs_Click(object sender, EventArgs e) => loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlSettings));
        private void btnConfigs_MouseEnter(object sender, EventArgs e)
        {
            btnConfigs.Image = btnConfigsImageHover;

            //Nome do botão
            lblButtonName.Text = "Configurações";

            Global.LabelFitWidth(lblButtonName);
            int btnX = btnConfigs.Location.X + btnConfigs.Width / 2 - lblButtonName.Width / 2;
            int btnY = btnConfigs.Location.Y + btnConfigs.Height;
            lblButtonName.Location = new Point(btnX, btnY);

            lblButtonName.Visible = true;
            lblButtonName.Enabled = true;
        }
        private void btnConfigs_MouseLeave(object sender, EventArgs e)
        {
            btnConfigs.Image = btnConfigsImage;
            lblButtonName.Visible = false;
            lblButtonName.Enabled = false;
        }


        //Botão do histórico
        private bool historyOpen = false;

        public bool HistoryOpen
        {
            get { return historyOpen; }
            set { historyOpen = value; } 
        }

        private void btnHistory_Paint(object sender, PaintEventArgs e)
        {
            int yy = (btnHistory.Height - btnHistory.Width) / 2;

            //Sombra
            e.Graphics.DrawImage(btnHistoryShadow, new Rectangle(0, yy + 3, btnHistory.Width, btnHistory.Width));

            //Imagem normal
            e.Graphics.DrawImage(btnHistory.Image, new Rectangle(0, yy, btnHistory.Width, btnHistory.Width));
        }

        private void btnHistory_Click(object sender, EventArgs e)
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

        private void btnHistory_MouseEnter(object sender, EventArgs e)
        {
            btnHistory.Image = btnHistoryImageHover;

            //Nome do botão
            lblButtonName.Text = "Histórico";

            Global.LabelFitWidth(lblButtonName);
            int btnX = btnHistory.Location.X + btnHistory.Width / 2 - lblButtonName.Width / 2;
            int btnY = btnHistory.Location.Y + btnHistory.Height;
            lblButtonName.Location = new Point(btnX, btnY);

            lblButtonName.Visible = true;
            lblButtonName.Enabled = true;
        }

        private void btnHistory_MouseLeave(object sender, EventArgs e)
        {
            btnHistory.Image = btnHistoryImage;
            lblButtonName.Visible = false;
            lblButtonName.Enabled = false;
        }


        //Botão de pausar comando de voz
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

        private void btnPause_MouseEnter(object sender, EventArgs e)
        {
            btnPause.Image = btnPauseImageHover;

            if (Global.VoiceRecognizer.isPaused)
                btnPause.Image = btnPlayImageHover;
        }

        private void btnPause_MouseLeave(object sender, EventArgs e)
        {
            btnPause.Image = btnPauseImage;

            if (Global.VoiceRecognizer.isPaused)
                btnPause.Image = btnPlayImage;
        }


        //Botão de parar comando de voz
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopListening();
        }

        private void btnStop_MouseEnter(object sender, EventArgs e) => btnStop.Image = btnStopImageHover;
        private void btnStop_MouseLeave(object sender, EventArgs e) => btnStop.Image = btnStopImage;
    }
}
