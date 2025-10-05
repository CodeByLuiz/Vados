using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Whisper.net;
using Newtonsoft.Json.Linq;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Win32;

namespace Vados
{
    public class WhisperRecognizer : IDisposable
    {
        private string modelPath;
        private WhisperFactory model;
        private WhisperProcessor processor;
        private WaveInEvent waveIn;
        private WaveFileWriter waveWriter;
        private MemoryStream audioBuffer;
        public bool isRunning = false;
        public bool isPaused = false;
        private int deviceNumber;
        public List<string> hints;
        public List<double> audioWaveBars = new List<double>();

        private bool hasSpoken = false;
        private DateTime lastSpeechTime;
        private DateTime pauseStartTime;
        private double pausedTime = 0;
        private int speechTimeoutMs = 1500;
        private double silenceDecibels = -60;   //Menor que esse número de decibeis é considerado silêncio
        public event EventHandler _OnSilence;
        public event EventHandler OnSilence
        {
            add { _OnSilence += value; }
            remove { _OnSilence -= value; }
        }

        public bool HasSubscribers => _OnSilence != null;

        public WhisperRecognizer(string modelPath_, List<string> hints_, int deviceNumber_ = 0)
        {
            modelPath = modelPath_;
            hints = hints_;
            deviceNumber = deviceNumber_;
        }


        //Faz download do modelo se não for encontrado no sistema
        private async Task EnsureModel()
        {
            if (!File.Exists(modelPath))
            {
                MessageBox.Show("Baixando modelo de reconhecimento de voz...");

                //Link do arquivo
                var url = "https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-base.bin";

                //Fazer download
                using var client = new HttpClient();
                var data = await client.GetByteArrayAsync(url);

                Directory.CreateDirectory(Path.GetDirectoryName(modelPath)!);   //Criar pasta "Models"
                await File.WriteAllBytesAsync(modelPath, data);

                MessageBox.Show("Model downloaded successfully.");
            }
        }


        //Inicializa o reconhecedor de voz
        public async Task Initialize()
        {
            try
            {
                await EnsureModel();
                model = WhisperFactory.FromPath(modelPath);
                processor = model.CreateBuilder().WithLanguage("pt").Build();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Erro de inicialização: " + ex.Message);
            }
        }


        public void Start()
        {
            if (isRunning)
                return;

            //Resetar ondas de áudio
            audioWaveBars.Clear();

            //Iniciar reconhecimento de voz
            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1)
            };

            audioBuffer = new MemoryStream();
            waveWriter = new WaveFileWriter(audioBuffer, waveIn.WaveFormat);

            waveIn.StartRecording();
            waveIn.DataAvailable += OnDataAvailable;

            isRunning = true;
            isPaused = false;

            pausedTime = 0;
            hasSpoken = false;
            lastSpeechTime = DateTime.Now;
        }

        public void Pause()
        {
            isPaused = true;
            pauseStartTime = DateTime.Now;
        }

        public void Resume()
        {
            isPaused = false;
            pausedTime += (DateTime.Now - pauseStartTime).TotalMilliseconds;
        }

        public async Task<string> Stop()
        {
            if(!isRunning)
                return string.Empty;

            //Parar de gravar o áudio
            waveIn.StopRecording();
            waveIn.Dispose();
            waveIn = null;

            audioBuffer.Position = 0;
            waveWriter?.Flush();

            //Transcrever audio
            string result = "";

            await foreach (var segment in processor.ProcessAsync(audioBuffer))
            {
                result += segment.Text;
            }

            audioBuffer.Dispose();
            audioBuffer = null;

            isRunning = false;
            isPaused = false;

            //Retornar transcrição do áudio
            return result.Trim();
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (isPaused) return;
            if (waveIn == null) return;

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
            audioWaveBars.Add(db);


            //Parar comando se detectar silêncio
            if (db > silenceDecibels)
            {
                hasSpoken = true;
                lastSpeechTime = DateTime.Now;
            }
            else if ((DateTime.Now - lastSpeechTime).TotalMilliseconds - pausedTime > speechTimeoutMs && hasSpoken)
            {
                //Silêncio detectado
                _OnSilence?.Invoke(this, EventArgs.Empty);
            }


            //Escrever bytes de áudio para o buffer
            waveWriter?.Write(e.Buffer, 0, e.BytesRecorded);
        }


        public void Dispose()
        {
            processor?.Dispose();
            model?.Dispose();
            waveIn?.Dispose();
            audioBuffer?.Dispose();
        }
    }
}
