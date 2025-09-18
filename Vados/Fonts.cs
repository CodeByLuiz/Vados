using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vados
{
    internal class Fonts
    {
        private static PrivateFontCollection _fontCollection = new PrivateFontCollection();

        public static Font DarkerRegular { get; private set; }
        public static Font DarkerBold { get; private set; }
        public static Font DarkerExtraBold { get; private set; }
        public static Font DarkerLight { get; private set; }
        public static Font DarkerSemiBold { get; private set; }
        public static Font DarkerMedium { get; private set; }
        public static Font DarkerBlack { get; private set; }

        // Fontes da família Maven Pro
        public static Font MavenRegular { get; private set; }
        public static Font MavenBold { get; private set; }
        public static Font MavenExtraBold { get; private set; }
        public static Font MavenLight { get; private set; }
        public static Font MavenSemiBold { get; private set; }
        public static Font MavenMedium { get; private set; }
        public static Font MavenBlack { get; private set; }

        static Fonts()
        {
            // Carrega as fontes e inicializa as propriedades
            DarkerRegular = LoadFont("Fonts\\DarkerGrotesque-Regular.ttf");
            DarkerBold = LoadFont("Fonts\\DarkerGrotesque-Bold.ttf");
            DarkerExtraBold = LoadFont("Fonts\\DarkerGrotesque-ExtraBold.ttf");
            DarkerLight = LoadFont("Fonts\\DarkerGrotesque-Light.ttf");
            DarkerSemiBold = LoadFont("Fonts\\DarkerGrotesque-SemiBold.ttf");
            DarkerMedium = LoadFont("Fonts\\DarkerGrotesque-Medium.ttf");
            DarkerBlack = LoadFont("Fonts\\DarkerGrotesque-Black.ttf");

            MavenRegular = LoadFont("Fonts\\MavenPro-Regular.ttf");
            MavenBold = LoadFont("Fonts\\MavenPro-Bold.ttf");
            MavenExtraBold = LoadFont("Fonts\\MavenPro-ExtraBold.ttf");
            MavenLight = LoadFont("Fonts\\MavenPro-Light.ttf");
            MavenSemiBold = LoadFont("Fonts\\MavenPro-SemiBold.ttf");
            MavenMedium = LoadFont("Fonts\\MavenPro-Medium.ttf");
            MavenBlack = LoadFont("Fonts\\MavenPro-Black.ttf");
        }




        private static Font LoadFont(string fontpath)
        {


            string fullPath = Path.Combine(Application.StartupPath, fontpath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Fonte não encontrada: " + fullPath);

            _fontCollection.AddFontFile(fullPath);

            // Sempre pega a última adicionada
            return _fontCollection.Families[_fontCollection.Families.Length - 1];
        }


        }
}
