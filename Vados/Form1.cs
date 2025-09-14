using System.Runtime.InteropServices.Marshalling;

namespace Vados
{
    public partial class Form1 : Form
    {
        Form overlayForm = new Form();

        private bool isFullscreen = false;
        private FormWindowState lastWindowState;
        private FormBorderStyle lastBorderStyle;
        private Rectangle lastBounds;
        private Rectangle lastMinimizedBounds;

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;

            //Otimizar pintura
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            //Criar form transparente para escurecer a tela quando preciso
            overlayForm.FormBorderStyle = FormBorderStyle.None;
            overlayForm.BackColor = Color.Black;
            overlayForm.Opacity = 0.25;
            overlayForm.ShowInTaskbar = false;
            overlayForm.Owner = this;
            overlayForm.StartPosition = FormStartPosition.Manual;

            //Definir variáveis da textbox que armazena a próxima mensagem de erro
            Global.nextErrorMessage.ForeColor = Color.Black;
            Global.nextErrorMessage.Font = new Font("Segoe UI", 11f, FontStyle.Regular);
        }

        public void ToggleOverlay(bool visible)
        {
            //Ativar tela escura
            if (visible)
            {
                overlayForm.Show();
                overlayForm.Bounds = this.RectangleToScreen(this.ClientRectangle);
            }
            //Desativar
            else
            {
                overlayForm.Hide();
            }
        }


        //Ativar mensagem
        public void ShowPopupMessage(bool isErrorMessage, Form form, UserControl userControl, CommandCriteria commandCriteria = null, string messageRtf = "")
        {
            ToggleOverlay(true);

            var message = new FormMessage(commandCriteria, isErrorMessage, messageRtf);
            message.Owner = form;
            message.userControl = userControl;
            message.Show();
        }


        public void CorrectMessageForm()
        {
            FormMessage messageForm = null;

            //Encontrar form
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is FormMessage)
                {
                    messageForm = (FormMessage)openForm;
                    break;
                }
            }

            if (messageForm == null) return;    //Parar se não encontrar form


            //Corrigir posição
            int newX = Width / 2 - messageForm.Width / 2;
            int newY = Height / 2 - messageForm.Height / 2;
            messageForm.Location =  PointToScreen(new Point(newX, newY));
        }


        //Trocar user control (página)
        public void LoadUserControl(UserControl userControl)
        {
            panelContainer.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(userControl);
            this.Focus();
        }


        //Trocar de página (user control) através dos eventos de outros user controls
        private void LoadPage(object sender, LoadPageEventArgs e)
        {
            LoadUserControl(e.userControl);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Carregar página inicial
            LoadUserControl(Global.userControlHome);

            //Eventos de mudar de página (pra cada user control)
            Global.userControlHome.loadPage += LoadPage;
            Global.userControlSettings.loadPage += LoadPage;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                //Ativar tela cheia
                if (isFullscreen == false)
                {
                    lastBounds = Bounds;
                    lastWindowState = WindowState;
                    lastBorderStyle = FormBorderStyle;

                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    Bounds = Screen.FromControl(this).Bounds;

                    isFullscreen = true;
                    return;
                }

                //Desativar tela cheia
                Bounds = lastBounds;
                WindowState = lastWindowState;
                FormBorderStyle = lastBorderStyle;

                isFullscreen = false;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //Corrigir tamanho da tela preta
            overlayForm.Bounds = RectangleToScreen(this.ClientRectangle);

            CorrectMessageForm();
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            //Corrigir posição da tela preta
            Rectangle clientRect = RectangleToScreen(this.ClientRectangle);
            overlayForm.Location = new Point(clientRect.Left, clientRect.Top);

            CorrectMessageForm();
        }
    }
}
