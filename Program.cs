using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    private DiscordSocketClient _client;
    private InteractionService _interactions;
    private IServiceProvider _services;
    private string[] _quotes;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged
        });

        _interactions = new InteractionService(_client.Rest);

        _client.Log += Log;
        _client.Ready += ReadyAsync;
        _client.InteractionCreated += HandleInteraction;

        // Load quotes from file
        _quotes = File.ReadAllLines("ShipTVQuotes.txt")
                      .Where(l => !string.IsNullOrWhiteSpace(l))
                      .Select(l => l.Trim('"'))
                      .ToArray();

        // Get bot token from environment variable (recommended)
        var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
        if (string.IsNullOrWhiteSpace(token))
        {
            Console.WriteLine("Missing bot token. Set DISCORD_TOKEN as an environment variable.");
            return;
        }

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task ReadyAsync()
    {
        var cmd = new Discord.SlashCommandBuilder()
            .WithName("democracy")
            .WithDescription("Receive wisdom from the Ship TV.");

        // 1) Register globally (works in any server the bot is in; may take a while to appear)
        try
        {
            var global = await _client.Rest.CreateGlobalCommand(cmd.Build());
            Console.WriteLine($"Upserted GLOBAL /democracy (id {global.Id})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Global upsert error: {ex}");
        }

        // 2) Register per-guild for INSTANT testing (guild overrides global in that server)
        foreach (var guild in _client.Guilds)
        {
            try
            {
                var created = await guild.CreateApplicationCommandAsync(cmd.Build());
                Console.WriteLine($"Upserted /democracy in {guild.Name} (id {created.Id})");

                var existing = await guild.GetApplicationCommandsAsync();
                Console.WriteLine($"Guild {guild.Name} now has {existing.Count} app command(s):");
                foreach (var c in existing)
                    Console.WriteLine($" - {c.Name} (id {c.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Guild upsert error in {guild.Name}: {ex}");
            }
        }
    }


    private async Task HandleInteraction(SocketInteraction interaction)
    {
        if (interaction is SocketSlashCommand command)
        {
            if (command.CommandName == "democracy")
            {
                var random = new Random();
                var quote = _quotes[random.Next(_quotes.Length)];

                var embed = new EmbedBuilder()
                    .WithTitle("📺 Super Earth Broadcast")
                    .WithDescription(quote)
                    .WithColor(Color.Red)
                    .Build();

                await command.RespondAsync(embed: embed);
            }
        }
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
