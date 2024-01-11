using DailyHealthLog.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DailyHealthLog.FileControl
{
    public static class FileHelper
    {
        public static void WipeFile(string fileName)
        {
            File.SetAttributes(fileName, FileAttributes.Normal);
            double sectors = Math.Ceiling(new FileInfo(fileName).Length / 512.0);
            byte[] dummyBuffer = new byte[5000];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            FileStream inputStream = new FileStream(fileName, FileMode.Open);

            for (int passIndex = 0; passIndex < Settings.WRITEITERATIONS; passIndex++)
            {
                inputStream.Position = 0;
                for (int sectorIndex = 0; sectorIndex < sectors; sectorIndex++)
                {
                    rng.GetBytes(dummyBuffer);
                    inputStream.Write(dummyBuffer, 0, dummyBuffer.Length);
                }
            }
            inputStream.SetLength(0);
            inputStream.Close();

            DateTime dt = new DateTime(2037, 1, 1, 0, 0, 0);
            File.SetCreationTime(fileName, dt);
            File.SetCreationTimeUtc(fileName, dt);
            File.SetLastAccessTime(fileName, dt);
            File.SetLastAccessTimeUtc(fileName, dt);
            File.SetLastWriteTime(fileName, dt);
            File.SetLastWriteTimeUtc(fileName, dt);

            File.Delete(fileName);
        }

        public static byte[] GetFileBytes(string filename)
        {
            int retryMax = 20;
            int retryCount = 0;

            do
            {
                try
                {
                    byte[] logFile = File.ReadAllBytes(filename);
                    return logFile;
                }
                catch
                {
                    retryCount++;
                }
                Thread.Sleep(1000);
            } while (retryCount <= retryMax);
            return null;
        }
    }
}
