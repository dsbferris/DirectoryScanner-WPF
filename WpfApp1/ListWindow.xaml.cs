using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
	/// <summary>
	/// Interaktionslogik für ListWindow.xaml
	/// </summary>
	public partial class ListWindow : Window
	{
		private readonly List<FerrisFileInfo> Files;


		public ListWindow(List<FerrisFileInfo> files)
		{
			Files = new List<FerrisFileInfo>(files);
			InitializeComponent();
			LVFiles.ItemsSource = Files;
		}

		private void LVFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (LVFiles.SelectedItem is FerrisFileInfo item)
			{
				if (item.Favourite) item.Favourite = false;
				else item.Favourite = true;
			}

		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			if(sfd.ShowDialog() == true)
			{
				Serializer.Serialize(Files, sfd.FileName);
			}
		}
	}
}
