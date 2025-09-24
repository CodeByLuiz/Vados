using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vados
{
    internal class Fonts
    {
        private static PrivateFontCollection _fontCollection = new PrivateFontCollection();

        static Fonts()
        {
            // Carrega as fontes
            DarkerRegular = LoadFont("Fonts/DarkerGrotesque-Regular.ttf");
            DarkerBold = LoadFont("Fonts\\DarkerGrotesque-Bold.ttf");
            DarkerExtraBold = LoadFont("Fonts\\DarkerGrotesque-ExtraBold.ttf");
            DarkerLight = LoadFont("Fonts\\DarkerGrotesque-Light.ttf");
            DarkerSemiBold = LoadFont("Fonts\\DarkerGrotesque-SemiBold.ttf");
            DarkerMedium = LoadFont("Fonts\\DarkerGrotesque-Medium.ttf");
            DarkerBlack = LoadFont("Fonts\\DarkerGrotesque-Black.ttf");

          
        }

        public static FontFamily DarkerRegular { get; private set; }
        public static FontFamily DarkerBold { get; private set; }
        public static FontFamily DarkerExtraBold { get; private set; }
        public static FontFamily DarkerLight { get; private set; }
        public static FontFamily DarkerSemiBold { get; private set; }
        public static FontFamily DarkerMedium { get; private set; }
        public static FontFamily DarkerBlack { get; private set; }

        // Maven Pro
        public static FontFamily MavenRegular { get; private set; }
        public static FontFamily MavenBold { get; private set; }
        public static FontFamily MavenExtraBold { get; private set; }
        public static FontFamily MavenLight { get; private set; }
        public static FontFamily MavenSemiBold { get; private set; }
        public static FontFamily MavenMedium { get; private set; }
        public static FontFamily MavenBlack { get; private set; }

        

        private static FontFamily LoadFont(string fontpath)
        {
            string fullPath = Path.Combine(Application.StartupPath, fontpath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Fonte não encontrada: " + fullPath);

            _fontCollection.AddFontFile(fullPath);

           
            return _fontCollection.Families[_fontCollection.Families.Length - 1];
        }

       
        public static Font GetFont(FontFamily family, float size)
        {
            return new Font(family, size);
        }
    }
}
