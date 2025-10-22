using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vados.BancoDeDados;
using System.Text.RegularExpressions;
//using static System.Net.Mime.MediaTypeNames;

namespace Vados
{
    //Variáveis que podem ser acessadas de qualquer lugar
    public static class Global
    {
        public static string DefaultFolder = "";            //Pasta padrão para os comandos
        public static string VoiceRecognitionFolder = "";   //Pasta do modelo de reconhecimento de voz
        public static WhisperRecognizer VoiceRecognizer = null;    //Objeto do reconhecedor de voz
        public static string decibeis = "nao iniciado";
        public static List<HistoryEntry> entradas = new List<HistoryEntry>();


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



        public static string driverPath = @"" + Comandos.DriveGetFirst();
        //public static string driverPath = "";

        public static List<string> defaultPriorities = new List<string>
        {
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Documents\Vados"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Favorites"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Desktop"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Documents"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Downloads"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Pictures"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Music"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Videos"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\OneDrive"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Searches"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Contacts"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Links"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\Saved Games"),
            Path.Combine(driverPath, @"Users\" + Environment.UserName + @"\3D Objects"),
            //Path.Combine(driverPath, @"Users\"+Environment.UserName+@""),

            Path.Combine(driverPath),
        };

        public static List<string> exePriorities = new List<string>
        {
            DefaultFolder,
            @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs",
            @"C:\Program Files",
            @"C:\Program Files (x86)",

            //Path.Combine(driverPath),
        };

        #endregion

        //Adiciona as entradas do banco de dados para uma lista 👎👎
          public static void InitializeDb()
          {
            using (var db = new BancoDeDados.DbConnection())
            {
                db.Database.EnsureCreated();
                entradas = db.Historico
                             .OrderByDescending(e => e.Data)
                             .ToList();
            }
          }

//Inicializar user controls
public static UserControlHome userControlHome;
        public static UserControlSettings userControlSettings;
        public static UserControlManual userControlManual;
      

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


        //Desenhar sombra de qualquer formato
        public static void DrawShadow(PaintEventArgs eventArgs, GraphicsPath path, Color shadowColor, PointF focusScales)
        {
            PathGradientBrush pathBrush = new PathGradientBrush(path);

            pathBrush.CenterColor = Color.FromArgb(255, shadowColor);
            pathBrush.SurroundColors = [Color.FromArgb(0, shadowColor)];
            pathBrush.FocusScales = focusScales;

            eventArgs.Graphics.FillPath(pathBrush, path);
        }
        
        //Criar a sombra de uma imagem
        public static Bitmap ImageCreateShadow(Image image, Color shadowColor, float opacity = 0.5f, int blurRadius = 10)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap shadowImage = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(shadowImage);
            g.Clear(Color.Transparent);

            //Pintar a imagem da cor da sombra
            Bitmap alphaMask = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = ((Bitmap)image).GetPixel(x, y);
                    alphaMask.SetPixel(x, y, Color.FromArgb(pixel.A, shadowColor.R, shadowColor.G, shadowColor.B));
                }
            }

            //Definir transpar"encia
            ImageAttributes attributes = new ImageAttributes();
            ColorMatrix matrix = new ColorMatrix(new float[][]
            {
                [0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0],
                [0, 0, 0, opacity, 0],
                [shadowColor.R / 255f, shadowColor.G / 255f, shadowColor.B / 255f, 0, 1]
            });

            attributes.SetColorMatrix(matrix);

            //Desenhar imagem
            g.DrawImage(alphaMask, new Rectangle(0, 0, shadowImage.Width, shadowImage.Height), 0, 0, shadowImage.Width, shadowImage.Height, GraphicsUnit.Pixel, attributes);

            //Adicionar blur à imagem
            return GaussianBlur(shadowImage, blurRadius);
        }


        //Criar uma imagem com blur
        private static Bitmap GaussianBlur(Bitmap image, int radius)
        {
            if (radius < 1) return image;

            Bitmap blurred = new Bitmap(image.Width, image.Height);
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            // Lock bits
            BitmapData srcData = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = blurred.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int bytes = srcData.Stride * srcData.Height;
            byte[] pixelBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, bytes);
            image.UnlockBits(srcData);

            int w = image.Width;
            int h = image.Height;
            int stride = srcData.Stride;
            int[] kernel = CreateGaussianKernel(radius);
            int kernelSum = 0;
            foreach (int k in kernel) kernelSum += k;

            // Horizontal pass
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int blue = 0, green = 0, red = 0, alpha = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        int offsetX = Math.Clamp(x + k, 0, w - 1);
                        int index = y * stride + offsetX * 4;
                        int weight = kernel[k + radius];

                        blue += pixelBuffer[index] * weight;
                        green += pixelBuffer[index + 1] * weight;
                        red += pixelBuffer[index + 2] * weight;
                        alpha += pixelBuffer[index + 3] * weight;
                    }
                    int i = y * stride + x * 4;
                    resultBuffer[i] = (byte)(blue / kernelSum);
                    resultBuffer[i + 1] = (byte)(green / kernelSum);
                    resultBuffer[i + 2] = (byte)(red / kernelSum);
                    resultBuffer[i + 3] = (byte)(alpha / kernelSum);
                }
            }

            // Vertical pass
            byte[] temp = (byte[])resultBuffer.Clone();
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int blue = 0, green = 0, red = 0, alpha = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        int offsetY = Math.Clamp(y + k, 0, h - 1);
                        int index = offsetY * stride + x * 4;
                        int weight = kernel[k + radius];

                        blue += temp[index] * weight;
                        green += temp[index + 1] * weight;
                        red += temp[index + 2] * weight;
                        alpha += temp[index + 3] * weight;
                    }
                    int i = y * stride + x * 4;
                    resultBuffer[i] = (byte)(blue / kernelSum);
                    resultBuffer[i + 1] = (byte)(green / kernelSum);
                    resultBuffer[i + 2] = (byte)(red / kernelSum);
                    resultBuffer[i + 3] = (byte)(alpha / kernelSum);
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, bytes);
            blurred.UnlockBits(dstData);

            return blurred;
        }

        private static int[] CreateGaussianKernel(int radius)
        {
            int[] kernel = new int[radius * 2 + 1];
            double sigma = radius / 2.0;
            double sum = 0;

            for (int i = -radius; i <= radius; i++)
            {
                double val = Math.Exp(-(i * i) / (2 * sigma * sigma));
                kernel[i + radius] = (int)(val * 1000);
                sum += val;
            }

            // Normalize
            for (int i = 0; i < kernel.Length; i++)
                kernel[i] = (int)(kernel[i] / sum * 1000);

            return kernel;
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
            Graphics g = textBox.CreateGraphics();
            SizeF size = g.MeasureString(textBox.Text, textBox.Font, width);
            textBox.Height = (int)Math.Ceiling(size.Height) + extraPadding + textBox.Padding.Vertical;
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


        public static Image ImageChangeBrightness(Image image, float correctionFactor)
        {
            //Normalizar brilho
            float brightness = Math.Clamp(correctionFactor, -1f, 1f);

            Bitmap newImage = new Bitmap(image.Width, image.Height);

            Graphics g = Graphics.FromImage(newImage);
            float scale = 1f;
            float offset = 0f;

            //Escurecer
            if (brightness < 0)
            {
                scale = 1f + brightness;    //Reduzir intensidade
                offset = 0f;
            }
            //Clarear
            else if (brightness > 0)
            {
                scale = 1f - brightness;    //Reduzir contraste
                offset = brightness * 255f;
            }

            //Definir cor nova
            float[][] ptsArray = {
                new float[] { scale, 0,     0,     0, 0 },   //Vermelho
                new float[] { 0,     scale, 0,     0, 0 },   //Verde
                new float[] { 0,     0,     scale, 0, 0 },   //Azul
                new float[] { 0,     0,     0,     1, 0 },   //Transparência (é mantida)
                new float[] { offset / 255f, offset / 255f, offset / 255f, 0, 1 }
            };

            var matrix = new System.Drawing.Imaging.ColorMatrix(ptsArray);
            var attributes = new System.Drawing.Imaging.ImageAttributes();
            attributes.SetColorMatrix(matrix);

            //Desenhar imagem
            g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

            return newImage;
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
        public static void AppendFormattedText(RichTextBox textBox, string text, Color color, Font font)
        {
            //Iniciar seleção no fim da string
            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;

            //Formatar texto
            textBox.SelectionColor = color;
            textBox.SelectionFont = font;

            //Adicionar texto
            textBox.AppendText(text);
        }

        //Muda apenas a fonte de um rtf (texto formatado)
        public static string RtfChangeFont(string rtf, Font newRegularFont, Font newBoldFont = null)
        {
            var rtb = new RichTextBox();
            rtb.Rtf = rtf;

            if (newBoldFont == null)
                newBoldFont = new Font(newRegularFont, FontStyle.Bold);

            //Mudar fonte de cada caractere
            for (int i = 0; i < rtb.TextLength; i++)
            {
                rtb.Select(i, 1);
                var currentFont = rtb.SelectionFont;

                if (currentFont != null)
                {
                    //Negrito
                    if (currentFont.Style == FontStyle.Bold)
                    {
                        rtb.SelectionFont = new Font(
                            newBoldFont.FontFamily,
                            newBoldFont.Size,
                            newBoldFont.Style
                        );
                    }
                    //Normal
                    else
                    {
                        rtb.SelectionFont = new Font(
                            newRegularFont.FontFamily,
                            newRegularFont.Size,
                            currentFont.Style
                        );
                    }
                }
            }

            rtb.Select(0, 0);
            return rtb.Rtf;
        }


        //Encontra a posição do primeiro grupo identificado pelo extrator
        public static int FindFirstGroupIndex(GroupCollection group)
        {
            for (var i = 1; i < group.Count; i++)
            {
                MessageBox.Show("valor: " + group[i].Value);
                if (group[i].Success)
                {
                    MessageBox.Show("sucesso");
                    return group[i].Index;
                }
            }

            return -1;
        }
    }
}
