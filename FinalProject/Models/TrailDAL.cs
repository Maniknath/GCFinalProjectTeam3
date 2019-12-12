﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace FinalProject.Models
{
    public class TrailDAL
    {


        public static LatLng GetLatLng(string location)
        {
            //this method uses the MapQuest API to convert a string location like "Grand Rapids, MI" to Latitude and Longitude for the Hiking API
            // this method has been tested and works -Sam <3
            string apiKey1 = Secret.APIkey1;

            HttpWebRequest request = WebRequest.CreateHttp($"http://www.mapquestapi.com/geocoding/v1/address?key={apiKey1}&location={location}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APItext = rd.ReadToEnd();
            JToken t = JToken.Parse(APItext);
            LatLng l = new LatLng(t);
            return l;
        }
        public static List<Trails> GetResults(string location)
        {
            // this method runs the Hiking API and returns a list of Trails

            string apiKey2 = Secret.APIkey2;

            LatLng l = GetLatLng(location);
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.hikingproject.com/data/get-trails?lat={l.Lat}&lon={l.Lng}&maxDistance=200&maxResult=100&key={apiKey2}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APItext = rd.ReadToEnd();
            JToken t = JToken.Parse(APItext);

            List<Trails> Result = new List<Trails>();
            List<JToken> x = t["trails"].ToList();
            foreach (JToken y in x)
            {
                Trails z = new Trails(y);
                Result.Add(z);
            }
            return Result;
        }
        // Gonna overload GetResults
        public static List<Trails> GetResults(string location, string difficulty)
        {
            string apiKey2 = Secret.APIkey2;
            LatLng l = GetLatLng(location);
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.hikingproject.com/data/get-trails?lat={l.Lat}&lon={l.Lng}&maxDistance=200&maxResult=100&key={apiKey2}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APItext = rd.ReadToEnd();
            JToken t = JToken.Parse(APItext);

            List<Trails> Result = new List<Trails>();
            List<JToken> x = t["trails"].ToList();
            foreach (JToken y in x)
            {
                if (y["difficulty"].ToString() == difficulty)
                {
                    Trails z = new Trails(y);
                    Result.Add(z);
                }
            }
            return Result;
        }
        //Created this method below to call for TrailsDetail -Sammyboy <3
        public static Trails GetTrailById(int Id)
        {
            string apiKey2 = Secret.APIkey2;
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.hikingproject.com/data/get-trails-by-id?ids={Id}&key={apiKey2}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APItext = rd.ReadToEnd();
            JToken t = JToken.Parse(APItext);

            //sorry, I understand this line is weird. Even though this is a diferent API URL, it still returns a list with one object in it.
            //I have to grab the first object of that list to get the Trails model to convert the properties propperly
            List<JToken> x = t["trails"].ToList();
            Trails myTrail = new Trails(x[0]);

            return myTrail;
        }
    }
}
