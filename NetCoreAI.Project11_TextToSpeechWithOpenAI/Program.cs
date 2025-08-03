using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

class Program
{
    static async Task Main(string[] args)
    {
        var speechConfig = SpeechConfig.FromSubscription("API_KEY", "swedencentral");
        speechConfig.SpeechSynthesisVoiceName = "tr-TR-AhmetNeural"; // Turkish man voice

        Console.WriteLine("Lütfen bir metin girin:");
        var text = Console.ReadLine();

        using (var synthesizer = new SpeechSynthesizer(speechConfig))
        {
            var result = await synthesizer.SpeakTextAsync(text);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Console.WriteLine("Ses başarıyla oluşturuldu.");
            }
            else
            {
                Console.WriteLine($"Hata: {result.Reason}");
            }
        }

        //saving audio to a file

        //speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio16Khz32KBitRateMonoMp3);
        //using (var audioConfig = AudioConfig.FromWavFileOutput("C:\\Users\\Arif\\Desktop\\cikti.mp3"))
        //using (var synthesizer = new SpeechSynthesizer(speechConfig, audioConfig))
        //{
        //    // Additional logic for saving audio can be added here if needed
        //}
    }
}
