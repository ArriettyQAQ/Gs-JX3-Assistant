﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsJX3NIA.Classes.HID.Mouse
{
    public interface IMouseSimulator
    {
        void Click(Point point, int mouseButton = 1, bool dblClick = false);
    }
}
