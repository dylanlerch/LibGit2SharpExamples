using System;
using System.IO;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace LibGit2SharpExamples
{
    public class CreateRepositoryWithAFile
    {
        public static async Task<string> Run(string path)
        {
            // Create the repository
            Repository.Init(path);
            var repository = new Repository(path);

            // Create a file
            var filePath = Path.Join(repository.Info.WorkingDirectory, "hello.txt");
            var fileContents = "hello world";
            await File.WriteAllTextAsync(filePath, fileContents);

            // Commit the file
            Commands.Stage(repository, "*");
            var signature = new Signature("Sample", "sample@test.com", DateTimeOffset.UtcNow);
            repository.Commit("Sample commit", signature, signature);

            return repository.Info.Path;
        }
    }
}