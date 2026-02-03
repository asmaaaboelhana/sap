using Frame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameCreation.Models
{
    public class SAP_Column
    {
        public string Name { get; set; }
        public string PropName { get; set; }
        public XYZ P1 { get; set; }
        public XYZ P2 { get; set; }
    }
}
