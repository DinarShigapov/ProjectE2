using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectE2.Model
{
    public partial class RaitingSystem
    {
        public string MarkName
        {
            get 
            {
                string fullName = string.Empty;

                switch (RaitingName)
                {
                    case 2:
                        fullName = $"{RaitingName} (неудовлетворительно)"; 
                        break;
                    case 3:
                        fullName = $"{RaitingName} (удовлетворительно)";
                        break;
                    case 4:
                        fullName = $"{RaitingName} (хорошо)";
                        break;
                    case 5:
                        fullName = $"{RaitingName} (отлично)";
                        break;

                }
                return fullName;
            }
        }
    }
}
