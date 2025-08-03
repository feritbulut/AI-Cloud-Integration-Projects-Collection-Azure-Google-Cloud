using System.Speech.Synthesis;
class Progtam
{
    static void Main(string[] args)
    {
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

        speechSynthesizer.Volume = 100;
        speechSynthesizer.Rate = 0;

        Console.Write("metni girin: ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            speechSynthesizer.Speak(input);
        }

    }
}