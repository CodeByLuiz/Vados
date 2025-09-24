using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Vados.BancoDeDados;


namespace Vados
{
    public partial class FormHistory : Form
    {
        private Panel historyPanel;
        private List<HistoryEntry> entradas = new List<HistoryEntry>();
        private System.Windows.Forms.Timer timer;

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

            this.Text = "Histórico de Comandos";
            this.Size = new Size(500, 700);
            this.BackColor = Color.White;

            // Painel com rolagem automática
            historyPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.White
            };
            this.Controls.Add(historyPanel);

            // Timer pra repintar 
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16;
            timer.Tick += Timer_Tick;
            timer.Start();

            LoadCommands();
        }

        private void LoadCommands()
        {
            historyPanel.Controls.Clear(); // Limpa comandos antigos

            using (var db = new BancoDeDados.DbConnection())
            {
                db.Database.EnsureCreated();
                entradas = db.Historico
                             .OrderBy(e => e.Data)
                             .ToList();
            }

            int margin = 10;
            int startY = 10;
            int rectangleHeight = 75;
            int rectangleWidth = historyPanel.ClientSize.Width - 40;

           

            foreach (var entry in entradas)
            {
                int posX = (historyPanel.ClientSize.Width - rectangleWidth) / 2;


                Panel entryPanel = new Panel
                {
                    Location = new Point(posX, startY),

                    Size = new Size(rectangleWidth, rectangleHeight),
                    BackColor = Color.LightGray,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(5),  // Leve padding para melhorar o visual
                    Cursor = Cursors.Hand
                };

                Label lbl = new Label
                {
                    Text = $"{entry.Data:dd/MM/yyyy HH:mm:ss}: {entry.Comando}",
                    Location = new Point(10, 15),
                    AutoSize = true
                };
                Label lbl2 = new Label
                {
                    Text = $"id: {entry.Id}",
                    
                    AutoSize = true
                };
                
                historyPanel.Controls.Add(entryPanel);
                entryPanel.Controls.Add(lbl);

                

                // Faz o hover bonito
                void HoverEnter(object sender, EventArgs e) => entryPanel.BackColor = Color.SkyBlue;
                void HoverLeave(object sender, EventArgs e) => entryPanel.BackColor = Color.LightGray;

                // Adiciona os eventos tanto ao painel quanto a label
                entryPanel.MouseEnter += HoverEnter;
                entryPanel.MouseLeave += HoverLeave;
                lbl.MouseEnter += HoverEnter;
                lbl.MouseLeave += HoverLeave;


                startY += rectangleHeight + margin;
            }
        }
       

        private void FormHistory_Resize_1(object sender, EventArgs e)
        {
            int roundValue = (int)(0.1 * Width);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, roundValue, roundValue));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            historyPanel.Invalidate(); // Força atualização visual se necessário
        }
    }
}