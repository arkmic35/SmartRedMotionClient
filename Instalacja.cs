using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SmartRedMotion_Klient
{
	class Instalacja
	{
		public static string NazwaProgramu = System.AppDomain.CurrentDomain.FriendlyName;

		static string AdresAktualny = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
		static string AdresDocelowy = Environment.ExpandEnvironmentVariables("%programdata%/Microsoft/Windows/Start Menu/Programs/StartUp/" + NazwaProgramu);
		static bool MaUprawnienia = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

		public static bool CzyZainstalowano = AdresAktualny.IndexOf("StartUp", StringComparison.OrdinalIgnoreCase) >= 0;

		public static void Zainstaluj()
		{
			if (MaUprawnienia == false)
			{
				OtworzProgram(AdresAktualny, "", true);
				ZamknijTenProgram();
			}
			else
			{
				KopiujPlik(AdresAktualny, AdresDocelowy);
				OdblokujPort();
				OtworzProgram(AdresDocelowy, "pierwszy");
				ZamknijTenProgram();
			}
		}

		private static void OtworzProgram(string lokalizacja, string parametry, bool admin = false, bool czekaj = false)
		{
			try
			{
				var psi = new ProcessStartInfo();
				psi.FileName = lokalizacja;
				psi.Arguments = parametry;

				if (admin == true)
				{
					psi.Verb = "runas";
				}

				var process = new Process();
				process.StartInfo = psi;
				process.Start();

				if (czekaj == true)
				{
					process.WaitForExit();
				}
			}
			catch (System.ComponentModel.Win32Exception blad) //odrzucenie przy zapytaniu o uprawnienia administratorskie
			{
				if (!admin)
				{
					throw blad;
				}
			}
		}

		private static void ZamknijTenProgram()
		{
			Debug.Zamknij();
		}

		private static bool CzyPlikIstnieje(string lokalizacja)
		{
			return System.IO.File.Exists(lokalizacja);
		}

		private static void UsunPlik(string lokalizacja)
		{
			if (CzyPlikIstnieje(lokalizacja))
			{
				System.IO.File.Delete(lokalizacja);
			}
		}

		private static void KopiujPlik(string lokalizacja_od, string lokalizacja_do)
		{
			UsunPlik(lokalizacja_do);
			System.IO.File.Copy(lokalizacja_od, lokalizacja_do);
		}

		private static void OdblokujPort()
		{
			OtworzProgram("netsh", "firewall add allowedprogram \"" + AdresDocelowy + "\" \"" + NazwaProgramu + "\" ENABLE", true, true);
		}
	}
}
