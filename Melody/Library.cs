using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Melody.Core {
    public class Library {
        private const string FNAME = "Melody.sqlite";
        private SQLiteConnection db;

        private static string ResolveFilePath() {
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                return Environment.GetEnvironmentVariable("HOME") + @"/.config/Melody/" + FNAME;
            else return Environment.ExpandEnvironmentVariables("%APPDATA%") + @"\Melody\" + FNAME;
        }

        public Library() {
            if (!File.Exists(ResolveFilePath()))
                createDB();
            db = new SQLiteConnection(string.Format("Data Source={0};Version=3", ResolveFilePath()));
            db.Open();
        }

        public Song fetchSong(string path) {
            return Song.Get(new SQLiteCommand("SELECT * FROM songs WHERE FilePath = " + path));    
        }

        public IEnumerable<Song> GetSongEnumerator() {
            SQLiteDataReader r = new SQLiteCommand("SELECT * FROM songs").ExecuteReader();
            while (r.NextResult())
                yield return fetchSong((string)r["FilePath"]);
        }

        private void createDB() {
            SQLiteConnection.CreateFile(ResolveFilePath());
            SQLiteConnection sql = new SQLiteConnection(string.Format("Data Source={0};Version=3", ResolveFilePath()));
            sql.Open();
            new SQLiteCommand("CREATE TABLE songs (" +
                "FilePath TEXT, " +
                "Title TEXT, " +
                "Artists TEXT, " +
                "Album TEXT, " +
                "AlbumArt BLOB, " + 
                "Year INTEGER, " +
                "Genres TEXT, " +
                "Track INT, " + 
                "TrackCount INT, " +
                "Disc INT, " +
                "DiscCount INT, " +
                "Lyrics TEXT, " + 
                "Copyright TEXT, " +
                "Composers TEXT, " +
                "Conductor TEXT, " + 
                "BPM TEXT, " + 
                "Heard BOOLEAN, " + 
                "Favorite BOOLEAN)", 
                sql)
                .ExecuteNonQuery();

            new SQLiteCommand("CREATE TABLE stations (" +
                "Name TEXT, " +
                "URL TEXT, " +
                "Genre TEXT, " +
                "Creator TEXT, " +
                "Description TEXT)",
                sql)
                .ExecuteNonQuery();
        }

        ~Library() {
            db.Close();
        }
    }
}
