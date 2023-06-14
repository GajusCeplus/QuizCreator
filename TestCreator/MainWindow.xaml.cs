using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<string> ExistingTestsNames { get; set; }

        public string SelectedTestName { get { return cboxExistingTests.SelectedValue.ToString(); } }

        public string TestName { get { return tboxName.Text; } }

        public Test CurrentTest { get; set; }

        public int LastSelected { get; set; }
        public int SelectedQuestionIndex { get { return questionsList.SelectedIndex; } }


        public MainWindow()
        {
            InitializeComponent();
            CurrentTest = new Test("");
            LastSelected = -1;
        }


        #region Events functions
        //dodanie pytania 
        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            AddQuestion();
        }
        //zmiana pytania
        private void btnEditQuestion_Click(object sender, RoutedEventArgs e)
        {
            EditQuestion();
        }
        //usunięcie pytania
        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            DeleteQuestion();
        }
        //zapis testu
        private void btnSaveTest_Click(object sender, RoutedEventArgs e)
        {
            SaveTest();
        }
        //combobox
        private void CboxExistingTests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTest();
        }
        private void CboxExistingTests_DropDownOpened(object sender, EventArgs e)
        {
            LoadExistingTests();
            Console.WriteLine("yo, angelo");
        }
        //lista pytań
        private void QuestionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillQuestionInputs();
        }
        //usunięcie testu
        private void BtnDeleteTest_Click(object sender, RoutedEventArgs e)
        {
            DeleteTest();
        }
        //nowy test
        private void BtnNewTest_Click(object sender, RoutedEventArgs e)
        {
            CurrentTest = new Test("");
            ClearAllInputs();
            ClearQuestionsList();
        }
        //nazwa testu display lewy
        private void TboxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            updatePreviewName();
        }
        #endregion



        #region Custom functions
        //dodanie pytania
        private void AddQuestion()
        {
            if (tboxQuestion.Text.Trim().Equals(""))
            {
                MessageBox.Show("Pytanie musi mieć treść", "Nie można dodać pytania", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            List<string> answers = new List<string>();

            answers.Add(tboxAnswerA.Text.Trim());
            answers.Add(tboxAnswerB.Text.Trim());
            answers.Add(tboxAnswerC.Text.Trim());
            answers.Add(tboxAnswerD.Text.Trim());

            if (answers[0].Equals("") || answers[1].Equals("") || answers[2].Equals("") || answers[3].Equals(""))
            {
                MessageBox.Show("Wszystkie odpowiedzi muszą być wypełnione", "Nie można dodać pytania", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (CurrentTest.QuestionExists(tboxQuestion.Text))
            {
                MessageBox.Show("Takie pytanie już istnieje", "Nie można dodać pytania", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //change item - backend
            int correctAnswer = GetCorrectAnswer();
            CurrentTest.Questions.Add(new Question(tboxQuestion.Text, answers, correctAnswer));

            //change item - frontend
            UpdateQuestions();
        }

        //zmiana pytania
        private void EditQuestion()
        {
            if (SelectedQuestionIndex < 0)
            {
                MessageBox.Show("Nie wybrano żadnego pytania", "Nie można wykonać akcji", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //change item - backend
            int correctAnswer = GetCorrectAnswer();
            CurrentTest.EditQuestion(SelectedQuestionIndex, new Question(tboxQuestion.Text, tboxAnswerA.Text, tboxAnswerB.Text, tboxAnswerC.Text, tboxAnswerD.Text, correctAnswer));

            //change item - frontend
            UpdateQuestions();
        }

        //zaznaczenie poprawnego pytania
        private int GetCorrectAnswer()
        {
            if (correctA.IsChecked == true) return 0;
            else if (correctB.IsChecked == true) return 1;
            else if (correctC.IsChecked == true) return 2;
            else return 3;
        }

        //usunięcie pytania
        private void DeleteQuestion()
        {
            if (LastSelected > -1)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć wybrane pytanie?", "Ostrzeżenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) return;

                //remove item - backend
                CurrentTest.Questions.RemoveAt(LastSelected);

                //remove item - frontend
                UpdateQuestions();
                ClearQuestionInputs();

                LastSelected--;
            }
        }

        //zapisanie testu (zapis do txt i do jsona)
        private void SaveTest()
        {
            //na szybko zmienione na txt z json
            MessageBoxResult check = MessageBoxResult.Yes;

            if (ExistingTestsNames.Contains(TestName))
            {
                check = MessageBox.Show($"Test o nazwie '{TestName}' już istnieje. Chcesz go nadpisać?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }

            if (check == MessageBoxResult.Yes)
            {
                if (TestName.Trim().Equals(""))
                {
                    MessageBox.Show("Nie podano nazwy testu", "Nie można zapisać testu", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {

                    CurrentTest.Name = TestName;
                    string wyj = "";
                    int ans = 0;
                    for (int i=0;i<CurrentTest.Questions.Count;i++)
                    {
                        Question quest = CurrentTest.Questions[i];
                        wyj += quest.QuestionContent + ";";
                        wyj += quest.Answers[0].ToString() + ";";
                        wyj += quest.Answers[1].ToString() + ";";
                        wyj += quest.Answers[2].ToString() + ";";
                        wyj += quest.Answers[3].ToString() + ";";
                        ans = quest.CorrectAnswer;
                        switch (ans)
                        {
                            case 0:
                                wyj += "A\n";
                                break;
                            case 1:
                                wyj += "B\n";
                                break;
                            case 2:
                                wyj += "C\n";
                                break;
                            case 3:
                                wyj += "D\n";
                                break;
                            default:
                                wyj += "A\n";
                                break;
                        }
                       
                    }
                    if (!Directory.Exists("tests"))
                    {
                        Directory.CreateDirectory("tests");
                    }
                    string wyjscieEnciphertxt = Caesar.Encipher(wyj, 5);
                    File.WriteAllText($@"tests\{TestName}.txt", wyjscieEnciphertxt);
                    MessageBox.Show("Zapisano pomyślnie do txt", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadExistingTests();
                    string wyjscie = JsonConvert.SerializeObject(CurrentTest);
                    //zakodowanie wyjscia klucz przesunięcia = 5
                    string wyjscieEncipher = Caesar.Encipher(wyjscie, 5);
                    File.WriteAllText($@"tests\{TestName}.json", wyjscieEncipher);

                    MessageBox.Show("Zapisano pomyślnie do jsona", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadExistingTests();
                }
            }
        }


        private void DeleteTest()
        {
            if (CurrentTest.Name.Equals(""))
            {
                MessageBox.Show("Nie wybrano żadnego testu", "Nie można wykonać akcji", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć test '{CurrentTest.Name}'?", "Ostrzeżenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No) return;

            File.Delete($@"tests\{TestName}.txt");
            File.Delete($@"tests\{TestName}.json");
            ClearAllInputs();
            LoadExistingTests();
        }

        //Wypełnienie pytań
        private void FillQuestionInputs()
        {
            if (SelectedQuestionIndex > -1)
            {
                Question quest = CurrentTest.Questions[SelectedQuestionIndex];

                tboxQuestion.Text = quest.QuestionContent;
                tboxAnswerA.Text = quest.Answers[0].ToString();
                tboxAnswerB.Text = quest.Answers[1].ToString();
                tboxAnswerC.Text = quest.Answers[2].ToString();
                tboxAnswerD.Text = quest.Answers[3].ToString();

                int correctAnswer = quest.CorrectAnswer;

                //check correct answer
                if (correctAnswer == 0) correctA.IsChecked = true;
                else correctA.IsChecked = false;

                if (correctAnswer == 1) correctB.IsChecked = true;
                else correctB.IsChecked = false;

                if (correctAnswer == 2) correctC.IsChecked = true;
                else correctC.IsChecked = false;

                if (correctAnswer == 3) correctD.IsChecked = true;
                else correctD.IsChecked = false;
            }

            LastSelected = SelectedQuestionIndex;
        }

        //Wczytanie testu
        private void LoadTest()
        {
            if (cboxExistingTests.SelectedIndex > -1)
            {
                CurrentTest = GetTestFromFile(SelectedTestName);

                tboxName.Text = CurrentTest.Name;
                ClearQuestionInputs();

                UpdateQuestions();
            }
        }

        //Wczytanie testu z pliku (z jsona)
        private Test GetTestFromFile(string path)
        {
            string fileContent = File.ReadAllText($"tests\\{path}.json");
            //odszyfrowanie tekstu klucz=5
            string ContentDecipher = Caesar.Decipher(fileContent, 5);
            Test test = JsonConvert.DeserializeObject<Test>(ContentDecipher);

            return test;
        }

        //Wczytanie istniejących testów
        private void LoadExistingTests()
        {
            ExistingTestsNames = new List<string>();

            foreach (string path in Directory.GetFiles("tests"))
            {
                if (System.IO.Path.GetExtension(path).Equals(".json"))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                    ExistingTestsNames.Add(fileName);
                }
            }

            cboxExistingTests.ItemsSource = null;
            cboxExistingTests.ItemsSource = ExistingTestsNames;
        }

        //update pytania
        private void UpdateQuestions()
        {
            questionsList.ItemsSource = null;
            questionsList.ItemsSource = CurrentTest.QuestionsNamesList();
        }
        #endregion



        //cleaning etc.
        #region View manipulation functions
        private void ClearAllInputs()
        {
            tboxName.Text = "";
            ClearQuestionInputs();
        }

        private void ClearQuestionInputs()
        {
            tboxQuestion.Text = "";
            tboxAnswerA.Text = "";
            tboxAnswerB.Text = "";
            tboxAnswerC.Text = "";
            tboxAnswerD.Text = "";
            correctA.IsChecked = true;
        }


        private void ClearQuestionsList()
        {
            questionsList.ItemsSource = null;
        }


        private void updatePreviewName()
        {
            if (!tboxName.Text.Equals("")) testNamePreview.Text = tboxName.Text;
            else testNamePreview.Text = "Nowy test";
        }

        #endregion
    }
}
