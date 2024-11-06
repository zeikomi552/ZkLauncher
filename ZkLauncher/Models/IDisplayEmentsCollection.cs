using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public interface IDisplayEmentsCollection
    {
        #region 表示要素
        /// <summary>
        /// 表示要素
        /// </summary>
        public ObservableCollection<DisplayElement> Elements
        {
            get;set;
        }
        #endregion

        #region 背景画像
        public string ControlBackgroundMediaPath {  get; set; }
        public string ViewerBackgroundMediaPath { get; set; }
        #endregion
        #region 選択要素
        /// <summary>
        /// 選択要素
        /// </summary>
        public DisplayElement SelectedItem
        {
            get;set;
        }
        #endregion

        public string DrawPictureSaveDirectoryPath { get; set; }
        public int WaitSecond { get; set; }

        public DisplayElement? First();
        public void SelectFirst();
        public void SelectLast();

        public void SetElements(DisplayElemetCollection item);

        public void LoadConfig();

        public void SaveConfig();

        public void SelectedNavigate();
        public void NextNavigate();
        public void PrevNavigate();

        public void Remove(DisplayElement delete_item);

        public void Add(DisplayElement item);
        public bool AddClipboardElement();

        public void SetupTimer();
        public void StopTimer();
        public void StartTimer();

        public void ReloadURL();

        public void MoveUP();
        public void MoveDown();
        public void SelectedItemDelete();

        public void AutoSetThumbnail();
        public void NextNavigatePage();
        public void PrevNavigatePage();
    }
}
