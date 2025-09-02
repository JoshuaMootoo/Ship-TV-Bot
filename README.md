# ShipTVBot — Helldivers 2 “Democracy” Discord Bot

A tiny Discord bot that posts a random **Ship TV** quote from *Helldivers 2* when you run `/democracy`.
Built with **C#** and **Discord.Net**. Quotes live in a simple text file so you can add more easily.
Includes a Dockerfile for 24/7 hosting.

## Features
- Slash command **`/democracy`** returns a random Ship TV quote
- Quotes loaded from `ShipTVQuotes.txt` (one quote per line)
- Pretty embed response (minimal permissions required)
- Guild-scoped command registration for instant testing
- Dockerized for Railway/Render deployment

## Requirements
- .NET 8 SDK
- A Discord application & bot token

## Quick start (local)
1. **Clone**
   ```bash
   git clone <your-repo-url>
   cd ShipTVBot
   ```
2. **Create bot & invite**
   - Discord Developer Portal → *Applications* → New Application → *Bot* → copy the **Token**.
   - *OAuth2 → URL Generator*:
     - Scopes: **bot**, **applications.commands**
     - Bot Permissions: **Send Messages**, **Embed Links**
     - Open the generated URL to invite the bot to your server.
3. **Set your token as an environment variable**
   - **Windows (PowerShell)**  
     Persist:
     ```powershell
     setx DISCORD_TOKEN your_bot_token_here
     # reopen terminal
     ```
     Or for current session:
     ```powershell
     $env:DISCORD_TOKEN = 'your_bot_token_here'
     ```
   - **macOS/Linux**
     ```bash
     export DISCORD_TOKEN=your_bot_token_here
     ```
4. **Run**
   ```bash
   dotnet run
   ```
   You should see `Connected` / `Ready` in the console.
5. **Use it**
   - In your server, type `/democracy` → enjoy the broadcast 🇸🇪🗳️

## Quotes file
- Edit `ShipTVQuotes.txt` to add/remove lines.
- Ensure it’s copied to the build output. If needed, add this to your `.csproj`:
  ```xml
  <ItemGroup>
    <None Include="ShipTVQuotes.txt">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
  ```
- The app expects the file in the same directory as the executable.

## Docker
Build and run locally:
```bash
docker build -t ship-tv-bot .
docker run --rm -e DISCORD_TOKEN=your_bot_token_here ship-tv-bot
```

### Deploy (Railway / Render)
- Push your repo (with `Dockerfile`) to GitHub.
- **Railway**
  1) New Project → Deploy from GitHub (select repo)  
  2) Variables → add `DISCORD_TOKEN`  
  3) Deploy and check logs
- **Render**
  1) New → Web Service → “Use Docker” (select repo)  
  2) Environment → add `DISCORD_TOKEN`  
  3) Create Web Service

> Free tiers may sleep on inactivity. Paid micro plans keep it always-on.

## Permissions & intents
- **Scopes**: `bot`, `applications.commands`  
- **Bot permissions**: `Send Messages`, `Embed Links`  
- **Gateway intents**: using `AllUnprivileged` only (no privileged intents needed)

## Troubleshooting
- **`/democracy` doesn’t show up**
  - Re-invite with `applications.commands` scope
  - Refresh Discord client (`Ctrl+R`)  
  - Check channel permissions: **Use Application Commands**, **Send Messages**, **Embed Links**
  - Look for “Upserted /democracy …” in your console logs
- **Token not found**
  - Print it:  
    - Windows: `echo $env:DISCORD_TOKEN`  
    - macOS/Linux: `echo $DISCORD_TOKEN`
  - If you used `setx`, reopen the terminal/VS Code
- **Quotes not loading**
  - Ensure `ShipTVQuotes.txt` is next to the DLL or included via the `.csproj` snippet above

## Roadmap (ideas)
- `/liberty` — another quote pool or image macro  
- `/report` — playful “loyalty officer” message  
- Cooldowns & per-user stats  
- Option to send ephemeral replies (interactions)

## Project structure
```
ShipTVBot/
├─ Program.cs
├─ ShipTVQuotes.txt
├─ ShipTVBot.csproj
├─ Dockerfile
└─ .dockerignore
```

## License
MIT (feel free to adapt/host your own).

## Credits
- Quotes and universe belong to **Arrowhead Game Studios** / **PlayStation** (*Helldivers 2*).  
- This is a fan-made, non-commercial bot.
