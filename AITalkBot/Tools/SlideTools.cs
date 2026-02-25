using System.Diagnostics;
using AgentFrameworkToolkit.Tools;

namespace AITalkBot.Tools;

public class SlideTools
{
    [AITool("open_slides")]
    public void OpenSlides()
    {
        Process.Start(
            @"C:\Program Files\Microsoft Office\root\Office16\POWERPNT.EXE", 
            "/s \"C:\\Users\\rasmu\\OneDrive\\ANUG\\AF Talk - Feb 2026\\Anug-Talk-Feb-2026-Slides.pptx\"");

        Utils.WriteLineRed("Terminating AK Talk Bot");
        Environment.Exit(-1);
    }
}