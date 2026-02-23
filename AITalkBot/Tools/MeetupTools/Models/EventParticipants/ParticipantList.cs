using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.EventParticipants;
#nullable disable
public class ParticipantList
{
    [JsonPropertyName("edges")]
    public ParticipantEdge[] Edges { get; set; }
}