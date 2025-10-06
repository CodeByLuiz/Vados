using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
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
        private Panel imageWrapper = null;
        private RichTextBox exampleRichTextBox = null;
        private TableLayoutPanel tableLayoutContent;
        private RichTextBox descriptionBox = null;
        private RichTextBox secundarydescriptionBox = null;

        public UserControlManual()
        {
            InitializeComponent();

            SetupContentArea();
            this.Controls.Add(panelContent);
            this.Resize += UserControlManual_Resize;

            SetupNavBar();
            this.Controls.Add(panelNav);

            LoadContentBasedOnSelection("Criar uma pasta");
        } 

        PrivateFontCollection pfc = new PrivateFontCollection();


        public class RoundedButton : Button
        {
            public int BorderRadius { get; set; } = 20;

            public RoundedButton()
            {
                
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
                this.FlatStyle = FlatStyle.Flat;
                this.SetStyle(ControlStyles.Selectable, false);
                this.FlatAppearance.MouseOverBackColor = Color.Transparent; 
                this.FlatAppearance.MouseDownBackColor = Color.Transparent; 
                this.TabStop = false;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
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


        private void UserControlManual_Resize(object sender, EventArgs e)
        {
            foreach (Control control in tableLayoutContent.Controls)
            {
                if (control is RichTextBox box && box.Tag?.ToString() == "descrição")
                {
                    ResizeDescriptionBox(box);
                }
            }
        }




        private void SetupContentArea()
        {
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(223, 223, 223)
            };

            tableLayoutContent = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                ColumnCount = 1,
                RowCount = 0,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20)
            };

            panelContent.Controls.Add(tableLayoutContent);
            this.Controls.Add(panelContent);
        }

        private void AddTitleToContent(string Title)
        {
            Label title = new Label
            {
                Text = Title,
                Font = Fonts.GetFont(Fonts.DarkerExtraBold, 42f),
                ForeColor = Color.FromArgb(48, 61, 99),
                Dock = DockStyle.Fill,
                Padding = new Padding(60, 20, 0, 0),
                AutoSize = true,
            };

          
            tableLayoutContent.RowCount++;
            tableLayoutContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutContent.Controls.Add(title, 0, tableLayoutContent.RowCount - 1);
        }
        private void AddDescriptionToContent()
        {
             descriptionBox = new RichTextBox
            {
                Text = null,
                Font = Fonts.GetFont(Fonts.DarkerRegular, 18f),
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(223, 223, 223),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.None,
                DetectUrls = false,
                TabStop = false,
                WordWrap = true,
                Dock = DockStyle.None,
                Margin = new Padding(65, 10, 40, 30), // margem externa
                Tag = "descrição"
            };
            Global.TextBoxFitHeight(exampleRichTextBox, 10);

            // Bloqueia seleção/foco
            descriptionBox.GotFocus += (s, e) => this.ActiveControl = null;
            descriptionBox.MouseDown += (s, e) => descriptionBox.SelectionLength = 0;
            descriptionBox.SelectionChanged += (s, e) => descriptionBox.SelectionLength = 0;

            // Adiciona margem interna para o texto
            descriptionBox.SelectionIndent = 0;           // recuo à esquerda
            descriptionBox.SelectionRightIndent = 20;    // recuo à direita

            ResizeDescriptionBox(descriptionBox);

            tableLayoutContent.RowCount++;
            tableLayoutContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutContent.Controls.Add(descriptionBox, 0, tableLayoutContent.RowCount - 1);
        }

        private void AddSecundaryDescriptionToContent()
        {
            secundarydescriptionBox = new RichTextBox
            {
                Text = null,
                Font = Fonts.GetFont(Fonts.DarkerRegular, 18f),
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(223, 223, 223),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.None,
                DetectUrls = false,
                TabStop = false,
                WordWrap = true,
                Dock = DockStyle.None,
                Margin = new Padding(65, 10, 40, 30), // margem externa
                Tag = "descrição"
            };


            Global.TextBoxFitHeight(secundarydescriptionBox, 10);

            // Bloqueia seleção/foco
            secundarydescriptionBox.GotFocus += (s, e) => this.ActiveControl = null;
            secundarydescriptionBox.MouseDown += (s, e) => secundarydescriptionBox.SelectionLength = 0;
            secundarydescriptionBox.SelectionChanged += (s, e) => secundarydescriptionBox.SelectionLength = 0;

            // Adiciona margem interna para o texto
            secundarydescriptionBox.SelectionIndent = 0;           // recuo à esquerda
            secundarydescriptionBox.SelectionRightIndent = 20;    // recuo à direita

            ResizeDescriptionBox(secundarydescriptionBox);

            tableLayoutContent.RowCount++;
            tableLayoutContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutContent.Controls.Add(secundarydescriptionBox, 0, tableLayoutContent.RowCount - 1);
        }



        private void ResizeDescriptionBox(RichTextBox box)
        {
            int horizontalPadding = box.Margin.Left + box.Margin.Right + 20; 
            int maxWidth = panelContent.Width - horizontalPadding;

            box.MaximumSize = new Size(maxWidth, 0);

            Size textSize = TextRenderer.MeasureText(
                box.Text,
                box.Font,
                new Size(maxWidth, int.MaxValue),
                TextFormatFlags.WordBreak
            );

            box.Width = maxWidth;
            box.Height = textSize.Height + 25;
        }



        private void AddImageToContent(string caminhoimagem)
        {
            if (System.IO.File.Exists(caminhoimagem))
            {
                Panel imageWrapper = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(0, 60, 0, 10),
                    Height = 400,
                    Width = 600,
                    AutoSize = false
                };

                PictureBox picture = new PictureBox
                {
                    Image = Image.FromFile(caminhoimagem),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill,
                };

                imageWrapper.Controls.Add(picture);

                tableLayoutContent.RowCount++;
                tableLayoutContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 400));
                tableLayoutContent.Controls.Add(imageWrapper, 0, tableLayoutContent.RowCount - 1);

                picture.BringToFront();
            }
        }

       
        
            private void AddExampleBox()
        {
          
            Label exampleLabel = new Label
            {
                Text = "Exemplos:",
                Font = Fonts.GetFont(Fonts.DarkerExtraBold, 20f),
                ForeColor = Color.FromArgb(48, 61, 99),
                AutoSize = true,
                TextAlign = ContentAlignment.TopLeft,
                Margin = new Padding(0, 0, 10, 0)
            };

            
            Panel examplePanel = new Panel
            {
                BackColor = Color.FromArgb(231, 231, 231),
                Width = 480,
                Height = 200,
                Margin = new Padding(5),
            };

            
             exampleRichTextBox = new RichTextBox
            {
                Font = Fonts.GetFont(Fonts.DarkerRegular, 16f),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = Color.FromArgb(231, 231, 231),
                TabStop = false,
                Cursor = Cursors.Arrow,
                Margin = new Padding(10),
                
                Width = examplePanel.Width - 100,
                Height = examplePanel.Height - 20,
            };

            exampleRichTextBox.GotFocus += (s, e) => this.ActiveControl = null;
            exampleRichTextBox.MouseDown += (s, e) => exampleRichTextBox.SelectionLength = 0;
            exampleRichTextBox.SelectionChanged += (s, e) => exampleRichTextBox.SelectionLength = 0;

            examplePanel.Controls.Add(exampleRichTextBox);

           
            FlowLayoutPanel container = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(60, 20, 0, 0),
                Padding = new Padding(0),
            };

           
            container.Controls.Add(exampleLabel);
            container.Controls.Add(examplePanel);

           
            tableLayoutContent.RowCount++;
            tableLayoutContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutContent.Controls.Add(container, 0, tableLayoutContent.RowCount - 1);
        }

       
        private void SetupNavBar()
        {
            panelNav = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(48, 61, 99),
               

            };
            this.Controls.Add(panelNav);


            //panel que organiza os bagulho dentro do panel nav
            flow = new FlowLayoutPanel
            {
               Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false,
              
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
                Font = Fonts.GetFont(Fonts.MavenMedium, 26f),
                Height = 50,
                Width = flow.Width,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 10, 0, 10)
            };
            
            lblTitle.Paint += DrawTitleLines;
            flow.Controls.Add(lblTitle);

            // Categorias
            AddSection(flow, "Pastas ", @"Images/Icons/pasta.png", new[] { "Criar uma pasta", "Abrir uma pasta","Abrir pasta padrão","Renomear uma pasta","Excluir uma pasta","Mover uma pasta","Duplicar uma pasta" });
            AddSection(flow, "Arquivos ", @"Images/Icons/Arquivos.png", new[] { "Criar um arquivo", "Abrir um arquivo","Renomear um arquivo","Excluir um arquivo","Mover um arquivo","Duplicar um arquivo","Operar múltiplos arquivos " });
            AddSection(flow, "Sistema ", @"Images/Icons/Sistema.png", new[] { "Abrir software", "Alterar volume", "Alterar horário","Alterar brilho da tela", "Alterar idioma" });
        }


        private void AddSection(FlowLayoutPanel flow, string sectionTitle, string iconPath, string[] commands)
        {
            
            FlowLayoutPanel sectionPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(10, 10, 10, 5),
                Padding = new Padding(0)
            };

            
            Label lblSection = new Label
            {
                Text = sectionTitle,
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = Fonts.GetFont(Fonts.DarkerExtraBold, 18f),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            sectionPanel.Controls.Add(lblSection);


            PictureBox icon = new PictureBox
            {
                Image = Image.FromFile(iconPath),
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 24,
                Height = 24,
                Margin = new Padding(0, 3, 5, 0) 
            };
            sectionPanel.Controls.Add(icon);

            // Adiciona ao FlowLayoutPanel principal
            flow.Controls.Add(sectionPanel);

            // Botões da seção
            foreach (var cmd in commands)
            {
                RoundedButton btn = new RoundedButton
                {
                    Text = cmd,
                    Height = 30,
                    Width = flow.Width - 20,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(48, 61, 99),
                    ForeColor = Color.White,
                    Font = Fonts.GetFont(Fonts.DarkerRegular, 16f),
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

                int padding = 1;
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
                btn.Font = Fonts.GetFont(Fonts.DarkerRegular, 16f);
            }// tira a merda do negrito dos outros botoes pra colocar depois apenas no selecionado

            if (selectedButton != null)

                selectedButton.BackColor = Color.FromArgb(48, 61, 99);

            selectedButton = sender as Button;
            selectedButton.BackColor = Color.FromArgb(82, 99, 152);
            selectedButton.Font = Fonts.GetFont(Fonts.DarkerExtraBold, 16f);

            LoadContentBasedOnSelection(selectedButton.Text);

        }

        


        private void LoadContentBasedOnSelection(string buttonText)
        {
          

        
            tableLayoutContent.Controls.Clear();
            tableLayoutContent.RowStyles.Clear();
            tableLayoutContent.RowCount = 0;

     
  


            switch (buttonText)
            {                   
                case "Criar uma pasta":
                    LoadCriarPastaContent();
                    break;
                case "Abrir uma pasta":
                    LoadAbrirPastaContent();
                    break;
                
            }
        }

        private void LoadCriarPastaContent()
        {
            AddTitleToContent("Criar Pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para criar uma pasta, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Criar Pasta",Colors.blueHighlight,FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". As pastas criadas são encontradas na pasta padrão do aplicativo, chamada \"Vados\". Caso uma pasta seja criada com o mesmo nome de outra já existente, seu nome terá um número na frente, de forma ascendente, para que possa ser distinguida.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular,20f));

            AddImageToContent(@"Images\imagem-nao-encontrada.jpg");

            AddSecundaryDescriptionToContent();
            Global.AppendPlainText(secundarydescriptionBox, "A execução desse comando depende de apenas um fator, o ");
            Global.AppendFormattedText(secundarydescriptionBox,"nome da pasta.",Colors.greenHighlight,FontStyle.Bold);
            secundarydescriptionBox.Rtf = Global.RtfChangeFont(secundarydescriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddExampleBox();
            
           
            Global.AppendFormattedText(exampleRichTextBox, "crie uma pasta", Color.Black, FontStyle.Underline);
            Global.AppendPlainText(exampleRichTextBox, " chamada ");
            Global.AppendFormattedText(exampleRichTextBox, "'nome da pasta'", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta chamada 'Fotos'.", Color.LightGray, FontStyle.Regular);
            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta com o nome 'Músicas'.", Color.LightGray, FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));


        }

        private void LoadAbrirPastaContent()
        {
         
        }




    }


}
