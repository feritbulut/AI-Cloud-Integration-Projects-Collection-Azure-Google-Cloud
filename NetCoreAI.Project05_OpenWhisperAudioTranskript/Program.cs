using System;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech.Audio;

class Program
{
    static async Task Main(string[] args)
    {
        string speechKey = "API_KEY";
        string region = "swedencentral"; // Azure portalında gördüğün bölge
        string audioFilePath = "ses.wav"; // Ses dosyanın yolu

        var config = SpeechConfig.FromSubscription(speechKey, region);
        config.SpeechRecognitionLanguage = "tr-TR"; // Türkçe tanıma dili

        using var audioInput = AudioConfig.FromWavFileInput(audioFilePath); // WAV daha stabil çalışır
        using var recognizer = new SpeechRecognizer(config, audioInput);

        Console.WriteLine("Ses tanınıyor, lütfen bekleyin...");

        var result = await recognizer.RecognizeOnceAsync();

        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            Console.WriteLine($"Tanınan Metin: {result.Text}");
        }
        else
        {
            Console.WriteLine($"Hata: {result.Reason}");
        }
    }
}
