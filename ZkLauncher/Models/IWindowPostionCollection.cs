using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkLauncher.Models
{
    public interface IWindowPostionCollection
    {
        public WindowPosition ViewerPosition { get; set; }

        public WindowPosition ControlPanelPosition {  get; set; }

        public void LoadConfig();
        public void SaveConfig();
    }
}
