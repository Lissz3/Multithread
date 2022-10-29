namespace Ex5
{
	internal class Program
	{
		static readonly private object l = new object();
		static bool finish = false;
		static int winner;
		static string[] horses = { "Horse Manuel", "Spirit", "Rocinante", "Roach", "Pegaso" };
		static Thread[] ts;

		public static int BetForAHorse()
		{
			string option;
			int horseselected;
			do
			{
				Console.WriteLine("Select the horse you want to bet for!");
				for (int i = 0; i < horses.Length; i++)
				{
					Console.WriteLine((i + 1) + ".- " + horses[i]);
				}
				Console.Write("Make your choice: ");
				while (!int.TryParse(Console.ReadLine(), out horseselected) || horseselected > 5 || horseselected < 1)
				{
					Console.WriteLine("Option not valid. Select one of the horses: ");
				}
				Console.WriteLine("Your choice was " + horses[horseselected - 1] + ".");
				Console.WriteLine("Press \"1\" if you want to start the race. If you want to change your horse press any other key.");
				option = Console.ReadLine();
			} while (option != "1");

			return horseselected;
		}

		public static void Run(Thread[] ts)
		{
			finish = false;
			for (int i = 0; i < ts.Length; i++)
			{
				int top = i;
				ts[i] = new Thread(() =>
				{
					int goal = 0;

					while (!finish)
					{

						lock (l)
						{
							goal += 1;// new Random().Next(0, 4);
							if (!finish)
							{
								if (goal >= 50)
								{
									goal = 50;
									winner = top;
									finish = true;
								}
								Console.SetCursorPosition(goal, top);
								Console.Write("*");
							}
						}
						Thread.Sleep(50);

					}
				});
				ts[i].Start();
			}

			Array.ForEach(ts, t => t.Join());
		}

		public static void Restart()
		{
			string op;
			do
			{
				ts = new Thread[5];

				int bet = BetForAHorse();
				Console.Clear();
				Run(ts);

				if (finish)
				{
					lock (l)
					{
						Console.SetCursorPosition(0, 10);
						Console.WriteLine(horses[winner] + " won!");
						if (winner + 1 == bet)
						{
							Console.WriteLine("Your horse won!");
						}
						else
						{
							Console.WriteLine("Good luck next time!");
						}
					}
				}

				Console.WriteLine("Press 1 to restart or any other key to end.");
				op = Console.ReadLine();

			} while (op == "1");
		}
		static void Main(string[] args)
		{
			//Thread[] ts = new Thread[5];

			//int bet = BetForAHorse();

			//Run(ts);

			//if (finish)
			//{
			//	lock (l)
			//	{
			//		Console.SetCursorPosition(0, 10);
			//		Console.WriteLine("Thread {0} has won!", winner);
			//		Console.ReadKey();
			//	}
			//}

			Restart();
		}
	}
}