using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vados
{
    //Classe dos argumentos da função de trocar de página
    //(Apenas qual o user control especificado)
    public class LoadPageEventArgs : EventArgs
    {
        public UserControl userControl;

        public LoadPageEventArgs(UserControl newUserControl)
        {
            userControl = newUserControl;
        }
    }
}
