using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.GroupEvents;
#nullable disable
public class GroupEventRoot
{
    [JsonPropertyName("events")]
    public EventList Events { get; set; }
}