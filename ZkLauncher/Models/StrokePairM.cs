using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace ZkLauncher.Models
{
    public class StrokePairM : BindableBase
    {
        public StrokePairM(StrokeCollection added, StrokeCollection removed)
        {
            this.AddedStroke = added;
            this.RemovedStroke = removed;
        }

        public StrokeCollection AddedStroke { get; set; }
        public StrokeCollection RemovedStroke { get; set; }
    }
}
