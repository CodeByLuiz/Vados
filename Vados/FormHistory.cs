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
        private List<HistoryEntry> entradas = new List<HistoryEntry>();
        private System.Windows.Forms.Timer timer;
        private Label title;



        private int margin = 10;
        private int rectangleHeight = 115;
        private int rectangleWidth;
        private int spacing;
        private int startY;
        private Color entryColor = Global.ChangeColorBrightness(ColorTranslator.FromHtml("#F0F5FF"), -0.1f);




        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public FormHistory()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            this.Paint += new PaintEventHandler(FormHistory_Paint);
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

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


          
            
            this.Controls.Add(historyPanel);
            this.Controls.Add(title);
            
            title.BringToFront();

            rectangleWidth = historyPanel.ClientSize.Width - 35;





            PositionFix();
            //MessageBox.Show("form: " + this.Height.ToString() + " panel: " + historyPanel.Height.ToString());

            // Timer pra repintar 
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16;
            timer.Tick += Timer_Tick;

            this.Resize += FormHistory_Resize_1;

            LoadCommands();
        }


        private void PositionFix()
        {

            //historyPanel.SuspendLayout();



            //tamanho

            
            spacing = ((historyPanel.ClientSize.Width - rectangleWidth) / 2);
            startY = spacing;

            historyPanel.Height = this.Height - (title.Location.Y + title.Height)-spacing;
            

            //posição
            title.Location = new Point((historyPanel.Width / 2) - (title.Width / 2), startY);
            historyPanel.Top = title.Bottom;
            

            //MessageBox.Show($"form {this.Height.ToString()} {this.Width.ToString()} \npanel {historyPanel.Height.ToString()} {historyPanel.Width.ToString()}");
            //historyPanel.ResumeLayout();
        }

        private void LoadCommands()
        {
            

            historyPanel.Controls.Clear(); 

            using (var db = new BancoDeDados.DbConnection())
            {
                db.Database.EnsureCreated();
                entradas = db.Historico
                             .OrderByDescending(e => e.Data)
                             .ToList();
            }

            startY = spacing;
            foreach (var entry in entradas)
            {
                int posX = (historyPanel.ClientSize.Width - rectangleWidth) / 2;
                string HistoryTitle = char.ToUpper(entry.Comandotitle[0]) + entry.Comandotitle.Substring(1).ToLower(); // titulo com a primeira letra maiuscula

                OptmizedPanel entryPanel = new OptmizedPanel
                {
                    Location = new Point(posX, startY),

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
                    Location = new Point(lbltitle.Location.X + 2, lbltitle.Location.Y + lbltitle.Height + 2),
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

                
                //ajusta a posição dos elementos necessarios
                btnEditar.Location = new Point(10,entryPanel.Height-btnEditar.Height-5);
                btnEditar.BringToFront();

                btnExcluir.Location = new Point(btnEditar.Location.X +btnExcluir.Width+15, btnEditar.Location.Y);
                btnExcluir.BringToFront();

                lblData.Location = new Point(entryPanel.Width - lblData.Width - 5, lbltitle.Location.Y);


                //ajusta o tamanho de elementos necessarios
                lbl.Width = rectangleWidth - lbl.Location.X - 10;
                lbl.Height =rectangleHeight-( rectangleHeight-btnEditar.Location.Y) - (lbltitle.Location.Y+lbltitle.Height);

                // Faz o hover bonito
                void HoverEnter(object sender, EventArgs e) 
                {
                   
                    entryPanel.BackColor = Global.ChangeColorBrightness(entryColor,-0.1f);
                   // lbl.ForeColor = Color.Black; 
                    
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
                    Global.userControlHome.HistoryOpen = false;
                    this.Close();
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

                startY += rectangleHeight + margin;

            }


            PositionFix();
        }


        private void FormHistory_Resize_1(object sender, EventArgs e)
        {
            int roundValue = (int)(0.1 * Width);
            PositionFix();
            //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, roundValue, roundValue));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            PositionFix();
            historyPanel.Invalidate();
        }
        private GraphicsPath RectArc(Rectangle rect, int raio) // deixa a borda arredondada
        {
            GraphicsPath path = new GraphicsPath();

            int diametro = raio * 2;

            
            path.AddArc(rect.X, rect.Y, diametro, diametro, 180, 90);
            path.AddArc(rect.Right - diametro, rect.Y, diametro, diametro, 270, 90);
            path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diametro, diametro, diametro, 90, 90); 

            path.CloseFigure();
            return path;
        }

        private void FormHistory_Paint(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);
            int raio = 20;

            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);


           Color cor = Color.White;

            using (GraphicsPath path = RectArc(rect, raio))
            {

                using (Brush brush = new SolidBrush(cor)) // preenchimento
                {
                    g.FillPath(brush, path);
                }

                
                using (Pen pen = new Pen(cor, 2)) // borda coisada
                {
                    g.DrawPath(pen, path);
                }
            }
        }
    }
}
