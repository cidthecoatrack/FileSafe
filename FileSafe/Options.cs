using System;
using System.Collections.Generic;
using System.Text;

namespace FileSafe
{
    [Serializable]
    class Options
    {
        public string BackUpDirectory;
        public LinkedList<string> OriginalDirectories;
        public LinkedList<string> Exceptions;
        public bool CheckOriginalsOnLoad;
        public bool CheckExceptionsOnLoad;

        public Options()
        {
            OriginalDirectories = new LinkedList<string>();
            Exceptions = new LinkedList<string>();
        }

        public Options(string directory)
        {
            OriginalDirectories = new LinkedList<string>();
            Exceptions = new LinkedList<string>();
            BackUpDirectory = directory;
            CheckOriginalsOnLoad = false;
            CheckExceptionsOnLoad = false;
        }
    }
}
