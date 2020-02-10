using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WpfApp1
{
	public static class Serializer
	{
		public static void Serialize(List<FerrisFileInfo> files, string filePath)
		{
			XmlSerializer serializer = new XmlSerializer(files.GetType());
			using (var writer = XmlWriter.Create(filePath))
			{
				serializer.Serialize(writer, files);
			}
		}

		public static List<FerrisFileInfo> Deserialize(string filePath)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<FerrisFileInfo>));
			using (var reader = XmlReader.Create(filePath))
			{
				var files = serializer.Deserialize(reader);
				return (List<FerrisFileInfo>)files;
			}
		}
	}
}
