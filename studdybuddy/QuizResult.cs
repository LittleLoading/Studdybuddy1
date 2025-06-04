using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studdybuddy
{
    public class QuizResult
    {
        public string Category { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public double Score { get; set; } // percentage 
    }
}
