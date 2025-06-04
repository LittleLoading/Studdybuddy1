using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studdybuddy
{
    public class FlashcardDetails
    {
        public Image? Image { get; set; }
        public string Answer { get; set; }

        public FlashcardDetails(Image? image, string answer)
        {
            Image = image;
            Answer = answer;
        }

    }
}
