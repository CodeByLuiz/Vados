using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vados
{
    //Variáveis que podem ser acessadas de qualquer lugar
    public class Global
    {
        public static string DefaultFolder = "";            //Pasta padrão para os comandos
        public static string VoiceRecognitionFolder = "";   //Pasta do modelo de reconhecimento de voz
        public static WhisperRecognizer VoiceRecognizer = null;    //Objeto do reconhecedor de voz
        public static string decibeis = "nao iniciado";



        #region PRIORIDADES E EXCEÇÕES

        public static List<string> defaultExceptions = new List<string>
        {
            "$RECYCLE.BIN",
            "System Volume Information",
            "Recovery",
            "Config.Msi",
            "Windows",
            "Program Files (x86)",
            "Program Files"
        };


        public static List<string> exeExceptions = new List<string>
        {
            "$RECYCLE.BIN",
            "System Volume Information",
            "Recovery",
            "Config.Msi"
        };



        public static string root = @"" + Comandos.driveverifica(null);

        public static List<string> defaultPriorities = new List<string>
        {
            Path.Combine(root, @"Users\"+Environment.UserName+@"\AppData\Roaming\Vados"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Favorites"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Desktop"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Documents"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Downloads"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Pictures"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Music"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Videos"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\OneDrive"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Searches"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Contacts"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Links"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\Saved Games"),
            Path.Combine(root, @"Users\"+Environment.UserName+@"\3D Objects"),
            //Path.Combine(root, @"Users\"+Environment.UserName+@""),

            Path.Combine(root),
        };

        public static List<string> exePriorities = new List<string>
        {
            DefaultFolder,
            @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs",
            @"C:\Program Files",
            @"C:\Program Files (x86)",

            //Path.Combine(root),
        };

        #endregion


        //Inicializar user controls
        public static UserControlHome userControlHome = new UserControlHome();
        public static UserControlSettings userControlSettings = new UserControlSettings();
        public static UserControlManual userControlManual = new UserControlManual();

      
        //Desenhar retângulo arredondado
        public static GraphicsPath RoundedRectangle(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2;

            //Canto cima esquerda
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            //Linha cima
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            //Canto cima direitaz
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            //Linha direita
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            //Canto baixo direita
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            //Linha baixo
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            //Canto baixo esquerda
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            //Linha esquerda
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);

            path.CloseFigure();
            return path;
        }


        //Checar se um ponto está dentro de um determinado retângulo
        public static bool InsideRectangle(Point point, RectangleF rect)
        {
            if (point.X < rect.X) { return false; }
            if (point.Y < rect.Y) { return false; }
            if (point.X > rect.X + rect.Width) { return false; }
            if (point.Y > rect.Y + rect.Height) { return false; }

            return true;
        }


        //Definir altura da textbox de acordo com o conteúdo
        public static void TextBoxFitHeight(RichTextBox textBox, int extraPadding = 2)
        {
            if (textBox == null) return;

            //Manter largura
            int width = textBox.Width - textBox.Padding.Horizontal;

            using (Graphics g = textBox.CreateGraphics())
            {
                SizeF size = g.MeasureString(textBox.Text, textBox.Font, width);
                textBox.Height = (int)Math.Ceiling(size.Height) + extraPadding + textBox.Padding.Vertical;
            }
        }


        public static void LabelFitWidth(Label label, int extraPadding = 2)
        {
            if (label == null) return;

            Graphics g = label.CreateGraphics();
            SizeF size = g.MeasureString(label.Text, label.Font, label.Width);
            label.Width = (int)Math.Ceiling(size.Width) + extraPadding + label.Padding.Horizontal;
        }


        //Mudar brilho de uma cor em porcentagem
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;

            if (correctionFactor < 0)
            {
                //Escurecer
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                //Clarear
                red = red + (255 - red) * correctionFactor;
                green = green + (255 - green) * correctionFactor;
                blue = blue + (255 - blue) * correctionFactor;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }


        //Adiciona texto simples a uma RichTextBox
        public static void AppendPlainText(RichTextBox textBox, string text)
        {
            //Iniciar seleção no fim da string
            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;

            //Resetar formatação
            textBox.SelectionColor = textBox.ForeColor;
            textBox.SelectionFont = textBox.Font;

            //Adicionar texto
            textBox.AppendText(text);
        }

        //Adiciona texto formatado a uma RichTextBox
        public static void AppendFormattedText(RichTextBox textBox, string text, Color color, FontStyle fontStyle)
        {
            //Iniciar seleção no fim da string
            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;

            //Formatar texto
            textBox.SelectionColor = color;
            textBox.SelectionFont = new System.Drawing.Font(textBox.Font, fontStyle);

            //Adicionar texto
            textBox.AppendText(text);
        }

        //Muda apenas a fonte de um rtf (texto formatado)
        public static string RtfChangeFont(string rtf, Font newFont)
        {
            using (var rtb = new RichTextBox())
            {
                rtb.Rtf = rtf;

                //Mudar fonte de cada caractere
                for (int i = 0; i < rtb.TextLength; i++)
                {
                    rtb.Select(i, 1);
                    var currentFont = rtb.SelectionFont;

                    if (currentFont != null)
                    {
                        rtb.SelectionFont = new Font(
                            newFont.FontFamily,
                            newFont.Size,
                            currentFont.Style   //Manter negrito, itálico, etc
                        );
                    }
                }

                rtb.Select(0, 0);
                return rtb.Rtf;
            }
        }
    }
}
