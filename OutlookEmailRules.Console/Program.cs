using System.Text.Json;

using Microsoft.Graph.Models;

using OutlookEmailRules.Core;

Console.WriteLine("Type token:");

// Get the access token from the command line
var token = Console.ReadLine();

var apiService = new GraphApiService(token);
var ruleService = new RuleService(apiService);

var foldersWithRules = await ruleService.GetFoldersWithRules();

Console.Clear();

// iterate through the folders and print the folder name and the rules
Console.WriteLine("Folders and rules");

PrintFoldersAndRules(foldersWithRules);

void PrintFoldersAndRules(IEnumerable<FolderWithRules> foldersWithRules)
{
    foreach (var folderWithRules in foldersWithRules)
    {
        if (folderWithRules.Rules.Any())
        {
            Console.WriteLine(folderWithRules.Folder.DisplayName);

            Console.WriteLine(" ## Rules");
            foreach (var rule in folderWithRules.Rules)
            {
                PrintRuleSummary(rule);
            }

            Console.WriteLine();
        }

        if (folderWithRules.SubFolders.Any())
        {
            PrintFoldersAndRules(folderWithRules.SubFolders);
        }
    }
}

void PrintRuleSummary(MessageRule rule)
{
    Console.WriteLine($"Rule: {rule.DisplayName}");
    // convert rule into json string and print to console
    Console.WriteLine(JsonSerializer.Serialize(rule.Conditions, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault }));

    Console.WriteLine();
}
