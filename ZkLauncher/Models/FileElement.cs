using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public class FileElement : BindableBase
    {
        #region ファイルパス
        /// <summary>
        /// ファイルパス
        /// </summary>
        string _Filepath = string.Empty;
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Filepath
        {
            get
            {
                return _Filepath;
            }
            set
            {
                _Filepath = value;
                RaisePropertyChanged("Filepath");
                RaisePropertyChanged("Filename");
            }
        }
        #endregion

        #region ファイル名(拡張子なし)
        /// <summary>
        /// ファイル名(拡張子なし)
        /// </summary>
        string _Filename = string.Empty;
        /// <summary>
        /// ファイル名(拡張子なし)
        /// </summary>
        public string Filename
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(Filepath);
            }
        }
        #endregion

        #region 編集中(Save前)フラグ
        /// <summary>
        /// 編集中(Save前)フラグ
        /// </summary>
        bool _EditingF = false;
        /// <summary>
        /// 編集中(Save前)フラグ
        /// </summary>
        public bool EditingF
        {
            get
            {
                return _EditingF;
            }
            set
            {
                if (!_EditingF.Equals(value))
                {
                    _EditingF = value;
                    RaisePropertyChanged("EditingF");
                }
            }
        }
        #endregion


    }
}
