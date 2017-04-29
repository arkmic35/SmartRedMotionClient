using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartRedMotion_Klient
{
	class Zamykanie
	{
		public static void Wylacz()
		{
#if DEBUG
			System.Diagnostics.Process.Start("cmd.exe");
#else
			System.Diagnostics.Process.Start("shutdown.exe", "-s -t 0");
#endif
		}

	}
}
