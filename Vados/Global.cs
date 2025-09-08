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
        public static string DefaultFolder = Comandos.CriarPastaPadrao();

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





        //Inicializar user controls
        public static UserControlHome userControlHome = new UserControlHome();
        public static UserControlSettings userControlSettings = new UserControlSettings();



      
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

    }
}
