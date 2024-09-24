using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public class WindowPosition : BindableBase
    {
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
                if (!_Height.Equals(value))
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
                if (!_Width.Equals(value))
                {
                    _Width = value;
                    RaisePropertyChanged("Width");
                }
            }
        }
        #endregion

        #region X座標位置
        /// <summary>
        /// X座標位置
        /// </summary>
        double _Xpos = 100;
        /// <summary>
        /// X座標位置
        /// </summary>
        public double Xpos
        {
            get
            {
                return _Xpos;
            }
            set
            {
                if (!_Xpos.Equals(value))
                {
                    _Xpos = value;
                    RaisePropertyChanged("Xpos");
                }
            }
        }
        #endregion

        #region Y座標位置
        /// <summary>
        /// Y座標位置
        /// </summary>
        double _Ypos = 100;
        /// <summary>
        /// Y座標位置
        /// </summary>
        public double Ypos
        {
            get
            {
                return _Ypos;
            }
            set
            {
                if (!_Ypos.Equals(value))
                {
                    _Ypos = value;
                    RaisePropertyChanged("Ypos");
                }
            }
        }
        #endregion


    }
}
