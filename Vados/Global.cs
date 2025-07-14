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
        //Inicializar user controls
        public static UserControlHome userControlHome = new UserControlHome();
        public static UserControlSettings userControlSettings = new UserControlSettings();

        public static string DefaultFolder = @"C:\Vados\";


        //Desenhar retângulo arredondado
        public static GraphicsPath RoundedRectangle(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2;

            //Canto cima esquera
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            //Linha cima
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            //Canto cima direita
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
    }
}
