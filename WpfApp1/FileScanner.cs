using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
	/// <summary>
	/// Scans a given Directory for files and filters out for multiple extensions
	/// </summary>
	public static class FileScanner
	{
		/// <summary>
		/// Scans a folder and all subfolders for files with given extension
		/// </summary>
		/// <param name="folderPath">Path to root folder for scan</param>
		/// <param name="extensions">Extensions to scan for. NULL for no filter</param>
		/// <returns></returns>
		public static Task<List<FerrisFileInfo>> scanFolder(string folderPath, List<string> extensions)
		{
			//Queue for subfolders
			Queue<string> queue = new Queue<string>();
			queue.Enqueue(folderPath);

			//List of all scanned files
			List<string> files = new List<string>();

			//Scan folders in queue
			while(queue.Count > 0)
			{
				var folder = queue.Dequeue();
				try
				{
					foreach (var f in Directory.EnumerateDirectories(folder))
					{
						queue.Enqueue(f);
					}

					//Add files to List
					if (extensions == null) files.AddRange(Directory.EnumerateFiles(folder));
					//Add files to list after filtering for extensions
					else files.AddRange(filterFiles(Directory.EnumerateFiles(folder), extensions));
				}
				catch (Exception ex)
				{
					if (ex.GetType() == typeof(UnauthorizedAccessException))
					{
						var dr = MessageBox.Show($"You don't have the permission to acces the following folder or files inside:\n{folder}\nRestarting programm as admin could solve the problem.\nWould you like to restart as admin now?", "No permission", MessageBoxButton.YesNo, MessageBoxImage.Error);
						if(dr == MessageBoxResult.Yes)
						{
							//TODO Restart as admin
						}
					}
				}
				
			}

			//Create a FerrisFileInfo for each file in List
			List<FerrisFileInfo> ferrisFiles = new List<FerrisFileInfo>();
			foreach(var f in files)
			{
				FileInfo fi = new FileInfo(f);
				ferrisFiles.Add(new FerrisFileInfo()
				{
					Name = fi.Name,
					Path = fi.FullName,
					Size = fi.Length,
					Favourite = false
				});
			}

			return Task.FromResult(ferrisFiles);
		}

		/// <summary>
		/// Returns files that end with one of the given extensions
		/// </summary>
		/// <param name="files">List of files to filter</param>
		/// <param name="extensions">List if extensions as filter</param>
		/// <returns></returns>
		private static IEnumerable<string> filterFiles(IEnumerable<string> files, List<string> extensions)
		{
			foreach(var f in files)
			{
				foreach(var ex in extensions)
				{
					if (f.EndsWith(ex)) yield return f;
				}
			}
		}
	}
}
