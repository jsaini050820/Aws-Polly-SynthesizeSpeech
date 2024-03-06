using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using System;
using System.IO;
using System.Threading.Tasks;
class Program {
    static async Task Main() 
    { 
        string accessKey = "YOUR_ACCESS_KEY";
        string secretKey = "YOUR_SECRET_KEY";
        string region = "YOUR_AWS_REGION"; 
        var textArray = new string[] { "Hello,first sample text.", "second sample text.", "And at last comes the third one." }; 
        var outputFiles = await SynthesizeSpeechAsync(accessKey, secretKey, region, textArray);
        Console.WriteLine("All requests Processed. Output files:"); 
        foreach (var file in outputFiles) 
        { 
            Console.WriteLine(file); 
        } 
    } 
    static async Task<string[]> SynthesizeSpeechAsync(string accessKey, string secretKey, string region, string[] textArray) 
    { 
        var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
        var pollyClient = new AmazonPollyClient(credentials, RegionEndpoint.GetBySystemName(region));
        var outputFiles = new string[textArray.Length];
        for (int i = 0; i < textArray.Length; i++) 
        { 
            var request = new SynthesizeSpeechRequest 
            { 
                OutputFormat = OutputFormat.Mp3, Text = textArray[i], VoiceId = VoiceId.Kendra };
                var response = await pollyClient.SynthesizeSpeechAsync(request);
            string outputFile = $"Audio_{i + 1}.mp3"; 
            await using (var fileStream = File.Create(outputFile)) 
            { 
                await response.AudioStream.CopyToAsync(fileStream);
            } 
            outputFiles[i] = outputFile; 
        }
        return outputFiles; 
    } 
}