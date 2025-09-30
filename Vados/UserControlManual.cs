using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vados
{
    public partial class UserControlManual : UserControl
    {
        private Button selectedButton = null;
        private Panel panelNav;

        public UserControlManual()
        {
            InitializeComponent();
            SetupNavBar();
        }


        public class RoundedButton : Button
        {
            public int BorderRadius { get; set; } = 20;

            public RoundedButton()
            {
                
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
                this.FlatStyle = FlatStyle.Flat;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

               
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                using (GraphicsPath path = GetRoundedPath(rect, BorderRadius))
                {
                    this.Region = new Region(path);

                    using (SolidBrush brush = new SolidBrush(this.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }

                
                Rectangle textRect = new Rectangle(
                    this.Padding.Left,
                    this.Padding.Top,
                    Math.Max(0, this.Width - this.Padding.Left - this.Padding.Right),
                    Math.Max(0, this.Height - this.Padding.Top - this.Padding.Bottom)
                );

               
                TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
                switch (this.TextAlign)
                {
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.BottomLeft:
                        flags |= TextFormatFlags.Left;
                        break;
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.BottomCenter:
                        flags |= TextFormatFlags.HorizontalCenter;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.TopRight:
                    case ContentAlignment.BottomRight:
                        flags |= TextFormatFlags.Right;
                        break;
                    default:
                        flags |= TextFormatFlags.Left;
                        break;
                }

                // Desenhar o texto usando o rect que considera padding
                TextRenderer.DrawText(e.Graphics, this.Text, this.Font, textRect, this.ForeColor, flags);
            }

            private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();
                return path;
            }
        }


        private void SetupNavBar()
        {
            panelNav = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(48, 61, 99),
                AutoScroll = true
            };
            this.Controls.Add(panelNav);

            //panel que organiza os bagulho dentro do panel nav
            FlowLayoutPanel flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };
            panelNav.Controls.Add(flow);

            // Título
            Label lblTitle = new Label
            {
                Text = "Comandos",
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Height = 50,
                Width = flow.Width,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 10, 0, 10)
            };
            flow.Controls.Add(lblTitle);

            // Categorias
            AddSection(flow, "Pastas ", new[] { "Criar uma pasta", "Abrir uma pasta" });
            AddSection(flow, "Arquivos ", new[] { "Criar um arquivo", "Abrir um arquivo" });
            AddSection(flow, "Sistema ", new[] { "Mudar Data", "Mudar Idioma" });
        }



        private void AddSection(FlowLayoutPanel flow, string sectionTitle, string[] commands)
        {
            Label lblSection = new Label
            {
                Text = sectionTitle,
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Height = 25,
                Width = flow.Width - 20,
                Margin = new Padding(10, 10, 10, 5)
            };
            flow.Controls.Add(lblSection);

            foreach (var cmd in commands)
            {
                RoundedButton btn = new RoundedButton
                {
                    Text = cmd,
                    Height = 25,
                    Width = flow.Width - 20,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(48, 61, 99),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9),
                    Padding = new Padding(15, 0, 0, 0),
                    Margin = new Padding(10, 3, 10, 3) 
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += NavButton_Click;
                flow.Controls.Add(btn);
            }
        }



        private void NavButton_Click(object sender, EventArgs e)
        {

            foreach (var btn in panelNav.Controls.OfType<Button>())
            {
                btn.Font = new Font(btn.Font, FontStyle.Regular);
            }// tira a merda do negrito dos outros botoes pra colocar depois apenas no selecionado

            if (selectedButton != null)

                selectedButton.BackColor = Color.FromArgb(48, 61, 99);

            selectedButton = sender as Button;
            selectedButton.BackColor = Color.FromArgb(82, 99, 152);
            selectedButton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

    }
}
