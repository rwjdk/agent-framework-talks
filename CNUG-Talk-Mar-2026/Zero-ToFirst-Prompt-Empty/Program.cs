using Microsoft.Extensions.Configuration;

Console.Clear();

//Step 1: Make an ai.azure.com resource
//Step 2: Get the Endpoint and API Key (Remove the '/api/projects/<project_name>' of the endpoint)
//Step 3: Deploy a model (gpt-4.1-mini)

IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string endpoint = config["Endpoint"]!;
string apiKey = config["ApiKey"]!;
string model = "gpt-4.1-mini";

//Step 4: Add two NuGet packages (Azure.AI.OpenAI + Microsoft.Agents.AI.OpenAI) [Note that all AI Packages are pre-release]

//Step 5: Create an AzureOpenAI Client

//Step 6: On the client create a ChatClient (defining the model to use) and convert it to a Microsoft Agent Framework 'ChatClientAgent' (or the more generic AIAgent)

//Step 7: Ask the Agent a question

//Step 8: Try a streaming answer

//Step 9: Lets rotate the test-project keys so my account is safe