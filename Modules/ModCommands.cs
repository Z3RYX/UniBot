using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UniBot.Modules
{
    [Group("m")]
    public class ModCommands : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task mHelp()
        {
            await ReplyAsync("List of Mod Commands:\n" +
                "Remember that UniBot needs specific permissions for most commands.\n\n" +
                "`u.m ban <user> [reason]` | Bans the mentioned user\n" +
                "`u.m kick <user> [reason]` | Kicks the mentioned user");
        }

        [Command("warn"), RequireUserPermission(Discord.GuildPermission.KickMembers)]
        public async Task mWarn(string user, [Remainder] string reason)
        {
            user = user.Substring(2);
            user = user.Remove(18);
            var userID = Context.Guild.GetUser(Convert.ToUInt64(user));

            string path = $"warns\\{Context.Guild.Id}.txt";
            if(!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine("0:WarnCon"); // 0 kick, 1 ban | Default is kick
                tw.WriteLine("0:WarnLevel"); // How many warns before bot takes action | Default is 0 (infinite)
                tw.WriteLine(user);
                tw.Close();
            }
            else
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(user);
                }
            }
            string[] lines = File.ReadAllLines(path);
            int warnLevel = 0;
            bool warnCon = false;
            int i = 0;
            foreach(string line in lines)
            {
                if (line.Contains("0:WarnCon")) warnCon = false;
                else if(line.Contains("1:WarnCon")) warnCon = true;
                if (line.Contains("WarnLevel"))
                {
                    string[] lvl = line.Split(':');
                    warnLevel = Convert.ToInt32(lvl[0]);
                }
                if (line.Contains(user)) i++;
            }
            if(!warnCon && i >= warnLevel && i != 0)
            {
                await ReplyAsync($"{userID} has been kicked\nReason: `Exceeded warn limit`");
                await userID.KickAsync(reason);
            }
            else if(warnCon && i >= warnLevel && i != 0)
            {
                await ReplyAsync($"{userID} has been banned\nReason: `Exceeded warn limit`");
                await Context.Guild.AddBanAsync(userID, 0, reason);
            }
            else await ReplyAsync($"{userID} has been warned\nReason: `{reason}`");
        }

        [Command("kick"), RequireBotPermission(Discord.GuildPermission.KickMembers), RequireUserPermission(Discord.GuildPermission.KickMembers)]
        public async Task mKick(string user, [Remainder] string reason)
        {
            try
            {
                user = user.Substring(2);
                user = user.Remove(18);
                var userID = Context.Guild.GetUser(Convert.ToUInt64(user));
                Discord.IDMChannel dm = await userID.GetOrCreateDMChannelAsync();
                await dm.SendMessageAsync($"You have been kicked from {Context.Guild.Name} with the reason: {reason}");
                await dm.CloseAsync();
                await userID.KickAsync(reason);
                await ReplyAsync($"{Context.Message.Author.Mention} has kicked {userID}.\nReaon:\n`{reason}`");
            }
            catch
            {
                await ReplyAsync("Couldn't kick user.\nPlease make sure that you have mentioned the user you want to kick.");
            }
        }

        [Command("ban"), RequireBotPermission(Discord.GuildPermission.BanMembers), RequireUserPermission(Discord.GuildPermission.BanMembers)]
        public async Task mBan(string user, [Remainder] string reason)
        {
            try
            {
                user = user.Substring(2);
                user = user.Remove(18);
                var userID = Context.Guild.GetUser(Convert.ToUInt64(user));
                Discord.IDMChannel dm = await userID.GetOrCreateDMChannelAsync();
                await dm.SendMessageAsync($"You have been banned from {Context.Guild.Name} with the reason: {reason}");
                await dm.CloseAsync();
                await Context.Guild.AddBanAsync(userID, 0, reason);
                await ReplyAsync($"{Context.Message.Author.Mention} has banned {userID}.\nReaon:\n`{reason}`");
            }
            catch
            {
                await ReplyAsync("Couldn't ban user.\nPlease make sure that you have mentioned the user you want to ban.");
            }
        }
    }
}
