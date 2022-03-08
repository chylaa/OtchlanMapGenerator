using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtchlanMapGenerator
{
    enum Language {PL,EN};
    class Text
    {
        public string text_correctButton;
        public string text_segmentPanel;
        public string text_textboxName;
        public string text_button_set_w = "w";
        public string text_button_set_s = "s";
        public string text_button_set_e = "e";
        public string text_button_set_n = "n";
        public string text_exitsLabel;
        public string text_deleteButton;
        public string text_infoLabel;
        public string text_languageGroupBox;
        public string text_FormName;

        public string msg_StartLocation;
        public string msg_DefaultName;
        public string msg_LanguageChange;
        public string msg_OnExit;

        public Text(Language language)
        {
            setLanguage(language);
        }


        public void setLanguage(Language language)
        {
            if(language == Language.EN)
            {
                this.text_FormName = "Otchłań Map Generator";

                this.text_correctButton = "Correct";
                this.text_segmentPanel = "Segment Panel";
                this.text_textboxName = "Location Name";
                this.text_exitsLabel = "Exits";
                this.text_deleteButton = "Delete";
                this.text_infoLabel = "temp Segment info";
                this.text_languageGroupBox = "Select language";
                
                this.msg_StartLocation = "Start Location";
                this.msg_DefaultName = "Default Name";

            }
            if(language == Language.PL)
            {
                this.text_FormName = "Otchłań - Generator mapy";

                this.text_correctButton = "Popraw";
                this.text_segmentPanel = "Informacje o Segmencie";
                this.text_textboxName = "Nazwa lokacji";
                this.text_exitsLabel = "Wyjścia";
                this.text_deleteButton = "Usuń";
                this.text_infoLabel = "temp Segment info";
                this.text_languageGroupBox = "Wybierz język";

                this.msg_StartLocation = "Lokacja startowa";
                this.msg_DefaultName = "Nazwa domyślna";
            }
        }
    }
}
