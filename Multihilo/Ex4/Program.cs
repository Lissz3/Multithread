namespace Ej4
{
	internal class Program
	{
		static readonly private object l = new object();
		static bool finish = false;
		static int run = 0;
		static void Main(string[] args)
		{

			Thread t1 = new Thread(() =>
			{
				while (!finish)
				{
					lock (l)
					{
						if (!finish)
						{
							run++;
							Console.ForegroundColor = ConsoleColor.DarkCyan;
							Console.Write("T1>"+run);
						}

						if (run == 1000)
						{
							finish = true;
						}

					}
				}
				


			});

			Thread t2 = new Thread(() =>
			{
				while (!finish)
				{
					lock (l)
					{
						if (!finish)
						{
							run--;
							Console.ForegroundColor = ConsoleColor.DarkGray;
							Console.Write("T2>"+run);
						}

						if (run == -1000)
						{
							finish = true;
						}


					}
				}


			});

			t1.Start();
			t2.Start();
			t1.Join();
			t2.Join();

			Console.WriteLine("");
			Console.ForegroundColor = ConsoleColor.White;

			if (finish)
			{
				if (run == 1000)
				{
					Console.WriteLine("T1 has won!");
				}
				else
				{
					Console.WriteLine("T2 has won!");
				}
			}

		}
	}
}