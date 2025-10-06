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
            overlayForm.GotFocus += overlayForm_GotFocus;
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
        public void ShowPopupMessage(bool isErrorMessage, Form form, UserControl userControl, CommandCriteria commandCriteria = null, string messageRtf = "",string Comandotxt="")
        {
            ToggleOverlay(true);

            var message = new FormMessage(commandCriteria, isErrorMessage, messageRtf,txtbd:Comandotxt);
            message.Owner = form;
            message.userControl = userControl;
            message.Show();
            CorrectMessageForm();
        }


        public FormMessage FindMessageForm()
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is FormMessage)
                {
                    return (FormMessage)openForm;
                }
            }

            return null;
        }


        public FormHistory FindHistoryForm()
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is FormHistory)
                {
                    return (FormHistory)openForm;
                }
            }

            return null;
        }

        public void CorrectMessageForm()
        {
            FormMessage messageForm = FindMessageForm();
            if (messageForm == null) return;    //Parar se não encontrar form

            //Corrigir posição
            int newX = Width / 2 - messageForm.Width / 2;
            int newY = Height / 2 - messageForm.Height / 2;
            messageForm.Location =  PointToScreen(new Point(newX, newY));
        }

         

        public void ShowHistoryTab(Form form, UserControl userControl)
        {
            ToggleOverlay(true);

            var history = new FormHistory();
            history.Owner = form;
            //history.userControl= userControl;
            history.Show();
            history.Focus();
            CorrectHistoryForm();
            Global.userControlHome.HistoryOpen = true;

        }
        public void CloseHistoryTab()
        {
            FormHistory historyForm = FindHistoryForm();

            if (historyForm != null)
            {
                historyForm.Close();
                Global.userControlHome.HistoryOpen = false;
            }
        }
        public void CorrectHistoryForm()
        {
            FormHistory historyForm = FindHistoryForm();

            if (historyForm == null) return;


            int historyRightMargin = 20;
            int historyTopMargin = 110;
            int historyBottomMargin = 20;

            int posX = ClientRectangle.Right - historyRightMargin - historyForm.Width;  
            int posY = historyTopMargin;

            historyForm.Location = this.PointToScreen(new Point(posX, posY));
            historyForm.Height = ClientRectangle.Height - historyBottomMargin - historyTopMargin;
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

            var parentForm = FindForm() as Form1;
            parentForm.CloseHistoryTab();
            

            LoadUserControl(e.userControl);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Initialize user controls
            Global.userControlHome = new UserControlHome();
            Global.userControlSettings = new UserControlSettings();
            Global.userControlManual = new UserControlManual();


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
            CorrectHistoryForm();
            CorrectMessageForm();
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            //Corrigir posição da tela preta
            Rectangle clientRect = RectangleToScreen(this.ClientRectangle);
            overlayForm.Location = new Point(clientRect.Left, clientRect.Top);
            CorrectHistoryForm();
            CorrectMessageForm();
        }

        private void overlayForm_GotFocus(object sender, EventArgs e)
        {
            //Focar form da mensagem de confirmação
            Form form = FindMessageForm();

            //Focar form do histórico
            if (form == null)
            {
                form = FindHistoryForm();
                if (form == null) return;
            }

            form.BringToFront();
            form.Focus();
        }
    }
}
