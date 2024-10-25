using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using ZkLauncher.Common.Utilities;
using ZkLauncher.Models;
using Clipboard = System.Windows.Clipboard;
using DialogResult = Prism.Dialogs.DialogResult;

namespace ZkLauncher.ViewModels.UserControl
{
    public class ucControlPanelViewModel : BindableBase, IDialogAware
    {
        #region IDialogAware Overwrite

        public string Title
        {
            get { return "Control Panel"; }
        }


        private DelegateCommand<string>? _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));
        public DialogCloseListener RequestClose { get; }

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.OK;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {

        }
        #endregion

        #region ウィンドウ位置
        /// <summary>
        /// ウィンドウ位置
        /// </summary>
        IWindowPostionCollection? _WindowPosition;
        /// <summary>
        /// ウィンドウ位置
        /// </summary>
        public IWindowPostionCollection? WindowPosition
        {
            get
            {
                return _WindowPosition;
            }
            set
            {
                if (_WindowPosition == null || !_WindowPosition.Equals(value))
                {
                    _WindowPosition = value;
                    RaisePropertyChanged("WindowPosition");
                }
            }
        }
        #endregion

        #region 表示要素
        /// <summary>
        /// 表示要素
        /// </summary>
        IDisplayEmentsCollection? _DisplayElements;
        /// <summary>
        /// 表示要素
        /// </summary>
        public IDisplayEmentsCollection? DisplayElements
        {
            get
            {
                return _DisplayElements;
            }
            set
            {
                if (_DisplayElements == null || !_DisplayElements.Equals(value))
                {
                    _DisplayElements = value;
                    RaisePropertyChanged("DisplayElements");
                }
            }
        }
        #endregion

        private IDialogService? _dialogService;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="displayElements"></param>
        public ucControlPanelViewModel(IDialogService dialogService, IDisplayEmentsCollection displayElements, IWindowPostionCollection windowPosition)
        {
            try
            {
                _dialogService = dialogService;
                this.DisplayElements = displayElements;
                this.DisplayElements.LoadConfig();

                this.WindowPosition = windowPosition;
                this.WindowPosition.LoadConfig();
            }
            catch
            {

            }
        }

        #region 登録処理
        /// <summary>
        /// 登録処理
        /// </summary>
        public void ContextMenu_Regist()
        {
            try
            {
                if (this.DisplayElements!.AddClipboardElement())
                {
                    this.DisplayElements.SaveConfig();
                    this.DisplayElements.LoadConfig();
                }
            }
            catch
            {

            }
        }
        #endregion


        #region リンクの削除処理
        /// <summary>
        /// リンクの削除処理
        /// </summary>
        public void ContextMenu_Delete()
        {
            try
            {
                if (this.DisplayElements != null && this.DisplayElements.SelectedItem != null)
                {
                    if (ShowMessage.ShowQuestionYesNo("選択しているリンクを削除します。よろしいですか？", "確認") == System.Windows.MessageBoxResult.Yes)
                    {
                        this.DisplayElements.Remove(this.DisplayElements.SelectedItem);
                        this.DisplayElements.SaveConfig();
                    }
                }
                else
                {
                    ShowMessage.ShowNoticeOK("削除対象が選択されていません。", "通知");
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 登録処理
        /// <summary>
        /// 登録処理
        /// </summary>
        public void ContextMenu_AutoSetTumbnail()
        {
            try
            {
                // サムネイルの作成処理
                this.DisplayElements!.AutoSetThumbnail();

                // Configへの保存
                this.DisplayElements.SaveConfig();

                // コンフィグのロード
                this.DisplayElements.LoadConfig();
            }
            catch
            {

            }
        }
        #endregion

        #region 背景の保存先ディレクトリ
        /// <summary>
        /// 背景の保存先ディレクトリ
        /// </summary>
        private string BackgroundDirectory
        {
            get
            {
                // アプリケーションフォルダの取得
                var dir = Path.Combine(PathManager.GetApplicationFolder(), "Config", "Background");
                PathManager.CreateDirectory(dir);

                return dir;
            }
        }
        #endregion

        #region 背景の登録
        /// <summary>
        /// 背景の登録
        /// </summary>
        public void RegistBackground()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new Microsoft.Win32.OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "メディアファイル (*.mp4;*.wav;*.jpg;*.png)|*.mp4;*.wav;*.jpg;*.png|全てのファイル (*.*)|*.*";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    var path = Path.Combine(BackgroundDirectory, "ControlPanel" + Path.GetExtension(dialog.FileName));

                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                    File.Copy(dialog.FileName, path, true);
                    this.DisplayElements!.ControlBackgroundMediaPath = path;
                    this.DisplayElements.SaveConfig();
                    this.DisplayElements.LoadConfig();

                }
            }
            catch(Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 選択要素の変更
        /// <summary>
        /// 選択要素の変更
        /// </summary>
        public void SelectionChanged()
        {
            try
            {
                this.DisplayElements!.SelectedNavigate();
            }
            catch
            {

            }
        }
        #endregion

        #region ChangeNameWindow表示画面の呼び出し
        /// <summary>
        /// ChangeNameWindow表示画面の呼び出し
        /// </summary>
        public void ShowChangeNameWindow()
        {
            try
            {
                if(this.DisplayElements != null && this.DisplayElements.SelectedItem != null)
                {
                    _dialogService.ShowDialog("ucNameChange", new DialogParameters($"CurrentName={this.DisplayElements.SelectedItem.Title}"), r =>
                    {
                        if (r.Result == ButtonResult.OK)
                        {
                            // 戻り値の取り出し
                            var result = r.Parameters.GetValue<string>("AfterName");

                            // 結果の設定
                            this.DisplayElements.SelectedItem.Title = result;

                            // 保存
                            this.DisplayElements.SaveConfig();
                        }
                    });
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

    }
}
