using System;
using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;

using Melody.Error;

namespace Melody.Core {
	public class LibraryFolders {
		private SQLiteConnection db;

		internal LibraryFolders(SQLiteConnection con) {
			db = con;
		}

		/// <summary>
		/// Get the list of all library folders.
		/// </summary>
		/// <returns>A list of string URIs pointing to library folders</returns>
		public List<string> GetFolders(){
			List<string> ret = new List<>();

			SQLiteDataReader r = new SQLiteCommand("SELECT * FROM settings " +
			                     "WHERE Key = '" + Library.LIBFOLDER + "'", db)
				.ExecuteReader();
			while(r.Read())
				ret.Add(r.GetString());
			return ret.AsReadOnly();
		}

		/// <summary>
		/// Add a new library folder
		/// </summary>
		/// <param name="uri">The folder URI.</param>
		public void AddFolder(string uri){
			using(List<string> folders = GetFolders()) {
				foreach(string f in folders)
					if(f == uri)
						throw new SettingsException("The folder is already in the list of library folders.");
			}

			if(!Directory.Exists(uri))
				throw new DirectoryNotFoundException();

			new SQLiteCommand("INSERT INTO settings (Key, Value) VALUES('" + Library.LIBFOLDER + "', '" + uri + "')", db)
				.ExecuteNonQuery();
		}

		/// <summary>
		/// Removes a library folder.
		/// </summary>
		/// <param name="uri">The folder URI.</param>
		public void RemoveFolder(string uri){
			new SQLiteCommand("DELETE FROM settings WHERE Key = '" + Library.LIBFOLDER + "' AND Value = '" + uri + "'", db)
				.ExecuteNonQuery();
		}
	}
}

