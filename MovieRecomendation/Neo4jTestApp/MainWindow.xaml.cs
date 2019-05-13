using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Neo4j.Driver.V1;

using CypherNet.Configuration;
using CypherNet.Graph;
using CypherNet.Transaction;
using Neo4jClient;
using Newtonsoft.Json;
using MovieRecommendationLibrary.Model;

namespace Neo4jTestApp
{
    



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isConnection = false;
        public bool IsConnected
        {
            get { return _isConnection; }
            set {
                _isConnection = value;
            }
        } 



        public MainWindow()
        {
            InitializeComponent();
            
            //CheckConnection();


            try
            {
                var client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "password");
                client.Connect();

                var user = new User { UserId = 500 };
                client.Cypher
                    .Create("(user:User {user})")
                    .WithParams(new { user })
                    .ExecuteWithoutResults();

                //result = querryResult.LastOrDefault().UserId;
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// 
        /// https://github.com/microsoftgraph/msgraph-sdk-dotnet
        /// 
        /// https://github.com/Readify/Neo4jClient/wiki/cypher
        /// 
        /// https://github.com/Readify/Neo4jClient/wiki/GettingStartedWithCSharp
        /// 
        /// 
        /// </summary>


        private void CheckConnection()
        {
            long? result = AdminDatabaseHandler.CurrentNumberCount();

            IsConnected = result.HasValue;

            if (IsConnected)
                tbUserCount.Text = result.Value.ToString();
            else
            {
                tbUserCount.Text = "???";
                MessageBox.Show("Could not connect");
            }
        }

        private void GetUserOne()
        {
            

        }


        private void GetRecommendation(string movieTitle = "Toy Story (1995)")
        {

            var client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "password");
            client.Connect();

            var result3 = client.Cypher
                .Match("(m:Movie)-[:HAS_GENRE]-(t:Genre)<-[:HAS_GENRE]-(other:Movie)")
                .Where((Movie m) => m.Title == movieTitle)
                .With("m, other, COUNT(t) AS intersection, COLLECT(t.name) AS i")
                .Match("(m)-[:HAS_GENRE]-(mt)")
                .With("other, intersection,i, COLLECT(mt.name) AS genreList1")
                .Match("(other)-[:HAS_GENRE]-(ot)")
                .With("other,intersection,i, genreList1, COLLECT(ot.name) AS genreList2")
                .With("other,intersection, genreList1, genreList2, ((1.0*intersection)/SIZE(genreList1+filter(x IN genreList2 WHERE NOT x IN genreList1))) AS jaccard")
                .Where("jaccard > 0.7")
                .With(@"{   title: other.title, 
                        movieId: other.movieId,
                        averageRating: other.averageRating,
                        jaccard: jaccard 
                    } as movie")
                .Return<Movie>(movie => movie.As<Movie>())
                .OrderByDescending("movie.averageRating")
                .Limit(10)
                .Results;


        }











        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            CheckConnection();
        }

        private void LoadUserButton_Click(object sender, RoutedEventArgs e)
        {
            GetUserOne();
        }

        private void LoadMoviesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecommendationBasedOnMovieButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecommendationBasedOnUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetPeopleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateNumberOfUsersLogButton_Click(object sender, RoutedEventArgs e)
        {
            CreateUsers(false);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CreateAllUsers_Click(object sender, RoutedEventArgs e)
        {
            CreateUsers(true);
        }

        private void CreateUsers(bool createAllUsers)
        {
            string text = NumberTextBox.Text;
            bool parsed = int.TryParse(text, out int value);

            if (parsed)
            {
                string result = "";
                bool neo4j = false;
                bool postgres = false;

                neo4j = MessageBox.Show("Create dataset in neo4j?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

                postgres = MessageBox.Show("Create dataset in Postgresql?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

                if (neo4j || postgres) {
                    if(createAllUsers)
                        result = AdminDatabaseHandler.CreateDatabasesFromDataset(neo4j, postgres, null);
                    else
                        result = AdminDatabaseHandler.CreateDatabasesFromDataset(neo4j, postgres, value);
                }
                else
                    MessageBox.Show("did not create any dataset, because no was selected for both Neo4j and Postgresql", "Information", MessageBoxButton.OK, MessageBoxImage.Information);



                MessageBox.Show(result, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Could not create users, because the input field is empty");
            }
        }
    }
}
