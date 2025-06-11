using System;
using System.Collections.Generic;
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
    }
}
