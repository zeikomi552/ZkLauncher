using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public interface IWindowPositionConfig
    {
        public WindowPosition ViewerPosition { get; }

        public WindowPosition ControlPanelPosition { get; }


        public void SaveConfig();

        public void LoadConfig();

    }
}
