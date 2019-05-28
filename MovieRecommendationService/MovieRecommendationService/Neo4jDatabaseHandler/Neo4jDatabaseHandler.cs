﻿using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendationLibrary.Neo4jDatabaseHandler
{
    public partial class Neo4jDatabaseHandler
    {

        private string _address = "";
        private string _user = "";
        private string _password = "";

        public string Address { get { return _address; } set { _address = value; } }
        public string User { get { return _user; } set { _user = value; } }
        public string Password { get { return _password; } set { _password = value; } }


        public Neo4jDatabaseHandler()
        {

        }

        public Neo4jDatabaseHandler(string address, string user, string password)
        {
            _address = address;
            _user = user;
            _password = password;
        }

        public void SetConnectionParameters(string address, string user, string password)
        {
            _address = address;
            _user = user;
            _password = password;
        }

        private GraphClient CreateClient()
        {
            return new GraphClient(new Uri(_address), _user, _password);
        }

        public bool CheckConnection()
        {
            bool connected = false;

            try
            {
                using (var graphClient = CreateClient())
                {
                    // Will throw an exception if it fails to connect
                    graphClient.Connect();

                    connected = graphClient.IsConnected; 
                    
                }
            }
            catch(Exception ex)
            {
            }
            return connected;
        }

        public void CreateIndexes()
        {
            try
            {

                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();
                    graphClient.Cypher
                            .Create("INDEX ON :Movie(movieId);")
                            .Create("INDEX ON :Links(movieId);")
                            .Create("INDEX ON :GenomeTag(tagId);")
                            .Create("INDEX ON :User(userId);")
                            .Create("INDEX ON :Genre(name);")
                            //.CreateUniqueConstraint("Movie", "movieId")
                            //.CreateUniqueConstraint("Links", "movieId")
                            //.CreateUniqueConstraint("GenomeTag", "tagId")
                            //.CreateUniqueConstraint("User", "userId")
                            //.CreateUniqueConstraint("Genre", "name")
                            .ExecuteWithoutResults();
                
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
