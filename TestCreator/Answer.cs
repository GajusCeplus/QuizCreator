using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCreator
{
    public class Answer
    {
        public string AnswerContent { get; set; }

        public Answer() { }
        public Answer(string content)
        {
            AnswerContent = content;
        }

        public override string ToString()
        {
            return AnswerContent;
        }
    }
}
