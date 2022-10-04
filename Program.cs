using System;
using System.IO;
using System.Threading.Tasks;

namespace LibGit2SharpExamples
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var path = CreateWorkingDirectory();
            await CommitWithoutCheckout.Run(path);
        }

        static string CreateWorkingDirectory()
        {
            var path = Path.Join(Environment.CurrentDirectory, "output");

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            return Directory.CreateDirectory(path).FullName;
        }
    }
}