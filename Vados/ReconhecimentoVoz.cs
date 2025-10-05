using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Vosk;
using Newtonsoft.Json.Linq;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Vados
{
    public class ReconhecimentoVoz : IDisposable
    {

        private Model model; // modelo de linguagem a ser carregado
        private VoskRecognizer recognizer; // realiza o reconhecimento de fala
        private WaveInEvent waveIn; // captura o audio do microfone
        private bool isPaused; //indica se esta pausado
        public List<string> hints;

        public event Action<string> OnFinalResult;
        public event Action<string> OnPartialResult;
        private int deviceNumber;

        public ReconhecimentoVoz(string modelPath, List<string> hints_, int deviceNumber = 0)
        {
            try
            {
                hints = hints_;
                this.deviceNumber = deviceNumber;

                if (!Directory.Exists(modelPath))
                    throw new DirectoryNotFoundException($"Modelo não encontrado: {modelPath}");

                MessageBox.Show("modelo encontrado");
                Vosk.Vosk.SetLogLevel(0);
                model = new Model(modelPath);
                recognizer = new VoskRecognizer(model, 16000.0f);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Start()
        {
            if (waveIn != null) return;

            waveIn = new WaveInEvent
            {
                DeviceNumber = deviceNumber,
                WaveFormat = new WaveFormat(16000, 1)
            };

            waveIn.DataAvailable += OnDataAvailable;

            try
            {
                waveIn.StartRecording();
            }
            catch (Exception ex)
            {
                waveIn.Dispose();
                waveIn = null;
                throw new InvalidOperationException("Erro ao iniciar captura de áudio", ex);
            }

            isPaused = false;
        }

        public void Pause() => isPaused = true;

        public void Resume() => isPaused = false;

        public void Stop()
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.DataAvailable -= OnDataAvailable;
                waveIn.Dispose();
                waveIn = null;
            }

            //recognizer?.Dispose();
            //recognizer = new VoskRecognizer(model, 16000.0f);
            isPaused = false;
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (isPaused) return;

            //Volume do áudio recebido
            float sum = 0;
            int bytesPerSample = 2; // 16-bit PCM
            int samplesRecorded = e.BytesRecorded / bytesPerSample; //Quantidade de amostras de áudio

            for (int i = 0; i < e.BytesRecorded; i += bytesPerSample)
            {
                short sample = BitConverter.ToInt16(e.Buffer, i);
                sum += sample * sample;
            }

            double rms = Math.Sqrt(sum / samplesRecorded); //Amplitude das amostras
            double db = 20 * Math.Log10(rms / short.MaxValue + double.Epsilon); //Valor em decibeis

            Global.decibeis = db.ToString();


            //Reconhecer fala final
            if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
            {
                var json = JObject.Parse(recognizer.Result());
                var text = json["text"]?.ToString();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    OnFinalResult?.Invoke(text);
                    //MessageBox.Show("final:" + text);
                } 
            }

            //Reconhecer fala parcial
            else
            {
                var partial = JObject.Parse(recognizer.PartialResult())["partial"]?.ToString();
                
                if (!string.IsNullOrWhiteSpace(partial))
                {
                    OnPartialResult?.Invoke(partial);
                    //MessageBox.Show("parcial:" + partial);
                }
            }
        }


        public void Dispose()
        {
            Stop();
            recognizer?.Dispose();
            recognizer = null;

            model?.Dispose();
            model = null;
        }
    }
}
