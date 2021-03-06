

namespace OtchlanMapGenerator
{
    enum Language {PL,EN};
    static class Texts //Make static
    {
        public static string mapFileExtentionPattern;
        public static string mapFileExtention = ".omg";

        public static string text_StartLocation;
        public static string text_DefaultName;
        public static string text_detailButton;
        public static string text_segmentPanel;
        public static string text_textboxName;
        public static string text_button_set_w = "w";
        public static string text_button_set_s = "s";
        public static string text_button_set_e = "e";
        public static string text_button_set_n = "n";
        public static string text_exitsLabel;
        public static string text_deleteButton;
        public static string text_infoLabel;
        public static string text_languageGroupBox;
        public static string text_MainFormName;
        public static string text_descriptionTextBox;
        public static string text_disableKeyInput;
        public static string text_enableKeyInput;
        public static string text_floorTextBox;

        public static string text_SearchFormName;
        public static string text_SearchControlLabel;
        public static string text_SearchControlButton;

        public static string text_menuMap;
        public static string text_menuViev;
        public static string text_menuHelp;
        public static string text_menuMapOpenFile;
        public static string text_menuMapNewFile;
        public static string text_menuMapSaveFile;
        public static string text_menuMapSaveFileAs;
        public static string text_menuSearch;
        public static string text_menuVievColors;
        public static string text_menuVievColorsMainColor;
        public static string text_menuVievColorsPanelColor;
        public static string text_menuVievColorsBrownTheme;
        public static string text_menuVievColorsBlackTheme;
        public static string text_menuHelpUsage;

        //public static string msg_LanguageChange;
        //public static string msg_OnExit;
        public static string msg_MessageBoxTitle;
        public static string msg_LocationNotFound;
        public static string msg_ReadError;
        public static string msg_FileReadError;
        public static string msg_FileSaveError;
        public static string msg_GameProcessNotFound;

        public static void setLanguage(Language language)
        {
            if(language == Language.EN)
            {
                text_MainFormName = "Otchłań Map Generator";

                text_detailButton = "Show Details";
                text_segmentPanel = "Segment Panel";
                text_textboxName = "Location Name";
                text_exitsLabel = "EXITS";
                text_deleteButton = "Delete";
                text_infoLabel = "temp Segment info";
                text_languageGroupBox = "Select language";
                text_descriptionTextBox = "Description";
                text_enableKeyInput = "Enable Key Input";
                text_disableKeyInput = "Disable Key Input";
                text_floorTextBox = "FLOOR: ";
                text_StartLocation = "Start Location";
                text_DefaultName = "Default Name";
                text_SearchFormName = "Search Location";

                msg_MessageBoxTitle = text_MainFormName + " Info";
                msg_LocationNotFound = "There is no location containing the specified string";
                msg_GameProcessNotFound = "Game process not detected!";
                msg_ReadError = "Error: Automatic read failed. Correct location info on your own, using \"" + text_detailButton + "\"";
                msg_FileReadError = "Error: Unknown problem reading file";
                msg_FileSaveError = "Error: Unknown problem saving file";

                text_menuMap = "Map";
                text_menuViev = "Viev";
                text_menuHelp = "Help";
                text_menuMapOpenFile = "Open";
                text_menuMapNewFile = "New";
                text_menuMapSaveFile = "Save";
                text_menuMapSaveFileAs = "Save As";

                text_menuSearch = "Search";
                text_SearchControlButton = "Search";
                text_SearchControlLabel = "Enter string or substring from name/description";


                text_menuVievColors = "Colors";
                text_menuVievColorsMainColor = "Choose Main Color";
                text_menuVievColorsPanelColor = "Choose Panel Color";
                text_menuHelpUsage = "Usage";
                text_menuVievColorsBrownTheme = "Brown Theme";
                text_menuVievColorsBlackTheme = "Black-Green Theme";

                mapFileExtentionPattern = "(Map files (*.omg))|*.omg";

            }
            if(language == Language.PL)
            {
                text_MainFormName = "Otchłań - Generator mapy";

                text_detailButton = "Szczegóły";
                text_segmentPanel = "Informacje o Segmencie";
                text_textboxName = "Nazwa lokacji";
                text_exitsLabel = "WYJŚCIA";
                text_deleteButton = "Usuń";
                text_infoLabel = "temp Segment info";
                text_languageGroupBox = "Wybierz język";
                text_descriptionTextBox = "Opis lokacji";
                text_enableKeyInput = "Załącz odczyt klawiszy";
                text_disableKeyInput = "Wyłącz odczyt klawiszy";
                text_floorTextBox = "PIĘTRO: ";
                text_StartLocation = "Lokacja startowa";
                text_DefaultName = "Nazwa domyślna";
                
                text_SearchFormName = "Wyszukaj lokalizację";
                text_SearchControlButton = "Wyszukaj";
                text_SearchControlLabel = "Podaj całość lub fragment nazwy/opisu lokalizacji";

                msg_MessageBoxTitle = text_MainFormName + " Info";
                msg_LocationNotFound = "Nie znaleziono lokalizacji zawierającej podany ciąg znaków";
                msg_GameProcessNotFound = "Nie wykryto procesu gry";
                msg_ReadError = "Error: Niepowodzenie automatycznego odczytu. Wprowadź informcje o lokacji samodzielnie, używając przycisku \"" + text_detailButton + "\"";
                msg_FileReadError = "Error: Wczytanie pliku zapisu nie powiodło się";
                msg_FileSaveError = "Error: Zapisanie pliku mapy nie powiodło się";

                text_menuMap = "Mapa";
                text_menuViev = "Widok";
                text_menuHelp = "Pomoc";
                text_menuMapOpenFile = "Otwórz";
                text_menuMapNewFile = "Nowa mapa";
                text_menuMapSaveFile = "Zapisz mapę";
                text_menuMapSaveFileAs = "Zapisz jako";
                text_menuSearch = "Szukaj";
                text_menuVievColors = "Motyw";
                text_menuVievColorsMainColor = "Wybierz Kolor Główny";
                text_menuVievColorsPanelColor = "Wybierz Kolor Panelu";
                text_menuVievColorsBrownTheme = "Motyw Brązowy";
                text_menuVievColorsBlackTheme = "Motyw Czarno-Zielony";
                text_menuHelpUsage = "Instrukcja";

                mapFileExtentionPattern = "(Pliki mapy (*.omg))|*.omg";
            }
        }
    }
}
