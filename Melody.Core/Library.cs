using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

using Melody.Error;

namespace Melody.Core {
	/// <summary>
	/// Contains references to all the media currently managed by the software,
	/// as well as any important settings needed for the software to run. This
	/// data is stored in SQLite format.
	/// </summary>
	public class Library {
		private const string DEF_LIBPATH = Path.Combine(Environment.SpecialFolder.ApplicationData, "Melody""Melody.db");
		internal const string LIBFOLDER = "libfolder";
		internal const string UPDATEMETA = "updatemeta";
		private SQLiteConnection db;

		/// <summary>
		/// Access information and utility functions about library folders.
		/// </summary>
		/// <value>The LibraryFolders instance</value>
		public LibraryFolders LibFolders { get; private set; }
			
		public Library() {
			if(!File.Exists(LIBPATH))
				createDB();
			db = new SQLiteConnection(string.Format("Data Source={0};Version=3", LIBPATH));
			db.Open();
			LibFolders = new LibraryFolders(db);
		}

		/// <summary>
		/// Fetches a song from the library using the song path.
		/// </summary>
		/// <returns>A song library entry</returns>
		/// <param name="path">The song path relative to the library folder.</param>
		public Song getSong(string uri){
			SQLiteDataReader r = 
				new SQLiteCommand("SELECT * FROM songs WHERE FilePath = " + uri, db).
				ExecuteReader();
			if(!r.HasRows)
				return null;
			r.Read();
			Song ret = new Song();
			ret.Album = (string)r["Album"];
			ret.AlbumArt = (IPicture)r["AlbumArt"];
			ret.Artist = ((string)r["Artist"]);
			ret.BPM = (uint)r["BPM"];
			ret.Broken = (bool)r["Broken"];
			ret.Composer = (string)r["Composer"]);
			ret.Conductor = (string)r["Conductor"];
			ret.Copyright = (string)r["Copyright"];
			ret.Disc = (uint)r["Disc"];
			ret.DiscCount = (uint)r["DiscCount"];
			ret.Favorite = (bool)r["Favorite"];
			ret.FilePath = (string)r["FilePath"];
			ret.Genre = (string)r["Genre"];
			ret.Heard = (bool)r["Heard"];
			ret.Lyrics = (string)r["Lyrics"];
			ret.Title = (string)r["Title"];
			ret.Track = (uint)r["Track"];
			ret.TrackCount = (uint)r["TrackCount"];
			ret.Year = (uint)r["Year"];
			r.Close();
			return ret; 
		}

		/// <summary>
		/// Add a song to the library
		/// </summary>
		/// <param name="song">The song to add</param>
		public void addSong(Song song){
			foreach(Song s in GetSongEnumerator())
				if(s.FilePath == song.FilePath) throw new LibraryException("The song is already in the library. Please update it instead.");
			new SQLiteCommand("INSERT INTO songs " +
				"(FilePath, Title, Artist, Album, AlbumArt, Year, Genre, Track, TrackCount, Disc, DiscCount, Lyrics, Copyright, Composer, Conductor, BPM, Heard, Favorite")

		}


		/// <summary>
		/// Gets the song enumerator.
		/// </summary>
		/// <returns>The song enumerator.</returns>
		public IEnumerable<Song> GetSongEnumerator(){
			SQLiteDataReader r = new SQLiteCommand("SELECT * FROM songs").ExecuteReader();
			while(r.NextResult())
				yield return fetchSong((string)r["FilePath"]);
		}

		// Create database and initialize to default values
		private void createDB(){
			SQLiteConnection.CreateFile(LIBPATH);
			SQLiteConnection sql = new SQLiteConnection(string.Format("Data Source={0};Version=3", LIBPATH));
			sql.Open();
			new SQLiteCommand("CREATE TABLE songs (" +
			"FilePath TEXT, " +
			"Title TEXT, " +
			"Artist TEXT, " +
			"Album TEXT, " +
			"AlbumArt BLOB, " +
			"Year INTEGER, " +
			"Genre TEXT, " +
			"Track INT, " +
			"TrackCount INT, " +
			"Disc INT, " +
			"DiscCount INT, " +
			"Lyrics TEXT, " +
			"Copyright TEXT, " +
			"Composer TEXT, " +
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

			new SQLiteCommand("CREATE TABLE settings (Key TEXT, Value TEXT)", sql)
				.ExecuteNonQuery();
			new SQLiteCommand("INSERT INTO settings " +
			"(Key, Value) VALUES ('" + LIBFOLDER + "', '" + Environment.SpecialFolder.MyMusic + "')", sql)
				.ExecuteNonQuery();
			new SQLiteCommand("Insert INTO settings " +
			"(Key, Value) VALUES ('" + UPDATEMETA + "', TRUE")
				.ExecuteNonQuery();
		}

		~Library() {
			db.Close();
		}
	}
}
