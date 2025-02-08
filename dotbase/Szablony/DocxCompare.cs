using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DotBase.Szablony
{
    public class DocxCompare
    {
        static HashSet<string> ignoreFiles = new HashSet<string>(new string[] {
            "docProps/core.xml"
        });

        class FileEntry
        {
            public byte[] content;
            public uint crc32;
        }

        Dictionary<string, FileEntry> files;

        public static bool Compare(string file1, string file2)
        {
            var zip1 = new DocxCompare(file1);
            var zip2 = new DocxCompare(file2);
            return zip1.Compare(zip2);
        }

        public DocxCompare(string file)
        {
            var reader = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete));
            try
            {
                files = new Dictionary<string, FileEntry>();
                while (true)
                {
                    var signature = reader.ReadUInt32();
                    if (signature == 0x08064b50 || signature == 0x02014b50) break;
                    if (signature != 0x04034b50) throw new ApplicationException("Invalid ZIP file");
                    var version = reader.ReadUInt16();
                    var flags = reader.ReadUInt16();
                    if ((flags & (1 << 3)) != 0) throw new ApplicationException("Unsupported ZIP format");
                    var method = reader.ReadUInt16();
                    var time = reader.ReadUInt16();
                    var date = reader.ReadUInt16();
                    var crc32 = reader.ReadUInt32();
                    var compressedSize = reader.ReadUInt32();
                    var size = reader.ReadUInt32();
                    var nameLength = reader.ReadUInt16();
                    var extraLength = reader.ReadUInt16();
                    var nameBytes = reader.ReadBytes(nameLength);
                    string name;
                    try { name = Encoding.UTF8.GetString(nameBytes); }
                    catch (Exception) { name = Encoding.Default.GetString(nameBytes); }
                    var extra = reader.ReadBytes(extraLength);
                    if ((name.EndsWith("/") || name.EndsWith("\\")) && size == 0)
                    {
                        // Skip directories
                        continue;
                    }
                    if (ignoreFiles.Contains(name))
                    {
                        // Skip ignored files
                        reader.ReadBytes((int)compressedSize);
                        continue;
                    }
                    var entry = new FileEntry();
                    entry.content = reader.ReadBytes((int)compressedSize);
                    entry.crc32 = crc32;
                    files[name] = entry;
                }
            }
            finally
            {
                try { reader.BaseStream.Dispose(); }
                catch (Exception) { }
                try { reader.Dispose(); }
                catch (Exception) { }
            }
        }

        public bool Compare(DocxCompare zip2)
        {
            var unprocessed2 = new HashSet<string>(zip2.files.Keys);
            foreach (var file in files)
            {
                if (!zip2.files.ContainsKey(file.Key)) return false;
                if (unprocessed2.Contains(file.Key)) unprocessed2.Remove(file.Key);
                var entry1 = file.Value;
                var entry2 = zip2.files[file.Key];
                if (entry1.crc32 != entry2.crc32) return false;
                if (!entry1.content.SequenceEqual(entry2.content)) return false;
            }
            return unprocessed2.Count == 0;
        }
    }
}
