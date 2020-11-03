using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace VideoAudioJoiner
{
    public class Joiner
    {
        private readonly string _mainFolder;
        public Joiner(string mainFolder)
        {
            _mainFolder = mainFolder;
        }

        public void Join()
        {
            JoinFilesFromFolder(_mainFolder);
        }

        private void JoinFilesFromFolder(string folderPath)
        {
            DirectoryInfo directory = new DirectoryInfo(folderPath);

            var files = directory.GetFiles();
            foreach (var filesForJoin in GetFilesForJoin(files))
            {
                JoinVideoAndAudio(filesForJoin.Item1, filesForJoin.Item2);
            }

            var directories = directory.GetDirectories();
        }

        private List<Tuple<FileInfo, FileInfo>> GetFilesForJoin(FileInfo[] files)
        {
            var filesForJoin = new List<Tuple<FileInfo, FileInfo>>();

            foreach (var file in files)
            {
                if (!file.Name.Contains(" audio") && files.Select(x => x.Name).Contains(file.Name.Replace(".mp4", "") + " audio.mp4"))
                    filesForJoin.Add(new Tuple<FileInfo, FileInfo>(file, files.FirstOrDefault(x => x.FullName == file.FullName.Replace(".mp4", "") + " audio.mp4")));
            }

            return filesForJoin;
        }

        private void JoinVideoAndAudio(FileInfo videoPath, FileInfo audioPath)
        {
            var outputPath = videoPath.Directory.Parent.CreateSubdirectory("output");
            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = _mainFolder + "ffmpeg.exe";
                    p.StartInfo.Arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" -c:v copy -c:a aac \"{outputPath}\\{videoPath.Name}\"";
                    p.Start();
                    p.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
