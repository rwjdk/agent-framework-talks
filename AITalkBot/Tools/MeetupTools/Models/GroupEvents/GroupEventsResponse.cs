using System.Text.Json.Serialization;

namespace AITalkBot.Tools.MeetupTools.Models.GroupEvents;
#nullable disable
public class GroupEventsResponse
{
    [JsonPropertyName("groupByUrlname")]
    public GroupEventRoot Root { get; set; }

    public IReadOnlyList<Event> GetAllEvents()
    {
        List<Event> result = Root.Events.Edges.Select(x => x.Node).ToList();
        return result.OrderByDescending(x => x.Date).ToList();
    }
}