using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using System.Text.Json;



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
            Global.DefaultFolder = Comandos.CriarPastaPadrao();


            #region RECONHECEDOR DE VOZ

            //Definir caminho do modelo do reconhecimento de voz
            //Global.VoiceRecognitionFolder = (await Comandos.SearchPaths("vosk-model-small-pt-0.3", true).ConfigureAwait(false)).FirstOrDefault();

            Global.VoiceRecognitionFolder = Path.Combine(Application.StartupPath, "Models", "ggml-base.bin");
            MessageBox.Show(Global.VoiceRecognitionFolder);

            //Definir palavras priorizadas
            List<string> hints = new List<string>() { };

            //Lista de palavras priorizadas
            hints = hints.Concat(Comandos.allCommands)
                        .Concat(Comandos.allObjects)
                        //.Concat(Comandos.allSizeUnitWords)
                        .Concat(Comandos.allSizeModifierWords)
                        .Concat(Comandos.allExtensionsWords)
                        .Concat(Comandos.sizeWords)
                        .Concat(Comandos.startWords)
                        .Concat(Comandos.insideWords)
                        .Concat(Comandos.fromWords)
                        .Concat(Comandos.namingWords)
                        .Concat(Comandos.amountWords)
                        .Concat(Comandos.extraSpeechWords)
                        .ToList();


            //Inicializar reconhecedor de voz
            Global.VoiceRecognizer = new WhisperRecognizer(Global.VoiceRecognitionFolder, hints);
            await Global.VoiceRecognizer.Initialize();

            #endregion


            //Iniciar aplicativo no Form1
            Application.Run(new Form1());
        }
    }
}