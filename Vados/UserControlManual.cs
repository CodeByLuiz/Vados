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
        private Panel panelNavFixed;     
        private FlowLayoutPanel flowScrollable; 


        private string pathreturnbutton = @"Images/Icons/closeIcon.png";

        Font descBold = new Font(Fonts.DarkerRegular, 20f, FontStyle.Bold);
        Font descActualBold = new Font(Fonts.DarkerSemiBold, 20f);
        Font descRegular = new Font(Fonts.DarkerRegular, 20f);
        Font descUnderline = new Font(Fonts.DarkerRegular, 20f, FontStyle.Underline);
        Font exBold = new Font(Fonts.DarkerRegular, 18f, FontStyle.Bold);
        Font exActualBold = new Font(Fonts.DarkerSemiBold, 18f);
        Font exRegular = new Font(Fonts.DarkerRegular, 18f);
        Font exUnderline = new Font(Fonts.DarkerRegular, 18f, FontStyle.Underline);


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
            foreach (Control ctrl in flowScrollable.Controls)
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
     
            int navWidth = panelNav?.Width ?? 320;

           
            panelContent = new Panel
            {
                BackColor = Color.FromArgb(223, 223, 223),
                Location = new Point(navWidth, 0),
                Size = new Size(Math.Max(100, this.ClientSize.Width - navWidth), this.ClientSize.Height),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoScroll = false 
            };
        
            this.Controls.Add(panelContent);

          
            btnReturn = new PictureBox
            {
                Width = 50,
                Height = 50,
                Image = Image.FromFile(pathreturnbutton),
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand,
               
            };
            btnReturn.Click += btnReturn_Click;
            panelContent.Controls.Add(btnReturn);
            btnReturn.BringToFront();

        
            int topPadding = 20; 

         
            tableLayoutContent = new TableLayoutPanel
            {
                AutoScroll = true,       
                ColumnCount = 1,
                RowCount = 0,
                Padding = new Padding(20, topPadding + 10, 20, 20), 
                Location = new Point(0, 0),
             
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            
            ResizeContentChildren(navWidth, topPadding);

            panelContent.Controls.Add(tableLayoutContent);

           
            this.Resize += (s, e) =>
            {
                
                panelContent.Location = new Point(navWidth, 0);
                panelContent.Size = new Size(Math.Max(100, this.ClientSize.Width - navWidth), this.ClientSize.Height);

                
                ResizeContentChildren(navWidth, topPadding);
            };
        }

        
        private void ResizeContentChildren(int navWidth, int topPadding)
        {
            if (panelContent == null || tableLayoutContent == null || btnReturn == null) return;

          
            int contentW = panelContent.ClientSize.Width;
            int contentH = panelContent.ClientSize.Height;

            
            contentW = Math.Max(60, contentW);
            contentH = Math.Max(60, contentH);


            btnReturn.Location = new Point(
       panelContent.ClientSize.Width - 40 - panelContent.Padding.Right - btnReturn.Width,
       panelContent.Padding.Top + 20
   );



            tableLayoutContent.Location = new Point(0, 0);
            tableLayoutContent.Size = new Size(contentW, contentH);

           
            tableLayoutContent.AutoScrollMinSize = new Size(0, 0);

            
            tableLayoutContent.PerformLayout();
            panelContent.PerformLayout();
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
                Margin = new Padding(65, 10, 40, 30),
                Tag = "descrição",
                Cursor = Cursors.Arrow
            };
            Global.TextBoxFitHeight(exampleRichTextBox, 10);

            
            descriptionBox.GotFocus += (s, e) => this.ActiveControl = null;
            descriptionBox.MouseDown += (s, e) => descriptionBox.SelectionLength = 0;
            descriptionBox.SelectionChanged += (s, e) => descriptionBox.SelectionLength = 0;

           
            descriptionBox.SelectionIndent = 0;          
            descriptionBox.SelectionRightIndent = 20;   

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
                Margin = new Padding(65, 10, 40, 30), 
                Tag = "descrição",
                Cursor = Cursors.Arrow
            };


            Global.TextBoxFitHeight(secundarydescriptionBox, 10);

           
            secundarydescriptionBox.GotFocus += (s, e) => this.ActiveControl = null;
            secundarydescriptionBox.MouseDown += (s, e) => secundarydescriptionBox.SelectionLength = 0;
            secundarydescriptionBox.SelectionChanged += (s, e) => secundarydescriptionBox.SelectionLength = 0;

            
            secundarydescriptionBox.SelectionIndent = 0;           
            secundarydescriptionBox.SelectionRightIndent = 20;    

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
                    AutoSize = true
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
                Padding = new Padding(10)
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
                Height = examplePanel.Height - 20
            };

            // Evitar foco e seleção
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
                Padding = new Padding(0)
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
                Width = 320,
                BackColor = Color.FromArgb(48, 61, 99),
                Location = new Point(0, 0),
                Height = this.Height,   // ocupa altura toda
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            this.Controls.Add(panelNav);

            // ------------------------------------------------------------
            // 1) PAINEL FIXO (LOGO + TÍTULO + LINHAS)
            // ------------------------------------------------------------
            panelNavFixed = new Panel
            {
                Width = panelNav.Width,
                Height = 280,
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(48, 61, 99)
            };

            // ADICIONE O FIXO PRIMEIRO
            panelNav.Controls.Add(panelNavFixed);

            // Logo (não centralize por Width aqui — ele ainda é 0)
            PictureBox pictureLogo = new PictureBox
            {
                Image = Image.FromFile("Images/LogoBranco.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 200,
                Height = 200,
                Top = 10,
                Left = (320 - 200) / 2  // 320 = largura do panelNav
            };
            panelNavFixed.Controls.Add(pictureLogo);

            Label lblTitle = new Label
            {
                Text = "Comandos",
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = Fonts.GetFont(Fonts.MavenMedium, 26f),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 50,
                Width = panelNavFixed.Width,
                Top = panelNavFixed.Height - 50,
                Left = 0,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
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
            panelNavFixed.Controls.Add(lblTitle);

            // ------------------------------------------------------------
            // 2) PARTE ROLÁVEL
            // ------------------------------------------------------------
            flowScrollable = new FlowLayoutPanel
            {
                AutoScroll = true,
                Width = panelNav.Width,
                Height = panelNav.Height - panelNavFixed.Height,
                Location = new Point(0, panelNavFixed.Height),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.FromArgb(48, 61, 99),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };

            // ADICIONE O ROLÁVEL DEPOIS
            panelNav.Controls.Add(flowScrollable);

            // NÃO USE MAIS SetChildIndex => CAUSA SOBREPOSIÇÃO ❌

            // Agora adicione as seções:
            AddSection(flowScrollable, "Pastas", @"Images/Icons/pasta.png", new[]
            {
        "Criar uma pasta", "Abrir uma pasta",
        "Renomear uma pasta", "Excluir uma pasta",
        "Mover uma pasta", "Duplicar uma pasta"
    });

            AddSection(flowScrollable, "Arquivos", @"Images/Icons/Arquivos.png", new[]
            {
        "Criar um arquivo", "Abrir um arquivo", "Renomear um arquivo",
        "Excluir um arquivo", "Mover um arquivo",
        "Duplicar um arquivo", "Operar múltiplos arquivos"
    });

            AddSection(flowScrollable, "Sistema", @"Images/Icons/Sistema.png", new[]
            {
        "Abrir site", "Abrir programa"
    });


            this.Resize += (s, e) =>
            {
                panelNav.Height = this.Height;
                flowScrollable.Height = panelNav.Height - panelNavFixed.Height;
            };

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

            // função local para calcular largura disponível do botão
            Func<int> availableWidth = () =>
            {
                int w = parentFlow.ClientSize.Width;
                // se ainda for 0 (ainda não layoutado), use panelNav width como fallback
                if (w <= 0 && panelNav != null) w = panelNav.ClientSize.Width;
                // subtrai um pouco para compensar possível scrollbar
                int scrollbarCompensation = SystemInformation.VerticalScrollBarWidth + 8;
                return Math.Max(40, w - scrollbarCompensation);
            };

            // Criação de botões responsivos
            foreach (var cmd in commands)
            {
                RoundedButton btn = new RoundedButton
                {
                    Text = cmd,
                    Height = 36,
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.FromArgb(48, 61, 99),
                    ForeColor = Color.White,
                    Font = Fonts.GetFont(Fonts.DarkerRegular, 16f),
                    Padding = new Padding(15, 0, 15, 0),
                    Margin = new Padding(15, 6, 15, 6),
                    AutoSize = false, // importante para definir Width manualmente
                    Cursor = Cursors.Hand
                };

                // Usa parentFlow para definir a largura
                int btnWidth = availableWidth() - btn.Margin.Left - btn.Margin.Right;
                btn.Width = Math.Max(120, btnWidth);

                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += NavButton_Click;
                parentFlow.Controls.Add(btn);
            }

            // Atualiza largura dos botões quando o painel for redimensionado
            parentFlow.Resize += (s, e) =>
            {
                int newAvail = availableWidth();
                foreach (RoundedButton btn in parentFlow.Controls.OfType<RoundedButton>())
                {
                    btn.Width = Math.Max(120, newAvail - btn.Margin.Left - btn.Margin.Right);
                }
            };
        }



        private void NavButton_Click(object sender, EventArgs e)
        {
           
            foreach (var btn in flowScrollable.Controls.OfType<Button>())
            {
                btn.Font = Fonts.GetFont(Fonts.DarkerRegular, 16f);
            }

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
                case "Duplicar arquivo":
                case "Duplicar um arquivo":
                    LoadDuplicarArquivoContent();
                    break;
                case "Mover arquivo":
                case "Mover um arquivo":
                    LoadMoverArquivoContent();
                    break;
                case "Excluir arquivo":
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
                case "Abrir site":
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
            Global.AppendFormattedText(descriptionBox, "Criar Pasta", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". As pastas criadas são encontradas na pasta padrão do aplicativo, chamada \"Vados\". Caso uma pasta seja criada com o mesmo nome de outra já existente, seu nome terá um número na frente, de forma ascendente, para que possa ser distinguida.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);

            AddImageToContent(@"Images\prints\Pastas\CriarPasta.png");

            AddSecundaryDescriptionToContent();
            Global.AppendPlainText(secundarydescriptionBox, "A execução desse comando depende de apenas um fator, o ");
            Global.AppendFormattedText(secundarydescriptionBox, "nome da pasta.", Colors.greenHighlight, descBold);
            secundarydescriptionBox.Rtf = Global.RtfChangeFont(secundarydescriptionBox.Rtf, descRegular, descActualBold);


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta", Color.Black, exUnderline);
            Global.AppendPlainText(exampleRichTextBox, " chamada ");
            Global.AppendFormattedText(exampleRichTextBox, "'nome da pasta'.\n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta chamada 'Fotos'.\n", Color.FromArgb(125, 125, 125), exRegular);
            Global.AppendFormattedText(exampleRichTextBox, "Crie uma pasta com o nome 'Músicas'.\n", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);


        }

        private void LoadAbrirPastaContent()
        {
            AddTitleToContent("Abrir Pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para abrir uma pasta do computador, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Abrir Pasta", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Caso a pasta esteja dentro da pasta padrão (chamada “vados”), ela terá prioridade na busca, e caso contrário, se não for encontrada, a busca será feita no resto do computador.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);

            AddImageToContent(@"Images\prints\Pastas\AbrirPasta.png");

            AddSecundaryDescriptionToContent();
            Global.AppendPlainText(secundarydescriptionBox, "Esse comando precisa de um único fator, o ");
            Global.AppendFormattedText(secundarydescriptionBox, "nome da pasta.", Colors.greenHighlight, descBold);
            secundarydescriptionBox.Rtf = Global.RtfChangeFont(secundarydescriptionBox.Rtf, descRegular, descActualBold);


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Abrir a pasta ", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta’.\n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Abra a pasta ‘Fotos’.”\n“Abrir a pasta chamada ‘Músicas’.”\n", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);

        }

        private void LoadRenomearPastaContent()
        {
            AddTitleToContent("Renomear pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para renomear uma pasta presente na pasta padrão (chamada “vados”), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, " Renomear pasta.", Colors.blueHighlight, descBold);



            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);

            AddImageToContent(@"Images\prints\Pastas\RenomearPasta.png");



            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para executar esse comando, diga o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " e o ");
            Global.AppendFormattedText(descriptionBox, "novo nome da pasta", Colors.greenHighlight, descBold);


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Renomeie a pasta ", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta' ", Color.Black, exBold);
            Global.AppendPlainText(exampleRichTextBox, "para ");
            Global.AppendFormattedText(exampleRichTextBox, "‘novo nome da pasta'. \n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Renomeie a pasta ‘Fotos’ para ‘Fotos 2025’. ”\n“Renomeie a pasta chamada ‘Músicas’ para ‘Músicas Pop/Rock’.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);

        }

        private void LoadExcluirPastaContent()
        {
            AddTitleToContent("Excluir uma Pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para excluir uma pasta presente na pasta padrão (chamada “vados”), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, " Excluir pasta.", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". As pastas excluídas podem ser encontradas na lixeira, e de lá podem ser recuperadas.");


            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);




            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa apenas de um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " a ser excluída.");



            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Excluir a pasta", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta'.\n", Color.Black, exBold);

            Global.AppendFormattedText(exampleRichTextBox, "“Exclua a pasta chamada ‘Jogos’.”\r\n“Delete a pasta ‘Fotos 2017’.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);

        }

        private void LoadMoverUmaPastaContent()
        {

            AddTitleToContent("Mover uma Pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para mover uma pasta para dentro de uma outra pasta,basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Mover uma pasta", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Por questão de simplicidade, as pastas movidas devem estar presentes na pasta padrão (chamada \"Vados\"), ou a pasta de dentro deve ser a pasta padrão.");


            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);




            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa de dois fatores: o");
            Global.AppendFormattedText(descriptionBox, " nome da pasta", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " a ser movida e o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " de destino.");




            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Mover Pasta", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta' ", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "para a pasta", Color.Black, exRegular);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta de destino'. \n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Mover a pasta ‘Currículos’ para a pasta 'Arquivos'.”\r\n“Mova a Pasta chamada 'Redações' para a pasta 'Documentos'.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);

        }

        private void LoadDuplicarUmaPastaContent()
        {


            AddTitleToContent("Duplicar pasta");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para duplicar uma pasta,basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Duplicar pasta", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". A pasta duplicada será criada na mesma pasta que a pasta original, e também possuirá todos os arquivos e pastas existentes dentro da original. Como a nova pasta terá o mesmo nome da pasta original, seu nome terá um numero na frenten de dorma ascendente, para que as pastas possam ser distinguidas.");


            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);

            AddImageToContent(@"Images\prints\Pastas\DuplicarPasta.png");


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa de apenas um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome da pasta", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " a ser duplicada  ");





            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Duplicar a pasta", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome da pasta'. \n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Duplicar a pasta 'Atividades'.”\r\n“Duplique a pasta chamada 'Jogos'”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);


        }




        private void LoadAbrirArquivoContent()
        {
            AddTitleToContent("Abrir Arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para Abrir um arquivo,basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Abrir Arquivo", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". O Arquivo será aberto com o software padrão para arquivos da determinada extensão. Caso nenhum software esteja definido como padrão, uma janela abrirá para a escolha do software");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa de um único fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Não é necessario dizer a ");
            Global.AppendFormattedText(descriptionBox, "extensão do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ", mas isso pode ajudar a distinguir arquivos que tenha o mesmo nome.");

            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Abrir o arquivo ", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, "'nome do arquivo'.\r\n ", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Abra o arquivo chamado 'Redação'.”\r\n“Abra o arquivo chamado 'Redação.txt'”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);


        }
        private void LoadCriarArquivoContent()
        {
            AddTitleToContent("Criar um arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para criar um arquivo, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Criar arquivo", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Os arquivos criados são encontrados na pasta padrão do aplicativo, chamada “vados”. Caso um arquivo seja criado com o mesmo nome de outro já existente, seu nome terá um número na frente, de forma ascendente, para que possa ser distinguido. ");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);



            AddImageToContent(@"Images\prints\Arquivos\CriarArquivo.png");


            AddDescriptionToContent();

            Global.AppendPlainText(descriptionBox, "A execução desse comando depende de apenas um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". É importante dizer o ");
            Global.AppendFormattedText(descriptionBox, "formato do arquivo ", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, "para que ele funcione da forma esperada.");



            AddExampleBox();



            Global.AppendFormattedText(exampleRichTextBox, "Crie um arquivo ", Color.Black, exUnderline);
            Global.AppendPlainText(exampleRichTextBox, "chamado");
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome do arquivo’.\r\n ", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Crie um arquivo chamado ‘Lista.txt’.”\r\n“Crie um arquivo com o nome ‘Apresentação.pptx’.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);
        }
        private void LoadExcluirArquivoContent()
        {

            AddTitleToContent("Excluir um Arquivo");
            AddDescriptionToContent();

            Global.AppendPlainText(descriptionBox, "Para excluir uma arquivo presente na pasta padrão (chamada “vados”), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Excluir arquivo", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Os arquivos excluídos podem ser encontradas na lixeira, e de lá podem ser recuperadas.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);


            AddDescriptionToContent();

            Global.AppendPlainText(descriptionBox, "Esse comando precisa apenas de um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Não é necessário dizer a ");
            Global.AppendFormattedText(descriptionBox, "extensão do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ", mas isso pode ajudar a distinguir arquivos que tenham o mesmo nome.");


            AddExampleBox();



            Global.AppendFormattedText(exampleRichTextBox, "Excluir o arquivo", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome do arquivo’. \r\n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Exclua o arquivo ‘Selfie’.”\r\n“Delete o arquivo chamado ‘Filme.mp4’.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);
        }
        private void LoadDuplicarArquivoContent()
        {


            AddTitleToContent("Duplicar Arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para duplicar um arquivo, basta utilizar o comando");
            Global.AppendFormattedText(descriptionBox, " Duplicar arquivo", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". O arquivo duplicado será criado na mesma pasta que o arquivo original, e por terem o mesmo nome, o novo arquivo terá um número na frente do nome, de forma ascendente, para que as pastas possam ser distinguidas.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);


            AddImageToContent(@"Images\prints\Arquivos\DuplicarArquivo.png");

            AddDescriptionToContent();

            Global.AppendPlainText(descriptionBox, "Esse comando precisa apenas um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo a ser duplicado", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Não é necessário dizer a ");
            Global.AppendFormattedText(descriptionBox, "extensão do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ", mas isso pode ajudar a distinguir arquivos que tenham o mesmo nome.");


            AddExampleBox();




            Global.AppendFormattedText(exampleRichTextBox, "Duplicar o arquivo", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome do arquivo’.\n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Duplicar o arquivo ‘Tutorial’.”\r\n“Duplique o arquivo chamado ‘Lista de compras.txt’.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);

        }

        private void LoadOperarMultiplosArquivosContent()
        {

            AddTitleToContent("Operar múltiplos arquivos");
            AddDescriptionToContent();


            Global.AppendPlainText(descriptionBox, "Os comandos ");
            Global.AppendFormattedText(descriptionBox, "Mover arquivo, Excluir arquivo", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " e ");
            Global.AppendFormattedText(descriptionBox, "Duplicar arquivo", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " podem funcionar em mais de um arquivo de uma vez. Dessa forma, é possível “filtrar” os arquivos a serem operados. As regras e fatores necessários para cada comando continuam os mesmos, apenas com a adição dos filtros e a pasta de origem dos arquivos.");
            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);


            AddDescriptionToContent();

            Global.AppendPlainText(descriptionBox, "Os arquivos podem ser filtrados através dos seguintes fatores: ");
            Global.AppendFormattedText(descriptionBox, "nome, formato, data de modificação ", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, "e ");
            Global.AppendFormattedText(descriptionBox, "tamanho ", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, "Além disso, também é possivel operar ");
            Global.AppendFormattedText(descriptionBox, "todos os arquivos ", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " sem filtros. Caso nenhuma pasta seja indicada, o comando será realizado nos arquivos da pasta padrão (chamada “vados”).");


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "‘Comando’", Color.Black, exUnderline);
            Global.AppendPlainText(exampleRichTextBox, " os arquivos da pasta ");
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta’ ", Color.Black, descBold);
            Global.AppendPlainText(exampleRichTextBox, "que ");
            Global.AppendFormattedText(exampleRichTextBox, "‘filtro’.\n ", Color.Black, descBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Excluir os arquivos da pasta ‘Fotos’ que tenham o nome ‘Praia’.”\r\n“Mova todos os arquivos da pasta ‘Escola’ que sejam ‘.txt’ para a pasta ‘Atividades’.”\r\n“Copiar os arquivos da pasta ‘Gravações’ de depois de 2022 par a pasta ‘Vídeos’.”\r\n“Delete todos os arquivos maiores que 10mb.”\r\n“Mover todos os arquivos da pasta ‘Selfies’ para a pasta ‘Fotos’.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);

            
        }

        private void LoadRenomearArquivoContent()
        {

            AddTitleToContent("Renomear um arquivo");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para renomear um arquivo presente na pasta padrão (chamada 'Vados'), basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Renomear arquivo", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Com esse comando também é possívelm alterar a extensão do arquivo. ");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);


            AddImageToContent(@"Images\prints\Arquivos\RenomearArquivo.png");


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para executar esse comando, diga o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " e o ");
            Global.AppendFormattedText(descriptionBox, "novo nome desejado", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Não é necessariamente obrigatório dizer a ");
            Global.AppendFormattedText(descriptionBox, "extensão do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ", mas isso pode ajudar a distinguir arquivos que tenham o mesmo nome.");
            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Renomeie o arquivo", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " 'nome do arquivo'", Color.Black, exBold);
            Global.AppendPlainText(exampleRichTextBox, " para ");
            Global.AppendFormattedText(exampleRichTextBox, "'novo nome do arquivo'.\n ", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Renomeie o arquivo 'Documentos' para 'Documentos Importantes'.”\r\n“Renomeie o arquivo 'Documentos errados' para 'Documentos certos' ", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);
        }

        private void LoadMoverArquivoContent()
        {

            AddTitleToContent("Mover um Arquivo");
            AddDescriptionToContent();

            Global.AppendPlainText(descriptionBox, "Para mover um arquivo para dentro de uma pasta, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Mover arquivo", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Por questão de simplicidade, os arquivos movidos devem ou estar presentes na pasta padrão (chamada “vados”), ou a pasta de destino deve ser a pasta padrão.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);


            AddDescriptionToContent();

            Global.AppendPlainText(descriptionBox, "Esse comando precisa de dois fatores: o ");
            Global.AppendFormattedText(descriptionBox, "nome do arquivo", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " a ser movido e o");
            Global.AppendFormattedText(descriptionBox, " nome da pasta", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, " de destino.");
            AddExampleBox();



            Global.AppendFormattedText(exampleRichTextBox, "Mover o arquivo", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " ‘nome do arquivo’ ", Color.Black, exBold);
            Global.AppendPlainText(descriptionBox, "para a pasta ");
            Global.AppendFormattedText(exampleRichTextBox, "‘nome da pasta’.\n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Mover o arquivo ‘Currículo’ para a pasta ‘Trabalho’.”\r\n“Mova o arquivo chamado ‘Redações’ para a pasta padrão.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);
        }



        private void LoadAbrirSiteContent()
        {

            AddTitleToContent("Abrir site ");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para abrir um site em seu computador, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Abrir site", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". Você pode informar o nome do site, caso ele já esteja definido como um site padrão, ou indicar diretamente o link do site. O site será aberto no seu navegador padrão.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando requer apenas um parâmetro: o ");
            Global.AppendFormattedText(descriptionBox, "nome do site ", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, "ou o");
            Global.AppendFormattedText(descriptionBox, "link do site", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ".");

            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Abrir site", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " 'nome do site/link'. \n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Abrir site Facebook.”\r\n“Abra o site Youtube.com”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);
        }
        private void LoadAbrirProgramaContent()
        {

            AddTitleToContent("Abrir programa");
            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Para abrir um programa do seu computador, basta utilizar o comando ");
            Global.AppendFormattedText(descriptionBox, "Abrir Programa", Colors.blueHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ". A busca será realizada em todo seu computador o programa procura tanto arquivos executáveis como atalhos.");

            descriptionBox.Rtf = Global.RtfChangeFont(descriptionBox.Rtf, descRegular, descActualBold);


            AddDescriptionToContent();
            Global.AppendPlainText(descriptionBox, "Esse comando precisa apenas de um fator, o ");
            Global.AppendFormattedText(descriptionBox, "nome do programa", Colors.greenHighlight, descBold);
            Global.AppendPlainText(descriptionBox, ".");


            AddExampleBox();


            Global.AppendFormattedText(exampleRichTextBox, "Abrir Programa", Color.Black, exUnderline);
            Global.AppendFormattedText(exampleRichTextBox, " 'nome do Programa'.\n", Color.Black, exBold);
            Global.AppendFormattedText(exampleRichTextBox, "“Abrir Programa Word”\r\n“Abrir Aplicativo PowerPoint.”", Color.FromArgb(125, 125, 125), exRegular);
            exampleRichTextBox.Rtf = Global.RtfChangeFont(exampleRichTextBox.Rtf, exRegular, exActualBold);
        }


        #endregion


        private void btnReturn_Click(object sender, EventArgs e)
        {

            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlHome));

        }
    }
}

