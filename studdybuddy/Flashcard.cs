using System;



namespace projekt

{

    public class Flashcard

    {

        public string ImagePath { get; set; }

        public string Answer { get; set; }



        // Constructor

        public Flashcard(string imagePath, string answer)

        {

            ImagePath = imagePath;

            Answer = answer;

        }



        public override string ToString()

        {

            return $"Image: {System.IO.Path.GetFileName(ImagePath)}, Answer: {Answer}";

        }

    }

}