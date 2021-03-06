﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.DataContracts
{
    [DataContract]
    public class Game
    {
        [DataMember]
        public string id;

        [DataMember]
        public int turn;

        [DataMember]
        public int maxTurns;

        [DataMember]
        public List<Hero> heroes;

        [DataMember]
        public Board board;

        [DataMember]
        public bool finished;
    }
}
