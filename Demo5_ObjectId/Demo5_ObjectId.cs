using System;
using System.Diagnostics;
using System.Threading;

namespace Demo5_ObjectId
{
	class Demo5_ObjectId
	{
		static void Main(string[] args)
		{
			Foo();
			Thread.Sleep(15);
			Debugger.Break();
		}

		public static void Foo()
		{
			var obj2 = new Demo5(42);

			ThreadPool.QueueUserWorkItem((state) =>
			{
				while (true)
				{
					obj2.Value = obj2.Value + 1;
				}
			});
			Debugger.Break();
		}
	}

	public class Demo5
	{
		public int Value { get; set; }

		public Demo5(int value)
		{
			Value = value;
		}
	}
}
