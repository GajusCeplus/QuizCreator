using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCreator
{
    public class Question
    {
        #region Properties

        public string QuestionContent { get; set; }
        public List<Answer> Answers { get; set; }
        public int CorrectAnswer { get; set; }
        #endregion


        #region Constructors
        // default constructor for json deserializer
        public Question() { }
        public Question(string content, string answerA, string answerB, string answerC, string answerD, int correctAnswer)
        {
            Answers = new List<Answer>();

            QuestionContent = content;

            Answers.Add(new Answer(answerA));
            Answers.Add(new Answer(answerB));
            Answers.Add(new Answer(answerC));
            Answers.Add(new Answer(answerD));

            CorrectAnswer = correctAnswer;
        }
        public Question(string content, List<string> answers, int correctAnswer)
        {
            Answers = new List<Answer>();

            QuestionContent = content;
            CorrectAnswer = correctAnswer;

            foreach (string answer in answers)
            {
                Answers.Add(new Answer(answer));
            }
        }
        #endregion
    }
}
