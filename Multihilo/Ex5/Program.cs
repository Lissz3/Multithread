namespace Ex5
{
	internal class Program
	{
		static readonly private object l = new object();
		static bool finish = false;
		static int winner;
		static void Main(string[] args)
		{
			Thread[] ts = new Thread[5];
			for (int i = 0; i < ts.Length; i++)
			{
				int top = i;
				ts[i] = new Thread(() =>
				{
					int goal = 0;

					while (goal < 50 && !finish)
					{

						lock (l)
						{
							goal += new Random().Next(0, 4);
							if (goal > 50)
							{
								goal = 50;
								winner = top;
							}
							Console.SetCursorPosition(goal, top);
							Console.Write("*");
							Thread.Sleep(50);
						}

					}
					finish = true;
				});
				ts[i].Start();
			}

			Array.ForEach(ts, t => t.Join());

			if (finish)
			{
				lock (l)
				{
					Console.SetCursorPosition(0, 10);
					Console.WriteLine("Thread {0} has won!", winner);
					Console.ReadKey();
				}
			}
		}
	}
}