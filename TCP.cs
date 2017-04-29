using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SmartRedMotion_Klient
{
	class TCP
	{
		TcpListener Serwer;
		string IP;
		const int Port = 2736;

		public void Rozruch()
		{
			IP = OkreslIP();

			Serwer = new TcpListener(IPAddress.Parse(IP), Port);
			Serwer.Start();

			while (true)
			{
				Czytaj();
			}
		}

		private void Czytaj()
		{
			TcpClient Klient;
			NetworkStream Strumien;
			StreamWriter Zapis;
			StreamReader Odczyt;
			string Linijka;

			Klient = Serwer.AcceptTcpClient();
			Strumien = Klient.GetStream();
			Strumien.WriteTimeout = 3000;
			Zapis = new StreamWriter(Strumien);
			Odczyt = new StreamReader(Strumien);

			while ((Linijka = Odczyt.ReadLine()) != null)
			{
				if (Linijka.Contains("T"))
				{
					Zapis.WriteLine("T_OK");
					Zapis.Flush();
				}
				else if (Linijka.Contains("W"))
				{
					Zapis.WriteLine("W_OK");
					Zapis.Flush();
					Zamykanie.Wylacz();
				}
			}
			Strumien.Close();
			Klient.Close();
		}

		private static string OkreslIP()
		{
			IPHostEntry host;
			string localIP = "";
			host = Dns.GetHostEntry(Dns.GetHostName());

			foreach (IPAddress ip in host.AddressList)
			{
				localIP = ip.ToString();

				string[] temp = localIP.Split('.');
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					break;
				}
				else localIP = null;
			}
			return localIP;
		}
	}
}
