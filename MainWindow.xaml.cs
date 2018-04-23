/*
 * Couper Ebbs-Picken
 * 4/18/2018
 * tell you what words you can make with the letters you drew for the first word of scrabble
 */ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace u3_Srabble_Couper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // declaring some global variables
        string allLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        StreamWriter streamWriter;
        StreamReader streamReader;
        int counterInLetters = 0;
        int counterInWord = 0;
        string line;
        bool hasLetter;
        bool letterAmount;
        int blankCounter = 0;
        int letterCounter = 0;
        int wrongCounter = 0;
        int tempInt;
        int maxPoints = 0;
        string maxPointWord;

        public MainWindow()
        {
            InitializeComponent();

            // creates an instance of the scrabble game class and draws initial tiles
            ScrabbleGame sg = new ScrabbleGame();
            string yourLetters = sg.drawInitialTiles();
            MessageBox.Show("The letters you drew are: " + yourLetters);

            // loop that will deleter the letters you drew from the letters you have, leaving letters you can't use
            for (int i = 0; i < allLetters.Length; i++)
            {
                if (yourLetters.Contains(allLetters[i]))
                {
                    allLetters = allLetters.Remove(i, 1);
                    i--;
                }
            }

            // initializing the streamreader and writer
            streamReader = new StreamReader("Words.txt");
            streamWriter = new StreamWriter("RightWords.txt");

            // loop that will run through the list of words
            while (!streamReader.EndOfStream)
            {
                // resetting the values of some variables
                blankCounter = 0;
                letterCounter = 0;
                wrongCounter = 0;

                // setting the line
                line = streamReader.ReadLine().ToUpper();

                // one set of conditions that will run if you don't have a blank tile
                if (!yourLetters.Contains(" "))
                {

                    // if a word has one of the letters you don't have it isn't written
                    for (int j = 0; j < allLetters.Length; j++)
                    {
                        hasLetter = false;
                        if (line.Contains(allLetters[j]))
                        {
                            hasLetter = true;
                            break;
                        }
                    }

                    // this loop will run if the word only has letters you have
                    if (hasLetter != true)
                    {

                        // checking to make sure that the amount of times a letter is in your letters, and in the word are the same
                        for (int i = 0; i < 7; i++)
                        {
                            // resets some variable values
                            counterInLetters = 0;
                            counterInWord = 0;
                            
                            // runs through the letters in the word 
                            foreach (char character in line)
                            {
                                if (character == yourLetters[i])
                                {
                                    counterInWord++;
                                }
                            }

                            // runs through the letters you have in your tiles
                            foreach (char character in yourLetters)
                            {
                                if (character == yourLetters[i])
                                {
                                    counterInLetters++;
                                }
                            }

                            // checks if a letter is in the word more than your tiles
                            if (counterInWord <= counterInLetters)
                            {
                                letterAmount = true;
                            }

                            else
                            {
                                letterAmount = false;
                                break;
                            }


                        }
                    }

                    // checks to see if all the criteria are met for the word to be used
                    if (hasLetter != true
                        && letterAmount == true)
                    {
                        tempInt = 0;
                        // calculates the score of the word, if it's higher than the previous highest, it is set as the highest
                        foreach (char character in line)
                        {
                            ScrabbleLetter scrabbleLetter = new ScrabbleLetter(character);
                            tempInt += scrabbleLetter.Points;
                            if (tempInt > maxPoints)
                            {
                                maxPoints = tempInt;
                                maxPointWord = line;
                            }
                        }
                        
                        // writes the word that works into a file
                        streamWriter.Write(line + "\r" + "\n");
                    }
                }

                // this code runs if you have a blank tile
                else
                {
                    // runs through your letters to see how many blank tiles you have
                    for (int l = 0; l < yourLetters.Length; l++)
                    {
                        if (yourLetters[l] == ' ')
                        {
                            blankCounter++;
                        }
                    }

                    // checks to see how many letters the word has that you don't have, if you have the same, or more blank tiles the word is still good
                    for (int j = 0; j < allLetters.Length; j++)
                    {
                        hasLetter = false;

                        if (line.Contains(allLetters[j]))
                        {
                            letterCounter++;
                        }

                        if (letterCounter > blankCounter)
                        {
                            hasLetter = true;
                            break;
                        }

                    }

                    // runs if the word works based off the code above
                    if (hasLetter != true)
                    {

                        // checking to make sure that the amount of times a letter is in your letters, and in the word are the same, and if not, that you have enough blank tiles
                        for (int i = 0; i < 7; i++)
                        {
                            wrongCounter = 0;
                            counterInLetters = 0;
                            counterInWord = 0;

                            // runs through and checks all the letters in your word
                            foreach (char character in line)
                            {
                                if (character == yourLetters[i])
                                {
                                    counterInWord++;
                                }

                                // adds one to the counter that sees how many letters don't work
                                if (!yourLetters.Contains(character))
                                {
                                    wrongCounter++;
                                }
                            }

                            // runs through all the letters in your tiles
                            foreach (char character in yourLetters)
                            {
                                if (character == yourLetters[i])
                                {
                                    counterInLetters++;
                                }
                            }

                            // checks to see if all the criteria are met
                            if (counterInWord <= counterInLetters 
                                && wrongCounter <= blankCounter)
                            {
                                letterAmount = true;
                            }

                            else
                            {
                                letterAmount = false;
                                break;
                            }
                        }

                        // checks to see if the word is good, and calculates how many points it would have, if it's higher than the previous highest it becomes the highest
                        if (hasLetter != true
                        && letterAmount == true)
                        {
                            tempInt = 0;
                            foreach (char character in line)
                            {
                                ScrabbleLetter scrabbleLetter = new ScrabbleLetter(character);
                                tempInt += scrabbleLetter.Points;
                                if (tempInt > maxPoints)
                                {
                                    maxPoints = tempInt;
                                    maxPointWord = line;
                                }
                            }
                            streamWriter.Write(line + "\r" + "\n");
                        }
                    }





                }
                



            }

            // closes the reader and writer
            streamReader.Close();
            streamWriter.Flush();
            streamWriter.Close();

            // shows which word has the most points
            MessageBox.Show("The word that will make the most points is: " + maxPointWord + "\r" + "\n" + "With " + maxPoints.ToString() + " points");
            MessageBox.Show("All the Words you can make are:");

            // opens the text file with all the right words in it
            System.Diagnostics.Process.Start("RightWords.txt");

            // closes the main window
            Application.Current.Shutdown();
        }
    }
}


