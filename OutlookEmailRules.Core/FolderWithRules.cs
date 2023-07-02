namespace OutlookEmailRules.Core;

using Microsoft.Graph.Models;

public class FolderWithRules
{
    public FolderWithRules(MailFolder folder, List<MessageRule> messageRules)
    {
        Folder = folder;
        Rules = messageRules;
    }

    public MailFolder Folder { get; set; }

    public List<MessageRule> Rules { get; set; }

    public List<FolderWithRules> SubFolders { get; set; } = new();
}
