using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SmartRedMotion_Klient
{
	static class Program
	{
		public static bool CzyPierwszyRozruch = false;

		[STAThread]
		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Debug.PrzechwycBlad);

			Application.EnableVisualStyles();

			foreach (string arg in args)
			{
				if (arg == "pierwszy")
				{
					CzyPierwszyRozruch = true;
				}
			}

			ZamknijInneProgramy();

			if (Instalacja.CzyZainstalowano == false)
			{
				Instalacja.Zainstaluj();
			}
			else
			{
				if (CzyPierwszyRozruch == true)
				{
					MessageBox.Show("Program został pomyślnie zainstalowany.", "SmartRedMotion Klient");
				}

				UruchomSerwer();
			}

			Application.Run();
		}

		private static void ZamknijInneProgramy()
		{
			Process AktualnyProces = Process.GetCurrentProcess();

			foreach (var Proces in Process.GetProcessesByName(AktualnyProces.ProcessName))
			{
				if (Proces.Id != AktualnyProces.Id)
				{
					Proces.Kill();
				}
			}
		}

		private static void UruchomSerwer()
		{
			new Thread(new ThreadStart(new TCP().Rozruch)).Start();
		}
	}
}
