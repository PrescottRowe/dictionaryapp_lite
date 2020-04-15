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
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

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
            //These write lines are used to debug at runtime when i want to quickly find chunks of code or Json
            Console.WriteLine("--------------------------START----------------------");
        }

        public static class WordItemModel //JSON model. Used "https://quicktype.io/" to generate .net object from API response.
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
        public static class GlobalVariables //only had one global for entry so stuck it here rather than app for readability
        {
            public static string searchWord { get; set; }
        }
        //This function triggers on the text entry field. Hitting enter or search will generate the event.
        async void OnButtonClicked(object sender, System.EventArgs e) 
        {
            //clear is needed incase a word does not return all objects to overwrite.
            Word.Text = null;
            Pronunciation.Text = null;
            Type.Text = null;
            DefinitionDefinition.Text = null;
            Example.Text = null;
            ImageUrl.Source = null;
            Emoji.Text = null;
            ConNet.Text = null;
            OnCheck();
            if (ConNet.Text != "No Connection")
            {
                //http client is used to generate the requests made to the owlbot dictionary rest api.
                HttpClient client = new HttpClient();
                var uri = new Uri(
                    string.Format(//the format to call to the IP is the link below with the word wanted appeneded to the uri
                        $"https://owlbot.info/api/v4/dictionary/"
                        + searchWord.Text));//this is the user entered word
                var request = new HttpRequestMessage();//generating the request
                //we also have to supply our api token in the header as the authorization token
                request.Headers.Authorization = new AuthenticationHeaderValue("Token", "42cd430cf80c99d27a5a628d4ff31aa70943ac57");
                request.Method = HttpMethod.Get; //get data
                request.RequestUri = uri; //add the uri we built earlier

                HttpResponseMessage response = await client.SendAsync(request);//send the request and store the JSON response
                WordItemModel.WordItem wordData = null; //initialize the data var that is about to be written to
                Console.WriteLine("--------------------------Start of API return 2 .net----------------------");
                if (response.IsSuccessStatusCode)//if 200
                {
                    BackgroundImageSource = "WP4.jpg";//default background (changes if error)
                    var content = await response.Content.ReadAsStringAsync();//gets json
                    Console.WriteLine("--------------------------3 json----------------------");
                    Console.WriteLine(content);//print json to console for debugging and development
                    wordData = JsonConvert.DeserializeObject<WordItemModel.WordItem>(content);//converts to .net object
                    //start writing data to xaml bindings
                    Word.Text = wordData.Word;
                    Pronunciation.Text = wordData.Pronunciation;
                    Type.Text = wordData.Definitions[0].Type;
                    DefinitionDefinition.Text = wordData.Definitions[0].DefinitionDefinition;
                    //only one that has pre appended text and needs an IF to stop from printing "Example:" when null.
                    if (wordData.Definitions[0].Example != null) { Example.Text = "Example: " + wordData.Definitions[0].Example; }
                    ImageUrl.Source = (string)wordData.Definitions[0].ImageUrl;
                    Emoji.Text = (string)wordData.Definitions[0].Emoji;


                }
                else //not 200 then show word not found
                {
                    BackgroundImageSource = "NF.jpg";
                    Word.Text = "Not Found";//uses word as container label for non 200s
                }
            }
        }
        protected override void OnAppearing()
        {
            OnCheck();
        }
        public void OnCheck()
        {
            try
            {
                ConNet.Text = CrossConnectivity.Current.ConnectionTypes.First().ToString();
                CrossConnectivity.Current.ConnectivityChanged += UpdateNetworkInfo;
            }
            catch (Exception e)
            {
                ConNet.Text = "No Connection";
            }
        }
        public void UpdateNetworkInfo(object sender, ConnectivityChangedEventArgs e)
        {
            var connectionType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();
            ConNet.Text = connectionType.ToString();
        }
    }

}
