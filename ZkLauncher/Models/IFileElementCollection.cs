using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public interface IFileElementCollection
    {
        public string SaveDirectoryPath { get; set; }

        public ObservableCollection<FileElement> Elements { get; set; }

        public void MoveUP();

        public void MoveDown();

        public void SelectedItemDelete();
        public FileElement SelectedItem { get; set; }

        public FileElement? First();

        public void SelectFirst();
        public void SelectLast();
        public void Remove(FileElement delete_item);
        public void Add(FileElement item);
        public string DirectoryPath { get;set; }
        public bool ExecuteReadDirF { get; set; }
        public void ReadDirectory();
        public void ReadDirectory(string dir);

        public string GetFilepath();
    }
}
