using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniBot.Modules
{
    public class E : ModuleBase<SocketCommandContext>
    {
        [Command("e")]
        public async Task EmoteAsync(string emoteID = "")
        {
            if (emoteID == "") await ReplyAsync("To use emotes type `u.e emotename` | You can find a list with all emotes here: http://uni.zeryx.xyz/emote");
            else return;
        }
    }
}
