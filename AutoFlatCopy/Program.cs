using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFlatCopy
{
	static class Program
	{
		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool onlyInstance = false;
			Mutex mutex = new Mutex(true, "AutoFlatCopy", out onlyInstance);
			if (!onlyInstance)
			{
				return;
			}
			Application.Run(new FlatCopyApplicationContext());
			GC.KeepAlive(mutex);
		}
	}
}
