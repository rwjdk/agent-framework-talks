using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.GroupEvents;
#nullable disable
public class EventList
{
    [JsonPropertyName("edges")]
    public EventEdge[] Edges { get; set; }
}