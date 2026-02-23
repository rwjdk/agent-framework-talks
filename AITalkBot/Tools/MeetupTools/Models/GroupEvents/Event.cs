using System.Diagnostics;
using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.GroupEvents;
#nullable disable
[DebuggerDisplay("{Id}: {Title} [{Status}] ({Date})")]
public class Event
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("dateTime")]
    public DateTime Date { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }

    public override string ToString()
    {
        return Title+" ("+Date.ToLongDateString()+")";
    }
}