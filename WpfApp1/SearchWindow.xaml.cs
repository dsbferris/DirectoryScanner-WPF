using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Handles drop of folders into ListView
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LVFolders_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] dirs = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach(var dir in dirs)
				{
					if (Directory.Exists(dir)) LVFolders.Items.Add(dir);
				}
			}
		}

		/// <summary>
		/// Handles delete keypress in ListView for folders
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LVFolders_KeyDown(object sender, KeyEventArgs e)
		{
			if (LVFolders.SelectedItems.Count > 1)
			{
				if(MessageBoxResult.Yes == MessageBox.Show("Are you sure to remove all folders from selection?", "Think about it", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes))
				{
					foreach (var item in LVFolders.SelectedItems) LVFolders.Items.Remove(item);
				}
			}
			else if (LVFolders.SelectedItems.Count == 1) LVFolders.Items.Remove(LVFolders.SelectedItem);
		}

		/// <summary>
		/// Handles adding of extensions into TreeView from Textbox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TbExtensionAdd_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				List<string> extensions = GetExtensions();
				string extension = TbExtensionAdd.Text;
				
				if(!String.IsNullOrEmpty(extension) && extension.Length > 0 && extension != " ")
				{
					if(!extensions.Contains(extension))
					TVExtensions.Items.Add(new CheckBox()
					{
						Content = extension,
						IsChecked = true,
					});
				}
					
			}
		}

		/// <summary>
		/// Opens dialog for adding folders
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LVFolders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog("Select folders to add")
			{
				IsFolderPicker = true,
				Multiselect = true,
				//DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
			};
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				var dirs = dialog.FileNames;
				foreach (var dir in dirs)
				{
					if (Directory.Exists(dir)) LVFolders.Items.Add(dir);
				}
			}
		}


		/// <summary>
		/// Gather all checked extensions from TreeView
		/// </summary>
		/// <returns></returns>
		private List<string> GetExtensions()
		{
			List<string> extensions = new List<string>();
			foreach(var item in TVExtensions.Items)
			{
				if(item.GetType() == typeof(CheckBox))
				{
					CheckBox cb = (CheckBox)item;
					if(cb.IsChecked == true) extensions.Add(cb.Content.ToString());
				}
				else if (item.GetType() == typeof(TreeViewItem))
				{
					foreach(var subitem in ((TreeViewItem)item).Items)
					{
						CheckBox cb = (CheckBox)subitem;
						if(cb.IsChecked == true) extensions.Add(cb.Content.ToString());
					}
				}
			}
			return extensions;
		}

		/// <summary>
		/// Get all selected Folders from ListView
		/// </summary>
		/// <returns></returns>
		private List<string> GetFolders()
		{
			List<string> folders = new List<string>();
			foreach(var item in LVFolders.Items)
			{
				if (Directory.Exists(item.ToString())) folders.Add(item.ToString());
			}
			return folders;
		}


		/// <summary>
		/// Handles start of scanning
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void BtnStart_Click(object sender, RoutedEventArgs e)
		{
			List<string> folders = GetFolders();
			if (folders.Count < 1) return;
			List<string> extensions = GetExtensions();
			if (extensions.Count < 1) return;
			if (extensions.Contains("*")) extensions = null;
			PBScan.Visibility = Visibility.Visible;

			List<FerrisFileInfo> files = new List<FerrisFileInfo>();
			foreach (var dir in folders)
			{
				var t = FileScanner.scanFolder(dir, extensions);
				files.AddRange(await t);
			}

			ListWindow lw = new ListWindow(files);
			lw.Show();
			this.Close();

		}

		private void BtnOpenScan_Click(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog("Select scan file")
			{
				Multiselect = false
			};
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				var files = Serializer.Deserialize(dialog.FileName);
				ListWindow lw = new ListWindow(files);
				lw.Show();
				this.Close();
			}
		}
	}
}
