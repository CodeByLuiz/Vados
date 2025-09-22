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
        private Panel scrollPanel;
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
            scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.White
            };
            this.Controls.Add(scrollPanel);

            // Timer opcional para repintura (caso precise de atualizações dinâmicas)
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16;
            timer.Tick += Timer_Tick;
            timer.Start();

            LoadCommands();
        }

        private void LoadCommands()
        {
            scrollPanel.Controls.Clear(); // Limpa comandos antigos

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
            int rectangleWidth = scrollPanel.ClientSize.Width - 40;

            foreach (var entry in entradas)
            {
                int posX = (scrollPanel.ClientSize.Width - rectangleWidth) / 2;


                Panel entryPanel = new Panel
                {
                    Location = new Point(posX, startY),

                    Size = new Size(rectangleWidth, rectangleHeight),
                    BackColor = Color.LightGray,
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lbl = new Label
                {
                    Text = $"{entry.Data:dd/MM/yyyy HH:mm:ss}: {entry.Comando}\n {entry.Id}",
                    Location = new Point(10, 15),
                    AutoSize = true
                };

                entryPanel.Controls.Add(lbl);
                scrollPanel.Controls.Add(entryPanel);

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
            scrollPanel.Invalidate(); // Força atualização visual se necessário
        }
    }
}