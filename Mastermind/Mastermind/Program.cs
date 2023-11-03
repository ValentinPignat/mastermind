using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace mastermind
{
    internal class Program
    {
        // Initialisations constante
        public const string colorPoolDefault = "YBRGWCP";
        public const string EXEMPLE = "YBYYBB";

        // Initialisations variables
        static private string colorPool = "";
        static private string currentGuess = "";
        static private char replay = 'r';
        static public StringBuilder easyFeedback = new StringBuilder("");
        static private string secretCode = "";
        static public bool reloop = false;
        static private ConsoleKeyInfo menuInput;
        static public bool exit = false;

        // Paramètre par défaut 
        static private string gameDifficulty = "Normal";
        static private int codeLength = 4;
        static private int colorPoolSize = 7;
        static private int multiplayer = 1;

        /// <summary>
        /// Jeu de Mastermind sur la console avec:
        /// - Mode Facile 
        /// - Selection du nombre de couleur/ longueur du code
        /// - Mode deux joueur 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Ouvre menu
            StartGame();

            while (replay == 'r' || replay == 'R' || replay == 'm' || replay == 'M')
            {
                // Ouvre menu si input m à la fin de la boucle
                if (replay == 'm' || replay == 'M')
                {
                    replay = 'n';
                    StartGame();
                }

                // Quitte le jeu
                if (exit)
                {
                    break;
                }

                // Efface menu
                Console.Clear();

                // Réinitialise code
                secretCode = "";

                //Nouveau colorPool selon taille
                colorPool = colorPoolDefault.Substring(0, colorPoolSize);

                // Demander code si multijoueur
                if (multiplayer == 2)
                {
                    Console.WriteLine("Le premier joueur peut entrer un code de " + codeLength + " lettres de long.\nLes lettres possible sont: " + colorPool + ":\n");
                    secretCode = Console.ReadLine().ToUpper();
                    Console.Clear();

                    // Re-genère si input n'a pas la bonne longeur
                    if (secretCode.Length != codeLength)
                    {
                        secretCode = "";
                        Console.WriteLine("Code rentré incorect, un code a été automatiquement généré!");
                        RandomCode();
                    }

                    // Re-genère si pas bonnes couleurs
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

                // Génère code aléatoire
                else
                {
                    RandomCode();
                }

                // Boucle de 10 essais
                for (int i = 0; i < 10; i++)
                {
                    // Local variables et secretCodePool pour verifier mal placés sans doublons
                    int currentRight = 0;
                    int currentWrongPlace = 0;
                    string secretCodePool = secretCode;
                    reloop = false;

                    //  Récupération de l'essai et rappel des couleurs/longueur
                    Console.WriteLine("Longueur du code: " + codeLength + "\nLettres possibles: " + colorPool + "\nEssai " + (i + 1) + ":");
                    currentGuess = Console.ReadLine().ToUpper();

                    // String pour verif et output en mode facile
                    easyFeedback = new StringBuilder("");
                    for (int j = 0; j < codeLength; j++)
                    {
                        easyFeedback.Append("_");
                    }


                    // Donne exemple si input n'a pas la bonne longeur et reloop == true
                    if (currentGuess.Length != codeLength)
                    {
                        ShowExemple();
                    }

                    // Loop et redonne essai
                    if (reloop == true)
                    {
                        i--;
                        continue;
                    }


                    // Donne exemple si pas bonne couleur et reloop == true
                    for (int j = 0; j < codeLength; j++)
                    {
                        if (colorPool.Contains(currentGuess[j]) == false)
                        {
                            ShowExemple();
                        }
                    }

                    // Loop et redonne essai
                    if (reloop == true)
                    {
                        i--;
                        continue;
                    }

                    // Verification bien placés
                    for (int j = 0; j < codeLength; j++)
                    {
                        if (currentGuess[j] == secretCode[j])
                        {
                            // Ajout dans la string de verification
                            easyFeedback[j] = currentGuess[j];

                            // Juste +1
                            currentRight++;

                            // Retire Vérifiés du Code pour pas qu'il soit verifiés une deuxième fois comme "mal placé"
                            int index = secretCodePool.IndexOf(currentGuess[j]);
                            secretCodePool = secretCodePool.Remove(index, 1);
                        }
                    }

                    // Verification mal placés
                    for (int j = 0; j < codeLength; j++)
                    {
                        if (secretCodePool.Contains(currentGuess[j]) && easyFeedback[j] == '_')
                        {

                            // Mal placés +1
                            currentWrongPlace++;

                            // Ajouter au feedback
                            easyFeedback[j] = Char.ToLower(currentGuess[j]);

                            // Retire vérifiés pour ne pas avoir de doublons
                            int index = secretCodePool.IndexOf(currentGuess[j]);
                            secretCodePool = secretCodePool.Remove(index, 1);
                        }
                    }

                    // Victoire -> résultats / message de fin 
                    if (currentRight == codeLength)
                    {
                        Console.WriteLine("\nBravo!\nVous avez découvert le code en " + (i + 1) + " essais.\n");
                        break;
                    }

                    // Feedback Facile 
                    if (gameDifficulty == "Facile")
                    {
                        // Expliquations
                        Console.Write("$ = bonne couleur mais mal placés: ");

                        // Change couleur de l'output selon caractère -- charactère minuscule sont $ avec couleur
                        for (int j = 0; j < easyFeedback.Length; j++)
                        {
                            switch (easyFeedback[j])
                            {
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
                    // Feedback normal
                    else
                    {
                        Console.WriteLine("\n=> Bien placé: " + currentRight + "\n=> Mauvaise position: " + currentWrongPlace + "\n");
                    }
                    // Défaite
                    if (i == 9)
                    {
                        Console.WriteLine("Vous avez perdu :<\nLe code secret était: " + secretCode + "\n");
                    }
                }

                // Option replay et retour au menu
                Console.WriteLine("Appuyez sur R pour rejouer ou M pour retourner au menu. Autre touche pour quitter.");
                replay = Console.ReadKey().KeyChar;
            }

            // Message au revoir
            Console.Clear();
            PrintLogo();
            Console.WriteLine("\n              Merci d'avoir joué et à bientôt!");


            // Garde la fenêtre ouverte
            Console.ReadLine();
        }

        /// <summary>
        /// Crée un code secret selon la longueur et le nombre de couleur
        /// </summary>
        static void RandomCode()
        {
            // Generer Code aléatoire (index aléatoire dans colorPool)
            Random rnd = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                int x = rnd.Next(0, colorPoolSize);
                secretCode += colorPool[x];
            }
        }
        /// <summary>
        /// Donne un exemple d'input en fonction de la longueur du code 
        /// </summary>
        static void ShowExemple()
        {
            // condition pour eviter doublon pas bonne longueur/couleur
            if (reloop == false)
            {
                Console.Write("\nAttention veuillez entrer " + codeLength + " lettres (ex.");
                for (int i = 0; i < codeLength; i++)
                {
                    Console.Write(EXEMPLE[i]);
                }
                Console.Write(")\n");
                reloop = true;
            }
        }
        /// <summary>
        /// Menu avec "curseur" sur Lancer la partie
        /// </summary>
        static void StartGame()
        {
            Console.Clear();
            PrintLogo();
            Console.WriteLine(
                "              Bienvenue sur Mastermind\n\n" +
                " Lancer la partie               < Enter > <-----\n" +
                " Difficulté:                    < " + gameDifficulty + " >\n" +
                " Longueur du code:              < " + codeLength + " >\n" +
                " Nombre de couleurs possibles:  < " + colorPoolSize + " >\n" +
                " Nombre de joueur:              < " + multiplayer + " >\n" +
                " Paramètre par défaut           < Enter >\n" +
                " Quitter                        < Enter >\n"
                );
            menuInput = Console.ReadKey();
            // Navigation et lancement de la partie selon input 
            switch (menuInput.Key)
            {
                case ConsoleKey.DownArrow:
                    Difficulty();
                    break;
                case ConsoleKey.Enter:
                    break;
                default:
                    StartGame();
                    break;
            }

        }
        /// <summary>
        /// Menu avec "curseur" sur Difficulté
        /// </summary>
        static void Difficulty()
        {
            Console.Clear();
            PrintLogo();
            Console.WriteLine(
                "              Bienvenue sur Mastermind\n\n" +
                " Lancer la Partie               < Enter >\n" +
                " Difficulté:                    < " + gameDifficulty + " >  <-----\n" +
                " Longueur du code:              < " + codeLength + " >\n" +
                " Nombre de couleurs possibles:  < " + colorPoolSize + " >\n" +
                " Nombre de joueur:              < " + multiplayer + " >\n" +
                " Paramètre par défaut           < Enter >\n" +
                " Quitter                        < Enter >\n"
                );
            menuInput = Console.ReadKey();

            // Navigation et changement paramètre selon input 
            switch (menuInput.Key)
            {
                case ConsoleKey.UpArrow:
                    StartGame();
                    break;
                case ConsoleKey.DownArrow:
                    Length();
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                    if (gameDifficulty == "Normal")
                    {
                        gameDifficulty = "Facile";
                    }
                    else
                    {
                        gameDifficulty = "Normal";
                    }
                    Difficulty();
                    break;
                default:
                    Difficulty();
                    break;
            }
        }
        /// <summary>
        /// Menu avec "curseur" Longueur du code
        /// </summary>
        static void Length()
        {
            Console.Clear();
            PrintLogo();
            Console.WriteLine(
                "              Bienvenue sur Mastermind\n\n" +
                " Lancer la Partie               < Enter >\n" +
                " Difficulté:                    < " + gameDifficulty + " >\n" +
                " Longueur du code:              < " + codeLength + " >  <-----\n" +
                " Nombre de couleurs possibles:  < " + colorPoolSize + " >\n" +
                " Nombre de joueur:              < " + multiplayer + " >\n" +
                " Paramètre par défaut           < Enter >\n" +
                " Quitter                        < Enter >\n"
                );
            menuInput = Console.ReadKey();

            // Navigation et changement paramètre selon input 
            switch (menuInput.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (codeLength > 2)
                    {
                        codeLength--;
                    }
                    Length();
                    break;
                case ConsoleKey.RightArrow:
                    if (codeLength < 6)
                    {
                        codeLength++;
                    }
                    Length();
                    break;
                case ConsoleKey.UpArrow:
                    Difficulty();
                    break;
                case ConsoleKey.DownArrow:
                    Pool();
                    break;
                default:
                    Length();
                    break;
            }
        }
        /// <summary>
        /// Menu avec "curseur" Nombre de couleurs possibles
        /// </summary>
        static void Pool()
        {
            Console.Clear();
            PrintLogo();
            Console.WriteLine(
                "              Bienvenue sur Mastermind\n\n" +
                " Lancer la Partie               < Enter >\n" +
                " Difficulté:                    < " + gameDifficulty + " >\n" +
                " Longueur du code:              < " + codeLength + " >\n" +
                " Nombre de couleurs possibles:  < " + colorPoolSize + " >  <-----\n" +
                " Nombre de joueur:              < " + multiplayer + " >\n" +
                " Paramètre par défaut           < Enter >\n" +
                " Quitter                        < Enter >\n"
                );
            menuInput = Console.ReadKey();

            // Navigation et changement paramètre selon input 
            switch (menuInput.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (colorPoolSize > 2)
                    {
                        colorPoolSize--;
                    }
                    Pool();
                    break;
                case ConsoleKey.RightArrow:
                    if (colorPoolSize < 7)
                    {
                        colorPoolSize++;
                    }
                    Pool();
                    break;
                case ConsoleKey.UpArrow:
                    Length();
                    break;
                case ConsoleKey.DownArrow:
                    Multi();
                    break;
                default:
                    Pool();
                    break;
            }
        }
        /// <summary>
        /// Menu avec "curseur" Nombre de joueur
        /// </summary>
        static void Multi()
        {
            Console.Clear();
            PrintLogo();
            Console.WriteLine(
                "              Bienvenue sur Mastermind\n\n" +
                " Lancer la Partie               < Enter >\n" +
                " Difficulté:                    < " + gameDifficulty + " >\n" +
                " Longueur du code:              < " + codeLength + " >\n" +
                " Nombre de couleurs possibles:  < " + colorPoolSize + " >\n" +
                " Nombre de joueur:              < " + multiplayer + " >  <-----\n" +
                " Paramètre par défaut           < Enter >\n" +
                " Quitter                        < Enter >\n"
                );
            menuInput = Console.ReadKey();

            // Navigation et changement paramètre selon input 
            switch (menuInput.Key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                    if (multiplayer == 1)
                    {
                        multiplayer = 2;
                    }
                    else
                    {
                        multiplayer = 1;
                    }
                    Multi();
                    break;
                case ConsoleKey.UpArrow:
                    Pool();
                    break;
                case ConsoleKey.DownArrow:
                    Default();
                    break;
                default:
                    Multi();
                    break;
            }
        }
        /// <summary>
        /// Menu avec "curseur" Paramètre par défaut
        /// </summary>
        static void Default()
        {
            Console.Clear();
            PrintLogo();
            Console.WriteLine(
                "              Bienvenue sur Mastermind\n\n" +
                " Lancer la Partie               < Enter >\n" +
                " Difficulté:                    < " + gameDifficulty + " >\n" +
                " Longueur du code:              < " + codeLength + " >\n" +
                " Nombre de couleurs possibles:  < " + colorPoolSize + " >\n" +
                " Nombre de joueur:              < " + multiplayer + " >\n" +
                " Paramètre par défaut           < Enter >  <-----\n" +
                " Quitter                        < Enter >\n"
                );
            menuInput = Console.ReadKey();

            // Navigation et changement paramètre selon input 
            switch (menuInput.Key)
            {
                case ConsoleKey.Enter:
                    gameDifficulty = "Normal";
                    codeLength = 4;
                    colorPoolSize = 7;
                    multiplayer = 1;
                    Default();
                    break;
                case ConsoleKey.UpArrow:
                    Multi();
                    break;
                case ConsoleKey.DownArrow:
                    Exit();
                    break;
                default:
                    Default();
                    break;
            }
        } 
        /// <summary>
        /// Menu avec "curseur sur exit
        /// </summary>
        static void Exit()
        {
            Console.Clear();
            PrintLogo();
            Console.WriteLine(
                "              Bienvenue sur Mastermind\n\n" +
                " Lancer la Partie               < Enter >\n" +
                " Difficulté:                    < " + gameDifficulty + " >\n" +
                " Longueur du code:              < " + codeLength + " >\n" +
                " Nombre de couleurs possibles:  < " + colorPoolSize + " >\n" +
                " Nombre de joueur:              < " + multiplayer + " >\n" +
                " Paramètre par défaut           < Enter >\n" +
                " Quitter                        < Enter >  <-----\n"
                );
            menuInput = Console.ReadKey();

            // Navigation et changement paramètre selon input 
            switch (menuInput.Key)
            {
                case ConsoleKey.Enter:
                    exit = true;
                    break;
                case ConsoleKey.UpArrow:
                    Default();
                    break;
                default:
                    Exit();
                    break;
            }
        }
        /// <summary>
        /// Dessine le logo dans la console.
        /// </summary>
        static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            // Adapté de: https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20
            Console.Write(
                "  __  __           _                      _           _ \n" +
                " |  \\/  |         | |                    (_)         | |\n" +
                " | \\  / | __ _ ___| |_ ___ _ __ _ __ ___  _ _ __   __| |\n" +
                " | |\\/| |/ _` / __| __/ _ \\ '__| '_ ` _ \\| | '_ \\ / _` |\n" +
                " | |  | | (_| \\__ \\ ||  __/ |  | | | | | | | | | | (_| |\n" +
                " |_|  |_|\\__,_|___/\\__\\___|_|  |_| |_| |_|_|_| |_|\\__,_|\n\n");

            Console.ResetColor();
        }
    }
}
