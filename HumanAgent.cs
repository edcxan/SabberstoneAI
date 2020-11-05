using System;
using System.Collections.Generic;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneBasicAI.PartialObservation;
using SabberStoneCore.Enums;
using System.Linq;
using SabberStoneCore.Model.Entities;

namespace SabberStoneBasicAI.AIAgents
{
	class HumanAgent : AbstractAgent
	{
		private Random Rnd = new Random();

		public override void InitializeAgent()
		{
			Rnd = new Random();
		}

		public override void FinalizeAgent()
		{
			//Nothing to do here
		}

		public override void FinalizeGame()
		{
			//Nothing to do here
		}

		public override PlayerTask GetMove(POGame poGame)
		{
			Console.WriteLine(poGame.PartialPrint());

			var player = poGame.CurrentPlayer;

			// During Mulligan: select Random cards
			if (player.MulliganState == Mulligan.INPUT)
			{
				List<int> mulligan = RandomMulliganRule().Invoke(player.Choice.Choices.Select(p => poGame.getGame().IdEntityDic[p]).ToList());
				return ChooseTask.Mulligan(player, mulligan);
			}

			List<PlayerTask> options = poGame.CurrentPlayer.Options();

			int count = 0;
			foreach (PlayerTask option in options)
			{
				count++;
				Console.WriteLine("[" +count.ToString() + "] " + option);
			}

			bool success = false;
			int choice = 0;
			while (!success)
			{
				string input = Console.ReadLine();
				choice = int.Parse(input);
				if (choice > 0 && choice <= count)
					success = true;
				else
					Console.WriteLine("Please enter a number for the option you choose.");
			}

			return options[choice-1];
		}

		public override void InitializeGame()
		{
			//Nothing to do here
		}

		public Func<List<IPlayable>, List<int>> RandomMulliganRule()
		{
			return p => p.Where(t => Rnd.Next(1, 3) > 1).Select(t => t.Id).ToList();
		}
	}
}
