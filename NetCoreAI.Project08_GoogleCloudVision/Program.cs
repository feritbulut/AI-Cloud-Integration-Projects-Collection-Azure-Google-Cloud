using Google.Cloud.Vision.V1;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Resim yolu: ");
        string imagePath = Console.ReadLine();
        Console.WriteLine();
        

        string credentialPath = @"";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

        try
        {
            var client = ImageAnnotatorClient.Create();

            var image = Image.FromFile(imagePath);
            var response = client.DetectText(image);
            Console.WriteLine("Resimden Okunan Metin...");
            Console.WriteLine("--------------------------------------------------");
            if (response.Count > 0 && !string.IsNullOrEmpty(response[0].Description))
            {
                Console.WriteLine(response[0].Description);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bir hata oluştu {ex.Message}");


        }
    }
}