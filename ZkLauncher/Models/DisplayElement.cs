using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public class DisplayElement : BindableBase
    {
        #region URI
        /// <summary>
        /// URI
        /// </summary>
        string _URI = string.Empty;
        /// <summary>
        /// URI
        /// </summary>
        public string URI
        {
            get
            {
                return _URI;
            }
            set
            {
                if (_URI == null || !_URI.Equals(value))
                {
                    _URI = value;
                    RaisePropertyChanged("URI");
                }
            }
        }
        #endregion

        #region イメージファイルパス
        /// <summary>
        /// イメージファイルパス
        /// </summary>
        string _ImagePath = string.Empty;
        /// <summary>
        /// イメージファイルパス
        /// </summary>
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                if (_ImagePath == null || !_ImagePath.Equals(value))
                {
                    _ImagePath = value;
                    RaisePropertyChanged("ImagePath");
                }
            }
        }
        #endregion


    }
}
