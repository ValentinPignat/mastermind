using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace mastermind
{
    /// <summary>
    /// 
    /// </summary>
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
            string secretCode = "";
            string currentGuess = "";

            /// Generer Code aléatoire
            Random rnd = new Random();
            for (int i = 0; i < 4 ; i++)
            {
                int x = rnd.Next(0, 6);
                secretCode += colorPool[x];

                ///TEST
                Console.WriteLine(secretCode);
            }

            /// Message de bienvenue
            Console.WriteLine("Bienvenue sur Masterming!\nCouleurs possibles: YBRGWCP\nDevine le code en 4 couleurs\n\n");

            ///Boucle de 10 essais
            for (int i = 0; i < 10; i++)
            {
                /// Local variables et secretCodePool pour verifier mal placés sans doublons
                int currentRight = 0;
                int currentWrongPlace = 0;
                string secretCodePool = secretCode;

                /// Récupération de l'essai
                Console.WriteLine("Essai " + (i+1) + ":" );
                currentGuess = Console.ReadLine();

                /// Comparaison avec code secret
                for (int j = 0; j < 4; j++)
                {

                    /// Verification Vrai
                    if (currentGuess[j] == secretCode[j])
                    {
                        currentRight++;

                        int index = secretCodePool.IndexOf(currentGuess[j]);
                        secretCodePool = secretCodePool.Remove(index, 1);
                        ///TEST
                        ///Console.WriteLine(secretCodePool);
                    }

                    /// Verification mal placés
                    else if (secretCodePool.Contains(currentGuess[j]))
                    {
                        currentWrongPlace++;
                        int index = secretCodePool.IndexOf(currentGuess[j]);
                        secretCodePool = secretCodePool.Remove(index, 1);

                        ///TEST
                        ///Console.WriteLine(secretCodePool);
                    }
                    else { }
                }

                /// Victoire -> résultats / message de fin 
                if (currentRight == 4)
                {
                    Console.WriteLine("Bravo!");
                    break; 
                }

                /// Feedback
                Console.WriteLine("=>Ok: " + currentRight + "\n=> Mauvaise position: " + currentWrongPlace + "\n");
                
                /// Défaite
                if (i == 9) {
                    Console.WriteLine("Vous avez perdu :<");
                }
            }

            /// AJouter message au revoir / n'importe quelle touche pour exit
           
       
            /// Garde la fenêtre ouverte
            Console.ReadLine();
        }
    }
}
