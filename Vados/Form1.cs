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
            overlayForm.StartPosition = FormStartPosition.Manual;
        }

        public void ToggleOverlay(bool visible)
        {
            if (visible)
            {
                overlayForm.Show();
                overlayForm.Bounds = this.RectangleToScreen(this.ClientRectangle);
            }
            else
            {
                overlayForm.Hide();
            }
        }

      

        //Função para trocar user control
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

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {
           
        }
    }
}
