using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCreator
{
    public class Test
    {
        #region Properties
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        #endregion


        #region Constructors
        // default constructor for json deserializer
        public Test() { }

        public Test(string name)
        {
            Name = name;
            Questions = new List<Question>();
        }

        public Test(string name, List<Question> questions)
        {
            Name = name;
            Questions = questions;
        }
        #endregion


        #region Functions
        //zapamiętanie istnienia pytania
        public bool QuestionExists(string question)
        {
            int occurrence = 0;
            foreach (var item in Questions)
            {
                if (item.QuestionContent.Equals(question))
                    occurrence++;
            }

            if (occurrence > 0) return true;
            else return false;
        }

        //znalezienie pytania
        public Question FindQestion(string question)
        {
            foreach (var item in Questions)
            {
                if (item.QuestionContent.Equals(question))
                    return item;
            }
            return null;
        }

        //edycja
        public void EditQuestion(int index, Question newQuestion)
        {
            Questions.RemoveAt(index);
            Questions.Insert(index, newQuestion);
        }

        //lista pytań
        public List<string> QuestionsNamesList()
        {
            List<string> names = new List<string>();

            foreach (Question quest in Questions)
            {
                names.Add(quest.QuestionContent);
            }

            return names;
        }
        #endregion
    }
}
