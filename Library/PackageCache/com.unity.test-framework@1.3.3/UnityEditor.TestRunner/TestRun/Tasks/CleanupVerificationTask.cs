using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner.TestRun.Tasks
{
    internal class CleanupVerificationTask : FileCleanupVerifierTaskBase
    {
        private const string k_Indent = "    ";

        internal Action<object> logAction = Debug.LogWarning;

        public override IEnumerator Execute(TestJobData testJobData)
        {
            var currentFiles = GetAllFilesInAssetsDirectory();
            var existingFiles = testJobData.existingFiles;

            if (currentFiles.Length != existingFiles.Length)
            {
                var existingFilesHashSet = new HashSet<string>(existingFiles);
                var newFiles = currentFiles.Where(file => !existingFilesHashSet.Contains(file)).ToArray();
                LogWarningForFilesIfAny(newFiles);
            }

            yield return null;
        }

        private void LogWarningForFilesIfAny(string[] filePaths)
        {
            if (filePaths.Length == 0)
            {
                return;
            }

            var stringWriter = new StringWriter();
            stringWriter.WriteLine("Files generated by test without cleanup.");
            stringWriter.WriteLine(k_Indent + "Found {0} new files.", filePaths.Length);

            foreach (var filePath in filePaths)
            {
                stringWriter.WriteLine(k_Indent + filePath);
            }

            logAction(stringWriter.ToString());
        }
    }
}
