using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;



namespace Vados
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Comandos.CriarPastaPadrao();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}