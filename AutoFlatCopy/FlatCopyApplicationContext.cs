using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFlatCopy
{
	class FlatCopyApplicationContext : ApplicationContext
	{
		static NotifyIcon notifyIcon;

		static MenuItem startSync;
		static MenuItem stopSync;

		public static string sourceFolder = "";
		public static string destFolder = "";

		public static string filter = "";

		public static Icon iconOn = (Icon)AutoFlatCopy.Properties.Resources.ResourceManager.GetObject("iconOn");
		public static Icon iconOff = (Icon)AutoFlatCopy.Properties.Resources.ResourceManager.GetObject("iconOff");
		public static Icon iconWork = (Icon)AutoFlatCopy.Properties.Resources.ResourceManager.GetObject("iconWork");

		static FileWatcher fileWatcher = new FileWatcher();

		public FlatCopyApplicationContext()
		{
			LoadSettings();

			MenuItem configMenuItem = new MenuItem("Configuration", new EventHandler(ShowConfig));
			MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));
			startSync = new MenuItem("Start Sync", new EventHandler(StartSync));
			stopSync = new MenuItem("Stop Sync", new EventHandler(StopSync));

			notifyIcon = new NotifyIcon();

			notifyIcon.Icon = (Icon)AutoFlatCopy.Properties.Resources.ResourceManager.GetObject("iconOff");
			notifyIcon.ContextMenu = new ContextMenu();
			notifyIcon.ContextMenu.MenuItems.Add(configMenuItem);
			notifyIcon.ContextMenu.MenuItems.Add("-");
			notifyIcon.ContextMenu.MenuItems.Add(startSync);
			notifyIcon.ContextMenu.MenuItems.Add(stopSync);
			notifyIcon.ContextMenu.MenuItems.Add("-");
			notifyIcon.ContextMenu.MenuItems.Add(exitMenuItem);

			stopSync.Enabled = false;

			notifyIcon.Visible = true;
		}

		public static void ResyncAllNow(){
			fileWatcher.ResyncAllNow(
				sourceFolder,
				destFolder,
				".",
				filter
				);
		}

		/// <summary>
		/// folder for saving of config
		/// </summary>
		public static string GetConfigSaveFolder()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			System.IO.Directory.CreateDirectory(path + @"\FlatAutoCopy\");
			return path + @"\FlatAutoCopy\";
		}
		
		//break capsuling and make alot stuff static just for a fancy icon stuff? HELL FUCKING YES!	
		public static void SetWorkIcon()
		{
			notifyIcon.Icon = iconWork;
		}

		public static void UnsetWorkicon(){
			if (stopSync.Enabled)
			{
				notifyIcon.Icon = iconOn;
			}else{
				notifyIcon.Icon = iconOff;
			}
		}

		public static void SaveSettings()
		{
			var path = GetConfigSaveFolder();

			using (StreamWriter outputFile = new StreamWriter(path + @"settings.ini"))
			{
				outputFile.WriteLine($"inDir={sourceFolder}");
				outputFile.WriteLine($"outDir={destFolder}");
				outputFile.WriteLine($"filter={filter}");
			}
		}

		public static void LoadSettings()
		{
			var path = GetConfigSaveFolder();
			if (File.Exists(path + @"settings.ini"))
			{
				using (StreamReader inputFile = new StreamReader(path + @"settings.ini"))
				{
					while (!inputFile.EndOfStream)
					{
						SetSettingFromLine(inputFile.ReadLine());
					}
				}
			}
		}

		public static void SetSettingFromLine(string line)
		{
			var split = line.Split('=');
			if (split.Length > 1)
			{
				switch (split[0])
				{
					case "inDir":
						sourceFolder = split[1];
						break;
					case "outDir":
						destFolder = split[1];
						break;
					case "filter":
						filter = split[1];
						break;
					default:
						break;
				}
			}
		}

		Form1 configWindow = new Form1();
		void ShowConfig(object sender, EventArgs e)
		{
			configWindow.SetSettings(sourceFolder, destFolder);
			if (configWindow.Visible)
			{
				configWindow.Activate();
			}
			else
			{
				configWindow.ShowDialog();
			}
		}

		void StartSync(object sender, EventArgs e)
		{
			if (fileWatcher.WatchDirectory(sourceFolder, destFolder,".",filter)){
				notifyIcon.Icon = iconOn;
				startSync.Enabled = false;
				stopSync.Enabled = true;
			}
		}

		void StopSync(object sender, EventArgs e)
		{
			fileWatcher.StopWatch();
			notifyIcon.Icon = iconOff;
			stopSync.Enabled = false;
			startSync.Enabled = true;
		}

		void Exit(object sender, EventArgs e)
		{
			// We must manually tidy up and remove the icon before we exit.
			// Otherwise it will be left behind until the user mouses over.
			notifyIcon.Visible = false;
			Application.Exit();
		}
	}
}

