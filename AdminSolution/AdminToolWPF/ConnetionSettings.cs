﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminToolWPF
{
    public class ConnetionSettings
    {
        public static string Neo4jAddress { get; set; } = "bolt://mint-green-block-flat-heloise.graphstory.me:7687";
        public static string Neo4jUserName { get; set; } = "mint_green_block_flat_heloise";
        public static string Neo4jUserPassword { get; set; } = "1vF5dh85lj7QkhIsJ8TaYUK";


        public static string PostgresMoviesAddress { get; set; } = "PostgresMoviesAddress";
        public static string PostgresMoviesUserName { get; set; } = "PostgresMoviesUserName";
        public static string PostgresMoviesUserPassword { get; set; } = "PostgresMoviesUserPassword";

        public static string PostgresUserAddress { get; set; } = "PostgresUserAddress";
        public static string PostgresUserUserName { get; set; } = "PostgresUserUserName";
        public static string PostgresUserUserPassword { get; set; } = "PostgresUserUserPassword";
    }
}
