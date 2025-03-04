using System;
using System.IO;

using Optuna.Study;

namespace Tunny.Storage
{
    public class StorageHandler : IStorage
    {
        public dynamic Storage { get; set; }

        public dynamic CreateNewStorage(bool useInnerPythonEngine, Settings.Storage storageSetting)
        {
            ICreateStorage storage;

            switch (Path.GetExtension(storageSetting.Path))
            {
                case null:
                    storage = new InMemoryStorage();
                    break;
                case ".sqlite3":
                case ".db":
                    storage = new SqliteStorage();
                    break;
                case ".log":
                    storage = new JournalStorage();
                    break;
                default:
                    throw new ArgumentException("Storage type not supported");
            }
            Storage = storage.CreateNewStorage(useInnerPythonEngine, storageSetting);
            return Storage;
        }

        public void DuplicateStudyInStorage(string fromStudyName, string toStudyName, Settings.Storage storageSetting)
        {
            IStorage storage;

            switch (Path.GetExtension(storageSetting.Path))
            {
                case ".sqlite3":
                case ".db":
                    storage = new SqliteStorage();
                    break;
                case ".log":
                    storage = new JournalStorage();
                    break;
                default:
                    throw new ArgumentException("Storage type not supported");
            }
            storage.DuplicateStudyInStorage(fromStudyName, toStudyName, storageSetting);
        }

        public StudySummary[] GetStudySummaries(string storagePath)
        {
            IStorage storage;

            switch (Path.GetExtension(storagePath))
            {
                case ".sqlite3":
                case ".db":
                    storage = new SqliteStorage();
                    break;
                case ".log":
                    storage = new JournalStorage();
                    break;
                default:
                    throw new ArgumentException("Storage type not supported");
            }

            return storage.GetStudySummaries(storagePath);
        }
    }
}
