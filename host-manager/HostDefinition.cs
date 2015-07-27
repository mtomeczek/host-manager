using System;
using System.Net;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;

namespace hostmanager
{
	public class HostDefinition
	{
		public IPAddress Address;

		public string Host;

		public HostDefinition (IPAddress address, string host)
		{
			this.Address = new IPAddress (address.GetAddressBytes ());
			this.Host = host.Trim();
		}

		public static List<HostDefinition> readHosts (string fileName)
		{
			List<HostDefinition> hosts = new List<HostDefinition> ();
			StreamReader hostsFile = File.OpenText (fileName);
			while (!hostsFile.EndOfStream) {
				string hostLine = hostsFile.ReadLine ();
				string hostText = hostLine.Split ('#') [0];
				string ipText = hostText.Substring (0, hostText.IndexOf (" ")).Trim ();
				string namesText = hostText.Substring (hostText.IndexOf (" ")).Trim ();
				foreach(string nameText in namesText.Split(" \t".ToCharArray())){
					IPAddress hostAddress;
					if(IPAddress.TryParse(ipText,out hostAddress)){
						hosts.Add (new HostDefinition (hostAddress, nameText));
					}
				}

			}
			return hosts;
		}
	}
}

