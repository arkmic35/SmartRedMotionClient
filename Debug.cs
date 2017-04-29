using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartRedMotion_Klient
{
	static class Debug
	{
		static string AdresDocelowy = Environment.ExpandEnvironmentVariables("%programdata%/srmclient.txt");

		public static void PrzechwycBlad(object sender, UnhandledExceptionEventArgs args)
		{
			Debug.Blad((Exception)args.ExceptionObject);
		}

		public static void Blad(Exception blad)
		{
			StreamWriter zapis = new StreamWriter(AdresDocelowy, true);
			zapis.WriteLine(blad.ToString());
			zapis.WriteLine("");
			zapis.Close();

			MessageBox.Show("Wystąpił błąd programu. Program zostanie zamknięty.\nProszę o poinformowanie opiekuna pracowni.", "SmartRedMotion Klient");
			Zamknij();
		}

		public static void Zamknij()
		{
			Application.Exit();
			Environment.Exit(0);
		}
	}
}
