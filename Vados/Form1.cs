using System.Runtime.InteropServices.Marshalling;

namespace Vados
{
    public partial class Form1 : Form
    {
        private bool isFullscreen = false;
        private FormWindowState lastWindowState;
        private FormBorderStyle lastBorderStyle;
        private Rectangle lastBounds;

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
        }

        //Função para trocar user control
        public void LoadUserControl(UserControl userControl)
        {
            panelContainer.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(userControl);
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
                    lastBounds = Screen.FromControl(this).Bounds;
                    lastWindowState = WindowState;
                    lastBorderStyle = FormBorderStyle;

                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;

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
    }
}
