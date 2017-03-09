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
		private const string DEF_LIBPATH = Path.Combine(Environment.SpecialFolder.ApplicationData, "Melody.db");
		internal SQLiteConnection db;

		/// <summary>
		/// Gets the list of all library folders.
		/// </summary>
		/// <value>The library folders.</value>
		public List<string> LibraryFolders {
			get {
				List<string> ret = new List<>();
				SQLiteDataReader r = new SQLiteCommand("SELECT * FROM settings WHERE Key = 'libfolder'")
					.ExecuteReader();

				while(r.Read())
					ret.Add(r["Value"]);
			}
		}

		public void AddLibraryFolder(string path){
			if(!Directory.Exists(path))
				throw new DirectoryNotFoundException("The path specified does not exist.");
			new SQLiteCommand("INSERT INTO settings " +
			"(Key, Value) VALUES ('libfolder', '" + path + "')", db)
				.ExecuteNonQuery();
		}

		public Library() {
			if(!File.Exists(LIBPATH))
				createDB();
			db = new SQLiteConnection(string.Format("Data Source={0};Version=3", LIBPATH));
			db.Open();
		}

		/// <summary>
		/// Fetches a song from the library using the song path.
		/// </summary>
		/// <returns>A song library entry</returns>
		/// <param name="path">The song path relative to the library folder.</param>
		public Song fetchSong(string uri){
			return Song.Get(new SQLiteCommand("SELECT * FROM songs WHERE FilePath = " + uri, db));    
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

			new SQLiteCommand("CREATE TABLE settings (Key TEXT, Value TEXT)", sql)
				.ExecuteNonQuery();
			new SQLiteCommand("INSERT INTO settings " +
			"(Key, Value) VALUES ('libfolder', '" + Environment.SpecialFolder.MyMusic + "')", sql)
				.ExecuteNonQuery();
		}

		~Library() {
			db.Close();
		}
	}
}
