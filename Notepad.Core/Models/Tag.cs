

using System.ComponentModel.DataAnnotations;

namespace Notepad.Core.Models
{
    [Flags]
    public enum Tag
    {
       None = 0,
       PHONE = 1,
       EMAIL = 2,
    }
}
