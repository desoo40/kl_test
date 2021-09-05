using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kl_test
{
    public class FileInfo
    {
        public string Zone;
        public GeneralInfo FileGeneralInfo;

        public class GeneralInfo
        {
            public string FileStatus;
            public string Sha1;
            public string Md5;
            public string Sha256;
            public string FirstSeen;
            public string LastSeen;
            public string Signer;
            public int Size;
            public string Type;
            public int HitsCount;
        }
    }
}
