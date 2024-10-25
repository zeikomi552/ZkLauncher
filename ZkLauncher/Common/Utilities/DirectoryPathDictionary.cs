using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Common.Utilities
{
    public class DirectoryPathDictionary
    {
        #region 画像ファイルの保存先ディレクトリパス
        /// <summary>
        /// 画像ファイルの保存先ディレクトリパス
        /// </summary>
        public static string ImageSaveDirectory
        {
            get
            {
                // アプリケーションフォルダの取得
                var dir = Path.Combine(PathManager.GetApplicationFolder(), "SaveImage");
                PathManager.CreateDirectory(dir);

                return dir;
            }
        }
        #endregion


        #region お絵描きファイルの保存先デフォルトパス
        /// <summary>
        /// お絵描きファイルの保存先デフォルトパス
        /// </summary>
        public static string ImageSaveDirectoryDefault
        {
            get
            {
                // アプリケーションフォルダの取得
                return Path.Combine(PathManager.GetApplicationFolder(), "SaveImage", "Default");
            }
        }
        #endregion
    }
}
