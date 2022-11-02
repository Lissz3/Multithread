namespace Eex7
{
	internal class Program
	{
		static readonly object l = new object();
		static int cont = 0;
		static bool paused = false;
		static bool finish = false;
		static void Main()
		{
			Thread player1 = new(() =>
			{
				Random r = new();
				while (!finish)
				{
					int num = r.Next(1, 11);
					lock (l)
					{
						//Cuando un hilo gana el otro hace una última jugada y podría aparecer un resultado incorrecto en pantalla
						//Estructura while lock if !finish
						if (!finish)
						{
							if (num == 5 || num == 7)
							{
								if (paused)
								{
									cont += 5;
								}
								else
								{
									cont++;
								}
								Console.SetCursorPosition(3, 4);
								Console.Write("{0,4}", cont);
								if (cont >= 20)
								{
									finish = true;
								}
								paused = true;
							}
							Console.SetCursorPosition(3, 1);
							Console.Write("{0,4}", num);
						}

					}
					Thread.Sleep(100 * num);

				}
			});

			Thread player2 = new(() =>
			{
				Random r = new();
				while (!finish)
				{
					int num = r.Next(1, 11);
					lock (l)
					{
						//Cuando un hilo gana el otro hace una última jugada y podría aparecer un resultado incorrecto en pantalla
						//Estructura while lock if !finish
						if (!finish)
						{
							if (num == 5 || num == 7)
							{
								if (!paused)
								{
									cont -= 5;
								}
								else
								{
									cont--;
								}
								Console.SetCursorPosition(3, 4);
								Console.Write("{0,4}", cont);
								if (cont <= -20)
								{
									finish = true;
								}
								paused = false;
								Monitor.Pulse(l);
							}
							Console.SetCursorPosition(3, 3);
							Console.Write("{0,4}", num);
						}
					}
					Thread.Sleep(100 * num);

				}
			});

			Thread display = new Thread(() =>
			{
				string[] displays = { "|", "/", "--", "\\" };
				int num = 0;
				while (!finish)
				{
					lock (l)
					{
						//Cuando un hilo gana el otro hace una última jugada y podría aparecer un resultado incorrecto en pantalla
						//Estructura while lock if !finish
						if (!finish)
						{
							if (paused)
							{
								Monitor.Wait(l);
							}
							Console.SetCursorPosition(3, 2);
							Console.Write("{0,4}", displays[num]);
							num++;
							if (num == displays.Length)
							{
								num = 0;
							}
						}
					}
					Thread.Sleep(200);

				}
			});

			player1.Start();
			player2.Start();
			display.Start();

			player1.Join();
			player2.Join();

			//El programa puede no terminar porque queda algún hilo corriendo. -> Join del display
			display.Join();

			Console.SetCursorPosition(3, 6);

			if (cont >= 20)
			{
				Console.WriteLine("Player 1 won!");
			}
			else
			{
				Console.WriteLine("Player 2 won!");
			}
		}
	}
}