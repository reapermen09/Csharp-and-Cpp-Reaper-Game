using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Media;
using NAudio.Wave;

class ReaperGame
{
    private static void PlaySound(string filepath)
    {
        SoundPlayer player = new SoundPlayer(filepath);
        player.Play();
    }

    static int taxAmount = 0;
    static int csAmount = 0;
    static int fontSize;
    static bool inPrison = false;
    static bool secretOneFound = false;
    static bool secretTwoFound = false;
    static bool begging = false;
    static bool beatRati = false;
    static bool mratiBeat = false;
    static int money = 10;
    static int points = 0;
    static string folderPath = @"C:\ReaperGame\SaveData";
    static string saveFilePath = Path.Combine(folderPath, "game_save.txt");
    static string settingsFilePath = Path.Combine(folderPath, "settings.txt");
    static string achievementsFilePath = Path.Combine(folderPath, "achievements.txt");

    static void AssetsFolder()
    {
        string folderPath = @"C:\ReaperGame\Assets";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    static WaveOutEvent outputDevice;
    static AudioFileReader audioFile;

    static void MP3Silently(string mp3File, bool playStop)
    {
        if (!File.Exists(mp3File))
        {
            Console.WriteLine("File does not exist: " + mp3File);
            return;
        }

        if (playStop)
        {
            StopAudio();
            try
            {
                audioFile = new AudioFileReader(mp3File);
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Volume = 0.5f;
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing file: " + ex.Message);
            }
        }
        else
        {
            StopAudio();
        }
    }

    static void StopAudio()
    {
        if (outputDevice != null)
        {
            outputDevice.Stop();
            outputDevice.Dispose();
            outputDevice = null;
        }
        if (audioFile != null)
        {
            audioFile.Dispose();
            audioFile = null;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct CONSOLE_FONT_INFOEX
    {
        public int cbSize;
        public int nFont;
        public short dwFontSizeX;
        public short dwFontSizeY;
        public int FontFamily;
        public int FontWeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FaceName;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    private const int STD_OUTPUT_HANDLE = -11;
    static void SetFontSize(int size)
    {
        if (size < 1 || size > 3)
        {
            Console.WriteLine("Invalid font size. Setting to default (3).");
            size = 3;
        }

        IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
        CONSOLE_FONT_INFOEX cfi = new CONSOLE_FONT_INFOEX();
        cfi.cbSize = Marshal.SizeOf(cfi);

        if (GetCurrentConsoleFontEx(hConsole, false, ref cfi))
        {
            switch (size)
            {
                case 1:
                    cfi.dwFontSizeX = 8;
                    cfi.dwFontSizeY = 12;
                    break;
                case 2:
                    cfi.dwFontSizeX = 12;
                    cfi.dwFontSizeY = 18;
                    break;
                case 3:
                    cfi.dwFontSizeX = 16;
                    cfi.dwFontSizeY = 24;
                    break;
            }

            SetCurrentConsoleFontEx(hConsole, false, ref cfi);
        }
        else
        {

        }
    }

    static int GetValidInt()
    {
        int value;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out value))
            {
                return value;
            }
            else
            {
                Console.Write("Invalid input. Please enter a number: ");
            }
        }
    }

    static bool money100 = false;
    static bool money1000 = false;
    static bool money10000 = false;
    static bool money100000 = false;
    static bool money1000000 = false;

    static bool firstTransaction = false;

    static bool foundReaper = false;
    static bool insultReaper = false;
    static bool begReaper = false;

    static bool wentToPrison = false;
    static bool escapedPrison = false;

    static bool points100 = false;
    static bool points1000 = false;
    static bool points10000 = false;
    static bool points100000 = false;
    static bool points1000000 = false;

    //checking stuff bruh
    static bool donotcheck = false;
    static bool donotcheck2 = false;

    static void SaveAchievements()
    {
        using (StreamWriter achievementsFile = new StreamWriter(achievementsFilePath))
        {
            achievementsFile.WriteLine(money100 ? 1 : 0);
            achievementsFile.WriteLine(money1000 ? 1 : 0);
            achievementsFile.WriteLine(money10000 ? 1 : 0);
            achievementsFile.WriteLine(money100000 ? 1 : 0);
            achievementsFile.WriteLine(money1000000 ? 1 : 0);
            achievementsFile.WriteLine(firstTransaction ? 1 : 0);
            achievementsFile.WriteLine(foundReaper ? 1 : 0);
            achievementsFile.WriteLine(insultReaper ? 1 : 0);
            achievementsFile.WriteLine(begReaper ? 1 : 0);
            achievementsFile.WriteLine(points100 ? 1 : 0);
            achievementsFile.WriteLine(points1000 ? 1 : 0);
            achievementsFile.WriteLine(points10000 ? 1 : 0);
            achievementsFile.WriteLine(points100000 ? 1 : 0);
            achievementsFile.WriteLine(points1000000 ? 1 : 0);
            achievementsFile.WriteLine(wentToPrison ? 1 : 0);
            achievementsFile.WriteLine(escapedPrison ? 1 : 0);
            achievementsFile.WriteLine(donotcheck ? 1 : 0);
            achievementsFile.WriteLine(donotcheck2 ? 1 : 0);
        }
    }

    static void LoadAchievements()
    {
        if (File.Exists(achievementsFilePath))
        {
            string[] lines = File.ReadAllLines(achievementsFilePath);

            if (lines.Length < 18)
            {
                Console.WriteLine("Achievements file is incomplete. Resetting achievements.");
                return;
            }

            money100 = lines[0] == "1";
            money1000 = lines[1] == "1";
            money10000 = lines[2] == "1";
            money100000 = lines[3] == "1";
            money1000000 = lines[4] == "1";
            firstTransaction = lines[5] == "1";
            foundReaper = lines[6] == "1";
            insultReaper = lines[7] == "1";
            begReaper = lines[8] == "1";
            points100 = lines[9] == "1";
            points1000 = lines[10] == "1";
            points10000 = lines[11] == "1";
            points100000 = lines[12] == "1";
            points1000000 = lines[13] == "1";
            wentToPrison = lines[14] == "1";
            escapedPrison = lines[15] == "1";
            donotcheck = lines[16] == "1";
            donotcheck2 = lines[17] == "1";
        }
        else
        {
            Console.WriteLine("No achievements file found... Starting fresh!");
        }
    }


    static void DisplayAchievements()
    {
        Console.WriteLine("Achievements:");
        if (money100) Console.WriteLine("Novice: 100+ money");
        if (money1000) Console.WriteLine("Grinder Type Guy: 1000+ money");
        if (money10000) Console.WriteLine("Commander: 10000+ money");
        if (money100000) Console.WriteLine("Destroyer Type Guy: 100000+ money");
        if (money1000000) Console.WriteLine("Rich Sigma: 1000000+ money");

        if (firstTransaction) Console.WriteLine("First Transaction: Completed your first transaction");
        if (foundReaper) Console.WriteLine("Finding the Reaper: You found Reaper who gave you 100 money");
        if (insultReaper) Console.WriteLine("Insulting the Reaper: You insulted Reaper, how dare you");
        if (begReaper) Console.WriteLine("Begging: Stop begging Reaper for money, please");

        if (points100) Console.WriteLine("Point Grinder: 100+ points");
        if (points1000) Console.WriteLine("Pointer: 1000+ points");
        if (points10000) Console.WriteLine("Amazing Player: 10000+ points");
        if (points100000) Console.WriteLine("Super Player: 100000+ points");
        if (points1000000) Console.WriteLine("Sigma Player: 1000000+ points");

        if (escapedPrison) Console.WriteLine("Criminal Crime: You escaped prison");
        if (wentToPrison) Console.WriteLine("Law Breaker: You went to prison");

        if (wentToPrison) Console.WriteLine("Rati's Defeat: You defeated Rati");
    }

    static void PlayAchievementSound()
    {
        PlaySound(@"C:\ReaperGame\Assets\achievement.wav");
    }

    static void PlayActionSound()
    {
        PlaySound(@"C:\ReaperGame\Assets\action.wav");
    }

    static void AchievementReachedCheck(int money, int points)
    {
        if (money >= 100 && !money100)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Novice achievement! (100+ money)");
            money100 = true;
        }
        if (money >= 1000 && !money1000)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Grinder Type Guy achievement! (1000+ money)");
            money1000 = true;
        }
        if (money >= 10000 && !money10000)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Commander achievement! (10000+ money)");
            money10000 = true;
        }
        if (money >= 100000 && !money100000)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Destroyer Type Guy achievement! (100000+ money)");
            money100000 = true;
        }
        if (money >= 1000000 && !money1000000)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Rich Sigma achievement! (1000000+ money)");
            money1000000 = true;
        }
        if (points > 0 && !firstTransaction)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the First Transaction achievement! (You did your first transaction)");
            firstTransaction = true;
        }
        if (secretOneFound && !foundReaper)
        {
            PlayAchievementSound();
            Console.WriteLine("Finding the Reaper (You found Reaper who gave you 100 money)");
            foundReaper = true;
        }
        if (secretTwoFound && !insultReaper)
        {
            PlayAchievementSound();
            Console.WriteLine("Insulting the Reaper (You insulted Reaper, how dare you)");
            insultReaper = true;
        }

        if (beatRati && !mratiBeat)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Boss Defeater achievement! (You beat Rati)\n");
            mratiBeat = true;
        }

        if (begging && !begReaper)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Begging achievement! (Stop begging Reaper for money, please)");
            begReaper = true;
        }
        if (points >= 100 && !points100)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Point Grinder achievement! (100+ points)");
            points100 = true;
        }
        if (points >= 1000 && !points1000)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Pointer achievement! (1000+ points)");
            points1000 = true;
        }
        if (points >= 10000 && !points10000)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Amazing Player achievement! (10000+ points)");
            points10000 = true;
        }
        if (points >= 100000 && !points100000)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Super Player achievement! (100000+ points)");
            points100000 = true;
        }
        if (points >= 1000000 && !points1000000)
        {
            PlayAchievementSound();
            Console.WriteLine("You got the Sigma Player achievement! (1000000+ points)");
            points1000000 = true;
        }

        SaveAchievements();
    }

    static void DisplayCommands()
    {
        Console.WriteLine("\nAvailable commands:");
        Console.WriteLine("/help - Show this help message");
        Console.WriteLine("/achievements - Shows your gained achievements");
        Console.WriteLine("/transaction - Convert your money into points (Includes tax)");
        Console.WriteLine("/playagame - Play a math game (Includes tax)");
        Console.WriteLine("/money - Show your money balance");
        Console.WriteLine("/points - Show your points");
        Console.WriteLine("/save - Save your game progress");
        Console.WriteLine("/load - Load your game progress");
        Console.WriteLine("/support - Check support");
        Console.WriteLine("/settings - Change game settings");
        Console.WriteLine("/gamble - Gamble your money for a chance to win (Includes tax)");
        Console.WriteLine("/taxes - Pay your taxes");
        Console.WriteLine("/childsupport - Pay your child support");
        Console.WriteLine("/exit - Exit game");
        Console.WriteLine("/clear - clears all text (keeps commands)\n");
    }

    static void LoadSettings(ref string autoload, ref string autosave, string settingsFilePath)
    {
        if (File.Exists(settingsFilePath))
        {
            string[] lines = File.ReadAllLines(settingsFilePath);
            autoload = lines[0];
            autosave = lines[1];
            fontSize = int.TryParse(lines[2], out int size) ? size : 3;
        }
        else
        {
            autoload = "off";
            autosave = "off";
            fontSize = 3;
            Console.WriteLine("Settings file could not be loaded. Using default settings.");
        }
    }

    static void SaveSettings(string autoload, string autosave, string settingsFilePath)
    {
        using (StreamWriter settingsFile = new StreamWriter(settingsFilePath, false))
        {
            settingsFile.WriteLine(autoload);
            settingsFile.WriteLine(autosave);
            settingsFile.WriteLine(fontSize);
        }
    }

    static void ResetSettings(string settingsFilePath)
    {
        SaveSettings("off", "off", settingsFilePath);
        fontSize = 3;
        Console.WriteLine("Settings reset to default values.");
    }

    static void Settings(ref string autoload, ref string autosave, string settingsFilePath)
    {
        while (true)
        {
            Console.WriteLine("Settings:");
            Console.WriteLine($"1 = Autoload save data: {autoload}");
            Console.WriteLine($"2 = Autosave save data: {autosave}");
            Console.WriteLine($"3 = Change Font Size (Current size: {fontSize})");
            Console.WriteLine("4 = Reset settings");
            Console.WriteLine("5 = Exit settings");
            Console.Write("Enter your choice (number ex: 1, 2, 3, 4, 5): ");

            int choice = GetValidInt();
            if (choice == 1)
            {
                Console.Write("Toggle Autoload save data (on/off): ");
                autoload = Console.ReadLine();
                PlayActionSound();
            }
            else if (choice == 2)
            {
                Console.Write("Toggle Autosave save data (on/off): ");
                autosave = Console.ReadLine();
                PlayActionSound();
            }
            else if (choice == 3)
            {
                Console.Write("Enter new font size (1 is for small, 2 is for medium, and 3 is for large): ");
                fontSize = GetValidInt();
                SetFontSize(fontSize);
                PlayActionSound();
            }
            else if (choice == 4)
            {
                ResetSettings(settingsFilePath);
                autoload = "off";
                autosave = "off";
            }
            else if (choice == 5)
            {
                SaveSettings(autoload, autosave, settingsFilePath);
                PlayActionSound();
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }

    static void HandleSupport()
    {
        Console.WriteLine("Game by Reapermen, contact zxteam27@gmail.com for any support or suggestions...");
    }

    static void HandleTransaction(ref int money, ref int points)
    {
        while (true)
        {
            Console.Write($"How much money would you like to convert into points? (Current money: {money}): ");
            int convertAmount = GetValidInt();

            if (convertAmount <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
            }
            else if (convertAmount > money)
            {
                Console.WriteLine($"You do not have enough money. You only have {money}.");
                Console.Write("Would you like to try again (y/n)? ");
                string response = Console.ReadLine();
                if (response.ToLower() != "y") break;
            }
            else
            {
                points += convertAmount;
                money -= convertAmount;
                Console.WriteLine($"Your points increased by {convertAmount}, and your money decreased by {convertAmount}.");
                double tax = Math.Ceiling(convertAmount / 4.0);
                taxAmount += (int)tax;
                PlayActionSound();
                AchievementReachedCheck(money, points);
                break;
            }
        }
    }

    static void HandlePlayAGame(ref int money, ref int points)
    {
        Random random = new Random();

        while (true)
        {
            int randomNum1 = random.Next(1, 102);
            int randomNum2 = random.Next(1, 102);
            int correctAnswer;
            string operation;

            Console.Write("Choose operation (add/subtract). Adding gives 3 points/money, subtracting gives 5 points/money: ");
            operation = Console.ReadLine();

            if (operation == "add")
            {
                Console.WriteLine($"What is {randomNum1} + {randomNum2}?");
                correctAnswer = randomNum1 + randomNum2;
            }
            else if (operation == "subtract")
            {
                Console.WriteLine($"What is {randomNum1} - {randomNum2}?");
                correctAnswer = randomNum1 - randomNum2;
            }
            else
            {
                Console.WriteLine("Invalid operation. Please choose 'add' or 'subtract'.");
                continue;
            }

            int playerAnswer = GetValidInt();
            if (playerAnswer == correctAnswer)
            {
                int reward = (operation == "add") ? 3 : 5;
                money += reward;
                points += reward;
                Console.WriteLine($"Correct! Your money and points increased by {reward}.");
                double tax = Math.Ceiling(reward / 3.0);
                taxAmount += (int)tax;
                csAmount += 1;
                PlayActionSound();
                AchievementReachedCheck(money, points);
                break;
            }
            else
            {
                PlayActionSound();
                Console.Write("Wrong answer! Would you like to try again (y/n)? ");
                string retry = Console.ReadLine();
                if (retry.ToLower() != "y") break;
            }
        }
    }

    static void HandleSave(string saveFilePath, int money, int points, bool notify)
    {
        using (StreamWriter saveFile = new StreamWriter(saveFilePath, false))
        {
            saveFile.WriteLine(money);
            saveFile.WriteLine(points);
            saveFile.WriteLine(taxAmount);
            saveFile.WriteLine(secretOneFound ? 1 : 0);
            saveFile.WriteLine(secretTwoFound ? 1 : 0);
            saveFile.WriteLine(csAmount);
            saveFile.WriteLine(inPrison ? 1 : 0);
            if (notify)
            {
                Console.WriteLine("Game saved!");
            }
        }
    }

    static void HandleLoad(string saveFilePath, ref int money, ref int points)
    {
        if (File.Exists(saveFilePath))
        {
            string[] lines = File.ReadAllLines(saveFilePath);

            if (lines.Length >= 7)
            {
                if (!int.TryParse(lines[0], out money)) money = 0;
                if (!int.TryParse(lines[1], out points)) points = 0;
                if (!int.TryParse(lines[2], out taxAmount)) taxAmount = 0;
                secretOneFound = lines[3] == "1";
                secretTwoFound = lines[4] == "1";
                if (!int.TryParse(lines[5], out csAmount)) csAmount = 0;
                inPrison = lines[6] == "1";

                Console.WriteLine($"Game loaded! (Money: {money}), (Points: {points}), (Taxes needed to pay: {taxAmount}), (Child support you owe: {csAmount})");
            }
        }
        else
        {
            Console.WriteLine("No saved game found.");
        }
    }

    static void HandleExit()
    {
        DateTime now = DateTime.Now;
        string message = (now.Hour >= 6 && now.Hour < 18) ? "Have a good day!" : "Have a good night!";
        Console.WriteLine($"Exiting game! {message}");
        AchievementReachedCheck(money, points);
    }

    static void HandlePoints(int points)
    {
        Console.WriteLine($"Current points: {points}");
        AchievementReachedCheck(money, points);
    }

    static void HandleMoney(int money)
    {
        Console.WriteLine($"Current money: {money}");
        AchievementReachedCheck(money, points);
    }

    static void HandleGamble(ref int money)
    {
        MP3Silently(@"C:\ReaperGame\Assets\gamble.mp3", true);

        if (money <= 0)
        {
            Console.WriteLine("You don't have enough money to gamble.");
            MP3Silently(@"C:\ReaperGame\Assets\gamble.mp3", false);
            return;
        }
        Gamble:
        Console.Write($"How much money would you like to gamble? (Current money: {money}): ");
        int gambleAmount = GetValidInt();

        if (gambleAmount <= 0)
        {
            Console.WriteLine("Invalid amount. Please enter a positive number.");
            goto Gamble;
        }
        else if (gambleAmount > money)
        {
            Console.WriteLine("You don't have enough money to gamble that amount.");
            MP3Silently(@"C:\ReaperGame\Assets\gamble.mp3", false);
            return;
        }

        Random random = new Random();
        int chance = new Random().Next(1, 102);
        double chance2 = (random.Next(0, 201) / 100.0) + 1.25;

        if (chance <= 66)
        {
            int winnings = (int)Math.Ceiling(gambleAmount * chance2);
            money += winnings;
            Console.WriteLine($"You won! You gained {winnings} money.");

            int tax = (int)Math.Ceiling(winnings / 3.0);
            taxAmount += tax;
            csAmount += 1;

            MP3Silently(@"C:\ReaperGame\Assets\gamble.mp3", false);
            PlayActionSound();
            AchievementReachedCheck(money, points);
        }
        else
        {
            money -= gambleAmount;
            Console.WriteLine($"You lost! You lost {gambleAmount} money.");

            MP3Silently(@"C:\ReaperGame\Assets\gamble.mp3", false);
            PlayActionSound();
        }
    }

    static void Prison()
    {
        MP3Silently(@"C:\ReaperGame\Assets\prison.mp3", true);
        inPrison = true;
        csAmount = 0;
        HandleSave(saveFilePath, money, points, true);

        Console.Clear();
        HandleSupport();
        SetFontSize(fontSize);
        string options;
        Console.WriteLine("\nYou are in prison for not paying your child support");
        wentToPrison = true;
        AchievementReachedCheck(money, points);
        SaveAchievements();

        while (true)
        {
            Console.WriteLine("\nYour options are:");
            Console.WriteLine("1. Math (Medium but free)");
            Console.WriteLine("2. Bribery (Easy but costs money)");
            Console.WriteLine("3. Prisonbreak (Hard but free)");

            options = Console.ReadLine();

            if (options.Equals("Bribery", StringComparison.OrdinalIgnoreCase))
            {
                if (money <= 0)
                {
                    Console.WriteLine("You don't have enough money to bribe.");
                    return;
                }

                Console.Write($"How much money would you like to bribe? (Current money: {money}) the higher the better chance: ");
                int bribeAmount = GetValidInt();

                if (bribeAmount <= 0)
                {
                    Console.WriteLine("Invalid amount. Please enter a positive number.");
                    return;
                }
                else if (bribeAmount > money)
                {
                    Console.WriteLine("You don't have enough money to bribe that amount.");
                    return;
                }

                int chance = new Random().Next(1, 102);
                if (chance <= Math.Pow(bribeAmount, 0.75))
                {
                    Console.WriteLine($"You lost {bribeAmount} money.");
                    MP3Silently(@"C:\ReaperGame\Assets\prison.mp3", false);
                    Console.WriteLine("YOU ARE OUT OF PRISON!!!");
                    inPrison = false;
                    escapedPrison = true;
                    AchievementReachedCheck(money, points);
                    Thread.Sleep(1000);
                    HandleSave(saveFilePath, money, points, true);
                    Game();
                }
                else
                {
                    Console.WriteLine("Your bribe was declined...");
                }
            }
            else if (options.Equals("Prisonbreak", StringComparison.OrdinalIgnoreCase))
            {
                int chance = new Random().Next(1, 102);
                if (chance <= 10)
                {
                    MP3Silently(@"C:\ReaperGame\Assets\prison.mp3", false);
                    Console.WriteLine("YOU ARE OUT OF PRISON!!!");
                    inPrison = false;
                    escapedPrison = true;
                    AchievementReachedCheck(money, points);
                    Thread.Sleep(1000);
                    HandleSave(saveFilePath, money, points, true);
                    Game();
                }
                else
                {
                    Console.WriteLine("You failed to escape...");
                }
            }
            else if (options.Equals("Math", StringComparison.OrdinalIgnoreCase))
            {
                while (true)
                {
                    int randomNum1 = new Random().Next(100, 1001);
                    int randomNum2 = new Random().Next(100, 1001);
                    int correctAnswer;
                    string operation;

                    Console.Write("Choose operation (add/subtract) numbers are generated randomly from 100 to 1000: ");
                    operation = Console.ReadLine();

                    if (operation == "add")
                    {
                        Console.WriteLine($"What is {randomNum1} + {randomNum2}?");
                        correctAnswer = randomNum1 + randomNum2;
                    }
                    else if (operation == "subtract")
                    {
                        Console.WriteLine($"What is {randomNum1} - {randomNum2}?");
                        correctAnswer = randomNum1 - randomNum2;
                    }
                    else
                    {
                        Console.WriteLine("Invalid operation. Please choose 'add' or 'subtract'.");
                        continue;
                    }

                    int playerAnswer = GetValidInt();
                    if (playerAnswer == correctAnswer)
                    {
                        MP3Silently(@"C:\ReaperGame\Assets\prison.mp3", false);
                        Console.WriteLine("YOU ARE OUT OF PRISON!!!");
                        inPrison = false;
                        escapedPrison = true;
                        AchievementReachedCheck(money, points);
                        Thread.Sleep(1000);
                        HandleSave(saveFilePath, money, points, true);
                        Game();
                        break;
                    }
                    else
                    {
                        Console.Write("Wrong answer! Would you like to try again (y/n)? ");
                        string retry = Console.ReadLine();
                        if (retry.ToLower() != "y") break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid response");
            }
        }
    }

    static void HandleTaxes(ref int money, ref int taxAmount)
    {
        if (taxAmount <= 0)
        {
            Console.WriteLine("You don't owe any taxes right now.");
            return;
        }

        string response;
        do
        {
            Console.WriteLine($"You owe: {taxAmount} in taxes.");
            Console.Write("Would you like to pay the taxes (y/n)? ");
            response = Console.ReadLine();

            if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                while (taxAmount > 0)
                {
                    Console.Write($"How much money would you like to pay for your taxes? (Current money: {money}, Remaining taxes: {taxAmount}): ");
                    int payment = GetValidInt();

                    if (payment > money)
                    {
                        Console.WriteLine($"You do not have enough money to pay this amount. You only have {money}.");
                    }
                    else if (payment > taxAmount)
                    {
                        Console.WriteLine($"You only owe {taxAmount} in taxes. Enter a smaller amount.");
                    }
                    else if (payment <= 0)
                    {
                        Console.WriteLine("Invalid input. Please enter a positive number.");
                    }
                    else
                    {
                        money -= payment;
                        taxAmount -= payment;
                        Console.WriteLine($"You paid {payment} in taxes. Your remaining balance is {money}.");
                        PlayActionSound();
                        if (taxAmount <= 0)
                        {
                            Console.WriteLine("You have fully paid your taxes.");
                            return;
                        }

                        Console.WriteLine($"Remaining taxes: {taxAmount}. Would you like to continue paying? (y/n): ");
                        response = Console.ReadLine();

                        if (!response.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"You chose not to pay any more taxes. Remaining taxes: {taxAmount}.");
                            return;
                        }
                    }
                }
            }
            else if (!response.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Invalid input. Please respond with 'y' or 'n'.");
            }

        } while (!response.Equals("n", StringComparison.OrdinalIgnoreCase));

        if (response.Equals("n", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"You chose not to pay taxes. The tax amount remains the same: {taxAmount}.");
        }
    }

    static void ChildSupport(ref int money, ref int childSupportAmount)
    {
        if (childSupportAmount <= 0)
        {
            Console.WriteLine("You don't owe child support.");
            return;
        }

        string response;
        do
        {
            Console.WriteLine($"You owe: {childSupportAmount} in child support.");
            Console.Write("Would you like to pay child support (y/n)? ");
            response = Console.ReadLine();

            if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                if (money >= childSupportAmount)
                {
                    money -= childSupportAmount;
                    childSupportAmount = 0;
                    Console.WriteLine($"Child support fully paid. Remaining money: {money}");
                    PlayActionSound();
                    AchievementReachedCheck(money, points);
                }
                else
                {
                    Console.WriteLine($"You do not have enough money to pay the full child support. Paying what you can: {money}");
                    childSupportAmount -= money;
                    money = 0;
                    Console.WriteLine($"Remaining child support amount: {childSupportAmount}");
                }
            }
            else if (!response.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Invalid input. Please respond with 'y' or 'n'.");
            }

        } while (!response.Equals("n", StringComparison.OrdinalIgnoreCase) && childSupportAmount > 0);

        if (childSupportAmount > 0)
        {
            Console.WriteLine($"You still owe {childSupportAmount} in child support. Please pay soon.");
        }
        else
        {
            Console.WriteLine("Child support is fully paid. Thank you!");
        }
    }

    static void TypeEffect(string text, int delayMs)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delayMs);
        }
        Console.WriteLine();
    }

    static void Mrati()
    {
        MP3Silently(@"C:\ReaperGame\Assets\mrati.mp3", true);

        int ratiHP = 70;
        int playerHP = 50;
        int playerPotions = 5;
        int money = 0;

        TypeEffect("MRATI: YOUR OPINION DOESN'T MATTER TO A MAN LIKE ME!@#$%\n", 40);
        Thread.Sleep(800);
        TypeEffect("MRATI: I'll make you pay...\n", 40);
        Thread.Sleep(2200);

        while (ratiHP > 0 && playerHP > 0)
        {
            Console.Clear();

            Console.WriteLine("PLAYER'S TURN!!!\n");
            Console.WriteLine($"RATI Health: {ratiHP}");
            Console.WriteLine($"Player Health: {playerHP}\n");

            Console.WriteLine("Attack Options");
            Console.WriteLine("Emotional Damage");
            Console.WriteLine("Tax Forms");
            Console.WriteLine("Child Support");
            if (playerPotions > 0)
            {
                Console.WriteLine($"Heal ({playerPotions} potions left)");
            }
            Console.WriteLine();

            string input = Console.ReadLine();
            int damageToRat = new Random().Next(1, 12);
            int chance = new Random().Next(1, 102);

            if (input == "Emotional Damage" || input == "emotional damage")
            {
                TypeEffect("Rolling Dice...\n", 60);
                Thread.Sleep(400);
                TypeEffect($"Dice came out as... {damageToRat} damage!\n", 60);

                if (chance <= 10)
                {
                    TypeEffect("BUT WAIT... You have missed and caused no emotional damage...\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                }
                else
                {
                    ratiHP -= damageToRat;
                    TypeEffect($"{damageToRat} emotional damage done to rati...\n", 60);
                    TypeEffect($"Rati has: {ratiHP} HP left. Cry...\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Thread.Sleep(2000);
                }
            }
            else if (input == "Tax Forms" || input == "tax forms")
            {
                TypeEffect("Rolling Dice...\n", 60);
                Thread.Sleep(400);
                TypeEffect($"Dice came out as... {damageToRat} damage!\n", 60);

                if (chance <= 10)
                {
                    TypeEffect("BUT WAIT... You have missed...\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                }
                else
                {
                    ratiHP -= damageToRat;
                    TypeEffect($"{damageToRat} Tax Forms given to rati...\n", 60);
                    TypeEffect($"Rati has: {ratiHP} HP left. I HATE TAXES!!!\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Thread.Sleep(2000);
                }
            }
            else if (input == "Child Support" || input == "child support")
            {
                TypeEffect("Rolling Dice...\n", 60);
                Thread.Sleep(400);
                TypeEffect($"Dice came out as... {damageToRat} damage!\n", 60);

                if (chance <= 10)
                {
                    TypeEffect("BUT WAIT... Rati doesn't have a child currently...\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                }
                else
                {
                    ratiHP -= damageToRat;
                    TypeEffect($"{damageToRat} Child Support money taken from rati...\n", 60);
                    TypeEffect($"Rati has: {ratiHP} HP left. I HATE PAYING CHILD SUPPORT!!!\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Thread.Sleep(2000);
                }
            }

            else if (input == "Attack Options" || input == "attack options")
            {
                ConsoleColor[] rainbowColors = new ConsoleColor[]
                {
        ConsoleColor.Red,
        ConsoleColor.Green,
        ConsoleColor.Yellow,
        ConsoleColor.Blue,
        ConsoleColor.Magenta
                };
                int colorIndex = 0;
                void CycleColor()
                {
                    Console.ForegroundColor = rainbowColors[colorIndex];
                    colorIndex = (colorIndex + 1) % rainbowColors.Length;
                }

                CycleColor();
                TypeEffect("Rolling Special Dice...\n", 60);
                Thread.Sleep(400);

                CycleColor();
                TypeEffect($"Special Dice came out as... {damageToRat * 2} damage!\n", 60);

                if (chance <= 10)
                {
                    CycleColor();
                    TypeEffect("BUT WAIT... Rati is sigma and you missed...\n", 60);

                    CycleColor();
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                }
                else
                {
                    ratiHP -= damageToRat * 2;

                    CycleColor();
                    TypeEffect($"{damageToRat * 2} damage done to rati...\n", 60);

                    CycleColor();
                    TypeEffect($"Rati has: {ratiHP} HP left.\n", 60);

                    CycleColor();
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Thread.Sleep(2000);
                }

                Console.ResetColor();
            }

            else if (input == "Heal" || input == "heal")
            {
                if (playerPotions > 0)
                {
                    playerHP += 20;
                    playerPotions--;
                    TypeEffect($"You used a healing potion! +20 HP.\n", 60);
                    TypeEffect($"You have {playerPotions} healing potions left.\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Thread.Sleep(2000);
                }
                else
                {
                    TypeEffect("No potions left! Try a different option...\n", 60);
                    continue;
                }
            }
            else
            {
                TypeEffect("Invalid option! Try again.\n", 60);
                continue;
            }

            // Rati's turn
            if (ratiHP > 0)
            {
                Console.Clear();
                Console.WriteLine("RATI'S TURN!!!\n");
                Console.WriteLine($"RATI Health: {ratiHP}");
                Console.WriteLine($"Player Health: {playerHP}\n");

                int ratiDamage = new Random().Next(1, 12);
                playerHP -= ratiDamage;
                TypeEffect($"Rati dealt {ratiDamage} damage to you.\n", 60);
                TypeEffect($"You have: {playerHP} HP left.\n", 60);
                TypeEffect("PLAYER'S TURN!!!\n\n", 30);
                Thread.Sleep(2000);
            }
        }

        if (ratiHP <= 0)
        {
            TypeEffect("Rati has been defeated! You win!\n", 60);
            TypeEffect("You won 5,000 money\n", 60);
            money += 5000;
            beatRati = true;
            AchievementReachedCheck(money, points);
            Thread.Sleep(1000);
        }
        else if (playerHP <= 0)
        {
            TypeEffect("You have been defeated by Rati... Game Over!\n", 60);
            Thread.Sleep(1000);
        }
    }

    static void Game()
    {
        Console.Clear();
        Console.Title = "Reaper's Cool Game (C# Edition)";

        AssetsFolder();
        HandleSupport();
        Console.WriteLine("You are using C# Edition");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string autoload = "off", autosave = "off";
        LoadSettings(ref autoload, ref autosave, settingsFilePath);
        SetFontSize(fontSize);

        if (autoload == "on")
        {
            HandleLoad(saveFilePath, ref money, ref points);
        }
        LoadAchievements();
        AchievementReachedCheck(money, points);
        DisplayCommands();

        while (true)
        {
            if (!inPrison)
            {
                if (csAmount == 15)
                {
                    Prison();
                }

                // Commands
                string command = Console.ReadLine();
                switch (command)
                {
                    case "/help":
                        DisplayCommands();
                        break;
                    case "/achievements":
                        DisplayAchievements();
                        break;
                    case "/support":
                        HandleSupport();
                        break;
                    case "/transaction":
                        HandleTransaction(ref money, ref points);
                        break;
                    case "/playagame":
                        HandlePlayAGame(ref money, ref points);
                        break;
                    case "/save":
                        HandleSave(saveFilePath, money, points, true);
                        break;
                    case "/load":
                        HandleLoad(saveFilePath, ref money, ref points);
                        break;
                    case "/settings":
                        Settings(ref autoload, ref autosave, settingsFilePath);
                        break;
                    case "/gamble":
                        HandleGamble(ref money);
                        break;
                    case "/taxes":
                        HandleTaxes(ref money, ref taxAmount);
                        break;
                    case "/childsupport":
                        ChildSupport(ref money, ref csAmount);
                        break;
                    case "/exit":
                        if (autosave == "on")
                        {
                            HandleSave(saveFilePath, money, points, true);
                        }
                        HandleExit();
                        Console.Write("Confirm (y/n)? ");
                        string response = Console.ReadLine();
                        if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            Environment.Exit(0);
                        }
                        break;
                    case "/clear":
                        Console.Clear();
                        HandleSupport();
                        LoadSettings(ref autoload, ref autosave, settingsFilePath);
                        SetFontSize(fontSize);
                        if (autoload == "on")
                        {
                            HandleLoad(saveFilePath, ref money, ref points);
                        }
                        DisplayCommands();
                        break;
                    case "/points":
                        HandlePoints(points);
                        break;
                    case "/money":
                        HandleMoney(money);
                        break;
                    case "/reaper":
                        if (!secretOneFound)
                        {
                            Console.WriteLine("Guh, I guess you have found me\nI will give you 1000 money, I paid the taxes for you btw and np.");
                            money += 1000;
                            secretOneFound = true;
                            AchievementReachedCheck(money, points);
                        }
                        else
                        {
                            Console.WriteLine("Bro STOP BEGGING ME FOR MONEY PLEASE BRUH...");
                            begging = true;
                            AchievementReachedCheck(money, points);
                        }
                        break;
                    case "/kys":
                        if (!secretTwoFound)
                        {
                            Console.WriteLine("Did you just tell me to keep myself safe??? I am stealing your money");
                            double moneyStolen = Math.Ceiling(money / 3.0);
                            money -= (int)moneyStolen;
                            Console.WriteLine($"Reaper just stole {moneyStolen} money from you");
                            secretTwoFound = true;
                            AchievementReachedCheck(money, points);
                        }
                        else
                        {
                            Console.WriteLine("Odd specimen");
                        }
                        break;
                    case "/therat":
                        string input;
                        TypeEffect("MRATI: LET ME TELL YOU A STORY...\n", 70);
                        Console.Write("(y/n)? ");
                        input = Console.ReadLine();
                        if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            TypeEffect("MRATI: Why thank you actually! :D\n", 70);
                            goto Story;
                        }
                        else if (input.Equals("n", StringComparison.OrdinalIgnoreCase))
                        {
                            TypeEffect("MRATI: (Ignoring your input)\n", 70);
                            goto Story;
                        }
                        else
                        {
                            TypeEffect("MRATI: Learn how to respond you!!!", 70);
                            goto Story;
                        }

                    Story:
                        TypeEffect("MRATI: Anyways... I was a crocodile once bit by an evil rat!\n", 70);
                        TypeEffect("MRATI: This is why I love joe biden he is so awesome! :3\n", 70);
                        TypeEffect("MRATI: This is why I am a rat...\n", 70);
                        TypeEffect("MRATI: Cool story?\n", 70);
                        Console.Write("(y/n)? ");
                        string input2 = Console.ReadLine();
                        if (input2.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            TypeEffect("MRATI: Thank you I am proud to be your guest :D\n", 70);
                        }
                        else if (input2.Equals("n", StringComparison.OrdinalIgnoreCase))
                        {
                            TypeEffect("MRATI: KYS\n", 70);
                            TypeEffect("......... Boss encounter!!!\n", 70);
                            Thread.Sleep(250);
                            Mrati();
                        }
                        else
                        {
                            TypeEffect("MRATI: Learn how to respond you!!!", 70);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid command. Type /help for available commands.");
                        break;
                }

                if (autosave == "on")
                {
                    HandleSave(saveFilePath, money, points, false);
                }
            }
        }
    }

    static void Main(string[] args)
    {
        Random random = new Random();
        Game();
    }
}