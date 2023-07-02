namespace OutlookEmailRules.Core;

using Microsoft.Graph.Models;

public class FolderWithRules
{
    public FolderWithRules(MailFolder folder, List<MessageRule> messageRules)
    {
        Folder = folder;
        Rules = messageRules;
        SubFolders = new List<FolderWithRules>(folder.ChildFolderCount ?? 0);
    }

    public MailFolder Folder { get; set; }

    public List<MessageRule> Rules { get; set; }

    public List<FolderWithRules> SubFolders { get; set; };
}
