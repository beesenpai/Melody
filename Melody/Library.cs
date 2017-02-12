using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Melody.Core {
    [Serializable]
    public class Library {
        private const string FNAME = "Melody.db";

        private static string ResolveDirectory() {
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                return Environment.GetEnvironmentVariable("HOME") + @"/.config/Melody/" + FNAME;
            else return Environment.ExpandEnvironmentVariables("%APPDATA%") + @"\Melody\" + FNAME;
        }

        public static Library Load() {
            if (File.Exists(ResolveDirectory())) {
                Library lib;
                IFormatter f = new BinaryFormatter();
                FileStream io = new FileStream(ResolveDirectory(), FileMode.Open, FileAccess.Read, FileShare.Read);
                try { lib = (Library)f.Deserialize(io); }
                catch(SerializationException) { return new Library(); }
                io.Close();
                return lib;
            }
            else return new Library();
        }

        public void Save() {
            IFormatter f = new BinaryFormatter();
            if (!Directory.Exists(ResolveDirectory().Substring(0, ResolveDirectory().Length - 10)))
                Directory.CreateDirectory(ResolveDirectory().Substring(0, ResolveDirectory().Length - 10));
            FileStream io = new FileStream(ResolveDirectory(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            f.Serialize(io, this);
            io.Close();
        }

        ~Library() {
            Save();
        }
    }
}
