using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniBot.Modules
{
    [Group("c")]
    public class C : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task CPing()
        {
            await ReplyAsync("Who pinged me?");
        }
		
		[Command("universe")]
		public async Task CUniverse()
		{
			await ReplyAsync($"There are {} solar systems in this universe");
		}
    }
}
