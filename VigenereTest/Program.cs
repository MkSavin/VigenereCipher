using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VigenereTest {
    /*
     Creator: MkSavin @ Elerance.com
    */
    class Program {

        public static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ,.;-!:?*()_=+/'|\"\\\n";
        public static string alphabet_eng = "abcdefghijklmnopqrstuvwxyz ,.;-!:?*()_=+/'|\"\\\n";

        public static int specialCharsAlphabetSize = 20;

        static void Main(string[] args) {

            Console.WriteLine("Code: ");
            var code = Console.ReadLine().ToLower();

            var inputString = "";
            var input = "";

            Console.WriteLine("Decode? [empty = no]: ");
            var encrypt = Console.ReadLine() == "";

            Console.WriteLine("Input string [empty = next step]: ");
            do {
                input = Console.ReadLine().ToLower();
                inputString += input + '\n';
            } while (input != "");

            inputString = inputString.Trim();

            var codeAlphabet = CheckIsEnglishAlphabet(code);
            var inputStringAlphabet = CheckIsEnglishAlphabet(inputString);

            if (codeAlphabet != inputStringAlphabet) {
                Console.WriteLine("Alphabets is not equal");
                Console.ReadKey();
                return;
            }

            var result = Vigenere(codeAlphabet ? alphabet_eng : alphabet, code, inputString, encrypt);

            Console.WriteLine("Result:");
            Console.WriteLine(result);

            Console.WriteLine("M? [empty = no]:");

            input = Console.ReadLine();

            if (input == "") {
                return;
            }

            int m;

            if (!int.TryParse(input, out m)) {
                Console.WriteLine("Input is not integer");
                Console.ReadKey();
                return;
            }

            result = Regex.Replace(result, @"[^\S\r\n]+", "");

            while (result.Length > m) {
                Console.Write(result.Substring(0, m) + " ");
                result = result.Substring(m);
            }

            Console.Write(result);

            Console.ReadKey();

        }

        static bool CheckIsEnglishAlphabet(string input) {

            int index;

            foreach (var c in input) {
                index = alphabet.IndexOf(c);

                if (index >= alphabet.Length - specialCharsAlphabetSize) {
                    continue;
                }

                if (index < 0) {
                    return true;
                }
            }

            return false;

        }

        private static int[] codePositions;
        private static int[] inputStringPositions;
        private static string resultPositions;

        static string Vigenere(string alphabet, string code, string inputString, bool encrypt) {

            codePositions = new int[code.Length];
            inputStringPositions = new int[inputString.Length];
            resultPositions = "";

            Console.WriteLine("Vigenere for code: " + code + ", inputString: " + inputString + ": [");

            int currentPos;

            int j = 0;

            Console.WriteLine("Code positions:");

            foreach (var c in code) {
                codePositions[j++] += currentPos = alphabet.IndexOf(c);
                Console.Write(currentPos + " ");
            }

            Console.WriteLine();

            j = 0;

            Console.WriteLine("Input string positions:");

            foreach (var c in inputString) {
                inputStringPositions[j++] += currentPos = alphabet.IndexOf(c);
                Console.Write(currentPos + " ");
            }

            Console.WriteLine();

            int spaces = 0;
            int codePos;

            Console.WriteLine("Result positions:");

            for (int i = 0; i < inputStringPositions.Length; i++) {

                if (inputStringPositions[i] >= alphabet.Length - specialCharsAlphabetSize) {
                    resultPositions += alphabet[inputStringPositions[i]];
                    spaces++;
                    continue;
                }

                codePos = i % code.Length - spaces % code.Length;

                if (codePos < 0) {
                    codePos += code.Length;
                }

                if (encrypt) {
                    currentPos = (inputStringPositions[i] + codePositions[codePos]) % (alphabet.Length - specialCharsAlphabetSize);
                } else {
                    currentPos = (inputStringPositions[i] - codePositions[codePos]) % (alphabet.Length - specialCharsAlphabetSize);
                    if (currentPos < 0) {
                        currentPos += (alphabet.Length - specialCharsAlphabetSize);
                    }
                }

                if (currentPos < 0) {
                    continue;
                }

                resultPositions += alphabet[currentPos];

                Console.Write(currentPos + " ");

            }

            Console.WriteLine();

            Console.WriteLine("Result:");

            Console.WriteLine(resultPositions);

            Console.WriteLine("]");

            return resultPositions;

        }

    }
}
