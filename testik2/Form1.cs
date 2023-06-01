using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace testik2
{
    public partial class Form1 : Form
    {
        private List<Question> questions;
        private int currentQuestionIndex;
        private int correctAnswersCount;
        public Form1()
        {
            InitializeComponent();
            LoadQuestionsFromFile("TextFile.txt");
            InitializeTest();
        }
        private void LoadQuestionsFromFile(string fileName)
        {
            questions = new List<Question>();

            try
            {
                string[] lines = File.ReadAllLines(fileName);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    string questionText = parts[0];
                    List<string> answerOptions = new List<string>();

                    for (int i = 1; i < parts.Length - 1; i++)
                    {
                        answerOptions.Add(parts[i]);
                    }

                    int correctAnswerIndex = int.Parse(parts[parts.Length - 1]);

                    Question question = new Question(questionText, answerOptions, correctAnswerIndex);
                    questions.Add(question);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке вопросов: " + ex.Message);
            }
        }
        private void InitializeTest()
        {
            currentQuestionIndex = 0;
            correctAnswersCount = 0;
            progressBar1.Value = 0;

            if (currentQuestionIndex < questions.Count)
            {
                Question currentQuestion = questions[currentQuestionIndex];
                ShowQuestion(currentQuestion);
            }
        }
        public class Question
        {
            public string Text { get; set; }
            public List<string> AnswerOptions { get; set; }
            public int CorrectAnswerIndex { get; set; }

            public Question(string text, List<string> answerOptions, int correctAnswerIndex)
            {
                Text = text;
                AnswerOptions = answerOptions;
                CorrectAnswerIndex = correctAnswerIndex;
            }
        }
        private void ClearQuestion()
        {
            label1.Text = string.Empty;

            foreach (Control control in Controls)
            {
                if (control is RadioButton)
                {
                    Controls.Remove(control);
                    control.Dispose();
                }
            }
        }
        private void ShowQuestion(Question question)
        {
            label1.Text = question.Text;

            for (int i = 0; i < question.AnswerOptions.Count; i++)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Text = question.AnswerOptions[i];
                radioButton.AutoSize = true;
                radioButton.Location = new System.Drawing.Point(20, 100 + i * 30);
                radioButton.Tag = i;
                radioButton.CheckedChanged += radioButton_CheckedChanged;

                Controls.Add(radioButton);
            }

            button1.Enabled = false;
        }
        private void ShowResult()
        {
            double percentage = (correctAnswersCount * 100.0) / questions.Count;
            MessageBox.Show("Результат: " + percentage.ToString("F2") + "% правильных ответов");

            InitializeTest();
        }
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            int selectedAnswerIndex = (int)radioButton.Tag;

            if (selectedAnswerIndex == questions[currentQuestionIndex].CorrectAnswerIndex)
            {
                correctAnswersCount++;
            }

            button1.Enabled = true;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            currentQuestionIndex++;
            progressBar1.Value = (currentQuestionIndex * 100) / questions.Count;

            ClearQuestion();
            if (currentQuestionIndex < questions.Count)
            {
                Question currentQuestion = questions[currentQuestionIndex];
                ShowQuestion(currentQuestion);
            }
            else
            {
                ShowResult();
            }
        }
    }
}
