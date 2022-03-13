

namespace OtchlanMapGenerator
{
    enum Language {PL,EN};
    static class Texts //Make static
    {
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
        public static string text_FormName;
        public static string text_descriptionTextBox;

        public static string msg_StartLocation;
        public static string msg_DefaultName;
        public static string msg_LanguageChange;
        public static string msg_OnExit;
        public static string msg_ReadError;
        public static string msg_GameProcessNotFound;

        public static void setLanguage(Language language)
        {
            if(language == Language.EN)
            {
                text_FormName = "Otchłań Map Generator";

                text_detailButton = "Show Details";
                text_segmentPanel = "Segment Panel";
                text_textboxName = "Location Name";
                text_exitsLabel = "Exits";
                text_deleteButton = "Delete";
                text_infoLabel = "temp Segment info";
                text_languageGroupBox = "Select language";
                text_descriptionTextBox = "Description";

                msg_GameProcessNotFound = "Game process not detected!";
                msg_StartLocation = "Start Location";
                msg_DefaultName = "Default Name";
                msg_ReadError = "Error: Automatic read failed. Correct location info on your own, using \"" + text_detailButton + "\""; 

            }
            if(language == Language.PL)
            {
                text_FormName = "Otchłań - Generator mapy";

                text_detailButton = "Szczegóły";
                text_segmentPanel = "Informacje o Segmencie";
                text_textboxName = "Nazwa lokacji";
                text_exitsLabel = "Wyjścia";
                text_deleteButton = "Usuń";
                text_infoLabel = "temp Segment info";
                text_languageGroupBox = "Wybierz język";
                text_descriptionTextBox = "Opis lokacji";

                msg_GameProcessNotFound = "Nie wykryto procesu gry";
                msg_StartLocation = "Lokacja startowa";
                msg_DefaultName = "Nazwa domyślna";
                msg_ReadError = "Error: Niepowodzenie automatycznego odczytu. Wprowadź informcje o lokacji samodzielnie, używając przycisku \"" + text_detailButton + "\"";
            }
        }
    }
}
