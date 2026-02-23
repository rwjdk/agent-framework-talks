using AgentFrameworkToolkit.Tools;
using AITalkBot.Tools.MeetupTools.Models.EventParticipants;
using AITalkBot.Tools.MeetupTools.Models.GroupEvents;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

namespace AITalkBot.Tools.MeetupTools;

public class MeetupTools
{
    private readonly GraphQLHttpClient _client = new("https://api.meetup.com/gql-ext", new SystemTextJsonSerializer());

    [AITool("meetup_get_events")]
    public async Task<IReadOnlyList<Event>> GetEvents()
    {
        GraphQLRequest request = new GraphQLRequest
        {
            Query = @"
            {
            groupByUrlname(urlname: ""anugdk"") {       
			events(status: ACTIVE)
      {
          edges {
          node {
              id,
              title    
              status
              dateTime            
          }
      	}
      }			      
		}
            }"
        };

        GraphQLResponse<GroupEventsResponse> response = await _client.SendQueryAsync<GroupEventsResponse>(request);
        return response.Data.GetAllEvents();
    }

    [AITool("meetup_get_participants_for_event")]
    public async Task<IReadOnlyList<Participant>> GetParticipantsForEvent(string eventId)
    {
        GraphQLRequest request = new GraphQLRequest
        {
            Query = @$"
            {{
              		event(id: ""{eventId}""){{
      rsvps(first: 1000) {{
        edges{{
          node {{
            member {{
              name,
              isLeader,
              memberUrl
            }}     
          }}
        }}
      }}
    }}
            }}",
        };

        GraphQLResponse<EventsParticipantsResponse> response = await _client.SendQueryAsync<EventsParticipantsResponse>(request);
        return response.Data.Root.Tickets.Edges.Select(x => x.Node.User).ToList();
    }
}