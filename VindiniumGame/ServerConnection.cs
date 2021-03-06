﻿using Common.Helpers;
using Common.Messaging.Messages;
using System;
using System.IO;
using System.Net;

namespace VindiniumGame
{
    public class ServerConnection
    {
        private readonly Log4netManager _logger;

        public string PrivateKey { get; private set; }
        public int NumberOfTurns { get; set; }
        public string ServerURL { get; private set; }
        public bool TrainingMode { get; set; }
        public EventHandler<string> DataReceived = delegate { };
        public EventHandler<string> ErrorOccurred = delegate { };

        public ServerConnection(StartNewGameMessage message)
        {
            PrivateKey = message.PrivateKey;
            NumberOfTurns = message.NumberOfTurns;
            TrainingMode = message.TrainingMode;

            if (TrainingMode)
                ServerURL = message.ServerURL + "/api/training";
            else
                ServerURL = message.ServerURL + "/api/arena";
        }

        public void BeginNewGame()
        {
            string myParameters = "key=" + PrivateKey;
            if (TrainingMode)
                myParameters += "&turns=" + NumberOfTurns;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                try
                {
                    string result = client.UploadString(ServerURL, myParameters);
                    DataReceived(this, result);
                }
                catch (WebException exception)
                {
                    using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                    {
                        ErrorOccurred(this, reader.ReadToEnd());
                    }
                }
            }
        }

        public void SendCommand(string playUrl, string direction)
        {
            _logger.DebugFormat(string.Format("Sending {0} command to {1}.", direction, playUrl), typeof(ServerConnection));

            string myParameters = "key=" + PrivateKey + "&dir=" + direction;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    string result = client.UploadString(playUrl, myParameters);
                    DataReceived(this, result);
                }
                catch (WebException exception)
                {
                    using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                    {
                        ErrorOccurred(this, reader.ReadToEnd());
                    }
                }
            }
        }
    }
}
