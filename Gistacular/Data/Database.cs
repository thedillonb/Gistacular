using System.IO;
using SQLite;
using MonoTouch;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Gistacular.Data
{
    public class Database : SQLiteConnection
    {
        //The current database version
        private static readonly string Path = Utilities.BaseDir + "/Documents/data.db";
        private static Database _database;

        public static Database Main
        {
            get 
            {
                if (_database == null)
                    _database = new Database();
                return _database;
            }
        }

        private Database() : base(Path)
        {
            CreateTable<Gistacular.Data.Account>();
        }
    }
}

