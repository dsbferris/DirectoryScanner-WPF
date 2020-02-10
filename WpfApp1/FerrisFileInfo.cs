using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Serialization;

namespace WpfApp1
{
	[Serializable]
	public class FerrisFileInfo
	{
#pragma warning disable CA2235 // Mark all non-serializable fields
		public bool Favourite { get; set; }
		public string Name { get; set; }
		public long Size { get; set; }
		public string Path { get; set; }
#pragma warning restore CA2235 // Mark all non-serializable fields

		public enum SortMode
		{
			PATH,
			NAME,
			SIZE,
		};

		public static List<FerrisFileInfo> Sort(List<FerrisFileInfo> files, SortMode sm)
		{
			switch (sm)
			{
				case SortMode.NAME: files.Sort(delegate (FerrisFileInfo f1, FerrisFileInfo f2) { return f1.Name.CompareTo(f2.Name); }); break;
				case SortMode.PATH: files.Sort(delegate (FerrisFileInfo f1, FerrisFileInfo f2) { return f1.Path.CompareTo(f2.Path); }); break;
				case SortMode.SIZE: files.Sort(delegate (FerrisFileInfo f1, FerrisFileInfo f2) { return f1.Size.CompareTo(f2.Size); }); break;
			}
			return files;
		}
	}

}
