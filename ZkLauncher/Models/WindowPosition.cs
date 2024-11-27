using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZkLauncher.Common.Utilities;

namespace ZkLauncher.Models
{
    public class WindowPosition : BindableBase
    {
        #region Y座標
        /// <summary>
        /// Y座標
        /// </summary>
        double _Top = 100;
        /// <summary>
        /// Y座標
        /// </summary>
        public double Top
        {
            get
            {
                return _Top;
            }
            set
            {
                if (!_Top.Equals(value) && value >= 0)
                {
                    _Top = value;
                    RaisePropertyChanged("Top");
                }
            }
        }
        #endregion

        #region X座標
        /// <summary>
        /// X座標
        /// </summary>
        double _Left = 100;
        /// <summary>
        /// X座標
        /// </summary>
        public double Left
        {
            get
            {
                return _Left;
            }
            set
            {
                if (!_Left.Equals(value) && value >= 0)
                {
                    _Left = value;
                    RaisePropertyChanged("Left");
                }
            }
        }
        #endregion

        #region 高さ
        /// <summary>
        /// 高さ
        /// </summary>
        double _Height = 100;
        /// <summary>
        /// 高さ
        /// </summary>
        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (!_Height.Equals(value) && value > 100)
                {
                    _Height = value;
                    RaisePropertyChanged("Height");
                }
            }
        }
        #endregion

        #region 幅
        /// <summary>
        /// 幅
        /// </summary>
        double _Width = 100;
        /// <summary>
        /// 幅
        /// </summary>
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (!_Width.Equals(value) && value > 100)
                {
                    _Width = value;
                    RaisePropertyChanged("Width");
                }
            }
        }
        #endregion

        public void RefreshPosition()
        {
            try
            {
                RaisePropertyChanged("Top");
                RaisePropertyChanged("Left");
                RaisePropertyChanged("Height");
                RaisePropertyChanged("Width");
            }
            catch
            {
                throw;
            }
        }
    }
}
