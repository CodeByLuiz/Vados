using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static Vados.BancoDeDados;

namespace Vados
{
    public partial class FormHistory : Form
    {

        public UserControl userControl;
        System.Windows.Forms.Timer timer;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
       (
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


            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16; //~60 FPS
            timer.Tick += Timer_Tick;
            timer.Start();


            //Otimizar pintura
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            this.Text = "Histórico de Comandos";
            this.Size = new Size(500, 700);
            LoadCommands();
        }

        private const int MaxItemsPerPage = 10;  // Número máximo de comandos por página
        private int currentPage = 0;  // Página atual
        private List<FlowLayoutPanel> pages = new List<FlowLayoutPanel>();  // Armazenamento das "páginas"

        private List<HistoryEntry> entradas = new List<HistoryEntry>(); // Armazena as "tabelas" do banco de dados

        private List<Label> labels = new List<Label>();

        private void LoadCommands()
        {
            pages.Clear();  // Limpa as páginas existentes
            currentPage = 0;  // Começa na primeira página
            AddNewPage();  // Cria o primeiro painel

            // Carrega os comandos do banco de dados com paginação
            entradas = GetCommands(currentPage, MaxItemsPerPage);

            foreach (var command in entradas)
            {
                AddCommandToPage(command);
            }

            
        }

        private List<HistoryEntry> GetCommands(int pageNumber, int pageSize)
        {
            using (var db = new BancoDeDados.DbConnection())
            {
                db.Database.EnsureCreated();
                return db.Historico
                    .OrderBy(e => e.Data)
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        private void AddNewPage()
        {
            var newPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoScroll = true,
                Height = this.ClientSize.Height / 2,  // Ajuste dinâmico baseado no tamanho do formulário
                Padding = new Padding(10)
            };

            pages.Add(newPanel);
            this.Controls.Add(newPanel);
        }

        private void AddCommandToPage(HistoryEntry command)
        {
            
            var commandLabel = new Label
            {
                
                Text = $"{command.Data}: {command.Comando}",
                AutoSize = true

            };
            labels.Add(commandLabel);

            var currentPanel = pages[currentPage];
            currentPanel.Controls.Add(commandLabel);

            // Verifica se a página atual já ultrapassou o limite de comandos
            if (currentPanel.Controls.Count >= MaxItemsPerPage)
            {
                currentPage++;
                AddNewPage();
            }

            // Adiciona as pastas como labels, se necessário
            //foreach (var pasta in command.Pastas)
            //{
            //    var pastaLabel = new Label
            //    {
            //        Text = $"Pasta: {pasta}",
            //        AutoSize = true
            //    };
            //    currentPanel.Controls.Add(pastaLabel);
            //}
        }
        private void FormHistory_Resize_1(object sender, EventArgs e)
        {
            int roundValue = (int)(0.1 * Width);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, roundValue, roundValue));
        }

        private void FormHistory_Paint(object sender, PaintEventArgs e)
        {
            //MessageBox.Show(entradas.Count.ToString());

            Brush brush = new SolidBrush(Color.Red);
            Rectangle rect = new Rectangle(10, 10, 100, 100);
            e.Graphics.FillRectangle(brush, rect);

            for (int i = 0; i < entradas.Count; i++)
            {
                //AddCommandToPage();

                int margin = 10;
                int startY = 5;
                int startX = 10;
                int rectangleHeight = 50;
                int rectangleWidth = 100;


                int top = startY + (rectangleHeight + margin) * i;
                labels[i].Top = top + 8;

                //Brush brush = new SolidBrush(Color.Gray);
                //Rectangle rect = new Rectangle(startX, top, rectangleWidth, rectangleHeight);
                //e.Graphics.FillRectangle(brush, rect);
            }
        }


        private void Timer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
