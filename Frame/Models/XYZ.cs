using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameCreation.Models
{
    public class XYZ
    {

        public string CreatedName { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public XYZ(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
