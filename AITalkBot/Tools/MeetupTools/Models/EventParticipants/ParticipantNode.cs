using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.EventParticipants;
#nullable disable
public class ParticipantNode
{
    [JsonPropertyName("member")]
    public Participant User { get; set; }
}