using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Xamarin.Forms;

namespace hw6_dictionary
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]

    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            Console.WriteLine("--------------------------+++1----------------------");
            GetWord_Entry();
        }
        public static class WordItemModel
        {
            public partial class WordItem
            {
                [JsonProperty("definitions")]
                public Definition[] Definitions { get; set; }

                [JsonProperty("word")]
                public string Word { get; set; }

                [JsonProperty("pronunciation")]
                public string Pronunciation { get; set; }
            }
            public partial class Definition
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("definition")]
                public string DefinitionDefinition { get; set; }

                [JsonProperty("example")]
                public string Example { get; set; }

                [JsonProperty("image_url")]
                public object ImageUrl { get; set; }

                [JsonProperty("emoji")]
                public object Emoji { get; set; }
            }
        }
        async void GetWord_Entry()//object sender, System.EventArgs e
        {
            HttpClient client = new HttpClient();
            var uri = new Uri(
                string.Format(
                    $"https://owlbot.info/api/v4/dictionary/dictionary"));
            var request = new HttpRequestMessage();
            request.Headers.Authorization= new AuthenticationHeaderValue("Token", "42cd430cf80c99d27a5a628d4ff31aa70943ac57"); 
            request.Method = HttpMethod.Get;
            request.RequestUri = uri;

            HttpResponseMessage response = await client.SendAsync(request);
            WordItemModel.WordItem wordData = null;
            Console.WriteLine("--------------------------+++2----------------------");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("--------------------------+++3----------------------");
                Console.WriteLine(content);
                wordData = JsonConvert.DeserializeObject<WordItemModel.WordItem>(content);
                Console.WriteLine("--------------------------+++4----------------------");
                Console.WriteLine(wordData.Definitions[0].Emoji);


                Word.Text = wordData.Word;
                Pronunciation.Text = wordData.Pronunciation;
                Type.Text = wordData.Definitions[0].Type;
                DefinitionDefinition.Text = wordData.Definitions[0].DefinitionDefinition;
                Example.Text = wordData.Definitions[0].Example;
                ImageUrl.Source = (string)wordData.Definitions[0].ImageUrl;
                Emoji.Text = (string)wordData.Definitions[0].Emoji;
                

            }
        }
    }

}
