using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Configuration;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("yt")]
    public class YoutubeCommand :BaseCommand
    {
        private readonly YoutubeConfig _youtubeConfig;
        public YoutubeCommand(YoutubeConfig youtubeConfig)
        {
            _youtubeConfig = youtubeConfig;
        }
        public override async Task<CommandExecutionResult> Execute(CommandRequest request, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return CommandExecutionResult.Failed;
            }
            var youTubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApplicationName = _youtubeConfig.Name,
                ApiKey = _youtubeConfig.ApiKey
            });

            var requestYt = youTubeService.Search.List("snippet");
            requestYt.Q = request.Content;
            requestYt.MaxResults = 20;
            
            var results = await requestYt.ExecuteAsync(token);
            var video = results.Items.First(x => x.Id.Kind == "youtube#video").Id.VideoId;

            await SendMessage($"https://www.youtube.com/watch?v={video}");
            return CommandExecutionResult.Success;
        }
    }
}