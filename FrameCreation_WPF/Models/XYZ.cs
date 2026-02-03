using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameCreation_WPF.Models
{
    public class XYZ
    {

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public string CreatedName { get; set; }
        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
