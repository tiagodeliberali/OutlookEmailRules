namespace OutlookEmailRules.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Graph.Models;

public interface IGraphApiService
{
    Task<List<MailFolder>> GetFolderChildrenAsync(string folderId);
    Task<List<MailFolder>> GetFoldersAsync();
    Task<List<MessageRule>> GetRulesAsync();
}