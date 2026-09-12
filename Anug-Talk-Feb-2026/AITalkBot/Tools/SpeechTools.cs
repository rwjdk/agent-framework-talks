using AgentFrameworkToolkit.Tools;
using OpenAI.Audio;
using System.ClientModel;
using NAudio.Wave;
using OpenAI;

namespace AITalkBot.Tools;

public class SpeechTools(OpenAIClient azureOpenAIClient)
{
    [AITool("speak_text")]
    public void Speak(string introduction)
    {
        AudioClient audioClient = azureOpenAIClient.GetAudioClient("tts");
        GeneratedSpeechVoice voice = new("echo"); //nova, shimmer, echo, onyx, fable, alloy'.
        ClientResult<BinaryData> result = audioClient.GenerateSpeech(introduction, voice);

        byte[] bytes = result.Value.ToArray();

        //Save to Disk
        File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "test.mp3"), bytes);

        //Play directly (NAudio nuget package (Windows Only))
        WaveStream waveStream = new Mp3FileReader(new MemoryStream(bytes));
        IWavePlayer player = new WaveOut();
        player.Init(waveStream);
        player.Play();

    }
}
