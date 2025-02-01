#include <iostream>
#include <random>
#include <fstream>
#include <string>
#include <limits>
#include <cstdlib>
#include <ctime>
#include <stdio.h>
#include <windows.h>
#include <thread>
#include <chrono>
#include <C:\cstuff\bass24\c\bass.h>
#pragma comment(lib, "winmm.lib")
#pragma comment(lib, "C:\\cstuff\\bass24\\c\\x64\\bass.lib") //reaper leak fr (also lazy tbh)

using namespace std;

#undef max

int taxamount = 0;
int csamount = 0;
int fontSize;
bool inprison = false;
bool secretonefound = false;
bool secrettwofound = false;
bool beatRati = false;
bool begging = false;
int money = 10;
int points = 0;
string folderPath = "C:\\ReaperGame\\SaveData";
string saveFilePath = folderPath + "\\game_save.txt";
string settingsFilePath = folderPath + "\\settings.txt";
string achievementsFilePath = folderPath + "\\achievements.txt";

void AssetsFolder() {
    const char* folderPath = "C:\\ReaperGame\\Assets";

    DWORD fileAttr = GetFileAttributesA(folderPath);

    if (fileAttr == INVALID_FILE_ATTRIBUTES || !(fileAttr & FILE_ATTRIBUTE_DIRECTORY)) {
        CreateDirectoryA(folderPath, NULL);
    }
}

bool FileExists(const char* filename) {
    FILE* file;
    if (fopen_s(&file, filename, "r") == 0) {
        fclose(file);
        return true;
    }
    return false;
}

void MP3Silently(const char* mp3File, bool playstop) {
    if (!FileExists(mp3File)) {
        return;
    }

    if (playstop) {
        if (!BASS_Init(-1, 44100, 0, 0, NULL)) {
            return;
        }

        HSTREAM stream = BASS_StreamCreateFile(FALSE, mp3File, 0, 0, BASS_STREAM_AUTOFREE);
        if (!stream) {
            BASS_Free();
            return;
        }

        if (!BASS_ChannelPlay(stream, FALSE)) {
            BASS_Free();
            return;
        }
    }
    else {
        HSTREAM stream = BASS_StreamCreateFile(FALSE, mp3File, 0, 0, BASS_STREAM_AUTOFREE);
        if (stream) {
            BASS_ChannelStop(stream);
            BASS_Free();
        }
    }
}

void SetFontSize(int size) {
    if (size < 1 || size > 3) {
        cout << "Invalid font size. Setting to default (3).\n";
        size = 3;
    }
    CONSOLE_FONT_INFOEX cfi;
    cfi.cbSize = sizeof(CONSOLE_FONT_INFOEX);
    GetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE), FALSE, &cfi);

    switch (size) {
    case 1:
        cfi.dwFontSize.X = 8;
        cfi.dwFontSize.Y = 12;
        break;
    case 2:
        cfi.dwFontSize.X = 12;
        cfi.dwFontSize.Y = 18;
        break;
    case 3:
        cfi.dwFontSize.X = 16;
        cfi.dwFontSize.Y = 24;
        break;
    }

    SetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE), FALSE, &cfi);
}

int getValidInt() {
    int value;
    while (true) {
        cin >> value;
        if (cin.fail()) {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            cout << "Invalid input. Please enter a number: ";
        }
        else {
            return value;
        }
    }
}

bool money100 = false;
bool money1000 = false;
bool money10000 = false;
bool money100000 = false;
bool money1000000 = false;

bool firstTransaction = false;

bool foundreaper = false;
bool insultreaper = false;
bool begreaper = false;

bool wentToPrison = false;
bool escapedPrison = false;
bool mratiBeat = false;

bool points100 = false;
bool points1000 = false;
bool points10000 = false;
bool points100000 = false;
bool points1000000 = false;

//checking stuff bruh
bool donotcheck = false;
bool donotcheck2 = false;

void SaveAchievements() {
    ofstream achievementsFile(achievementsFilePath);
    if (achievementsFile.is_open()) {
        achievementsFile << money100 << "\n";
        achievementsFile << money1000 << "\n";
        achievementsFile << money10000 << "\n";
        achievementsFile << money100000 << "\n";
        achievementsFile << money1000000 << "\n";

        achievementsFile << firstTransaction << "\n";

        achievementsFile << foundreaper << "\n";
        achievementsFile << insultreaper << "\n";
        achievementsFile << begreaper << "\n";

        achievementsFile << points100 << "\n";
        achievementsFile << points1000 << "\n";
        achievementsFile << points10000 << "\n";
        achievementsFile << points100000 << "\n";
        achievementsFile << points1000000 << "\n";

        achievementsFile << wentToPrison << "\n";
        achievementsFile << escapedPrison << "\n";

        achievementsFile << donotcheck << "\n";
        achievementsFile << donotcheck2 << "\n";

        achievementsFile << beatRati << "\n";

        achievementsFile.close();
    }
    else {
        cout << "Error: Unable to save achievements.\n";
    }
}

void LoadAchievements() {
    ifstream achievementsFile(achievementsFilePath);
    if (achievementsFile.is_open()) {
        achievementsFile >> money100;
        achievementsFile >> money1000;
        achievementsFile >> money10000;
        achievementsFile >> money100000;
        achievementsFile >> money1000000;

        achievementsFile >> firstTransaction;
        achievementsFile >> foundreaper;
        achievementsFile >> insultreaper;
        achievementsFile >> begreaper;

        achievementsFile >> points100;
        achievementsFile >> points1000;
        achievementsFile >> points10000;
        achievementsFile >> points100000;
        achievementsFile >> points1000000;

        achievementsFile >> wentToPrison;
        achievementsFile >> escapedPrison;

        achievementsFile >> donotcheck;
        achievementsFile >> donotcheck2;

        achievementsFile >> beatRati;

        achievementsFile.close();
    }
    else {
        cout << "No achievements file found... Starting fresh!\n";
    }
}

void DisplayAchievements() {
    cout << "Achievements:\n";

    if (money100) cout << "Novice: 100+ money\n";
    if (money1000) cout << "Grinder Type Guy: 1000+ money\n";
    if (money10000) cout << "Commander: 10000+ money\n";
    if (money100000) cout << "Destroyer Type Guy: 100000+ money\n";
    if (money1000000) cout << "Rich Sigma: 1000000+ money\n";

    if (firstTransaction) cout << "First Transaction: Completed your first transaction\n";
    if (foundreaper) cout << "Finding the Reaper: You found Reaper who gave you 100 money\n";
    if (insultreaper) cout << "Insulting the Reaper: You insulted Reaper, how dare you\n";
    if (begreaper) cout << "Begging: Stop begging Reaper for money, please\n";

    if (points100) cout << "Point Grinder: 100+ points\n";
    if (points1000) cout << "Pointer: 1000+ points\n";
    if (points10000) cout << "Amazing Player: 10000+ points\n";
    if (points100000) cout << "Super Player: 100000+ points\n";
    if (points1000000) cout << "Sigma Player: 1000000+ points\n";

    if (escapedPrison) cout << "Criminal Crime: You escaped prison\n";
    if (wentToPrison) cout << "Law Breaker: You went to prison\n";

    if (mratiBeat) cout << "Rati's Defeat: You defeated Rati\n";

}

void PlayAchievementSound() {
    PlaySound(TEXT("C:\\ReaperGame\\Assets\\achievement.wav"), NULL, SND_FILENAME | SND_ASYNC);
}
void PlayActionSound() {
    PlaySound(TEXT("C:\\ReaperGame\\Assets\\action.wav"), NULL, SND_FILENAME | SND_ASYNC);
}
void AchievementReachedCheck(int money, int points) {
    if (money >= 100 && !money100) {
        PlayAchievementSound();
        cout << "You got the Novice achievement! (100+ money)\n";
        money100 = true;
    }
    if (money >= 1000 && !money1000) {
        PlayAchievementSound();
        cout << "You got the Grinder Type Guy achievement! (1000+ money)\n";
        money1000 = true;
    }
    if (money >= 10000 && !money10000) {
        PlayAchievementSound();
        cout << "You got the Commander achievement! (10000+ money)\n";
        money10000 = true;
    }
    if (money >= 100000 && !money100000) {
        PlayAchievementSound();
        cout << "You got the Destroyer Type Guy achievement! (100000+ money)\n";
        money100000 = true;
    }
    if (money >= 1000000 && !money1000000) {
        PlayAchievementSound();
        cout << "You got the Rich Sigma achievement! (1000000+ money)\n";
        money1000000 = true;
    }

    if (points >= 1 && !firstTransaction)
    {
        PlayAchievementSound();
        cout << "You got the First Transaction achievement! (first transaction)\n";
        firstTransaction = true;
    }

    if (points > 0 && !firstTransaction)
    {
        PlayAchievementSound();
        cout << "You got the First Transaction achievement! (first transaction)\n";
        firstTransaction = true;
    }

    if (points >= 100 && !points100) {
        PlayAchievementSound();
        cout << "You got the Point Grinder achievement! (100+ points)\n";
        points100 = true;
    }
    if (points >= 1000 && !points1000) {
        PlayAchievementSound();
        cout << "You got the Pointer achievement! (1000+ points)\n";
        points1000 = true;
    }
    if (points >= 10000 && !points10000) {
        PlayAchievementSound();
        cout << "You got the Amazing Player achievement! (10000+ points)\n";
        points10000 = true;
    }
    if (points >= 100000 && !points100000) {
        PlayAchievementSound();
        cout << "You got the Super Player achievement! (100000+ points)\n";
        points100000 = true;
    }
    if (points >= 1000000 && !points1000000) {
        PlayAchievementSound();
        cout << "You got the Sigma Player achievement! (1000000+ points)\n";
        points1000000 = true;
    }

    
    //reaper and rati stuff
    if (secretonefound && !foundreaper)
    {
        cout << "Finding the Reaper (You found Reaper who gave you 100 money)\n";
        foundreaper = true;
    }
    if (secrettwofound > 0 && !insultreaper)
    {
        cout << "Insulting the Reaper (You insulted Reaper, how dare you)\n";
        insultreaper = true;
    }
    if (begging && !begreaper)
    {
        cout << "You got the Begging achievement! (Stop begging Reaper for money, please)\n";
        begreaper = true;
    }

    if (beatRati && !mratiBeat)
    {
        cout << "You got the Boss Defeater achievement! (You beat Rati)\n";
        mratiBeat = true;
    }




    if (points > 0 && !firstTransaction) {
        PlayAchievementSound();
        cout << "You got the First Transaction achievement! (You did your first transaction)\n";
        firstTransaction = true;
    }

    if (wentToPrison) {
        if (!donotcheck)
        {
            PlayAchievementSound();
            cout << "You got the Law Breaker achievement! (You went to prison)\n";
            donotcheck = true;
        }
    }

    if (escapedPrison) {
        if (!donotcheck2)
        {
            PlayAchievementSound();
            cout << "You got the Criminal Crime achievement! (You escaped prison)\n";
            donotcheck2 = true;
        }

    }
    SaveAchievements();
}

void DisplayCommands() {
    cout << "\nAvailable commands:\n";
    cout << "/help - Show this help message\n";
    cout << "/achievements - Shows your gained achievements\n";
    cout << "/transaction - Convert your money into points (Includes tax)\n";
    cout << "/playagame - Play a math game (Includes tax)\n";
    cout << "/money - Show your money balance\n";
    cout << "/points - Show your points\n";
    cout << "/save - Save your game progress\n";
    cout << "/load - Load your game progress\n";
    cout << "/support - Check support\n";
    cout << "/settings - Change game settings\n";
    cout << "/gamble - Gamble your money for a chance to win (Includes tax)\n";
    cout << "/taxes - Pay your taxes\n";
    cout << "/childsupport - Pay your child support\n";
    cout << "/exit - Exit game\n";
    cout << "/clear - clears all text (keeps commands)\n\n";
}

void LoadSettings(string& autoload, string& autosave, const string& settingsFilePath) {
    string fontsize = "";
    ifstream settingsFile(settingsFilePath);
    if (settingsFile.is_open()) {
        getline(settingsFile, autoload);
        getline(settingsFile, autosave);
        getline(settingsFile, fontsize);

        try {
            fontSize = stoi(fontsize);
        }
        catch (const invalid_argument& e) {
            fontSize = 3;
        }
    }
    else {
        autoload = "off";
        autosave = "off";
        fontSize = 3;
        cout << "Settings file could not be loaded. Using default settings.\n";
    }
}
void SaveSettings(const string& autoload, const string& autosave, const string& settingsFilePath) {
    ofstream settingsFile(settingsFilePath, ios::out | ios::trunc);
    if (settingsFile.is_open()) {
        settingsFile << autoload << "\n";
        settingsFile << autosave << "\n";
        settingsFile << fontSize << "\n";
    }
    else {
        cout << "Error saving settings.\n";
    }
}

void ResetSettings(const string& settingsFilePath) {
    ofstream settingsFile(settingsFilePath, ios::out | ios::trunc);
    if (settingsFile.is_open()) {
        settingsFile << "off\n";
        settingsFile << "off\n";
        settingsFile << "3\n";
        cout << "Settings reset to default values.\n";
    }
    else {
        cout << "Error resetting settings.\n";
    }
}

void Settings(string& autoload, string& autosave, const string& settingsFilePath) {
    while (true) {
        cout << "Settings:\n";
        cout << "1 = Autoload save data: " << autoload << "\n";
        cout << "2 = Autosave save data: " << autosave << "\n";
        cout << "3 = Change Font Size (Current size: " << fontSize << ")\n";
        cout << "4 = Reset settings\n";
        cout << "5 = Exit settings\n";
        cout << "Enter your choice (number ex: 1, 2, 3, 4, 5): ";

        int choice = getValidInt();
        if (choice == 1) {
            cout << "Toggle Autoload save data (on/off): ";
            cin >> autoload;
            PlayActionSound();
        }
        else if (choice == 2) {
            cout << "Toggle Autosave save data (on/off): ";
            cin >> autosave;
            PlayActionSound();
        }
        else if (choice == 3) {
            cout << "Enter new font size (1 is for small, 2 is for medium, and 3 is for large): ";
            fontSize = getValidInt();
            SetFontSize(fontSize);
            PlayActionSound();
        }
        else if (choice == 4) {
            ResetSettings(settingsFilePath);
            autoload = "off";
            autosave = "off";
        }
        else if (choice == 5) {
            SaveSettings(autoload, autosave, settingsFilePath);
            PlayActionSound();
            break;
        }
        else {
            cout << "Invalid choice. Please try again.\n";
        }
    }
}

void HandleSupport() {
    cout << "Game by Reapermen, contact zxteam27@gmail.com for any support or suggestions...\n";
}

void HandleTransaction(int& money, int& points) {
    while (true) {
        cout << "How much money would you like to convert into points? (Current money: " << money << "): ";
        int convertAmount = getValidInt();

        if (convertAmount <= 0) {
            cout << "Invalid input. Please enter a positive number.\n";
        }
        else if (convertAmount > money) {
            cout << "You do not have enough money. You only have " << money << ".\n";
            cout << "Would you like to try again (y/n)? ";
            string response;
            cin >> response;
            if (response != "y" || response != "Y") break;
        }
        else {
            points += convertAmount;
            money -= convertAmount;
            cout << "Your points increased by " << convertAmount << ", and your money decreased by " << convertAmount << ".\n";
            double tax = convertAmount / 4;
            tax = ceil(tax);
            taxamount += static_cast<int>(tax);
            PlayActionSound();
            AchievementReachedCheck(money, points);
            break;
        }
    }
}

void HandlePlayAGame(int& money, int& points) {


    while (true) {
        int randomNum1 = rand() % 101 + 1;
        int randomNum2 = rand() % 101 + 1;
        int correctAnswer;
        string operation;

        cout << "Choose operation (add/subtract). Adding gives 3 points/money, subtracting gives 5 points/money: ";
        cin >> operation;

        if (operation == "add") {
            cout << "What is " << randomNum1 << " + " << randomNum2 << "? ";
            correctAnswer = randomNum1 + randomNum2;
        }
        else if (operation == "subtract") {
            cout << "What is " << randomNum1 << " - " << randomNum2 << "? ";
            correctAnswer = randomNum1 - randomNum2;
        }
        else {
            cout << "Invalid operation. Please choose 'add' or 'subtract'.\n";
            continue;
        }

        int playerAnswer = getValidInt();
        if (playerAnswer == correctAnswer) {
            int reward = (operation == "add") ? 3 : 5;
            money += reward;
            points += reward;
            cout << "Correct! Your money and points increased by " << reward << ".\n";
            double tax = reward / 3;
            tax = ceil(tax);
            taxamount += static_cast<int>(tax);
            csamount += 1;
            PlayActionSound();
            AchievementReachedCheck(money, points);
            break;
        }
        else {
            PlayActionSound();
            cout << "Wrong answer! Would you like to try again (y/n)? ";
            string retry;
            cin >> retry;
            if (retry != "y" || retry != "Y") break;
        }
    }
}

void HandleSave(const string& saveFilePath, int money, int points, bool notify) {
    ofstream saveFile(saveFilePath, ios::out | ios::trunc);
    if (saveFile.is_open()) {
        saveFile << money << "\n" << points << "\n" << taxamount << "\n" << secretonefound << "\n" << secrettwofound << "\n" << csamount << "\n" << inprison << endl;
        if (notify == true)
        {
            cout << "Game saved!\n";
        }
    }
    else {
        cout << "Error saving game.\n";
    }
}

void HandleLoad(const string& saveFilePath, int& money, int& points) {
    ifstream loadFile(saveFilePath);
    if (loadFile.is_open()) {
        string line;
        if (getline(loadFile, line)) money = stoi(line);
        if (getline(loadFile, line)) points = stoi(line);
        if (getline(loadFile, line)) taxamount = stoi(line);
        if (getline(loadFile, line)) secretonefound = stoi(line);
        if (getline(loadFile, line)) secrettwofound = stoi(line);
        if (getline(loadFile, line)) csamount = stoi(line);
        if (getline(loadFile, line)) inprison = stoi(line);

        cout << "Game loaded! (Money: " << money << "), (Points: " << points << "), (Taxes needed to pay: " << taxamount << "), (Child support you owe: " << csamount << ")\n";
    }
    else {
        cout << "No saved game found.\n";
    }
}

void HandleExit() {
    time_t now = time(0);
    tm ltm;
    localtime_s(&ltm, &now);
    int hour = ltm.tm_hour;
    cout << "Exiting game! " << (hour >= 6 && hour < 18 ? "Have a good day!" : "Have a good night!") << "\n";
    AchievementReachedCheck(money, points);
}

void HandlePoints(int points) {
    cout << "Current points: " << points << endl;
    AchievementReachedCheck(money, points);
}

void HandleMoney(int money) {
    cout << "Current money: " << money << endl;
    AchievementReachedCheck(money, points);
}

void HandleGamble(int& money) {
    MP3Silently("C:\\ReaperGame\\Assets\\gamble.mp3", true);

    if (money <= 0) {
        cout << "You don't have enough money to gamble.\n";
        MP3Silently("C:\\ReaperGame\\Assets\\gamble.mp3", false);
        return;
    }

    Gamble:
    cout << "How much money would you like to gamble? (Current money: " << money << "): ";
    int gambleAmount = getValidInt();

    if (gambleAmount <= 0) {
        cout << "Invalid amount. Please enter a positive number.\n";
        goto Gamble;
    }
    else if (gambleAmount > money) {
        cout << "You don't have enough money to gamble that amount.\n";
        return;
    }

    int chance = rand() % 101;
    double chance2 = ((rand() % 201) / 100.0) + 1.25;  //between 1.25 and 3.25

    if (chance <= 66) {
        int winnings = ceil(gambleAmount * chance2);
        money += winnings;
        cout << "You won! You gained " << winnings << " money.\n";

        int tax = ceil(winnings / 3.0);
        taxamount += tax;
        csamount += 1;

        MP3Silently("C:\\ReaperGame\\Assets\\gamble.mp3", false);
        PlayActionSound();
        AchievementReachedCheck(money, points);
    }
    else {
        money -= gambleAmount;
        cout << "You lost! You lost " << gambleAmount << " money.\n";

        MP3Silently("C:\\ReaperGame\\Assets\\gamble.mp3", false);
        PlayActionSound();
    }
}
void Game(); //needed a declaration thanks so much w3schools for the help :3

void Prison()
{
    //prison
    MP3Silently("C:\\ReaperGame\\Assets\\prison.mp3", true);
    inprison = true;
    csamount = 0;
    HandleSave(saveFilePath, money, points, true);

#ifdef _WIN32
    system("cls");
#else
    system("clear");
#endif

    HandleSupport();
    SetFontSize(fontSize);
    string options;
    cout << "\nYou are in prison for not paying your child support" << endl;
    wentToPrison = true;
    AchievementReachedCheck(money, points);
    SaveAchievements();

Prisonlist:
    cout << "\nYour options are:\n";
    cout << "1. Math (Medium but free)\n\n";
    cout << "2. Bribery (Easy but costs money)\n\n";
    cout << "3. Prisonbreak (Hard but free)\n\n";

    cin >> options;

    if (options == "Bribery" || options == "bribery")
    {
        if (money <= 0) {
            cout << "You don't have enough money to bribe.\n";
            return;
        }

        cout << "How much money would you like to bribe? (Current money: " << money << ") the higher the better chance: ";
        int bribeAmount = getValidInt();

        if (bribeAmount <= 0) {
            cout << "Invalid amount. Please enter a positive number.\n";
            return;
        }
        else if (bribeAmount > money) {
            cout << "You don't have enough money to bribe that amount.\n";
            return;
        }

        int chance = rand() % 101 + 1;

        if (chance <= pow(bribeAmount, 0.75f)) {
            cout << "You lost " << bribeAmount << " money.\n";
            MP3Silently("C:\\ReaperGame\\Assets\\prison.mp3", false);
            cout << "YOU ARE OUT OF PRISON!!!";
            inprison = false;
            escapedPrison = true;
            AchievementReachedCheck(money, points);
            Sleep(1000);
            HandleSave(saveFilePath, money, points, true);
            Game();
        }
        else
        {
            cout << "Your bribe was declined...";
            goto Prisonlist;
        }
    }
    else if (options == "Prisonbreak" || options == "prisonbreak")
    {
        int chance = rand() % 101 + 1;
        if (chance <= 10)
        {
            MP3Silently("C:\\ReaperGame\\Assets\\prison.mp3", false);
            cout << "YOU ARE OUT OF PRISON!!!";
            inprison = false;
            escapedPrison = true;
            AchievementReachedCheck(money, points);
            Sleep(1000);
            HandleSave(saveFilePath, money, points, true);
            Game();
        }
        else
        {
            cout << "You failed to escape...\n";
            goto Prisonlist;
        }
    }
    else if (options == "Math" || options == "math")
    {
        while (true) {
            int randomNum1 = rand() % 901 + 100;
            int randomNum2 = rand() % 901 + 100;
            int correctAnswer;
            string operation;

            cout << "Choose operation (add/subtract) numbers are generated random from 100 to 1000: ";
            cin >> operation;

        TryAgain:
            if (operation == "add") {
                cout << "What is " << randomNum1 << " + " << randomNum2 << "? ";
                correctAnswer = randomNum1 + randomNum2;
            }
            else if (operation == "subtract") {
                cout << "What is " << randomNum1 << " - " << randomNum2 << "? ";
                correctAnswer = randomNum1 - randomNum2;
            }
            else {
                cout << "Invalid operation. Please choose 'add' or 'subtract'.\n";
                continue;
            }

            int playerAnswer = getValidInt();
            if (playerAnswer == correctAnswer) {
                MP3Silently("C:\\ReaperGame\\Assets\\prison.mp3", false);
                cout << "YOU ARE OUT OF PRISON!!!";
                inprison = false;
                escapedPrison = true;
                AchievementReachedCheck(money, points);
                Sleep(1000);
                HandleSave(saveFilePath, money, points, true);
                Game();
                break;
            }
            else {
                cout << "Wrong answer! Would you like to try again (y/n)? ";
                string retry;
                cin >> retry;
                if (retry != "y" && retry != "Y") goto TryAgain;
            }
        }
    }
    else {
        if (options != "Options")
        {
            cout << "Invalid response\n";
            goto Prisonlist;
        }
    }
}

void HandleTaxes(int& money, int& taxamount) {
    if (taxamount <= 0) {
        cout << "You don't owe any taxes right now.\n";
        return;
    }

    string response;

    do {
        cout << "You owe: " << taxamount << " in taxes." << endl;
        cout << "Would you like to pay the taxes (y/n)? ";
        cin >> response;

        if (response == "y" || response == "Y") {
            while (taxamount > 0) {
                cout << "How much money would you like to pay for your taxes? (Current money: " << money << ", Remaining taxes: " << taxamount << "): ";
                int payment = getValidInt();

                if (payment > money) {
                    cout << "You do not have enough money to pay this amount. You only have " << money << ".\n";
                }
                else if (payment > taxamount) {
                    cout << "You only owe " << taxamount << " in taxes. Enter a smaller amount.\n";
                }
                else if (payment <= 0) {
                    cout << "Invalid input. Please enter a positive number.\n";
                }
                else {
                    money -= payment;
                    taxamount -= payment;
                    cout << "You paid " << payment << " in taxes. Your remaining balance is " << money << ".\n";
                    PlayActionSound();
                    if (taxamount <= 0) {
                        cout << "You have fully paid your taxes.\n";
                        return;
                    }

                    cout << "Remaining taxes: " << taxamount << ". Would you like to continue paying? (y/n): ";
                    cin >> response;

                    if (response != "y" && response != "Y") {
                        cout << "You chose not to pay any more taxes. Remaining taxes: " << taxamount << ".\n";
                        return;
                    }
                }
            }
        }
        else if (response != "n" && response != "N") {
            cout << "Invalid input. Please respond with 'y' or 'n'.\n";
        }

    } while (response != "n" && response != "N");

    if (response == "n" || response == "N") {
        cout << "You chose not to pay taxes. The tax amount remains the same.\n";
    }
}

void ChildSupport(int& money, int& childsupportamount)
{
    if (childsupportamount <= 0) {
        cout << "You don't owe child support.\n";
        return;
    }

    string response;

    do {
        cout << "You owe: " << childsupportamount << " in child support." << endl;
        cout << "Would you like to pay child support (y/n)? ";
        cin >> response;

        if (response == "y" || response == "Y") {
            if (money >= childsupportamount) {
                money -= childsupportamount;
                childsupportamount = 0;
                cout << "Child support fully paid. Remaining money: " << money << "\n";
                PlayActionSound();
                AchievementReachedCheck(money, points);
            }
            else {
                cout << "You do not have enough money to pay the full child support.\n";
                cout << "Paying what you can: " << money << "\n";
                childsupportamount -= money;
                money = 0;
                cout << "Remaining child support amount: " << childsupportamount << "\n";
            }
        }
        else if (response != "n" && response != "N") {
            cout << "Invalid input. Please respond with 'y' or 'n'.\n";
        }

    } while (response != "n" && response != "N" && childsupportamount > 0);

    if (childsupportamount > 0) {
        cout << "You still owe " << childsupportamount << " in child support. Please pay soon.\n";
    }
    else {
        cout << "Child support is fully paid. Thank you!\n";
    }
}

void TypeEffect(const string& text, int delay_ms) {
    for (char c : text) {
        cout << c << flush;
        this_thread::sleep_for(chrono::milliseconds(delay_ms));
    }
    cout << endl;
}

void Mrati();

void Game()
{
#ifdef _WIN32
    system("cls");
#else
    system("clear");
#endif
    system("title Reaper's Cool Game (C++ Edition)");

    AssetsFolder();
    HandleSupport();
    cout << "You are using C++ Edition\n";

    if (system(("mkdir " + folderPath).c_str())) {}

    string autoload, autosave;
    LoadSettings(autoload, autosave, settingsFilePath);
    SetFontSize(fontSize);

    if (autoload == "on") {
        HandleLoad(saveFilePath, money, points);
    }
    LoadAchievements();
    AchievementReachedCheck(money, points);
    DisplayCommands();

    while (true)
        if (!inprison)
        {
            if (csamount == 15)
            {
                Prison();
            }
            //commands
            string command;
            cin >> command;
            if (command == "/help") DisplayCommands();
            else if (command == "/achievements") DisplayAchievements();
            else if (command == "/support") HandleSupport();
            else if (command == "/transaction") HandleTransaction(money, points);
            else if (command == "/playagame") HandlePlayAGame(money, points);
            else if (command == "/save") HandleSave(saveFilePath, money, points, true);
            else if (command == "/load") HandleLoad(saveFilePath, money, points);
            else if (command == "/settings") Settings(autoload, autosave, settingsFilePath);
            else if (command == "/gamble") HandleGamble(money);
            else if (command == "/taxes") HandleTaxes(money, taxamount);
            else if (command == "/childsupport") ChildSupport(money, csamount);
            else if (command == "/exit") {
                if (autosave == "on") {
                    HandleSave(saveFilePath, money, points, true);
                }
                HandleExit();
                string response;
                cout << "Confirm (y/n)?";
                cin >> response;
                if (response == "y" || response == "Y") {
                    exit(0);
                }
            }
            else if (command == "/clear")
            {
#ifdef _WIN32
                system("cls");
#else
                system("clear");
#endif
                HandleSupport();
                LoadSettings(autoload, autosave, settingsFilePath);
                SetFontSize(fontSize);
                if (autoload == "on") {
                    HandleLoad(saveFilePath, money, points);
                }
                DisplayCommands();
            }
            else if (command == "/points") HandlePoints(points);
            else if (command == "/money") HandleMoney(money);

            else if (command == "/reaper")
            {
                if (secretonefound == false)
                {
                    cout << "Guh, I guess you have found me\nI will give you 1000 money, i paid the taxes for you btw and np.\n";
                    money += 1000;
                    secretonefound = true;
                    AchievementReachedCheck(money, points);
                }
                else {
                    cout << "Bro STOP BEGGING ME FOR MONEY PLEASE BRUH...\n";
                    begging = true;
                    AchievementReachedCheck(money, points);
                }
            }
            else if (command == "/kys")
            {
                if (secrettwofound == false)
                {
                    cout << "Did you just tell me to keep myself safe??? I am stealing your money\n";
                    double moneystolen = money / 3;
                    moneystolen = ceil(moneystolen);
                    money -= static_cast<int>(moneystolen);
                    cout << "reaper just stole " << moneystolen << " money from you\n";
                    secrettwofound = true;
                    AchievementReachedCheck(money, points);
                }
                else {
                    cout << "Odd specimen\n";
                }
            }
            else if (command == "/therat")
            {
                string input;
                TypeEffect("MRATI: LET ME TELL YOU A STORY...\n", 70);
                cout << "(y/n)?";
                cin >> input;
                if (input == "y" || input == "Y")
                {
                    TypeEffect("MRATI: Why thank you actually! :D\n", 70);
                    goto Story;
                }
                else if (input == "n" || input == "N")
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
                cout << "(y/n)?";
                string input2;
                cin >> input2;
                if (input2 == "y" || input2 == "Y")
                {
                    TypeEffect("MRATI: Thank you I am proud to be your guest :D\n", 70);
                }
                else if (input2 == "n" || input2 == "N")
                {
                    TypeEffect("MRATI: KYS\n", 70);
                    TypeEffect("......... Boss encounter!!!\n", 70);
                    Sleep(250);
                    Mrati();
                }
                else
                {
                    TypeEffect("MRATI: Learn how to respond you!!!", 70);
                }
            }
            else {
                    cout << "Invalid command. Type /help for available commands.\n";
            }

            if (autosave == "on") {
                HandleSave(saveFilePath, money, points, false);
            }
        }
}
int main() {
    srand(time(0)); //cant believe i accidently put this in a while(true) loop before lol
    Game();
}











//BOSS!!!

void Mrati() {
    MP3Silently("C:\\ReaperGame\\Assets\\mrati.mp3", true);

    int ratiHP = 70;
    int playerHP = 50;
    int playerPotions = 5;
    int money = 0;

        TypeEffect("MRATI: YOUR OPINION DOESNT MATTER TO A MAN LIKE ME!@#$%\n", 40);
        Sleep(800);
        TypeEffect("MRATI: I'll make you pay...\n", 40);
        Sleep(2200);

        while (ratiHP > 0 && playerHP > 0) {
#ifdef _WIN32
            system("cls");
#else
            system("clear");
#endif

            cout << "PLAYERS TURN!!!\n\n";
            cout << "RATI Health: " + to_string(ratiHP) + "\n";
            cout << "Player Health: " + to_string(playerHP) + "\n\n";

            cout << "Attack Options:\n";
            cout << "Emotional Damage\n";
            cout << "Tax Forms\n";
            cout << "Child Support\n";
            if (playerPotions > 0) {
                cout << "Heal (" << playerPotions << " potions left)\n";
            }
            cout << "\n";

            string input;
            getline(cin, input);
            int damageToRat = rand() % 11 + 1;
            int chance = rand() % 101 + 1;

            if (input == "Emotional Damage" || input == "emotional damage") {
                TypeEffect("Rolling Dice...\n", 60);
                Sleep(400);
                TypeEffect("Dice came out as... " + to_string(damageToRat) + " damage!\n", 60);

                if (chance <= 10) {
                    TypeEffect("BUT WAIT... You have missed and caused no emotional damage...\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                }
                else {
                    ratiHP -= damageToRat;
                    TypeEffect(to_string(damageToRat) + " emotional damage done to rati... \n", 60);
                    TypeEffect("Rati has: " + to_string(ratiHP) + " HP left. Cry... \n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Sleep(2000);
                }
            }
            else if (input == "Tax Forms" || input == "tax forms") {
                TypeEffect("Rolling Dice...\n", 60);
                Sleep(400);
                TypeEffect("Dice came out as... " + to_string(damageToRat) + " damage!\n", 60);

                if (chance <= 10) {
                    TypeEffect("BUT WAIT... You have missed...\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                }
                else {
                    ratiHP -= damageToRat;
                    TypeEffect(to_string(damageToRat) + " Tax Forms given to rati... \n", 60);
                    TypeEffect("Rati has: " + to_string(ratiHP) + " HP left. I HATE TAXES!!! \n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Sleep(2000);
                }
            }
            else if (input == "Child Support" || input == "child support") {
                TypeEffect("Rolling Dice...\n", 60);
                Sleep(400);
                TypeEffect("Dice came out as... " + to_string(damageToRat) + " damage!\n", 60);

                if (chance <= 10) {
                    TypeEffect("BUT WAIT... Rati doesnt have a child currently...\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                }
                else {
                    ratiHP -= damageToRat;
                    TypeEffect(to_string(damageToRat) + " Child Support money taken from rati... \n", 60);
                    TypeEffect("Rati has: " + to_string(ratiHP) + " HP left. I HATE PAYING CHILD SUPPORT!!! \n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Sleep(2000);
                }
            }
            if (input == "Attack Options" || input == "attack options")
            {
                const string rainbowColors[] = {
                    "\033[31m", //Red
                    "\033[32m", //Green
                    "\033[33m", //Yellow
                    "\033[34m", //Blue
                    "\033[35m"  //Magenta
                };

                int colorIndex = 0;
                auto CycleColor = [&]() {
                    cout << rainbowColors[colorIndex];
                    colorIndex = (colorIndex + 1) % 5;
                    };

                CycleColor();
                TypeEffect("Rolling Special Dice...\n", 60);
                this_thread::sleep_for(chrono::milliseconds(400));

                CycleColor();
                TypeEffect("Special Dice came out as... " + to_string(damageToRat * 2) + " damage!\n", 60);

                if (chance <= 10)
                {
                    CycleColor();
                    TypeEffect("BUT WAIT... Rati is sigma and you missed...\n", 60);

                    CycleColor();
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                }
                else
                {
                    ratiHP -= damageToRat *= 2;

                    CycleColor();
                    TypeEffect(to_string(damageToRat) + " damage done to rati...\n", 60);

                    CycleColor();
                    TypeEffect("Rati has: " + to_string(ratiHP) + " HP left.\n", 60);

                    CycleColor();
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    this_thread::sleep_for(chrono::milliseconds(2000));
                }

                cout << "\033[0m"; //reset
            }
            else if (input == "Heal" || input == "heal") {
                if (playerPotions > 0) {
                    playerHP += 20;
                    playerPotions--;
                    TypeEffect("You used a healing potion! +" + to_string(20) + " HP.\n", 60);
                    TypeEffect("You have " + to_string(playerPotions) + " healing potions left.\n", 60);
                    TypeEffect("RATI'S TURN!!!\n\n", 30);
                    Sleep(2000);
                }
                else {
                    TypeEffect("No potions left! Try a different option...\n", 60);
                    continue;
                }
            }
            else {
                    TypeEffect("Invalid option! Try again.\n", 60);
                continue;
            }

            //Rati's turn
            if (ratiHP > 0) {
#ifdef _WIN32
                system("cls");
#else
                system("clear");
#endif
                cout << "RATIS TURN!!!\n\n";
                cout << "RATI Health: " + to_string(ratiHP) + "\n";
                cout << "Player Health: " + to_string(playerHP) + "\n\n";

                int ratiDamage = rand() % 11 + 1;
                playerHP -= ratiDamage;
                TypeEffect("Rati dealt " + to_string(ratiDamage) + " damage to you.\n", 60);
                TypeEffect("You have: " + to_string(playerHP) + " HP left.\n", 60);
                TypeEffect("PLAYER'S TURN!!!\n\n", 30);
                Sleep(2000);
            }
        }

        if (ratiHP <= 0) {
            TypeEffect("Rati has been defeated! You win!\n", 60);
            TypeEffect("You won 5,000 money\n", 60);
            money += 5000;
            beatRati = true;
            AchievementReachedCheck(money, points);
            Sleep(1000);
        }
        else if (playerHP <= 0) {
            TypeEffect("You have been defeated by Rati... Game Over!\n", 60);
            Sleep(1000);
        }
    }