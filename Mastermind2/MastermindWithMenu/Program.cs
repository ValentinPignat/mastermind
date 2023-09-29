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
        static public StringBuilder easyFeedback = new StringBuilder("");
        public const string EXEMPLE = "YBYYBB";
        static private string secretCode = "";
        static public bool reloop = false;

        /// Paramètre par défaut 
        static private char gameDifficulty;
        static private int codeLength = 4;
        static private int colorPoolSize = 7;
        static private bool multiplayer = false;
        
        /// <summary>
        /// Jeu de Mastermind sur la console avec:
        /// - Mode Facile 
        /// - Selection du nombre de couleur/ longueur du code
        /// - Mode deux joueur 
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

                /// Option changer paramètres
                Console.WriteLine("\nVoulez-vous changer les paramètres du jeu ?\nAppuyer sur Y si oui\n");
                char changerParam = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (changerParam == 'y' || changerParam == 'Y')
                {
                    Selection();
                }
                Console.WriteLine();

                /// Nouveau colorPool selon taille
                colorPool = colorPoolDefault.Substring(0, colorPoolSize);

                /// Generer Code ou demander code
                if (multiplayer == true)
                {
                    Console.WriteLine("\nLe premier joueur peut entrer un code de " + codeLength + " lettres de long.\nLes lettres possible sont: " + colorPool + ":\n");
                    secretCode = Console.ReadLine().ToUpper();
                    Console.Clear();

                    /// Re-genère si input n'a pas la bonne longeur
                    if (secretCode.Length != codeLength)
                    {
                        secretCode = "";
                        Console.WriteLine("Code rentré incorect, un code a été automatiquement généré!");
                        RandomCode();
                    }

                    /// Re-genère si pas bonnes couleurs
                    for (int j = 0; j < codeLength; j++)
                    {
                        if (colorPool.Contains(secretCode[j]) == false)
                        {
                            secretCode = "";
                            Console.WriteLine("Code rentré incorect, un code a été automatiquement généré!");
                            RandomCode();
                        }
                    }
                }
                else
                {
                    RandomCode();
                }



                ///Boucle de 10 essais
                for (int i = 0; i < 10; i++)
                {
                    /// Local variables et secretCodePool pour verifier mal placés sans doublons
                    int currentRight = 0;
                    int currentWrongPlace = 0;
                    string secretCodePool = secretCode;
                    reloop = false;

                    /// Récupération de l'essai et rappel des couleurs/longueur
                    Console.WriteLine("\nLongueur du code: " + codeLength + "\nLettres possibles: " + colorPool + "\nEssai " + (i + 1) + ":");
                    currentGuess = Console.ReadLine().ToUpper();

                    /// Mode Facile
                    if (gameDifficulty == 'f')
                    {
                        easyFeedback = new StringBuilder("");
                        for (int j = 0; j < codeLength; j++)
                        {
                            easyFeedback.Append("_");
                        }
                    }

                    /// Re-loop si input n'a pas la bonne longeur
                    if (currentGuess.Length != codeLength)
                    {
                        ShowExemple();
                    }

                    /// Loop et redonne essai
                    if (reloop == true)
                    {
                        i--;
                        continue;
                    }


                    /// Re-loop si pas couleur
                    for (int j = 0; j < codeLength; j++)
                    {   
                        if (colorPool.Contains(currentGuess[j]) == false)
                        {
                            ShowExemple();
                        }
                    }

                    /// Loop et redonne essai
                    if (reloop == true) {
                        i--;
                        continue;
                    }

                    /// Verification Vrai
                    for (int j = 0; j < codeLength; j++)
                    {
                        if (currentGuess[j] == secretCode[j])
                        {
                            if (gameDifficulty == 'f')
                            {
                                easyFeedback[j] = currentGuess[j];
                            }

                            /// Juste +1
                            currentRight++;

                            /// Retire Vérifiés du Code pour pas qu'il soit verifiés une deuxième fois comme "mal placé"
                            int index = secretCodePool.IndexOf(currentGuess[j]);
                            secretCodePool = secretCodePool.Remove(index, 1);

                            ///TEST
                            ///Console.WriteLine(index);
                        }
                    }

                    for (int j = 0; j < currentGuess.Length; j++) {
                        if (secretCodePool.Contains(currentGuess[j]) && easyFeedback[j] == '_') {
                            
                            /// Mal placés +1
                            currentWrongPlace++;

                            /// Ajouoter feedback
                            easyFeedback[j] = Char.ToLower(currentGuess[j]); 
                            
                            /// Retire vérifiés pour ne pas avoir de doublons
                            int index = secretCodePool.IndexOf(currentGuess[j]);
                            secretCodePool = secretCodePool.Remove(index, 1);
                        }
                    }

                    /// Victoire -> résultats / message de fin 
                    if (currentRight == codeLength)
                    {
                        Console.WriteLine("\nBravo!\nTu as découvert le code en " + (i + 1) + " essais.\n");
                        break;
                    }

                    /// Feedback Facile 
                    if (gameDifficulty == 'f')
                    {
                        /// Expliquations
                        Console.Write("$ = bonne couleur mais mal placés: ");

                        /// Change couleur de l'output selon caractère -- charactère minuscule sont $ avec couleur
                        for (int j = 0; j < easyFeedback.Length; j++) {
                            switch (easyFeedback[j]) {
                                case 'Y':
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write('Y');
                                    Console.ResetColor();
                                    break;
                                case 'y':
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write('$');
                                    Console.ResetColor();
                                    break;
                                case 'B':
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.Write('B');
                                    Console.ResetColor();
                                    break;
                                case 'b':
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.Write('$');
                                    Console.ResetColor();
                                    break;
                                case 'R':
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write('R');
                                    Console.ResetColor();
                                    break;
                                case 'r':
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write('$');
                                    Console.ResetColor();
                                    break;
                                case 'G':
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write('G');
                                    Console.ResetColor();
                                    break;
                                case 'g':
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write('$');
                                    Console.ResetColor();
                                    break;
                                case 'C':
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write('C');
                                    Console.ResetColor();
                                    break;
                                case 'c':
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write('$');
                                    Console.ResetColor();
                                    break;
                                case 'P':
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write('P');
                                    Console.ResetColor();
                                    break;
                                case 'p':
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write('$');
                                    Console.ResetColor();
                                    break;
                                case 'W':
                                    Console.Write('W');
                                    break;
                                case 'w':
                                    Console.Write('$');
                                    break;
                                default:
                                    Console.Write('_');
                                        break;
                            }
                        }
                        Console.Write("\n\n");
                    }
                    else
                    {
                        /// Feedback
                        Console.WriteLine("\n=> Ok: " + currentRight + "\n=> Mauvaise position: " + currentWrongPlace + "\n");

                    }
                    /// Défaite
                    if (i == 9)
                    {
                        Console.WriteLine("Vous avez perdu :<\nLe code secret était: " + secretCode + "\n");
                    }
                }

                /// Option replay
                Console.WriteLine("Appuyez sur R pour rejouer");
                replay = Console.ReadKey().KeyChar;
            } while (replay == 'r' || replay == 'R');

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
                Console.WriteLine("\nVeuillez selectionner le mode de jeu avec les touches suivantes:\nClassique - C\nFacile - F");
                char gameMode = Console.ReadLine()[0];
                if (gameMode == 'f' || gameMode == 'F')
                {
                    gameDifficulty = 'f';
                }
                else {
                    gameDifficulty = 'c';
                }
                Console.WriteLine();

                /// Change multiplayer - Défaut: Solo
                Console.WriteLine("\nVeuillez selectionner le mode de jeu avec les touches suivantes:\nSolo - S\nDeux Joueurs - D");
                gameMode = Console.ReadLine()[0];
                if (gameMode == 'd' || gameMode == 'D')
                {
                    multiplayer = true;
                }
                else
                {
                    multiplayer = false;
                }
                Console.WriteLine();

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
                Console.WriteLine();

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

            ///Paramètres restants inchangé si erreur 
            catch (Exception)
            {
                Console.WriteLine("Valeur invalide => paramètres par défaut");
            }

        }

        /// <summary>
        /// Crée un code secret selon la longueur et le nombre de couleur
        /// </summary>
        static void RandomCode()
        {
            /// Generer Code aléatoire (index aléatoire dans colorPool)
            Random rnd = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                int x = rnd.Next(0, colorPoolSize - 1);
                secretCode += colorPool[x];

                ///TEST
                Console.WriteLine(secretCode);
            }
        }
        /// <summary>
        /// Donne un exemple d'input en fonction de la longueur du code 
        /// </summary>
        static void ShowExemple() {
            if (reloop == false) {
                Console.Write("\nAttention veuillez entrer " + codeLength + " lettres (ex.");
                for (int i = 0; i < codeLength; i++) { 
                    Console.Write(EXEMPLE[i]);
                }
                Console.Write(")\n");
                reloop = true;
            }
        }

    }



}
