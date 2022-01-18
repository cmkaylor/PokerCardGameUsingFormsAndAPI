using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerCardGameAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GenerateCards();
        }

        private string deckID;
        private List<Card> theCards = new List<Card>();
        private int TurnNumber = 3;
        private void button1_Click(object sender, EventArgs e)
        {
            ButtonClick();
        }

        private void ButtonClick()
        {
            if (TurnNumber < 7)
            {
                foreach (Control control in this.Controls)
                {
                    if ((string)control.Tag == TurnNumber.ToString())
                    {
                        control.BackgroundImage = CardImage(theCards[TurnNumber].image);
                    }
                }

                TurnNumber++;
            }

            else
            {
                foreach (Control control in this.Controls)
                {
                    if ((string)control.Tag == "3" || (string)control.Tag == "4" || (string)control.Tag == "5" || (string)control.Tag == "6")
                    {
                        control.BackgroundImage = null;
                        control.BackColor = Color.Green;
                    }
                }
                TurnNumber = 3;
                GenerateCards();
            }
        }
        private void GenerateCards()
        {
            string s = GetJson("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");
            GenRoot deserializedGenDeck = JsonConvert.DeserializeObject<GenRoot>(s);

            deckID = deserializedGenDeck.deck_id;

            string s2 = GetJson($"https://deckofcardsapi.com/api/deck/{deckID}/draw/?count=7");
            Root deserializedDeck = JsonConvert.DeserializeObject<Root>(s2);
            theCards = deserializedDeck.cards;

            playerCardOne.Image = CardImage(theCards[0].image);
            playerCardTwo.Image = CardImage(theCards[1].image);
            stack1.Image = CardImage(theCards[2].image);
        }

        private Image CardImage(string url)
        {
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(url);
            MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

            return img;
        }

        private string GetJson(string url)
        {
            string returnable = "";
            using (WebClient client = new WebClient())
            {
                returnable = client.DownloadString(url);
            }

            return returnable;
        }
    }
}
