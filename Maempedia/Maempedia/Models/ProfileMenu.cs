using System;

namespace Maempedia.Models
{
    public class ProfileMenu
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Action Action { get; set; }
    }
}
