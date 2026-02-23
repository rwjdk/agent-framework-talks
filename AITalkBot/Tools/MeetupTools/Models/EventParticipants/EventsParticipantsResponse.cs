using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.EventParticipants;
#nullable disable
public class EventsParticipantsResponse
{
    [JsonPropertyName("event")]
    public ParticipantsRoot Root { get; set; }
}