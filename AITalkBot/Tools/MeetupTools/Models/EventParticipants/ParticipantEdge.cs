using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.EventParticipants;
#nullable disable
public class ParticipantEdge
{
    [JsonPropertyName("node")]
    public ParticipantNode Node { get; set; }
}