using System;
using System.IO;

namespace TriangulationToolApp.Business
{
    internal class EvaluationProtector
    {
        public const string ContactEmail = "fdiaz@softcrits.es";
        private const string CounterFileName = "f00d8ef0-9993-4759-bfb3-41a99979bd76";
        private const int MaximumNumberOfUsages = 100;

        private static string UserDataFolder
        {
            get
            {
                var folderBase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return folderBase;
            }
        }

        private static string CounterFilePath
        {
            get { return Path.Combine(UserDataFolder, CounterFileName); }
        }

        public static int UseApplication()
        {
            CheckIfCounterFileExists();
            var usageCount = DecrementUsageCount();
            return usageCount;
        }

        private static void CheckIfCounterFileExists()
        {
            if (!File.Exists(CounterFilePath))
            {
                CreateDefaultCounterFile();
            }
        }

        private static int DecrementUsageCount()
        {
            var remainingUsages = GetRemainingUsages();
            if (remainingUsages <= 0)
            {
                throw new InvalidOperationException();
            }
            remainingUsages--;
            UpdateUsagesOnFile(remainingUsages);
            return remainingUsages;
        }

        private static int GetRemainingUsages()
        {
            int remainingUsages;
            using (var reader = new BinaryReader(File.Open(CounterFilePath, FileMode.Open)))
            {
                remainingUsages = reader.ReadInt32();
            }
            return remainingUsages;
        }

        private static void UpdateUsagesOnFile(int remainingUsages)
        {
            using (var writer = new BinaryWriter(File.Open(CounterFilePath, FileMode.Truncate)))
            {
                writer.Write(remainingUsages);
            }
        }

        private static void CreateDefaultCounterFile()
        {
            using (var writer = new BinaryWriter(File.Open(CounterFilePath, FileMode.Create)))
            {
                writer.Write(MaximumNumberOfUsages);
            }
        }
    }
}