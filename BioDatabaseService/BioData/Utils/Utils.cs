using System;
using System.IO;

namespace BioData.Utils
{
    public class IOUtils
    {
        public string LocalStorage
        {
            get { return "F:\\Biometric Software\\Client\\"; }
        }

        public Google.Protobuf.ByteString GetFileDescription( string path )
        {
            if (!File.Exists(path))
                return null;

            byte[] bytes = File.ReadAllBytes(path);
            return Google.Protobuf.ByteString.CopyFrom(bytes);
        }

        public string GetLocationImagePath(long locationid)
        {
            DateTime date = DateTime.Now;

            string filename = date.Ticks.ToString() + ".jpg";
            string localPath = string.Format("media\\location\\{0}\\{1}\\{2}\\{3}\\photo\\{4}"
                                            , locationid
                                            , date.Year
                                            , date.Month
                                            , date.Day
                                            , filename);           

            return localPath;
        }

        public string GetLocationFirPath(long locationid)
        {
            DateTime date = DateTime.Now;

            string filename = date.Ticks.ToString() + ".fir";
            string localPath = String.Format("media\\location\\{0}\\{1}\\{2}\\{3}\\fir\\{4}"
                                            , locationid
                                            , date.Year
                                            , date.Month
                                            , date.Day
                                            , filename);

            return localPath;
        }

        public string GetPersonImagePath(long personid )
        {
           string filename = DateTime.Now.Ticks.ToString() + ".jpg";
           string localPath = String.Format("media\\person\\{0}\\photo\\{1}", personid, filename);
           
           return localPath;
        }

        public string GetPersonFirPath(long personid)
        {
            string filename = DateTime.Now.Ticks.ToString() + ".fir";
            string localPath = String.Format("media\\person\\{0}\\fir\\{1}", personid, filename);

            return localPath;
        }

       public string SavePersonImage( Google.Protobuf.ByteString bytes, long personid)
       {
          string filename  = DateTime.Now.Ticks.ToString() + ".jpg";
          string localPath = String.Format("media\\person\\{0}\\photo\\{1}", personid, filename );
          string fullPath  = String.Format("{0}{1}", LocalStorage, localPath );

          SaveFile(bytes.ToByteArray(), fullPath);
          
          return localPath;
       }

       public long GetIDFromPath(string path)
       {         
         string[] paths = Path.GetDirectoryName(path).Split('\\');
         long id = -1;
         if (paths.Length > 2)           
            long.TryParse(paths[2], out id);              

         return id;
       }

       public string ChangePathID( string from, long newID, bool withFileMove = true )
       {         
          string filename = Path.GetFileName(from);
          string[] paths = Path.GetDirectoryName(from).Split('\\');
          long id = 0;
          string toPath = "";
          if (paths.Length > 2)
          {
              bool result = long.TryParse(paths[2], out id);
              if (result)
              {
                  paths[2] = newID.ToString();
                  toPath = Path.Combine(paths) + "\\" + filename;
                  if (toPath == "" || !withFileMove)
                      return toPath;
                  try
                  {
                      File.Move(LocalStorage + from, LocalStorage + toPath);
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine(ex.Message);
                  }
              }
          }

          return toPath;
       }

       public string SavePersonFir(Google.Protobuf.ByteString bytes, long personid)
       {
           string filename  = DateTime.Now.Ticks.ToString() + ".fir";
           string localPath = String.Format("media\\person\\{0}\\fir\\{1}", personid, filename);
           string fullPath  = String.Format("{0}{1}", LocalStorage, localPath);

           SaveFile(bytes.ToByteArray(), fullPath);

           return localPath;
       }
           
       public string SaveLocationImage( Google.Protobuf.ByteString bytes, long locationid )
       {
           DateTime date = DateTime.Now;

           string filename  = date.Ticks.ToString() + ".jpg";
           string localPath = String.Format("media\\location\\{0}\\{1}\\{2}\\{3}\\photo\\{4}"
                                           , locationid
                                           , date.Year
                                           , date.Month
                                           , date.Day
                                           , filename  );

           string fullPath = String.Format("{0}{1}", LocalStorage, localPath);

           return localPath;
       }

       public string SaveLocationFir(Google.Protobuf.ByteString bytes, long locationid)
       {
           DateTime date = DateTime.Now;

           string filename = date.Ticks.ToString() + ".fir";
           string localPath = String.Format("media\\location\\{0}\\{1}\\{2}\\{3}\\fir\\{4}"
                                           , locationid
                                           , date.Year
                                           , date.Month
                                           , date.Day
                                           , filename);

           string fullPath = String.Format("{0}{1}", LocalStorage, localPath);

           return localPath;
       }

       public void SaveLocalFile(byte[] bytes, string path)
       {
           string fullPath = LocalStorage + path;
           SaveFile(bytes, fullPath);
       }

        public void SaveFile(byte[] bytes, string path)
        {
          if (File.Exists(path))
             return;

           try
           {
             Directory.CreateDirectory(Path.GetDirectoryName(path));
           
             var fs = new BinaryWriter(new FileStream(path, FileMode.CreateNew, FileAccess.Write));
             fs.Write(bytes);
             fs.Close();
           }
           catch ( Exception ex) {
             Console.WriteLine(ex.Message);
           }
        }

    }
}
