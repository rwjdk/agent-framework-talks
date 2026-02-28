using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.EventParticipants;
#nullable disable
public class ParticipantsRoot
{
    [JsonPropertyName("rsvps")]
    public ParticipantList Tickets { get; set; }
}