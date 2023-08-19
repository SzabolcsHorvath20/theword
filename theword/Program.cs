using Discord;
using Discord.WebSocket;
using System.Threading;
using System.Timers;
using Timer = System.Threading.Timer;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();


    private DiscordSocketClient _client;
    private Timer _timer;

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;

        //  You can assign your bot token to a string, and pass that in to connect.
        //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
        var token = Environment.GetEnvironmentVariable("bottoken");

        // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
        // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
        // var token = File.ReadAllText("token.txt");
        // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Replace with your channel id
        ulong channelId = 1142522856043987074;


        // Create a TimeSpan object that represents 24 hours
        var timerInterval = TimeSpan.FromHours(48);

        // Create a DateTime object that represents 9 am of today
        var today9am = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9, 0, 0);

        // Create a DateTime object that represents 9 am of 2 days from now
        var tomorrow9am = today9am.Add(timerInterval);

        // Create a TimeSpan object that represents the due time
        var dueTime = tomorrow9am.Subtract(DateTime.Now);

        // Create a timer that invokes SendDailyMessage every 24 hours starting from tomorrow 9 am
        _timer = new Timer(SendDailyMessage, channelId, dueTime, timerInterval);


        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private string[] quotes = { "The pain you feel is temporary.",
        "The life you live is temporary.",
        "The agony coursing through you is temporary.",
        "The blood in your veins is temporary.", 
        "Do not dally, you were born to serve the Order, only then your death can have a meaning.",
        "Those killed in the way of the Patriarch are alive by the way of Sílluce.",
        "Fight in the cause of Sílluce, and through victory, grant yourself his light in the Afterlife.",
        "Believe in what the Patriarch has revealed to ye, for that is the message of Sílluce.",
        "Life is temporary. The power of our faith is eternal.",
        "Nobody must touch the walls surrounding Hammelynne.",
        "No colors are allowed on public clothing, rather than simple beige, grey or pale brown.",
        "Women and men must wear pants, only the Church and Clergy are allowed to wear robes.",
        "Stray animals are plenty on the street but can be killed or used for food during Famine.",
        "Poems, singing, reciting are not allowed in public unless you are praising the Church.",
        "You must attend every public hanging.",
        "Everything you have abundance of must be given to the Church.",
        "Work diligently for the Church.",
        "What you harvest the Church will divide.",
        "Do not look into the eyes of superiors, especially any member of the Clergy.",
        "Do not stare or look long during mass at Church.",
        "Do not make the priest uncomfortable in any way, with your questions or your look.",
        "Never look up from your feet during public ceremonies.",
        "Speaking elvish without permission is prohibited.",
        "Praising anyone else but the Patriarch is prohibitied.",
        "Gifts only for the Patriarch, not for each other.",
        "Do not stare in public.",
        "Do not swear in public.",
        "You can show emotions unless its around a noble, do not burden them with yourself.",
        "You are allowed to discuss work in public.",
        "Wear shoes if you can afford it, if not, clean your feet before entering a church.",
        "Never appear dirty for Church."};

    private async void SendDailyMessage(object state)
    {
        Random rnd = new Random();
        int num = rnd.Next(quotes.Length);
        // Get the channel id from the state object
        ulong channelId = (ulong)state;

        // Wait for the Ready event
        _client.Ready += async () =>
        {
            // Get the channel from Discord
            var channel = await _client.GetChannelAsync(channelId) as IMessageChannel;

            // Send a message to the channel
            await channel.SendMessageAsync(quotes[num]);
        };

    }

}
