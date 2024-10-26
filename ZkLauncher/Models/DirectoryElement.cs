using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public class DirectoryElement : BindableBase
    {
        #region ディレクトリパス
        /// <summary>
        /// ディレクトリパス
        /// </summary>
        string _DirectoryPath = string.Empty;
        /// <summary>
        /// ディレクトリパス
        /// </summary>
        public string DirectoryPath
        {
            get
            {
                return _DirectoryPath;
            }
            set
            {
                if (_DirectoryPath == null || !_DirectoryPath.Equals(value))
                {
                    _DirectoryPath = value;
                    RaisePropertyChanged("DirectoryPath");
                    RaisePropertyChanged("DirectoryName");
                }
            }
        }
        #endregion

        #region ディレクトリ名
        /// <summary>
        /// ディレクトリ名
        /// </summary>
        public string DirectoryName
        {
            get
            {
                if (!string.IsNullOrEmpty(this._DirectoryPath))
                {
                    return System.IO.Path.GetFileName(this._DirectoryPath)!;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion


    }
}
