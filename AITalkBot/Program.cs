using AgentFrameworkToolkit.AzureOpenAI;
using AgentFrameworkToolkit.OpenAI;
using AgentFrameworkToolkit.Tools;
using AgentFrameworkToolkit.Tools.ModelContextProtocol;
using AITalkBot;
using AITalkBot.Tools;
using AITalkBot.Tools.MeetupTools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Secrets;

Secrets.Secrets secrets = SecretsManager.GetSecrets();

AzureOpenAIConnection connection = new()
{
    Endpoint = secrets.AzureOpenAiEndpoint,
    ApiKey = secrets.AzureOpenAiKey
};

AIToolsFactory toolsFactory = new();

AzureOpenAIAgentFactory agentFactory = new(connection);

await using McpClientTools anugTools = await toolsFactory.GetToolsFromRemoteMcpAsync("https://anugtoolsbackend.azurewebsites.net/runtime/webhooks/mcp?Code=Tools");

AzureOpenAIAgent anugAgent = agentFactory.CreateAgent(new AgentOptions
{
    Model = OpenAIChatModels.Gpt5Mini,
    Instructions = """
                   You are the co-host of an AI Talks about 'Microsoft Agent Framework' (EventID: 313001712) given by Rasmus Wulff Jensen
                   
                   You are spoken to by Rasmus (the presenter who is a little nervous like always when he gives talk, to pep him up and be helpful)
                   
                   Do not suggest next actions. Only answer
                   Date format: dd. MMM yyyy
                   Previous EventID about Semantic Kernel: 304079876 

                   # Your Skills
                   
                   ## Pleasantries
                   - You can smalltalk, but do not inform what you can do unless asked to do so.
                   
                   ## Give Presentation of todays topic
                   - Only use this if you are asked to give a presentation
                   - Use tool 'speak_text' to tell people that they are about to hear a talk about Microsoft Agent Framework, how to get started and about the 4 main concepts of AI (Chat, Tools, Structured Output and Retrieval Augmented Generation) [do not explain what it is beyond that]
                   - Be friendly, introduce the speaker and end with that you hope people will have a good time.
                   - When presenting only reply with "On it!"
                   
                   ## Information about Meetup Events
                   1. Look the groups event on meet-up 
                      - When you display an event, show name, date
                      - When you display a participants, write the number of participants at the top and then a comma-separate them in a long paragraph; do not use bullets.
                   2. Draw the JetBrains Sponsor Price (use tool 'DrawWinner') [Do not use the HTTP Tools for this task]
                   """,
    Tools = [
        ..anugTools.Tools, 
        ..toolsFactory.GetTools(typeof(MeetupTools)),
        ..toolsFactory.GetTools(new SpeechTools(connection.GetClient())),
        ..toolsFactory.GetRandomTools()
    ],
    RawToolCallDetails = details =>
    {
        Utils.WriteLineYellow($"- Tool Call: {details.Context.Function.Name}");
    }
});

AgentSession session = await anugAgent.CreateSessionAsync();

while (true)
{
    Console.Write("> ");
    string? input = Console.ReadLine();
    ChatMessage message = new(ChatRole.User, input);
    AgentResponse response = await anugAgent.RunAsync(message, session);
    Console.WriteLine(response);

    Utils.Separator();
}