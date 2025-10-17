using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static Vados.BancoDeDados;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Vados
{
    public partial class FormHistory : Form
    {
        private OptmizedPanel historyPanel;
       // private List<HistoryEntry> entradas = new List<HistoryEntry>();
        private System.Windows.Forms.Timer timer;
        private Label title;
       



        private int rectangleSideMargin = 17;
        private int rectangleBottomMargin = 15;
        private int rectangleHeight = 115;
        private int rectangleWidth;
        private int spacing;
        private int startY;
        private Color entryColor = Global.ChangeColorBrightness(ColorTranslator.FromHtml("#F0F5FF"), -0.1f);
        private Form1 parentForm;
        private int scrollBarWidth = 5;



        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public FormHistory(Form1 parent)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            this.Paint += new PaintEventHandler(FormHistory_Paint);

            LostFocus += FormHistory_LostFocus;
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            parentForm = parent;

            title = new Label
            {
                Text = "Histórico de comandos",
                AutoSize = true,

                TextAlign = ContentAlignment.MiddleCenter,
                 BackColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Colors.bluePrimary,



            };

            historyPanel = new OptmizedPanel
            {
                //Dock = DockStyle.Fill,
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height ),
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.White
                
                

            };

            PictureBox btnExit = new PictureBox
            {
                Size = new Size(25, 25),
                Cursor = Cursors.Hand,
                BackColor= Color.White,
                BackgroundImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\closeicon.png")),
                BackgroundImageLayout = ImageLayout.Zoom,


            };


          
            
            this.Controls.Add(historyPanel);
            this.Controls.Add(title);
            this.Controls.Add(btnExit);
            
            //ajustes de posicionamento e tamanho que só podem ser feitos após o negocio ja estar criado no form 😭
            title.BringToFront();
            
            rectangleWidth = historyPanel.ClientSize.Width - rectangleSideMargin * 2;// - scrollBarWidth;
            btnExit.Location = new Point((this.Width - 20) - (btnExit.Width), 20);
            
            void ExitForm(object sender, EventArgs e) 
            {
                parentForm.CloseHistoryTab();
            }

            btnExit.Click += ExitForm;
            this.Resize += FormHistory_Resize_1;
        }


        public void FormHistory_Load(object sender, EventArgs e)
        {
            LoadCommands();
            
        }


        private void PositionFix()
        {
            if (historyPanel == null)
                return;

            
            //spacing = ((historyPanel.ClientSize.Width - rectangleWidth) / 2);
            spacing = 20;
            historyPanel.Height = this.Height - (title.Location.Y + title.Height) - spacing * 2;


            //posição
            title.Left = (historyPanel.Width / 2) - (title.Width / 2);
            title.Top = spacing;
            historyPanel.Top = title.Bottom + spacing;

            entriesHVerification();
        }

        private void LoadCommands()
        {


            historyPanel.Controls.Clear();

            Global.InitializeDb();


            startY = 0;
            int lastDrawnY = 0;



            foreach (var entry in Global.entradas)
            {
                string HistoryTitle = char.ToUpper(entry.Comandotitle[0]) + entry.Comandotitle.Substring(1).ToLower(); // titulo com a primeira letra maiuscula

                OptmizedPanel entryPanel = new OptmizedPanel
                {
                    Location = new Point(rectangleSideMargin, startY),

                    Size = new Size(rectangleWidth, rectangleHeight),
                    BackColor = entryColor,
                    BorderStyle = BorderStyle.None,
                    Padding = new Padding(5)




                };

                Label lbltitle = new Label
                {
                    Text = $"{HistoryTitle}",
                    Location = new Point(10, 5),
                    AutoSize = true,

                    ForeColor = Color.Black,
                    Font = new Font("Arial", 14, FontStyle.Bold),



                };
                Label lbl = new Label
                {
                    Text = $"{entry.Comando}",
                    // AutoSize = false,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 12, FontStyle.Regular),
                    //BackColor = Color.Red



                };

                Label lblData = new Label
                {
                    Text = $"{entry.Data.ToString("g", CultureInfo.CurrentCulture)}",
                    AutoSize = true,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 12, FontStyle.Italic),
                };

                PictureBox btnExcluir = new PictureBox
                {

                    //BackColor = Color.White,
                    //BackgroundImage = Image.FromFile("\\Vados\\Vados\\Images\\Icons\\deleteicon")
                    Size = new Size(25, 25),
                    Cursor = Cursors.Hand,
                    BackgroundImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\deleteicon.png")),
                    BackgroundImageLayout = ImageLayout.Stretch

                };

                PictureBox btnEditar = new PictureBox
                {

                    //BackColor = Color.White,
                    //BackgroundImage = Image.FromFile("\\Vados\\Vados\\Images\\Icons\\deleteicon")
                    Size = new Size(25, 25),
                    Cursor = Cursors.Hand,
                    BackgroundImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\editicon.png")),
                    BackgroundImageLayout = ImageLayout.Stretch

                };

                //arredonda as bordas do painel

                int radius = 15;
                entryPanel.Region = Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, entryPanel.Width, entryPanel.Height, radius, radius)
                );



                historyPanel.Controls.Add(entryPanel);
                entryPanel.Controls.Add(lbl);
                entryPanel.Controls.Add(lbltitle);
                entryPanel.Controls.Add(lblData);
                entryPanel.Controls.Add(btnEditar);
                entryPanel.Controls.Add(btnExcluir);


                // Faz o hover bonito
                void HoverEnter(object sender, EventArgs e)
                {

                    entryPanel.BackColor = Global.ChangeColorBrightness(entryColor, -0.1f);

                }
                void HoverLeave(object sender, EventArgs e)
                {
                    entryPanel.BackColor = entryColor;
                    //lbl.ForeColor = Colors.bluePrimary;
                }

                void DeleteHoverEnter(object sender, EventArgs e)
                {
                    btnExcluir.BackgroundImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\deleteiconhover.png"));
                }

                void DeleteHoverLeave(object sender, EventArgs e)
                {
                    btnExcluir.BackgroundImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\deleteicon.png"));
                }

                void EditHoverEnter(object sender, EventArgs e)
                {
                    btnEditar.BackgroundImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\editiconhover.png"));

                }

                void EditHoverLeave(object sender, EventArgs e)
                {
                    btnEditar.BackgroundImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\Icons\editicon.png"));
                }

                void DeleteEntry(object sender, EventArgs e)
                {
                    BancoDeDados.DeleteEntryID(entry.Id);
                    LoadCommands();
                }
                void EditEntry(object sender, EventArgs e)
                {
                    Global.userControlHome.TxtComandoEditar = entry.Comando;

                    parentForm.CloseHistoryTab();
                }
                void ResizeEntryPanel(object sender, EventArgs e)
                {
                    entryPositionFix(btnEditar, btnExcluir, lblData, lbltitle,lbl, entryPanel);
                }

                // Adiciona os eventos aos elementos
                entryPanel.MouseEnter += HoverEnter;
                entryPanel.MouseLeave += HoverLeave;
                lbl.MouseEnter += HoverEnter;
                lbl.MouseLeave += HoverLeave;
                lbltitle.MouseEnter += HoverEnter;
                lbltitle.MouseLeave += HoverLeave;
                lblData.MouseEnter += HoverEnter;
                lblData.MouseLeave += HoverLeave;

                btnExcluir.MouseEnter += HoverEnter;
                btnExcluir.MouseLeave += HoverLeave;
                btnEditar.MouseEnter += HoverEnter;
                btnEditar.MouseLeave += HoverLeave;

                btnExcluir.MouseEnter += DeleteHoverEnter;
                btnExcluir.MouseLeave += DeleteHoverLeave;
                btnEditar.MouseEnter += EditHoverEnter;
                btnEditar.MouseLeave += EditHoverLeave;

                btnExcluir.Click += DeleteEntry;
                btnEditar.Click += EditEntry;

                entryPanel.Resize += ResizeEntryPanel;

                startY += rectangleHeight + rectangleBottomMargin;
                lastDrawnY = entryPanel.Bottom + rectangleBottomMargin;
                entryPositionFix(btnEditar, btnExcluir, lblData, lbltitle, lbl, entryPanel);
            }
            //entriesHVerification();
        }

        void entryPositionFix(PictureBox btnEditar, PictureBox btnExcluir, Label lblData, Label lbltitle,Label lbl, Panel entryPanel)
        {
            //ajusta a posição dos elementos necessarios
            btnEditar.Location = new Point(10, entryPanel.Height - btnEditar.Height - 5);
            btnEditar.BringToFront();

            btnExcluir.Location = new Point(btnEditar.Location.X + btnExcluir.Width + 15, btnEditar.Location.Y);
            btnExcluir.BringToFront();

            lblData.Location = new Point(entryPanel.Width - lblData.Width - 5, lbltitle.Location.Y);

            lbl.Location = new Point(lbltitle.Location.X + 2, lbltitle.Location.Y + lbltitle.Height + 2);
            lbl.Width = rectangleWidth - lbl.Location.X - 10;
            lbl.Height = rectangleHeight - (rectangleHeight - btnEditar.Location.Y) - (lbltitle.Location.Y + lbltitle.Height);
        }


        bool trecoDeControle = false;
        private void entriesHVerification()
        {

            int entradaHeight = -rectangleBottomMargin;
            int visibleFormHeight = historyPanel.Height;
            foreach(var entry in Global.entradas)
            {
                entradaHeight+=rectangleHeight + rectangleBottomMargin;
            }

            //MessageBox.Show($"entradas: {entradaHeight} visivel: {visibleFormHeight} ");
            
            if (entradaHeight > visibleFormHeight && !trecoDeControle)
            {

                rectangleWidth = historyPanel.ClientSize.Width - rectangleSideMargin * 2 - scrollBarWidth;
                LoadCommands();
                trecoDeControle = true;
            }
            else if (entradaHeight <= visibleFormHeight && trecoDeControle )
            {
                //MessageBox.Show("cu");
                rectangleWidth = historyPanel.ClientSize.Width - rectangleSideMargin * 2;
                LoadCommands();
                trecoDeControle = false;
            }

        }

        private void FormHistory_Resize_1(object sender, EventArgs e)
        {
            int roundValue = (int)(0.1 * Width);
            PositionFix();
            //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, roundValue, roundValue));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            historyPanel.Invalidate();
        }

        private void FormHistory_Paint(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);
            int raio = 20;

            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);


            //Desenhar fundo com bordas arredondadas
            Brush brush = new SolidBrush(Color.White);
            GraphicsPath area = Global.RoundedRectangle(new RectangleF(0, 0, Width, Height), 0.05f * Width);
            e.Graphics.FillPath(brush, area);
        }

        private void FormHistory_LostFocus(object sender, EventArgs e)
        {
            //parentForm.CloseHistoryTab();
        }
    }
}
