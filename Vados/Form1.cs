namespace Vados
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
    }
}
