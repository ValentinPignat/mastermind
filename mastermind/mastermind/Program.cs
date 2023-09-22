using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace mastermind
{
    internal class Program
    {
        /// <summary>
        /// Jeu de Mastermind sur la console 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ///Initialisations
            string colorPool = "YBRGWCP";
            string currentGuess = "";
            char replay;

            /// Message de bienvenue
            Console.WriteLine("Bienvenue sur Mastermind!\nCouleurs possibles: YBRGWCP\nDevine le code en 4 couleurs (ex.YBYP)\n");

            do
            {
                /// Réinitialisation code
                string secretCode = "";

                /// Generer Code aléatoire (index aléatoire dans colorPool)
                Random rnd = new Random();
                for (int i = 0; i < 4; i++)
                {
                    int x = rnd.Next(0, 6);
                    secretCode += colorPool[x];

                    ///TEST
                    ///Console.WriteLine(secretCode);
                }
                Console.WriteLine();
                ///Boucle de 10 essais
                for (int i = 0; i < 10; i++)
                {
                    /// Local variables et secretCodePool pour verifier mal placés sans doublons
                    int currentRight = 0;
                    int currentWrongPlace = 0;
                    string secretCodePool = secretCode;

                    /// Récupération de l'essai
                    Console.WriteLine("Essai " + (i + 1) + ":");
                    currentGuess = Console.ReadLine().ToUpper();

                    /// Re-loop si input n'a pas la bonne longeur
                    if (currentGuess.Length != 4)
                    {
                        Console.WriteLine("\nAttention veuillez entrer 4 lettres (ex.YBYP)\n");
                        i--;
                        continue;
                    }

                    /// Re-loop si pas couleur
                    for (int j = 0; j < 4; j++)
                    {
                        if (colorPool.Contains(currentGuess[j]) == false){
                            Console.WriteLine("\nAttention veuillez entrer 4 lettres (ex.YBYP)\n");
                            i--;
                            break;
                        }
                    }

                    /// Copie pour éviter double vérification
                    string currentGuessPool = currentGuess;

                    /// Verification Vrai
                    for (int j = 0; j < 4; j++)
                    {
                        if (currentGuess[j] == secretCode[j])
                        {
                            /// Retire Verifiés du currentGuessPool
                            currentGuessPool = currentGuessPool.Remove(j - currentRight, 1);

                            /// Juste +1
                            currentRight++;

                            /// Retire Vérifiés du Code pour pas qu'il soit verifiés une deuxième fois comme "mal placé"
                            int index = secretCodePool.IndexOf(currentGuess[j]);
                            secretCodePool = secretCodePool.Remove(index, 1);

                            ///TEST
                            ///Console.WriteLine(index);
                        }
                    }

                    /// Verification le reste du currentGuessPool pour les mal placés
                    for (int j = 0; j < currentGuessPool.Length; j++)
                    {

                        if (secretCodePool.Contains(currentGuessPool[j]))
                        {
                            /// Mal placés +1
                            currentWrongPlace++;

                            /// Retire vérifiés pour ne pas avoir de doublons
                            int index = secretCodePool.IndexOf(currentGuessPool[j]);
                            secretCodePool = secretCodePool.Remove(index, 1);
                        }
                    }

                    /// Victoire -> résultats / message de fin 
                    if (currentRight == 4)
                    {
                        Console.WriteLine("\nBravo!\nTu as découvert le code en " + (i + 1) + " essais.\n");
                        break;
                    }

                    /// Feedback
                    Console.WriteLine("=> Ok: " + currentRight + "\n=> Mauvaise position: " + currentWrongPlace + "\n");

                    /// Défaite
                    if (i == 9)
                    {
                        Console.WriteLine("Vous avez perdu :<\nLe code secret était: " + secretCode + "\n");
                    }
                }
                
                /// Option replay
                Console.WriteLine("Appuyez sur R pour rejouer");
                replay = Console.ReadKey().KeyChar;
            } while (replay == 'r');
           
            /// Message au revoir
            Console.WriteLine("\nMerci d'avoir jouer et à bientôt!");

       
            /// Garde la fenêtre ouverte
            Console.ReadLine();
        }
    }
}
