using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Configuration;
using Asperand.IrcBallistic.Worker.Modules.Command.Dependencies;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace Asperand.IrcBallistic.Worker.Modules.Command.Commands
{
    [CommandGroup("yt")]
    public class YoutubeCommand :BaseCommand
    {
        private readonly YoutubeConfig _youtubeConfig;
        public YoutubeCommand(YoutubeConfig youtubeConfig)
        {
            _youtubeConfig = youtubeConfig;
        }

        [Content]
        public string Content { get; set; }
        public override async Task<CommandResult> Execute(CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                return CommandResult.Failed;
            }
            var youTubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApplicationName = _youtubeConfig.Name,
                ApiKey = _youtubeConfig.ApiKey
            });

            var requestYt = youTubeService.Search.List("snippet");
            requestYt.Q = Content;
            requestYt.MaxResults = 20;
            
            var results = await requestYt.ExecuteAsync(token);
            var video = results.Items.First(x => x.Id.Kind == "youtube#video").Id.VideoId;

            await SendMessage($"https://www.youtube.com/watch?v={video}");
            return CommandResult.Success;
        }
    }
}