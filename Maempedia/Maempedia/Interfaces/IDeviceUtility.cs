using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maempedia.Interfaces
{
    public interface IDeviceUtility
    {
        string DeviceName { get; }

        string AppName { get; }

        string AppVersion { get; }

        string BuildVersion { get; }

        string Copyright { get; }

        string AppPath { get; }

        double StatusBarHeight { get; }

        bool CanVibrate { get; }

        void Vibration(TimeSpan? vibrateSpan = null);
    }
}
