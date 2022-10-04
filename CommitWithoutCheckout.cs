using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace LibGit2SharpExamples
{
    public class CommitWithoutCheckout
    {
        public static async Task Run(string path)
        {
            // Create a repository somewhere and commit something there, this will be our 'remote'
            var remoteRepositoryPath = await CreateRepositoryWithAFile.Run(Path.Join(path, "remote"));

            // Create a bare clone
            var clonePath = Path.Join(path, "clone.git");
            Repository.Clone(remoteRepositoryPath, clonePath, new CloneOptions { IsBare = true });
            var repository = new Repository(clonePath);

            // Create a new tree definition from the current head of the current branch
            var treeDefinition = TreeDefinition.From(repository.Head.Tip.Tree);

            // Create a new file in the tree
            var bytes = Encoding.UTF8.GetBytes("new file contents!");
            var blobId = repository.ObjectDatabase.Write<Blob>(bytes);
            treeDefinition.Add("new-file.txt", blobId, Mode.NonExecutableFile);

            // Update the existing file in the tree
            treeDefinition.Add("hello.txt", blobId, Mode.NonExecutableFile);

            // Create the tree in the object database
            var tree = repository.ObjectDatabase.CreateTree(treeDefinition);

            // Create the commit
            var branch = repository.Branches.First(b => !b.IsRemote);
            var signature = new Signature("Sample", "sample@test.com", DateTimeOffset.UtcNow);
            var commit = repository.ObjectDatabase.CreateCommit(signature, signature, "Commit message", tree, new Commit[] { branch.Tip }, false);

            repository.Refs.UpdateTarget(branch.Reference, commit.Id);
        }
    }
}