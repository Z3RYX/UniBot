using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniBot.Modules
{
    [Group("help")]
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultHelp()
        {
            await ReplyAsync("UniBot uses the prefix u.\n" +
                "\n" +
                "`u.c <command>` to use normal commands\n" +
                "`u.m <command>` for moderator commands\n" +
                "`u.e <emote ID>` to have UniBot replace your command with one of many emotes that can be found on http://uni.zeryx.xyz/emote \n" +
                "\n" +
                "For more information visit http://uni.zeryx.xyz");
        }
    }
}
