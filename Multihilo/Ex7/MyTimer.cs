using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex7
{

	internal class MyTimer
	{
		Thread run;
		public int Interval { get; set; }
		static bool paused;
		static readonly object l = new object();
		ThreadStart func;

		public MyTimer(ThreadStart function)
		{
			run = new Thread(Foo);
			func = function;
			paused = true;
			run.IsBackground = true;
			run.Start();
		}

		public void Foo()
		{
			while (true)
			{
				lock (l)
				{
					if (paused)
					{
						Monitor.Wait(l);
					}

					if (!paused)
					{
						func.Invoke();
						Thread.Sleep(Interval);
					}

				}

			}

		}

		public void Run()
		{
			paused = false;
			lock (l)
			{
				Monitor.Pulse(l);
			}
		}

		public void Pause()
		{
			paused = true;
			//lock (l)
			//{
			//	Monitor.Wait(l);
			//}
		}
	}
}
