using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using System.Text.Json;
using NAudio.Wave;



namespace Vados
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            ApplicationConfiguration.Initialize();

            //Criar pasta padrão para os comandos
            string folderPath = Comandos.CriarPastaPadrao();
            Global.DefaultFolder = folderPath;

            #region RECONHECEDOR DE VOZ

            //Definir caminho do modelo do reconhecimento de voz
            Global.VoiceRecognitionFolder = Path.Combine(Application.StartupPath, "Models", "ggml-small.bin");

            //Definir palavras priorizadas
            List<string> hints = new List<string>() { };

            //Lista de palavras priorizadas
            hints = hints.Concat(Comandos.allCommands)
                        .Concat(Comandos.allObjects)
                        //.Concat(Comandos.allExtensionsWords)
                        //.Concat(Comandos.extraSpeechWords)
                        //.Concat(Comandos.allSizeUnitWords)
                        //.Concat(Comandos.allSizeModifierWords)
                        //.Concat(Comandos.sizeWords)
                        //.Concat(Comandos.insideWords)
                        //.Concat(Comandos.fromWords)
                        //.Concat(Comandos.namingWords)
                        //.Concat(Comandos.amountWords)
                        .ToList();

            //Inicializar reconhecedor de voz
            Global.VoiceRecognizer = new WhisperRecognizer(Global.VoiceRecognitionFolder, hints, WaveIn.DeviceCount - 1);
            await Global.VoiceRecognizer.Initialize();

            #endregion


            //Iniciar aplicativo no Form1
            Application.Run(new Form1());
        }
    }
}