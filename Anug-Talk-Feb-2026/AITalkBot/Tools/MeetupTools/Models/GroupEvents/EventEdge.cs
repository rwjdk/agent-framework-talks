using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.GroupEvents;
#nullable disable
public class EventEdge
{
    [JsonPropertyName("node")]
    public Event Node { get; set; }
}