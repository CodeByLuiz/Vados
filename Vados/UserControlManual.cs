using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
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
        private FlowLayoutPanel flow;
        private Panel panelContent = null;


        



        public UserControlManual()
        {
            InitializeComponent();

            SetupContentArea();
            this.Controls.Add(panelContent);


            SetupNavBar();
            this.Controls.Add(panelNav);
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

        


        private void AddTitleToContent(string Title)
        {
            Label title = new Label
            {
                Text = Title,
                Font = new Font("Arial", 40),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Padding = new Padding(10),
                AutoSize = true,
            };
            panelContent.Controls.Add(title);
        }
        private void AddDescriptionToContent(string description)
        {
            Label descriptionLabel = new Label
            {
                Text = description,
                Font = new Font("Arial", 12),
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(10),
                AutoSize = false
            };
            panelContent.Controls.Add(descriptionLabel);
        }

       


        private void SetupNavBar()
        {
            panelNav = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.FromArgb(48, 61, 99),
               

            };
            this.Controls.Add(panelNav);


            //panel que organiza os bagulho dentro do panel nav
            flow = new FlowLayoutPanel
            {
               Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };
            panelNav.Controls.Add(flow);

            PictureBox pictureLogo = new PictureBox
            {
                Image = Image.FromFile("Images/LogoBranco.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 134,
                Height = 134,
                Margin = new Padding(0, 10, 0, 10),
                Anchor = AnchorStyles.None
            };
            flow.Controls.Add(pictureLogo);

            // Título
            Label lblTitle = new Label
            {
                Text = "Comandos",
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = new Font("Darker Grotesque", 22, FontStyle.Bold),
                Height = 50,
                Width = flow.Width,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 10, 0, 10)
            };
            lblTitle.Paint += DrawTitleLines;
            flow.Controls.Add(lblTitle);

            // Categorias
            AddSection(flow, "Pastas ", new[] { "Criar uma pasta", "Abrir uma pasta","Abrir pasta padrão","Renomear uma pasta","Excluir uma pasta","Mover uma pasta","Duplicar uma pasta" });
            AddSection(flow, "Arquivos ", new[] { "Criar um arquivo", "Abrir um arquivo","Renomear um arquivo","Excluir um arquivo","Mover um arquivo","Duplicar um arquivo","Operar múltiplos arquivos " });
            AddSection(flow, "Sistema ", new[] { "Abrir software", "Alterar volume", "Alterar horário","Alterar brilho da tela", "Alterar idioma" });
        }

        private void SetupContentArea()
        {
            panelContent = new Panel
            {
                
               Dock = DockStyle.Fill,
                BackColor = Color.Red
            };
            this.Controls.Add(panelContent);
        }

        private void AddSection(FlowLayoutPanel flow, string sectionTitle, string[] commands)
        {
            Label lblSection = new Label
            {
                Text = sectionTitle,
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = new Font("Darker Grotesque", 18, FontStyle.Bold),
                Height = 30,
                Width = flow.Width - 20,
                Margin = new Padding(10, 10, 10, 5)
            };
            flow.Controls.Add(lblSection);

            foreach (var cmd in commands)
            {
                RoundedButton btn = new RoundedButton
                {
                    Text = cmd,
                    Height = 30,
                    Width = flow.Width -20,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(48, 61, 99),
                    ForeColor = Color.White,
                    Font = new Font("Darker Grotesque", 16,FontStyle.Regular),
                    Padding = new Padding(15, 0, 15, 0),
                    Margin = new Padding(15, 3, 15, 3) 
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += NavButton_Click;
                flow.Controls.Add(btn);
            }
        }
        private void DrawTitleLines(object sender, PaintEventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl == null) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            
            using (Pen pen = new Pen(Color.FromArgb(200, 219, 236), 2)) 
            {
             
                SizeF textSize = e.Graphics.MeasureString(lbl.Text, lbl.Font);

                int textWidth = (int)textSize.Width;
                int textHeight = (int)textSize.Height;

                int centerY = lbl.Height / 2; 
                int lineY = centerY; 

                int padding = 5;
                int lineLength = (lbl.Width - textWidth) / 2 - padding;

                if (lineLength > 0)
                {
                    // Linha à esquerda
                    e.Graphics.DrawLine(pen, 20, lineY, lineLength, lineY);

                    // Linha à direita
                    e.Graphics.DrawLine(pen, lbl.Width - lineLength, lineY, lbl.Width - 20, lineY);
                }
            }
        }

        private void NavButton_Click(object sender, EventArgs e)
        {

            foreach (var btn in flow.Controls.OfType<Button>())
            {
                btn.Font = new Font(btn.Font, FontStyle.Regular);
            }// tira a merda do negrito dos outros botoes pra colocar depois apenas no selecionado

            if (selectedButton != null)

                selectedButton.BackColor = Color.FromArgb(48, 61, 99);

            selectedButton = sender as Button;
            selectedButton.BackColor = Color.FromArgb(82, 99, 152);
            selectedButton.Font = new Font("Darker Grotesque", 15, FontStyle.Bold);

            LoadContentBasedOnSelection(selectedButton.Text);

        }

        private void LoadContentBasedOnSelection(string buttonText)
        {
            panelContent.Controls.Clear(); // Limpa o conteúdo atual

            if (buttonText == "Criar uma pasta")
            {
                // AddImageToContent("caminho_da_imagem_da_pasta.png");
                AddDescriptionToContent("Para criar uma nova pasta, clique no botão 'Criar Pasta'...");
                AddTitleToContent("Criar Pasta");
                
            }
            
        }


    }
}
