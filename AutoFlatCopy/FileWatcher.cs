using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFlatCopy
{
	public class FileWatcher
	{

		FileSystemWatcher watcher;
		string sourceDir;
		string destinationDir;

		public bool WatchDirectory(string sourceDir, string destinationDir, string folderSeparator, string fileFilter)
		{
			if (!sourceDir.EndsWith("\\"))
			{
				sourceDir += "\\";
			}

			if (!destinationDir.EndsWith("\\"))
			{
				destinationDir += "\\";
			}
			this.destinationDir = destinationDir;
			this.sourceDir = sourceDir;
			//stop old instance maybe
			if (null != watcher)
			{
				watcher.EnableRaisingEvents = false;
				watcher.Dispose();
			}

		//	Console.WriteLine("starting watch " + sourceDir + " -> " + destinationDir);

			if (!Directory.Exists(sourceDir))
			{
				MessageBox.Show("There is no such dir: " + sourceDir);
				return false;
			} else if (!Directory.Exists(destinationDir))
			{
				MessageBox.Show("There is no such dir: " + destinationDir);
				return false;
			} else if (destinationDir.Equals(sourceDir, StringComparison.InvariantCultureIgnoreCase))
			{
				MessageBox.Show($"Really? sync from {sourceDir} to {destinationDir}? Nope!");
				return false;
			}

			watcher = new FileSystemWatcher();
			watcher.Path = sourceDir;
			watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
			watcher.Filter = fileFilter;
			watcher.Changed += new FileSystemEventHandler(OnChanged);
			watcher.Created += new FileSystemEventHandler(OnCreated);
			watcher.Deleted += new FileSystemEventHandler(OnDeleted);
			watcher.Renamed += new RenamedEventHandler(OnRenamed);
			watcher.IncludeSubdirectories = true;

			watcher.Error += new ErrorEventHandler(OnError);

			watcher.EnableRaisingEvents = true;

			return true;
		}

		public void ResyncAllNow(string src, string dest,string separator, string filter)
		{
			if (!Directory.Exists(dest)){
				MessageBox.Show($"There is no such folder:{dest}", "Problem",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			if (!Directory.Exists(src))
			{
				MessageBox.Show($"There is no such folder:{src}", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			this.sourceDir = src;
			this.destinationDir = dest;

			if (!sourceDir.EndsWith("\\"))
			{
				sourceDir += "\\";
			}

			if (!destinationDir.EndsWith("\\"))
			{
				destinationDir += "\\";
			}

			var srcInfo = new DirectoryInfo(src);
			var destInfo = new DirectoryInfo(dest);

			//delete all in dest
			foreach (DirectoryInfo dir in destInfo.GetDirectories())
			{
				dir.Delete(true);
			}
			foreach (FileInfo file in destInfo.GetFiles())
			{
				file.Delete();
			}

			//copy in new files
			foreach (FileInfo file in srcInfo.GetFiles(filter, SearchOption.AllDirectories))
			{	
				var counterPart = GetCounterFullPath(file.FullName);
				Console.WriteLine($"copy {file.FullName} -> {counterPart}" );
				File.Copy(file.FullName, counterPart, true);
			}
		}

		public void OnError(object source, ErrorEventArgs e)
		{
			Console.WriteLine("errror:" + e);
		}

		public void StopWatch()
		{
			if (null != watcher)
			{
				watcher.EnableRaisingEvents = false;
				watcher.Dispose();
				watcher = null;
			}
		}

		private void OnRenamed(object source, RenamedEventArgs e)
		{
			FlatCopyApplicationContext.SetWorkIcon();

			Console.WriteLine($"file renamed {e.OldFullPath} to {e.FullPath} ");
			var oldCounterPart = GetCounterFullPath(e.OldFullPath);
			var newCounterPart = GetCounterFullPath(e.FullPath);
			Console.WriteLine($"rename->  {oldCounterPart} to {newCounterPart}");
			try
			{
				if (!IsDirectory(e.FullPath))
				{
					File.Move(oldCounterPart, newCounterPart);
				}
				else
				{
					var files = Directory.EnumerateFiles(e.FullPath, "*.*", SearchOption.AllDirectories);
					string pathPart;
					foreach (var file in files)
					{
						var tmp = file.Replace(e.FullPath, "");
						pathPart = e.OldFullPath + tmp;
						pathPart = pathPart.Replace(sourceDir, "");
						pathPart = pathPart.Replace(@"\", ".");
						var oldCounterFile = destinationDir + pathPart;


						pathPart = file.Replace(sourceDir, "");
						pathPart = pathPart.Replace(@"\", ".");
						var newCounterFile = destinationDir + pathPart;

						File.Move(oldCounterFile, newCounterFile);
					}
				}

			}
			catch (Exception) { }
			FlatCopyApplicationContext.UnsetWorkicon();
		}

		private void OnCreated(object source, FileSystemEventArgs e)
		{
			FlatCopyApplicationContext.SetWorkIcon();
			Console.WriteLine($"file created {e.FullPath}");
			var counterPart = GetCounterFullPath(e.FullPath);
			Console.WriteLine($"overwrite->  {counterPart}");
			if (!IsDirectory(e.FullPath))
			{
				try
				{
					File.Copy(e.FullPath, counterPart);
				}
				catch (Exception) { }
			}
			FlatCopyApplicationContext.UnsetWorkicon();
		}

		private void OnChanged(object source, FileSystemEventArgs e)
		{
			FlatCopyApplicationContext.SetWorkIcon();
			Console.WriteLine($"file Changed {e.FullPath}");
			var counterPart = GetCounterFullPath(e.FullPath);
			Console.WriteLine($"overwrite->  {counterPart}");
			if (!IsDirectory(e.FullPath))
			{
				try
				{
					File.Copy(e.FullPath, counterPart,true);
				}
				catch (Exception ex) 
				{
				
				}
			}
			FlatCopyApplicationContext.UnsetWorkicon();
		}

		private void OnDeleted(object source, FileSystemEventArgs e)
		{
			FlatCopyApplicationContext.SetWorkIcon();
			Console.WriteLine($"file deleted {e.FullPath}");
			var counterPart = GetCounterFullPath(e.FullPath);
			Console.WriteLine($"del->        {counterPart}");
			if (!IsDirectory(e.FullPath))
			{
				try
				{
					File.Delete(counterPart);
				}
				catch (Exception) { }
			}
			else
			{
				var files = Directory.EnumerateFiles(e.FullPath, "*.*", SearchOption.AllDirectories);
				string pathPart;
				foreach (var file in files)
				{
					var tmp = file.Replace(e.FullPath, "");
					pathPart = e.FullPath + tmp;
					pathPart = pathPart.Replace(sourceDir, "");
					pathPart = pathPart.Replace(@"\", ".");
					var oldCounterFile = destinationDir + pathPart;

					File.Delete(oldCounterFile);
				}
			}
			FlatCopyApplicationContext.UnsetWorkicon();
		}

		private string GetCounterFullPath(string fullPath)
		{
			string pathPart = fullPath.Replace(sourceDir, "");
			pathPart = pathPart.Replace(@"\", ".");
			var fullDestPath = destinationDir + pathPart;

			return fullDestPath;
		}

		private bool IsDirectory(string path)
		{
			try
			{
				FileAttributes attr = File.GetAttributes(path);

				if (attr.HasFlag(FileAttributes.Directory))
				{
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}

		}
	}
}
