using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace mastermind
{
    internal class Program
    {
        ///Initialisations
        static private string colorPoolDefault = "YBRGWCP";
        static private string colorPool = "";
        static private string currentGuess = "";
        static private char replay;

        /// Paramètre par défaut 
        //static private char gameDifficulty;
        static private int codeLength = 4;
        static private int colorPoolSize = 7;
        static private bool multiplayer = false;
        static private string secretCode = "";

        /// <summary>
        /// Jeu de Mastermind sur la console 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            /// Message de bienvenue
            Console.WriteLine("Bienvenue sur Mastermind!\nCouleurs possibles: YBRGWCP\nDevine le code couleurs (ex.YBYP)\n");
            
            do
            {
                /// Réinitialisation code
                secretCode = "";
                
                /// Option changer param
                Console.WriteLine("\nVoulez-vous changer les paramètres du jeu ?\nAppuyer sur Y si oui\n");
                char changerParam = Console.ReadKey().KeyChar;
                if (changerParam == 'y')
                {
                    Selection();
                }
                Console.WriteLine();

                /// Generer Code ou demander code
                if (multiplayer == true) {
                    Console.WriteLine("\nLe premier joueur peut entrer un code de " + codeLength + " lettres de long.\nLes lettres possible sont: " + colorPool+":\n");
                    secretCode = Console.ReadLine().ToUpper(); ;
                    Console.Clear();
                }
                else { 
                    randomCode();
                    
                }
                
                ///Boucle de 10 essais
                for (int i = 0; i < 10; i++)
                {
                    /// Local variables et secretCodePool pour verifier mal placés sans doublons
                    int currentRight = 0;
                    int currentWrongPlace = 0;
                    string secretCodePool = secretCode;

                    /// Récupération de l'essai
                    Console.WriteLine("Color Pool: " + colorPool + "\nEssai " + (i + 1) + ":");
                    currentGuess = Console.ReadLine().ToUpper();

                    /// Re-loop si input n'a pas la bonne longeur
                    if (currentGuess.Length != codeLength)
                    {
                        Console.WriteLine("\nAttention veuillez entrer " + codeLength + " lettres (ex.YBYY)\n");
                        i--;
                        continue;
                    }

                    /// Re-loop si pas couleur
                    for (int j = 0; j < codeLength; j++)
                    {
                        if (colorPool.Contains(currentGuess[j]) == false)
                        {
                            Console.WriteLine("\nAttention veuillez entrer " + codeLength + " lettres (ex.YBYP)\n");
                            i--;
                            break;
                        }
                    }

                    /// Copie pour éviter double vérification
                    string currentGuessPool = currentGuess;

                    /// Verification Vrai
                    for (int j = 0; j < codeLength; j++)
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
                    if (currentRight == codeLength)
                    {
                        Console.WriteLine("\nBravo!\nTu as découvert le code en " + (i + 1) + " essais.\n");
                        break;
                    }

                    /// Feedback
                    Console.WriteLine("\n=> Ok: " + currentRight + "\n=> Mauvaise position: " + currentWrongPlace + "\n");

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

        /// <summary>
        /// Changement des paramètres de la partie
        /// </summary> 
        static void Selection()
        {
            /// Try si Input invalide 
            try
            {
                /// Change difficulté - Défaut: Classique
                /*Console.WriteLine("\nVeuillez selectionner le mode de jeu avec les touches suivantes:\nClassique - C\nFacile - F");
                char gameMode = Console.ReadLine()[0];
                if (gameMode == 'f' || gameMode == 'F')
                {
                    gameDifficulty = 'f';
                }
                else {
                    gameDifficulty = 'c';
                }*/

                /// Change multiplayer - Défaut: Solo
                Console.WriteLine("\nVeuillez selectionner le mode de jeu avec les touches suivantes:\nSolo - S\nDeux Joueurs - D");
                char gameMode = Console.ReadLine()[0];
                if (gameMode == 'd' || gameMode == 'D')
                {
                    multiplayer = true;
                }
                else {
                    multiplayer = false;
                }
                /// Change nombre de couleur - Défaut : 7
                Console.WriteLine("\nVeuillez selectionner le nombre de couleur entre 2 et 7");
                int gameInt = Convert.ToInt16(Console.ReadLine());


                if (gameInt >= 2 && gameInt <= 7)
                {
                    colorPoolSize = gameInt;
                }
                else
                {
                    colorPoolSize = 7;
                }

                /// Nouveau colorPool selon taille
                colorPool = colorPoolDefault.Substring(0, colorPoolSize);

                /// Change longueur du code - Défaut : 4
                Console.WriteLine("\nVeuillez selectionner la longeur du code entre 2 et 6");
                gameInt = Convert.ToInt16(Console.ReadLine());

                if (gameInt >= 2 && gameInt <= 6)
                {
                    codeLength = gameInt;
                }
                else
                {
                    codeLength = 4;
                }
            }

            ///Paramètres inchangé si erreur 
            catch (Exception)
            {
                Console.WriteLine("Valeur invalide => paramètres par défaut");
            }

        }

        /// <summary>
        /// Crée un code secret selon la longueur et le nombre de couleur
        /// </summary>
        static void randomCode() {
            /// Generer Code aléatoire (index aléatoire dans colorPool)
            Random rnd = new Random();
                for (int i = 0; i< codeLength; i++)
                {
                    int x = rnd.Next(0, colorPoolSize-1);
                    secretCode += colorPool[x];

                    ///TEST
                    Console.WriteLine(secretCode);
                }}

    }

        

}
