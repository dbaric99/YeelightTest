using System;
using System.Collections.Generic;
using System.Text;

namespace LightControl
{
    public class LightState
    {
        public bool IsTurnOn { get; set; }
        public string Color { get; set; }
        public string DeviceName { get; set; }
        public int BrightnessLevel { get; set; }
    }
}
