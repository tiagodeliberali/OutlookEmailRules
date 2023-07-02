namespace OutlookEmailRules.Core;

using Microsoft.Graph.Models;

public class RuleService
{
    private readonly IGraphApiService graphService;

    public RuleService(IGraphApiService graphService)
    {
        this.graphService = graphService;
    }

    public async Task<List<FolderWithRules>> GetFoldersWithRules()
    {
        var rules = await graphService.GetRulesAsync();
        var folders = await graphService.GetFoldersAsync();
        var foldersWithRules = new List<FolderWithRules>();

        foreach (var folder in folders)
        {
            var folderWithRules = await BuildFolder(folder, rules);
            foldersWithRules.Add(folderWithRules);
        }

        return foldersWithRules;
    }

    private async Task<FolderWithRules> BuildFolder(MailFolder folder, List<MessageRule> rules)
    {
        var folderWithRules = new FolderWithRules(folder, GetRules(rules, folder));

        if (folder.ChildFolderCount > 0)
        {
            var subFolders = await graphService.GetFolderChildrenAsync(folder.Id!);
            foreach (var subFolder in subFolders)
            {
                var subFolderWithRules = await BuildFolder(subFolder, rules);
                folderWithRules.SubFolders.Add(subFolderWithRules);
            }
        }

        return folderWithRules;
    }

    private List<MessageRule> GetRules(List<MessageRule> rules, MailFolder folder) =>
        rules.Where(x => x.Actions?.CopyToFolder == folder.Id
                    || x.Actions?.MoveToFolder == folder.Id).ToList();
}
