namespace OutlookEmailRules.Tests
{
    using Microsoft.Graph.Models;

    using Moq;

    using OutlookEmailRules.Core;

    public class RuleServiceTests
    {
        private readonly Mock<IGraphApiService> _graphApiServiceMock = new Mock<IGraphApiService>();
        private readonly RuleService _ruleService;

        public RuleServiceTests()
        {
            _ruleService = new RuleService(_graphApiServiceMock.Object);
        }

        [Fact]
        public async Task GetFoldersWithRules_ReturnsFoldersWithRules()
        {
            // Arrange
            var rules = new List<MessageRule>
            {
                new MessageRule { Id = "1", DisplayName = "Rule1", Actions = new MessageRuleActions { CopyToFolder = "folder1" } },
                new MessageRule { Id = "2", DisplayName = "Rule2", Actions = new MessageRuleActions { MoveToFolder = "folder2" } },
                new MessageRule { Id = "3", DisplayName = "Rule3", Actions = new MessageRuleActions { } }
            };
            var folders = new List<MailFolder>
            {
                new MailFolder { Id = "folder1", DisplayName = "Folder1" },
                new MailFolder { Id = "folder2", DisplayName = "Folder2" },
            };
            _graphApiServiceMock.Setup(x => x.GetRulesAsync()).ReturnsAsync(rules);
            _graphApiServiceMock.Setup(x => x.GetFoldersAsync()).ReturnsAsync(folders);
            _graphApiServiceMock.Setup(x => x.GetFolderChildrenAsync("folder1")).ReturnsAsync(new List<MailFolder>());
            _graphApiServiceMock.Setup(x => x.GetFolderChildrenAsync("folder2")).ReturnsAsync(new List<MailFolder>());

            // Act
            var result = await _ruleService.GetFoldersWithRules();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            // folder1 with rule1, folder2 with rule2
            Assert.Equal("Folder1", result[0].Folder.DisplayName);
            Assert.Single(result[0].Rules);
            Assert.Equal("Rule1", result[0].Rules[0].DisplayName);

            Assert.Equal("Folder2", result[1].Folder.DisplayName);
            Assert.Single(result[1].Rules);
            Assert.Equal("Rule2", result[1].Rules[0].DisplayName);
        }


        [Fact]
        public async Task GetFoldersWithRules_ReturnsEmptyList_WhenNoFoldersFound()
        {
            // Arrange
            var rules = new List<MessageRule>();
            var folders = new List<MailFolder>();
            _graphApiServiceMock.Setup(x => x.GetRulesAsync()).ReturnsAsync(rules);
            _graphApiServiceMock.Setup(x => x.GetFoldersAsync()).ReturnsAsync(folders);

            // Act
            var result = await _ruleService.GetFoldersWithRules();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // Add more tests as needed
    }
}
