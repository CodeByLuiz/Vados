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

        private string pathreturnbutton = @"Images/Icons/closeicon.png";


        private PictureBox btnReturn = null;



        public event EventHandler<LoadPageEventArgs> loadPage;

        public UserControlManual()
        {
            InitializeComponent();

            SetupNavBar();
            SetupContentArea();

            this.Controls.Add(panelContent);
            this.Controls.Add(panelNav);

            this.Resize += UserControlManual_Resize;

            // Seleciona botão padrão
            foreach (Control ctrl in flow.Controls)
            {
                if (ctrl is Button btn && btn.Text == "Criar uma pasta")
                {
                    NavButton_Click(btn, EventArgs.Empty);
                    break;
                }
            }
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
            this.Controls.Add(panelContent);


            btnReturn = new PictureBox
            {
                Width = 50,
                Height = 50,
                Image = Image.FromFile(pathreturnbutton),
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand
            };
            btnReturn.Click += btnReturn_Click;
            btnReturn.Location = new Point(this.Width - btnReturn.Width - 20, 20);
            btnReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Controls.Add(btnReturn);
            btnReturn.BringToFront();


            tableLayoutContent = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                ColumnCount = 1,
                RowCount = 0,
                Padding = new Padding(20, 20, 20, 20)
            };
            panelContent.Controls.Add(tableLayoutContent);

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
                Tag = "descrição",
                Cursor = Cursors.Arrow
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
                Height = 80,
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
                Tag = "descrição",
                Cursor = Cursors.Arrow
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
            if (string.IsNullOrEmpty(box.Text))
                return;

            int horizontalPadding = box.Margin.Left + box.Margin.Right;
            int maxWidth = panelContent.Width - horizontalPadding - btnReturn.Width - 20;

            box.Width = maxWidth;

            int lastCharIndex = box.Text.Length - 1;
            Point lastCharPos = box.GetPositionFromCharIndex(lastCharIndex);


            box.Height = lastCharPos.Y + box.Font.Height + 10;
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
                Margin = new Padding(5, 5, 5, 80),
                Padding = new Padding(10),

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
                Dock = DockStyle.Fill,
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
            // Painel principal da NavBar
            panelNav = new Panel
            {
                Dock = DockStyle.Left,
                Width = 320,
                BackColor = Color.FromArgb(48, 61, 99),


            };
            this.Controls.Add(panelNav);

            flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,

                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            panelNav.Controls.Add(flow);

            // Logo no topo
            PictureBox pictureLogo = new PictureBox
            {
                Image = Image.FromFile("Images/LogoBranco.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 200,
                Height = 200,
                Margin = new Padding(0, 10, 0, 10),
                Anchor = AnchorStyles.Top,
            };
            flow.Controls.Add(pictureLogo);

            // Título
            Label lblTitle = new Label
            {
                Text = "Comandos",
                Height = 60,
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = Fonts.GetFont(Fonts.MavenMedium, 26f),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 10, 0, 10)
            };


            lblTitle.Width = flow.ClientSize.Width;


            flow.Resize += (s, e) =>
            {
                lblTitle.Width = flow.ClientSize.Width;
                lblTitle.Invalidate();
            };

            lblTitle.Paint += (sender, e) =>
            {
                Label lbl = sender as Label;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(200, 219, 236), 2))
                {
                    SizeF textSize = e.Graphics.MeasureString(lbl.Text, lbl.Font);
                    int textWidth = (int)textSize.Width;
                    int centerY = lbl.Height / 2;
                    int lineY = centerY;
                    int padding = 1;
                    int lineLength = (lbl.Width - textWidth) / 2 - padding;
                    if (lineLength > 0)
                    {
                        e.Graphics.DrawLine(pen, 20, lineY, lineLength, lineY);
                        e.Graphics.DrawLine(pen, lbl.Width - lineLength, lineY, lbl.Width - 20, lineY);
                    }
                }
            };
            flow.Controls.Add(lblTitle);



            // Categorias e comandos
            AddSection(flow, "Pastas", @"Images/Icons/pasta.png", new[]
            {
        "Criar uma pasta", "Abrir uma pasta",
        "Renomear uma pasta", "Excluir uma pasta", "Mover uma pasta", "Duplicar uma pasta"
    });

            AddSection(flow, "Arquivos", @"Images/Icons/Arquivos.png", new[]
            {
        "Criar um arquivo", "Abrir um arquivo", "Renomear um arquivo",
        "Excluir um arquivo", "Mover um arquivo", "Duplicar um arquivo", "Operar múltiplos arquivos"
    });

            AddSection(flow, "Sistema", @"Images/Icons/Sistema.png", new[]
            {
        "Abrir site","Abrir Programa"
    });




        }




        private void AddSection(FlowLayoutPanel parentFlow, string sectionTitle, string iconPath, string[] commands)
        {
            // Painel horizontal para título + ícone
            FlowLayoutPanel sectionPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(10, 10, 10, 5),
                Padding = new Padding(0)
            };

            // Label da seção
            Label lblSection = new Label
            {
                Text = sectionTitle,
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = Fonts.GetFont(Fonts.DarkerExtraBold, 18f),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            sectionPanel.Controls.Add(lblSection);

            // Ícone da seção
            PictureBox icon = new PictureBox
            {
                Image = Image.FromFile(iconPath),
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 24,
                Height = 24,
                Margin = new Padding(0, 3, 5, 0)
            };
            sectionPanel.Controls.Add(icon);

            // Adiciona a seção ao Flow principal
            parentFlow.Controls.Add(sectionPanel);

            // Criação de botões responsivos
            foreach (var cmd in commands)
            {
                RoundedButton btn = new RoundedButton
                {
                    Text = cmd,
                    Height = 30,
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.FromArgb(48, 61, 99),
                    ForeColor = Color.White,
                    Font = Fonts.GetFont(Fonts.DarkerRegular, 16f),
                    Padding = new Padding(15, 0, 15, 0),
                    Margin = new Padding(15, 3, 15, 3),
                    AutoSize = false, // importante para definir Width manualmente
                    Cursor = Cursors.Hand
                };

                // Define a largura do botão baseado na largura do FlowLayoutPanel
                btn.Width = flow.ClientSize.Width - btn.Margin.Left - btn.Margin.Right;

                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += NavButton_Click;
                flow.Controls.Add(btn);
            }

            // Atualiza largura dos botões quando a janela é redimensionada
            flow.Resize += (s, e) =>
            {
                foreach (RoundedButton btn in flow.Controls.OfType<RoundedButton>())
                {
                    btn.Width = flow.ClientSize.Width - btn.Margin.Left - btn.Margin.Right;
                }
            };



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
                case "Renomear uma pasta":
                    LoadRenomearPastaContent();
                    break;
                case "Excluir uma pasta":
                    LoadExcluirPastaContent();
                    break;
                case "Mover uma pasta":
                    LoadMoverUmaPastaContent();
                    break;
                case "Duplicar uma pasta":
                    LoadDuplicarUmaPastaContent();
                    break;
                case "Abrir um arquivo":
                    LoadAbrirArquivoContent();
                    break;
                case "Criar um arquivo":
                    LoadCriarArquivoContent();
                    break;
                case "Duplicar um arquivo":
                    LoadDuplicarArquivoContent();
                    break;
                case "Mover um arquivo":
                    LoadMoverArquivoContent();
                    break;
                case "Excluir um arquivo":
                    LoadExcluirArquivoContent();
                    break;
                case "Renomear um arquivo":
                    LoadRenomearArquivoContent();
                    break;
                case "Operar múltiplos arquivos":
                    LoadOperarMultiplosArquivosContent();
                    break;
                case "Abrir programa":
                    LoadAbrirProgramaContent();
                    break;
                case "Abrir  site":
                    LoadAbrirSiteContent();
                    break;
            }

            // Redimensiona todas as RichTextBox do conteúdo **uma única vez**
            foreach (Control control in tableLayoutContent.Controls)
            {
                if (control is RichTextBox box && box.Tag?.ToString() == "descrição")
                {
                    ResizeDescriptionBox(box);
                }
            }

            // Também redimensiona o exemplo, se existir
            if (exampleRichTextBox != null)
                ResizeDescriptionBox(exampleRichTextBox);



            tableLayoutContent.ResumeLayout();
            tableLayoutContent.PerformLayout();
        }


        #region Métodos Load
        private void LoadCriarPastaContent()
        {
            AddTitleToContent("Criar Pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para criar uma pasta, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Criar Pasta", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". As pastas criadas são encontradas na pasta padrão do aplicativo, chamada \"Vados\". Caso uma pasta seja criada com o mesmo nome de outra já existente, seu nome terá um número na frente, de forma ascendente, para que possa ser distinguida.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));

            AddImageToContent(@"Images\imagem-nao-encontrada.jpg");

            AddSecundaryDescriptionToContent();
            Global.AppendPlainText(secundarydescriptionBox, "A execução desse comando depende de apenas um fator, o ");
            Global.AppendFormattedText(secundarydescriptionBox, "nome da pasta.", Colors.greenHighlight, FontStyle.Bold);
            secundarydescriptionBox.Rtf = Global.RtfChangeFont(secundarydescriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "crie uma pasta", Color.Black, FontStyle.Underline);
            Global.AppendPlainText(exampleRichTextBox, " chamada ");
            Global.AppendFormattedText(exampleRichTextBox, "'nome da pasta'\n", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta chamada 'Fotos'.\n", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta com o nome 'Músicas'.\n", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));


        }

        private void LoadAbrirPastaContent()
        {
            AddTitleToContent("Abrir Pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para abrir uma pasta do computador, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Abrir Pasta", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Caso a pasta esteja dentro da pasta padrão (chamada “vados”), ela terá prioridade na busca, e caso contrário, se não for encontrada, a busca será feita no resto do computador.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));

            AddImageToContent(@"Images\imagem-nao-encontrada.jpg");

            AddSecundaryDescriptionToContent();
            Global.AppendPlainText(secundarydescriptionBox, "Esse comando precisa de um único fator, o ");
            Global.AppendFormattedText(secundarydescriptionBox, "nome da pasta.", Colors.greenHighlight, FontStyle.Bold);
            secundarydescriptionBox.Rtf = Global.RtfChangeFont(secundarydescriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Abrir a pasta ", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta’\n", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Abra a pasta ‘Fotos’.”\n“Abrir a pasta chamada ‘Músicas’.”\n", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));

        }

        private void LoadRenomearPastaContent()
        {
            AddTitleToContent("Renomear pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para renomear uma pasta presente na pasta padrão (chamada “vados”), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, " Renomear pasta.", Colors.blueHighlight, FontStyle.Bold);



            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));




            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para executar esse comando, diga o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, " e o ");
            Global.AppendFormattedText(descriptionBox, "novo nome da pasta", Colors.greenHighlight, FontStyle.Bold);


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Renomeie a pasta ", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta' ", Color.Black, FontStyle.Bold);
            Global.AppendPlainText(exampleRichTextBox, "para ");
            Global.AppendFormattedText(exampleRichTextBox, "‘novo nome da pasta'\n", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Renomeie a pasta ‘Fotos’ para ‘Fotos 2025’.”\n“Renomeie a pasta chamada ‘Músicas’ para ‘Músicas Pop/Rock’.”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));

        }

        private void LoadExcluirPastaContent()
        {
            AddTitleToContent("Excluir uma Pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para excluir uma pasta presente na pasta padrão (chamada “vados”), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, " Excluir pasta.", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". As pastas excluídas podem ser encontradas na lixeira, e de lá podem ser recuperadas.");


            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));




            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa apenas de um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, " a ser excluída.");



            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Excluir a pasta", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta'\n", Color.Black, FontStyle.Bold);

            Global.AppendFormattedText(exampleRichTextBox, "“Exclua a pasta chamada ‘Jogos’.”\r\n“Delete a pasta ‘Fotos 2017’.”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));

        }

        private void LoadMoverUmaPastaContent()
        {

            AddTitleToContent("Mover uma Pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para mover uma pasta para dentro de uma outra pasta,basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Mover uma pasta", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Por questão de simplicidade, as pastas movidas devem estar presentes na pasta padrão(chamada \"Vados\"), ou a pasta de dentro deve ser a pasta padrão.");


            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));




            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa de dois fatores:");
            Global.AppendFormattedText(descriptionBox, " o nome da pasta a ser movida", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, " e o  ");
            Global.AppendFormattedText(descriptionBox, " nome da pasta de destino.", Colors.greenHighlight, FontStyle.Bold);




            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Mover Pasta", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta'\n", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "para a pasta", Color.Black, FontStyle.Regular );
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta de destino'\n", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Mover a pasta ‘Currículos’ pa a pasta 'Arquivos'.”\r\n“Mova a Pasta chamada 'Redações' para a pasta 'Documentos'.”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));

        }

        private void LoadDuplicarUmaPastaContent()
        {


            AddTitleToContent("Duplicar pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para duplicar uma pasta,basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Duplicar pasta", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". A pasta duplicada será criada na mesma pasta que a pasta original, e também possuirá todos os arquivos e pastas existentes dentro da original. Como a nova pasta terá o mesmo nome da pasta original, seu nome terá um numero na frenten de dorma ascendente, para que as pastas possam ser distinguidas.");


            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));




            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa de apenas um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, " a ser duplicada  ");
            




            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Duplicar a pasta", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta'\n", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Duplicar a pasta 'Atividades'.”\r\n“Duplique a pasta chamada 'Jogos'”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));


        }




       private void LoadAbrirArquivoContent()
        {
            AddTitleToContent("Abrir Arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para Abrir um arquivo,basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Abrir Arquivo", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". O Arquivo será aberto com o software padrão para arquivos da determinada extensão. Caso nenhum software esteja definido como padrão, uma janela abrirá para a escolha do software");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa de um único fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Não é necessario dizer a ");
            Global.AppendFormattedText(descriptionBox, "extensão do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ", mas isso pode ajudar a distinguir arquivos que tenha o mesmo nome.");

            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Abrir o arquivo", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "'nome do arquivo'", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Abra o arquivo chamado 'Redação'.”\r\n“Abra o arquivo chamado 'Redação.txt'”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));


        }
        private void LoadCriarArquivoContent()
        {
            AddTitleToContent("Criar um arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para criar um arquivo, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Criar arquivo", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Os arquivos criados são encontrados na pasta padrão do aplicativo, chamada “vados”. Caso um arquivo seja criado com o mesmo nome de outro já existente, seu nome terá um número na frente, de forma ascendente, para que possa ser distinguido. ");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "A execução desse comando depende de apenas um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". É importante dizer o ");
            Global.AppendFormattedText(descriptionBox, "formato do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "para que ele funcione da forma esperada.");



            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Crie um arquivo ", Color.Black, FontStyle.Underline);
            Global.AppendPlainText(descriptionBox, "chamado");
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome do arquivo’", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Crie um arquivo chamado ‘Lista.txt’.”\r\n“Crie um arquivo com o nome ‘Apresentação.pptx’.”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));
        }
       private void LoadExcluirArquivoContent() {
            AddTitleToContent("Excluir um Arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para excluir uma arquivo presente na pasta padrão (chamada “vados”), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Excluir arquivo", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Os arquivos excluídos podem ser encontradas na lixeira, e de lá podem ser recuperadas.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa apenas de um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Não é necessário dizer a ");
            Global.AppendFormattedText(descriptionBox, "extensão do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ", mas isso pode ajudar a distinguir arquivos que tenham o mesmo nome.");


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Excluir o arquivo", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome do arquivo’.", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Exclua o arquivo ‘Selfie’.”\r\n“Delete o arquivo chamado ‘Filme.mp4’.”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));
        }
        private void LoadDuplicarArquivoContent() {

            AddTitleToContent("Duplicar Arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para duplicar um arquivo, basta utilizar o comando");
            Global.AppendFormattedText(descriptionBox, " Duplicar arquivo", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". O arquivo duplicado será criado na mesma pasta que o arquivo original, e por terem o mesmo nome, o novo arquivo terá um número na frente do nome, de forma ascendente, para que as pastas possam ser distinguidas.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa apenas um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo a ser duplicado", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Não é necessário dizer a ");
            Global.AppendFormattedText(descriptionBox, "extensão do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ", mas isso pode ajudar a distinguir arquivos que tenham o mesmo nome.");


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Duplicar o arquivo ", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "‘nome do arquivo’.", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Duplicar o arquivo ‘Tutorial’.”\r\n“Duplique o arquivo chamado ‘Lista de compras.txt’.”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));

        }

        private void LoadOperarMultiplosArquivosContent() {
            AddTitleToContent("Operar múltiplos arquivos");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Os comandos ");
            Global.AppendFormattedText(descriptionBox, "Mover arquivo, Excluir arquivo", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, " e ");
            Global.AppendFormattedText(descriptionBox, "Duplicar arquivo", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, " podem funcionar em mais de um arquivo de uma vez. Dessa forma, é possível “filtrar” os arquivos a serem operados. As regras e fatores necessários para cada comando continuam os mesmos, apenas com a adição dos filtros e a pasta de origem dos arquivos.");
            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Os arquivos podem ser filtrados através dos seguintes fatores: ");
            Global.AppendFormattedText(descriptionBox, "nome, formato, data de modificação ", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "e ");
            Global.AppendFormattedText(descriptionBox, "tamanho ", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "Além disso, também é possivel operar ");
            Global.AppendFormattedText(descriptionBox, "todos os arquivos ", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, " sem filtros. Caso nenhuma pasta seja indicada, o comando será realizado nos arquivos da pasta padrão (chamada “vados”).");


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "‘Comando’", Color.Black, FontStyle.Underline);
            Global.AppendPlainText(descriptionBox, " os arquivos da pasta ");
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta’ ", Color.Black, FontStyle.Underline);
            Global.AppendPlainText(descriptionBox, "que");
            Global.AppendFormattedText(exampleRichTextBox, "‘filtro’. ", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "“Excluir os arquivos da pasta ‘Fotos’ que tenham o nome ‘Praia’.”\r\n“Mova todos os arquivos da pasta ‘Escola’ que sejam ‘.txt’ para a pasta ‘Atividades’.”\r\n“Copiar os arquivos da pasta ‘Gravações’ de depois de 2022 par a pasta ‘Vídeos’.”\r\n“Delete todos os arquivos maiores que 10mb.”\r\n“Mover todos os arquivos da pasta ‘Selfies’ para a pasta ‘Fotos’.”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));
        }

        private void LoadRenomearArquivoContent() {

            AddTitleToContent("Renomear um arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para renomear um arquivo presente na pasta padrão(chamada 'Vados'), basta utilizar o comando");
            Global.AppendFormattedText(descriptionBox, "Renomear arquivo", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Com esse comando também é possívelm alterar a extensão do arquivo. ");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para executar esse comando, diga o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, " e o ");
            Global.AppendFormattedText(descriptionBox, "novo nome desejado", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Não é necessariamente dizer a ");
            Global.AppendFormattedText(descriptionBox, "extensão do arquivo", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ", mas isso pode ajudar a distinguir arquivos que tenham o mesmo nome.");
            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Renomeie o arquivo", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "'nome do arquivo'", Color.Black, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "para ");
            Global.AppendFormattedText(exampleRichTextBox, " 'novo nome do arquivo'", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Renomeie o arquivo 'Documentos' para 'Documentos Importantes'.”\r\n“Renomeie o arquivo 'Documentos errados' para 'Documentos certos' ", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));
        }

        private void LoadMoverArquivoContent()
        {
            AddTitleToContent("Mover um Arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para mover um arquivo para dentro de uma pasta, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Mover arquivo", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, ". Por questão de simplicidade, os arquivos movidos devem ou estar presentes na pasta padrão (chamada “vados”), ou a pasta de destino deve ser a pasta padrão.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa de dois fatores: o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta a ser movida ", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "e o");
            Global.AppendFormattedText(descriptionBox, " nome da pasta de destino.", Colors.greenHighlight, FontStyle.Bold);

            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Mover o arquivo", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "‘nome do arquivo’", Color.Black, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "para a pasta ");
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta’.", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "“Mover o arquivo ‘Currículo’ para a pasta ‘Trabalho’.”\r\n“Mova o arquivo chamado ‘Redações’ para a pasta padrão.”", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));
        }



        private void LoadAbrirSiteContent() {
            AddTitleToContent("Abrir Site ");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "");
            Global.AppendFormattedText(descriptionBox, "", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "");
            Global.AppendFormattedText(descriptionBox, "", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "");


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));
        }
        private void LoadAbrirProgramaContent() {
            AddTitleToContent("Abrir Programa");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "");
            Global.AppendFormattedText(descriptionBox, "", Colors.blueHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "");
            Global.AppendFormattedText(descriptionBox, "", Colors.greenHighlight, FontStyle.Bold);
            Global.AppendPlainText(descriptionBox, "");


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "", Color.Black, FontStyle.Underline);
            Global.AppendFormattedText(exampleRichTextBox, "", Color.Black, FontStyle.Bold);
            Global.AppendFormattedText(exampleRichTextBox, "", Color.FromArgb(125, 125, 125), FontStyle.Regular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));
        }


        #endregion

        private void btnReturn_Click(object sender, EventArgs e)
        {

            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlHome));

        }
    }


}
=======
﻿using System;
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

        private string pathreturnbutton = @"Images/Icons/closeIcon.png"; 


        private PictureBox btnReturn = null;



        public event EventHandler<LoadPageEventArgs> loadPage;

        public UserControlManual()
        {
            InitializeComponent();

            SetupContentArea();
            this.Controls.Add(panelContent);
            this.Resize += UserControlManual_Resize;


            SetupNavBar();

            foreach (Control ctrl in flow.Controls)
            {
                if (ctrl is Button btn && btn.Text == "Criar uma pasta")
                {
                    NavButton_Click(btn, EventArgs.Empty);
                    break;
                }
            }

            this.Controls.Add(panelNav);

            //  LoadContentBasedOnSelection("Criar uma pasta");
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
                BackColor = Color.FromArgb(231, 231, 231)
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

            btnReturn = new PictureBox
            {
                Location = new Point(panelContent.Width-70-20,20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Width = 50,
                Height = 50,
                Margin = new Padding(10,10,10,10),
                Image = Image.FromFile(pathreturnbutton),
                SizeMode = PictureBoxSizeMode.Zoom

            };
            btnReturn.Click += btnReturn_Click;
            btnReturn.Cursor = Cursors.Hand;

            panelContent.Controls.Add(btnReturn);


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
                BackColor = Color.FromArgb(231, 231, 231),
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
                BackColor = Color.FromArgb(231, 231, 231),
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
                Margin = new Padding(5, 5, 5, 80),
                Padding = new Padding(10),

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
                Dock = DockStyle.Fill,
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
            AddSection(flow, "Pastas ", @"Images/Icons/pasta.png", new[] { "Criar uma pasta", "Abrir uma pasta", "Abrir pasta padrão", "Renomear uma pasta", "Excluir uma pasta", "Mover uma pasta", "Duplicar uma pasta" });
            AddSection(flow, "Arquivos ", @"Images/Icons/Arquivos.png", new[] { "Criar um arquivo", "Abrir um arquivo", "Renomear um arquivo", "Excluir um arquivo", "Mover um arquivo", "Duplicar um arquivo", "Operar múltiplos arquivos " });
            AddSection(flow, "Sistema ", @"Images/Icons/Sistema.png", new[] { "Abrir software", "Alterar volume", "Alterar horário", "Alterar brilho da tela", "Alterar idioma" });
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

            panelContent.Layout += (s, e) =>
            {
                foreach (Control control in tableLayoutContent.Controls)
                {
                    if (control is RichTextBox box && box.Tag?.ToString() == "descrição")
                    {
                        ResizeDescriptionBox(box);
                    }
                }
            };




            switch (buttonText)
            {
                case "Criar uma pasta":
                    LoadCriarPastaContent();
                    break;
                case "Abrir uma pasta":
                    LoadAbrirPastaContent();
                    break;
                case "Renomear uma pasta":
                    LoadRenomearPastaContent();
                    break;
                case "Excluir uma pasta":
                    LoadExcluirPastaContent();
                    break;

            }
        }


        #region Métodos Load

        private void LoadCriarPastaContent()
        {
            //Título
            AddTitleToContent("Criar Pasta");

            //Descrição
            AddDescriptionToContent();
            var descBold = new Font(descriptionBox.Font, FontStyle.Bold);

            Global.AppendPlainText(descriptionBox, "Para criar uma pasta, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Criar Pasta", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". As pastas criadas são encontradas na pasta padrão do aplicativo, chamada \"Vados\". Caso uma pasta seja criada com o mesmo nome de outra já existente, seu nome terá um número na frente, de forma ascendente, para que possa ser distinguida.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));

            //Imagem
            AddImageToContent(@"Images\imagem-nao-encontrada.jpg");

            //Descrição secundária
            AddSecundaryDescriptionToContent();
            var secDescBold = new Font(secundarydescriptionBox.Font, FontStyle.Bold);

            Global.AppendPlainText(secundarydescriptionBox, "A execução desse comando depende de apenas um fator, o ");
            Global.AppendFormattedText(secundarydescriptionBox, "nome da pasta.", Colors.greenHighlight, secDescBold);
            secundarydescriptionBox.Rtf = Global.RtfChangeFont(secundarydescriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));

            //Exemplos
            AddExampleBox();
            var exampleRegular = exampleRichTextBox.Font;
            var exampleBold = new Font(exampleRichTextBox.Font, FontStyle.Bold);
            var exampleUnderline = new Font(exampleRichTextBox.Font, FontStyle.Underline);

            Global.AppendFormattedText(exampleRichTextBox, "crie uma pasta", Color.Black, exampleUnderline);
            Global.AppendPlainText(exampleRichTextBox, " chamada ");
            Global.AppendFormattedText(exampleRichTextBox, "'nome da pasta'\n", Color.Black, exampleBold);
            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta chamada 'Fotos'.\n", Color.FromArgb(125, 125, 125), exampleRegular);
            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta com o nome 'Músicas'.\n", Color.FromArgb(125, 125, 125), exampleRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));


        }

        private void LoadAbrirPastaContent()
        {
            //Título
            AddTitleToContent("Abrir Pasta");

            //Descrição
            AddDescriptionToContent();
            var descBold = new Font(descriptionBox.Font, FontStyle.Bold);

            Global.AppendPlainText(descriptionBox, "Para abrir uma pasta do computador, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Abrir Pasta", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Caso a pasta esteja dentro da pasta padrão (chamada “vados”), ela terá prioridade na busca, e caso contrário, se não for encontrada, a busca será feita no resto do computador.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));

            //Imagem
            AddImageToContent(@"Images\imagem-nao-encontrada.jpg");

            //Descrição secundária
            AddSecundaryDescriptionToContent();
            var secDescBold = new Font(secundarydescriptionBox.Font, FontStyle.Bold);

            Global.AppendPlainText(secundarydescriptionBox, "Esse comando precisa de um único fator, o ");
            Global.AppendFormattedText(secundarydescriptionBox, "nome da pasta.", Colors.greenHighlight, secDescBold);
            secundarydescriptionBox.Rtf = Global.RtfChangeFont(secundarydescriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));

            //Exemplos
            AddExampleBox();
            var exampleRegular = exampleRichTextBox.Font;
            var exampleBold = new Font(exampleRichTextBox.Font, FontStyle.Bold);
            var exampleUnderline = new Font(exampleRichTextBox.Font, FontStyle.Underline);

            Global.AppendFormattedText(exampleRichTextBox, "Abrir a pasta ", Color.Black, exampleUnderline);
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta’\n", Color.Black, exampleBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Abra a pasta ‘Fotos’.”\n“Abrir a pasta chamada ‘Músicas’.”\n", Color.FromArgb(125, 125, 125), exampleRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));

        }

        private void LoadRenomearPastaContent()
        {
            //Título
            AddTitleToContent("Renomear pasta");

            //Descrição
            AddDescriptionToContent();
            var descBold = new Font(descriptionBox.Font, FontStyle.Bold);

            Global.AppendPlainText(descriptionBox, "Para renomear uma pasta presente na pasta padrão (chamada “vados”), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, " Renomear pasta.", Colors.blueHighlight, descBold);



            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));




            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para executar esse comando, diga o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " e o ");
            Global.AppendFormattedText(descriptionBox, "novo nome da pasta", Colors.greenHighlight, descBold);


            //Exemplos
            AddExampleBox();
            var exampleRegular = exampleRichTextBox.Font;
            var exampleBold = new Font(exampleRichTextBox.Font, FontStyle.Bold);
            var exampleUnderline = new Font(exampleRichTextBox.Font, FontStyle.Underline);

            Global.AppendFormattedText(exampleRichTextBox, "Renomeie a pasta ", Color.Black, exampleUnderline);
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta' ", Color.Black, exampleBold);
            Global.AppendPlainText(exampleRichTextBox, "para ");
            Global.AppendFormattedText(exampleRichTextBox, "‘novo nome da pasta'\n", Color.Black, exampleBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Renomeie a pasta ‘Fotos’ para ‘Fotos 2025’.”\n“Renomeie a pasta chamada ‘Músicas’ para ‘Músicas Pop/Rock’.”", Color.FromArgb(125, 125, 125), exampleRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));

        }

        private void LoadExcluirPastaContent()
        {
            //Título
            AddTitleToContent("Excluir uma Pasta");

            //Descrição
            AddDescriptionToContent();
            var descBold = new Font(descriptionBox.Font, FontStyle.Bold);

            Global.AppendPlainText(descriptionBox, "Para excluir uma pasta presente na pasta padrão (chamada “vados”), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, " Excluir pasta.", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". As pastas excluídas podem ser encontradas na lixeira, e de lá podem ser recuperadas.");


            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 20f));




            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa apenas de um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " a ser excluída.");


            //Exemplos
            AddExampleBox();
            var exampleRegular = exampleRichTextBox.Font;
            var exampleBold = new Font(exampleRichTextBox.Font, FontStyle.Bold);
            var exampleUnderline = new Font(exampleRichTextBox.Font, FontStyle.Underline);

            Global.AppendFormattedText(exampleRichTextBox, "Excluir a pasta", Color.Black, exampleUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta'\n", Color.Black, exampleBold);

            Global.AppendFormattedText(exampleRichTextBox, "“Exclua a pasta chamada ‘Jogos’.”\r\n“Delete a pasta ‘Fotos 2017’.”", Color.FromArgb(125, 125, 125), exampleRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, Fonts.GetFont(Fonts.DarkerRegular, 18f));

        }
        #endregion

        private void btnReturn_Click(object sender, EventArgs e)
        {

            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlHome));
      
    }
    }


}