using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtchlanMapGenerator
{
    public partial class SearchLocationUserControl : UserControl
    {
        String SearchedText;
        Boolean flag_ButtonClicked=false;
        public SearchLocationUserControl()
        {
            InitializeComponent();
            setTexts();
        }

        private void setTexts()
        {
            this.Text = Texts.text_SearchFormName; 
            SearchButton.Text = Texts.text_SearchControlButton;
            SearchControlLabel.Text = Texts.text_SearchControlLabel;
        }

        public String getSearchedText()
        {
            if(SearchedText != null)
            return this.SearchedText;
            return "";
        }

        public Boolean WasButtonClicked() 
        {
            return flag_ButtonClicked;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchedText = this.textBox1.Text;
            flag_ButtonClicked = true;

            ((Form)this.TopLevelControl).Close(); //closes associated Form
        }

        private void SearchLocationUserControl_BackColorChanged(object sender, EventArgs e)
        {
            if ((this.BackColor.R * 0.299) + (this.BackColor.G * 0.587) + (this.BackColor.B * 0.114) < 64)
            {
                SearchControlLabel.ForeColor = Color.White;
            }
            else
            {
                SearchControlLabel.ForeColor = Color.Black;
            }
        }

        private void SearchLocationUserControl_Load(object sender, EventArgs e)
        {

        }
    }
}
