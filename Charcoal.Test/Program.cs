using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charcoal;

namespace Charcoal.Test
{
	class Program
	{

		static void Main(string[] args)
		{
			using (var game = new Charcoal.Game1())
			{
				game.Run();
			}
		}
	}
}
